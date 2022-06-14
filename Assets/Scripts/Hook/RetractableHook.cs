using System;
using System.Collections;
using System.Collections.Generic;
using Hook.RetractionControllers;
using UnityEngine;

namespace Hook
{
    public class RetractableHook : MonoBehaviour
    {
        [SerializeField] private Rope _rope;
        [SerializeReference, SubclassSelector] private IRetractionController _retractionController;
        
        [SerializeField] private float _retractSpeed;
        [SerializeField] private float _tensionToJoin;
        [SerializeField] private float _length;
        [SerializeField] private float _shootSpeed;
        [SerializeField] private float _drag;
        [SerializeField] private float _minShootLength;
        [SerializeField] private float _collisionRadius;
        [SerializeField] private LayerMask _mask;
        
        private bool _firstShot;
        private bool _secondShot;
        
        private RaycastHit2D[] _raycastsBuffer = new RaycastHit2D[5];
        private List<Connection> _connectedTips = new List<Connection>();
        
        public bool IsStillConnected => !_secondShot;

        public Rope Rope => _rope;

        private bool BothTipsShot => _firstShot && _secondShot;
        private bool BothTipsConnected => _connectedTips.Count == 2;

        public void Initialize(Vector2 position)
        {
            transform.position = position;
            _rope.StartPoint.position = position;
            _rope.EndPoint.position = position;
            _rope.StartPoint.Weight = 1.0f;
            _rope.EndPoint.Weight = 1.0f;
            _rope.Initialize(position);
            _rope.Length = _length;
        }

        public void Shoot(Vector2 dir, Transform parent)
        {
            if (!_firstShot)
            {
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
            StartCoroutine(ShootTip(dir, ropeTip));
        }

        private IEnumerator ShootTip(Vector2 dir, RopeTip ropeTip)
        {
            var normDir = dir.normalized;
            var startPosition = ropeTip.position;

            bool targetReached = false;
            bool connected = false;
            Vector2 connectionPoint = Vector2.zero;
            float lastTime = Time.time;
            while (!targetReached)
            {
                float deltaTime = Time.time - lastTime;
                var deltaDist = deltaTime * _shootSpeed;
                
                targetReached = CheckTargetReach(
                    startPosition,
                    ropeTip.position,
                    normDir,
                    deltaDist,
                    out connected,
                    out connectionPoint);

                if (!targetReached)
                {
                    MoveRopeTip(ropeTip, normDir, _shootSpeed, deltaTime);
                }

                yield return null;
            }

            if (connected)
            {
                var hooked = _raycastsBuffer[0].collider;
                Connect(ropeTip, hooked, connectionPoint);

                if (BothTipsShot)
                {
                    if (BothTipsConnected)
                    {
                        yield return RetractRope();
                    }
                    else
                    {
                        // disappear rope
                    }
                }
            }
            else
            {
                ropeTip.Weight = 0.1f;
                yield return KeepMovingWithDrag(ropeTip, normDir);
            }
        }

        private void Connect(RopeTip ropeTip, Collider2D coll, Vector2 connectionPoint)
        {
            var hooked = coll.GetComponent<IHookable>();
            ropeTip.position = connectionPoint;
            ropeTip.transform.SetParent(hooked.GetTransform(), true);
            _connectedTips.Add(new Connection(ropeTip, hooked));
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

        private IEnumerator RetractRope()
        {
            float length = _rope.Length;
            while (_rope.CurrentTension < _tensionToJoin)
            {
//                print($"length {_rope.Length}; tension: {_rope.CurrentTension}");
                length -= _retractSpeed;
                _rope.Length = length;
                yield return null;
            }

            _rope.Length = 0;
            
            // start retracting
            yield return _retractionController.Retract(_connectedTips[0], _connectedTips[1]);
        }

        private bool CheckTargetReach(
            Vector2 startPosition,
            Vector2 tipPosition,
            Vector2 dir,
            float moveDistance,
            out bool connected,
            out Vector2 connectionPoint)
        {
            var hits = Physics2D.CircleCastNonAlloc(
                tipPosition,
                _collisionRadius,
                dir,
                _raycastsBuffer,
                moveDistance,
                _mask);
            
            connectionPoint = Vector2.zero;

            bool targetReached = false;
            if (hits > 0)
            {
                connected = true;
                var hit = _raycastsBuffer[0];
                connectionPoint = hit.point;
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