using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AI.Conditions;
using UnityEngine;

namespace AI
{
    public abstract class AiBehaviour : MonoBehaviour
    {
        [SerializeField] private List<ConditionToBehaviour> _conditions;

        public ReadOnlyCollection<ConditionToBehaviour> Conditions => _conditions.AsReadOnly();

        public void Enable()
        {
            enabled = true;
            _conditions.ParentBehaviourEnabled();
        }

        public void Disable()
        {
            enabled = false;
            _conditions.ParentBehaviourDisabled();
        }
    }
    
    [Serializable]
    public class ConditionToBehaviour
    {
        [SerializeField] private ChangeAiBehaviourCondition _condition;
        [SerializeField] private AiBehaviour _behaviour;

        public ChangeAiBehaviourCondition Condition => _condition;

        public AiBehaviour Behaviour => _behaviour;

        public void Deconstruct(out ChangeAiBehaviourCondition condition, out AiBehaviour behaviour)
        {
            condition = _condition;
            behaviour = _behaviour;
        }
    }

    public static class ConditionToStateListExtensions
    {
        public static void ParentBehaviourEnabled(this List<ConditionToBehaviour> conditions)
        {
            conditions.ForEach(condition => condition.Condition.ParentBehaviourEnabled());
        }
        
        public static void ParentBehaviourDisabled(this List<ConditionToBehaviour> conditions)
        {
            conditions.ForEach(condition => condition.Condition.ParentBehaviourDisabled());
        }
        
        public static bool ShouldChangeBehaviour(this IList<ConditionToBehaviour> conditions, out AiBehaviour nextBehaviour)
        {
            foreach (var (condition, behaviour) in conditions)
            {
                if (condition.IsMet())
                {
                    nextBehaviour = behaviour;
                    return true;
                }
            }

            nextBehaviour = null;
            return false;
        }
    }
}