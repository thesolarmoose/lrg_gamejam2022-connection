using System;
using Character;
using UnityEditor;
using UnityEngine;
using Utils.Extensions;

namespace AI.Behaviours
{
    public class MoveRandom : AiBehaviour
    {
        [SerializeField] private CharacterMovement _movement;
        [SerializeField] private Rect _globalBounds;
        [SerializeField] private Rect _localBounds;
        [SerializeField] private float _minDistance;
        [SerializeField] private float _targetReachEpsilon;
        [SerializeField] private float _speed;

        private Vector2 _currentTarget;
        private float _originalSpeed;

        public Rect GlobalBounds => _globalBounds;

        public Rect LocalBounds => _localBounds;

        private void OnEnable()
        {
            ComputeNextTarget();
            _originalSpeed = _movement.MaxSpeed;
            _movement.MaxSpeed = _speed;
        }

        private void OnDisable()
        {
            _movement.MaxSpeed = _originalSpeed;
        }

        private void ComputeNextTarget()
        {
            Vector2 selfPos = _movement.transform.position;
            var local = new Rect(_localBounds);
            local.x += selfPos.x;
            local.y += selfPos.y;
            
            var intersection = _globalBounds.Intersection(local);
            if (intersection.width < 0 || intersection.height < 0)
            {
                intersection = _globalBounds;
            }
            
            var target = intersection.RandomPositionInside();
            var movement = target - selfPos;
            if (movement.magnitude < _minDistance)
            {
                target = selfPos + movement.normalized * _minDistance;
            }

            _currentTarget = target;
        }

        private void Update()
        {
            Vector2 selfPos = _movement.transform.position;
            var dir = (_currentTarget - selfPos).normalized;
            _movement.Move(dir);

            if (TargetReached())
            {
                ComputeNextTarget();
            }
        }
        
        private bool TargetReached()
        {
            Vector2 selfPos = _movement.transform.position;
            var dist = Vector2.Distance(selfPos, _currentTarget);
            return dist < _targetReachEpsilon;
        }
    }
}