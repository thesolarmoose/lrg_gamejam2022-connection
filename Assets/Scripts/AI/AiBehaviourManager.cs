using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI.States
{
    public class AiBehaviourManager : MonoBehaviour
    {
        [SerializeField] private AiBehaviour _firstBehaviour;
        [SerializeField] private List<AiBehaviour> _behaviours;
        [SerializeField] private List<ConditionToBehaviour> _anyBehaviourConditions;

        private AiBehaviour _currentBehaviour;

        private void Awake()
        {
            DisableAll();
        }

        private void Start()
        {
            ChangeBehaviour(_firstBehaviour);
        }

        private void LateUpdate()
        {
            if (_anyBehaviourConditions.ShouldChangeBehaviour(out var nextBehaviour))
            {
                ChangeBehaviour(nextBehaviour);
            }
            
            if (_currentBehaviour.Conditions.ShouldChangeBehaviour(out nextBehaviour))
            {
                ChangeBehaviour(nextBehaviour);
            }
        }

        private void ChangeBehaviour(AiBehaviour behaviour)
        {
            if (_currentBehaviour != behaviour)
            {
                DisableBehaviour(_currentBehaviour);
                EnableBehaviour(behaviour);
                _currentBehaviour = behaviour;
            }
        }

        private void EnableBehaviour(AiBehaviour behaviour)
        {
            behaviour.Enable();
            _anyBehaviourConditions.ParentBehaviourEnabled();
        }

        private void DisableBehaviour(AiBehaviour behaviour)
        {
            if (behaviour != null)
            {
                behaviour.Disable();
                _anyBehaviourConditions.ParentBehaviourDisabled();
            }
        }

        private void DisableAll()
        {
            _behaviours.ForEach(DisableBehaviour);
        }
    }
}