using UnityEngine;

namespace AI.Conditions
{
    public class RandomTimerCondition : ChangeAiBehaviourCondition
    {
        [SerializeField] private float _minRandomTime;
        [SerializeField] private float _maxRandomTime;

        private float _timeOver;
        
        public override void ParentBehaviourEnabled()
        {
            var randomSpan = Random.Range(_minRandomTime, _maxRandomTime);
            _timeOver = Time.time + randomSpan;
        }

        public override void ParentBehaviourDisabled()
        {
            // do nothing
        }

        public override bool IsMet()
        {
            return Time.time > _timeOver;
        }
    }
}