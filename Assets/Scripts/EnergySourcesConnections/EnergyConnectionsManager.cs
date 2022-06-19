using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utils.Extensions;

namespace EnergySourcesConnections
{
    public class EnergyConnectionsManager : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onAllConnected;
        [SerializeField] private EnergyConsumer _energyProvider;
        [SerializeField] private EnergyConsumer _target;
        
        private readonly List<EnergyConsumersGroup> _groups = new List<EnergyConsumersGroup>();

        private void Awake()
        {
            EnergyConnectionsReference.Instance.Manager = this;
        }

        public void OnEnergyConsumersConnected(EnergyConsumer first, EnergyConsumer second)
        {
            AddConnectionToGroups(first, second, out bool changed);
            if (changed)
                CheckVictoryCondition();
        }

        private void AddConnectionToGroups(EnergyConsumer first, EnergyConsumer second, out bool changed)
        {
            changed = false;
            bool haveEnergy = first == _energyProvider || second == _energyProvider;
            bool firstHaveGroup = TryGetGroup(first, out var firstGroup);
            bool secondHaveGroup = TryGetGroup(second, out var secondGroup);
            bool bothHaveGroup = firstHaveGroup && secondHaveGroup;

            if (bothHaveGroup && firstGroup != secondGroup)
            {
                firstGroup.Merge(secondGroup);
                _groups.Remove(secondGroup);
                changed = true;
            }
            else
            {
                if (!firstHaveGroup && secondHaveGroup)
                {
                    secondGroup.AddConsumer(first);
                    secondGroup.WithEnergy |= haveEnergy;
                }
                else if (firstHaveGroup)
                {
                    firstGroup.AddConsumer(second);
                    firstGroup.WithEnergy |= haveEnergy;
                }
                else
                {
                    var groupList = new List<EnergyConsumer> {first, second};
                    var newGroup = new EnergyConsumersGroup(haveEnergy, groupList);
                    _groups.Add(newGroup);
                }
                changed = true;
            }
        }

        private void CheckVictoryCondition()
        {
            if (TryGetGroup(_target, out var targetGroup))
            {
                if (targetGroup.WithEnergy)
                {
                    // you won :)
                    _onAllConnected?.Invoke();
                }
            }
        }

        private bool TryGetGroup(EnergyConsumer consumer, out EnergyConsumersGroup group)
        {
            group = null;
            
            bool exists = _groups.Exists(group => group.Contains(consumer));
            if (exists)
            {
                group = _groups.Find(group => group.Contains(consumer));
            }

            return exists;
        }
    }

    public class EnergyConsumersGroup
    {
        private bool _withEnergy;
        private List<EnergyConsumer> _connectedConsumers;

        public bool WithEnergy
        {
            get => _withEnergy;
            set
            {
                if (!_withEnergy && value)
                {
                    ProvideEnergyToAll();
                }
                _withEnergy = value;
            }
        }

        public List<EnergyConsumer> ConnectedConsumers => _connectedConsumers;

        public EnergyConsumersGroup(bool withEnergy, List<EnergyConsumer> connectedConsumers)
        {
            _withEnergy = withEnergy;
            _connectedConsumers = connectedConsumers;
            if (withEnergy)
            {
                ProvideEnergyToAll();
            }
        }

        private void ProvideEnergyToAll()
        {
            foreach (var consumer in _connectedConsumers)
            {
                consumer.OnEnergyReceived();
            }
        }

        public bool Contains(EnergyConsumer consumer)
        {
            return _connectedConsumers.Contains(consumer);
        }
        
        public bool Overlap(EnergyConsumersGroup other)
        {
            foreach (var consumer in other.ConnectedConsumers)
            {
                if (_connectedConsumers.Contains(consumer))
                {
                    return true;
                }
            }

            return false;
        }

        public void AddConsumer(EnergyConsumer consumer)
        {
            if (!Contains(consumer))
            {
                _connectedConsumers.Add(consumer);
                if (_withEnergy)
                    consumer.OnEnergyReceived();
            }
        }

        public void Merge(EnergyConsumersGroup other)
        {
            if (!_withEnergy && other._withEnergy)
            {
                ProvideEnergyToAll();
            }
            if (_withEnergy && !other._withEnergy)
            {
                other.ProvideEnergyToAll();
            }
            _withEnergy |= other.WithEnergy;
            _connectedConsumers.AddRangeIfNotExists(other.ConnectedConsumers);
        }
    }
}