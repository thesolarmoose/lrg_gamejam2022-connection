using System.Collections.Generic;
using UnityEngine;

namespace AI.Conditions
{
    public class AndCondition : ChangeAiBehaviourCondition
    {
        [SerializeField] private List<ChangeAiBehaviourCondition> _conditions;
        
        public override void ParentBehaviourEnabled()
        {
            // do nothing
        }

        public override void ParentBehaviourDisabled()
        {
            // do nothing
        }

        public override bool IsMet()
        {
            return _conditions.TrueForAll(condition => condition.IsMet());
        }
    }
}