using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hook
{
    public class RetractableHook : MonoBehaviour
    {
        [SerializeField] private Rope _rope;
        [SerializeField] private float _length;
        [SerializeField] private float _shootSpeed;
        [SerializeField] private float _drag;
        [SerializeField] private float _minShootLength;

        [SerializeField] private float _collisionRadius;
        [SerializeField] private LayerMask _mask;

        private bool _firstShot;
        private bool _secondShot;
        
        private Collider2D[] _collidersBuffer = new Collider2D[10];
        private Dictionary<RopeTip, Collider2D> _connectedTips = new Dictionary<RopeTip,Collider2D>();
        
        public bool IsStillConnected => !_secondShot;

        public void Initialize(Vector2 position)
        {
            transform.position = position;
            _rope.StartPoint.position = position;
            _rope.EndPoint.position = position;
            _rope.Initialize(position);
            _rope.Length = _length;
        }

        public void Shoot(Vector2 dir, Transform parent)
        {
            if (!_firstShot)
            {
                _rope.EndPoint.Weight = 1.0f;
                _rope.EndPoint.SetParent(parent);
                
                Shoot(dir, _rope.StartPoint);
                _firstShot = true;
            }
            else if (!_secondShot)
            {
                _rope.EndPoint.SetParent(null);
                
                Shoot(dir, _rope.EndPoint);
                _secondShot = true;
            }
        }

        private void Shoot(Vector2 dir, RopeTip ropeTip)
        {
            ropeTip.Weight = 1.0f;
            StartCoroutine(ShootTip(dir, ropeTip));
        }

        private IEnumerator ShootTip(Vector2 dir, RopeTip ropeTip)
        {
            var normDir = dir.normalized;
            var startPosition = ropeTip.position;

            bool targetReached = false;
            bool connected = false;
            float lastTime = Time.time;
            while (!targetReached)
            {
                float deltaTime = Time.time - lastTime;
                var newPos = MoveRopeTip(ropeTip, normDir, _shootSpeed, deltaTime);

                targetReached = CheckTargetReach(startPosition, newPos, out connected);

                yield return null;
            }

            if (connected)
            {
                
            }
            else
            {
                ropeTip.Weight = 0.5f;
                yield return KeepMovingWithDrag(ropeTip, normDir);
            }
        }

        private Vector2 MoveRopeTip(RopeTip ropeTip, Vector2 normDir, float speed, float deltaTime)
        {
            var currentPos = ropeTip.position;
            var movement = deltaTime * speed * normDir;
            var newPos = currentPos + movement;
            ropeTip.position = newPos;
            return newPos;
        }

        private IEnumerator KeepMovingWithDrag(RopeTip ropeTip, Vector2 normDir)
        {
            float speed = _shootSpeed;

            float lastTime = Time.time;
            while (speed > 0)
            {
                float deltaTime = Time.time - lastTime;
                MoveRopeTip(ropeTip, normDir, speed, deltaTime);
                speed -= _drag;
                yield return null;
            }
        }

        private bool CheckTargetReach(Vector2 startPosition, Vector2 tipPosition, out bool connected)
        {
            int collisions = Physics2D.OverlapCircleNonAlloc(
                tipPosition,
                _collisionRadius,
                _collidersBuffer,
                _mask);

            bool targetReached = false;
            if (collisions > 0)
            {
                connected = true;
                targetReached = true;
            }
            else
            {
                connected = false;
                float traveledDistance = Vector2.Distance(startPosition, tipPosition);
                targetReached = traveledDistance > _minShootLength;
            }

            return targetReached;
        }
    }
}