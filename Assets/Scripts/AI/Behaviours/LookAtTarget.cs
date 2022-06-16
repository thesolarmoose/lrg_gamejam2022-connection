using Character;
using DummySOA;
using UnityEngine;

namespace AI.Behaviours
{
    public class LookAtTarget : AiBehaviour
    {
        [SerializeField] private RuntimeGameObjectsList _playerList;
        [SerializeField] private CharacterMovement _movement;

        private bool PlayerExists => _playerList.Count > 0;
        
        private void Update()
        {
            if (PlayerExists)
            {
                var selfPos = _movement.transform.position;
                var targetPos = _playerList[0].transform.position;
                var dir = (targetPos - selfPos).normalized;
                _movement.Look(dir);
            }
        }
    }
}