using System;
using Character;
using DummySOA;
using UnityEngine;

namespace AI.Behaviours
{
    public class MoveTowardTarget : AiBehaviour
    {
        [SerializeField] private RuntimeGameObjectsList _playerList;
        [SerializeField] private CharacterMovement _movement;
        [SerializeField] private Vector2 _scale;

        private bool PlayerExists => _playerList.Count > 0;
        
        private void Update()
        {
            if (PlayerExists)
            {
                var selfPos = _movement.transform.position;
                var targetPos = _playerList[0].transform.position;
                var dir = (targetPos - selfPos) * _scale;
                dir.Normalize();
                _movement.Move(dir);
            }
        }
    }
}