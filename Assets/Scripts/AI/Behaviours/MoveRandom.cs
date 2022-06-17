using System.Linq;
using Character;
using Terrain;
using UnityEngine;
using Utils.Extensions;

namespace AI.Behaviours
{
    public class MoveRandom : AiBehaviour
    {
        [SerializeField] private CharacterMovement _movement;
        [SerializeField] private TerrainReference _terrainReference;
        [SerializeField] private Rect _localBounds;
        [SerializeField] private float _minDistance;
        [SerializeField] private float _targetReachEpsilon;
        [SerializeField] private float _speed;

        private Vector2 _currentTarget;
        private float _originalSpeed;

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
            var terrainRects = _terrainReference.Terrain.Rects;

            Vector2 selfPos = _movement.transform.position;
            var local = new Rect(_localBounds);
            local.x += selfPos.x;
            local.y += selfPos.y;

            var intersections = terrainRects.Select(rect => rect.Intersection(local))
                .Where(intersect => intersect.width > 0 && intersect.height > 0)
                .ToList();

            var intersection = intersections.Count == 0 ? terrainRects.GetRandom() : intersections.GetRandom();
            
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