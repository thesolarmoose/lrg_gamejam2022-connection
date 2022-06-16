using System;
using Character;
using DummySOA;
using UnityEngine;

namespace AI.Conditions
{
    [Serializable]
    public class PlayerDistanceCondition : ChangeAiBehaviourCondition
    {
        [SerializeField] private Transform _self;
        [SerializeField] private RuntimeGameObjectsList _playerList;
        
        [SerializeField] private float _sqrDistance;
        [SerializeField] private bool _greater;

        private bool PlayerExists => _playerList.Count > 0;
        
        public override bool IsMet()
        {
            if (PlayerExists)
            {
                var selfPos = _self.position;
                var playerPos = _playerList[0].transform.position;

                var sqrDist = (playerPos - selfPos).sqrMagnitude;

                bool conditionIsMet = _greater
                    ? _sqrDistance < sqrDist
                    : _sqrDistance > sqrDist;
                
                return conditionIsMet;
            }
            else
            {
                return _greater;
            }
        }
        
        public override void ParentBehaviourEnabled()
        {
            // do nothing
        }

        public override void ParentBehaviourDisabled()
        {
            // do nothing
        }
    }
}