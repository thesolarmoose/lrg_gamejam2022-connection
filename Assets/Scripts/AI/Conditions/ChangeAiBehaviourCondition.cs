using UnityEngine;
using Utils.Serializables;

namespace AI.Conditions
{
    public abstract class ChangeAiBehaviourCondition : MonoBehaviour, ISerializablePredicate
    {
        public abstract void ParentBehaviourEnabled();
        public abstract void ParentBehaviourDisabled();
        public abstract bool IsMet();
    }
}