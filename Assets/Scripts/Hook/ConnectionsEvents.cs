using System;
using System.Collections.Generic;
using Hook.ConnectionsPredicates;
using Hook.ConnectionsResponses;
using UnityEngine;
using Utils;

namespace Hook
{
    [CreateAssetMenu(fileName = "ConnectionsEvents", menuName = "Connections/ConnectionsEvents", order = 0)]
    public class ConnectionsEvents : ScriptableObjectSingleton<ConnectionsEvents>
    {
        [SerializeField] private List<ConnectionEvent> _events;

        public void NotifyCollision(Collision collision)
        {
            foreach (var @event in _events)
            {
                @event.ExecuteIfMetConditions(collision);
            }
        }
    }

    [Serializable]
    public class ConnectionEvent
    {
        [SerializeField] private string _description;
        [SerializeReference, SubclassSelector] private IConnectionPredicate _firstConnectionCondition;
        [SerializeReference, SubclassSelector] private IConnectionPredicate _secondConnectionCondition;
        [SerializeReference, SubclassSelector] private IConnectionResponse _firstConnectionResponse;
        [SerializeReference, SubclassSelector] private IConnectionResponse _secondConnectionResponse;
        [SerializeReference, SubclassSelector] private ICollisionResponse _collisionResponse;

        private bool IsMet(Collision collision, out bool invert)
        {
            bool firstMet = _firstConnectionCondition.IsMet(collision.First);
            bool secondMet = _secondConnectionCondition.IsMet(collision.Second);
            if (firstMet && secondMet)
            {
                invert = false;
                return true;
            }
            else
            {
                firstMet = _firstConnectionCondition.IsMet(collision.Second);
                secondMet = _secondConnectionCondition.IsMet(collision.First);

                invert = true;
                return firstMet && secondMet;
            }
        }

        public void ExecuteIfMetConditions(Collision collision)
        {
            if (IsMet(collision, out bool invert))
            {
                var first = invert ? collision.Second : collision.First;
                var second = invert ? collision.First : collision.Second;
                _firstConnectionResponse?.Execute(first);
                _secondConnectionResponse?.Execute(second);
                _collisionResponse?.Execute(collision);
            }
        }
    }
}