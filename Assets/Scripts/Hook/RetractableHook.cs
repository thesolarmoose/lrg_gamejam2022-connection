using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utils.Tweening;

namespace Hook
{
    public class RetractableHook : MonoBehaviour
    {
        [SerializeField] private Rope _rope;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private RetractionController _retractionController;
        
        [SerializeField] private float _retractSpeed;
        [SerializeField] private float _tensionToJoin;
        [SerializeField] private float _length;
        [SerializeField] private float _lengthToDisconnect;
        [SerializeField] private float _shootSpeed;
        [SerializeField] private float _drag;
        [SerializeField] private float _collisionRadius;
        [SerializeField] private LayerMask _mask;
        [SerializeField] private float _disappearTime;

        public UnityEvent OnTipShot;
        public UnityEvent OnTipConnected;
        public UnityEvent OnBothTipsConnected;
        public UnityEvent OnDisconnected;
        public UnityEvent OnStartRetracting;
        public UnityEvent OnFinishedRetracting;
        
        private bool _firstShot;
        private bool _secondShot;
        
        private RaycastHit2D[] _raycastsBuffer = new RaycastHit2D[5];
        private List<Connection> _connectedTips = new List<Connection>();

        public bool IsStillConnected => !_secondShot;

        public Rope Rope => _rope;

        private bool BothTipsShot => _firstShot && _secondShot;
        private bool BothTipsConnected => _connectedTips.Count == 2;

        private Connection FirstConnection => _connectedTips[0];
        private Connection SecondConnection => _connectedTips[1];
        private Hookable FirstConnected => _connectedTips[0].Hooked;
        private Hookable SecondConnected => _connectedTips[1].Hooked;

        private void Update()
        {
            // check max distance
            if (_firstShot && !_secondShot)
            {
                var dist = _rope.DistanceBetweenTips;
                if (dist > _lengthToDisconnect)
                {
                    Disconnect(_rope.EndPoint);
                    _secondShot = true;
                    StartCoroutine(DisappearRope());
                    OnDisconnected?.Invoke();
                }
            }
        }

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
                Disconnect(_rope.EndPoint);
                Shoot(dir, _rope.EndPoint);
                _secondShot = true;
            }
        }

        private void Shoot(Vector2 dir, RopeTip ropeTip)
        {
            OnTipShot?.Invoke();
            StartCoroutine(ShootTip(dir, ropeTip));
        }

        private IEnumerator ShootTip(Vector2 dir, RopeTip ropeTip)
        {
            var normDir = dir.normalized;

            bool targetReached = false;
            bool connected = false;
            Vector2 connectionPoint = Vector2.zero;
            float maxDist = _lengthToDisconnect - _collisionRadius * 2;
            float movedDistance = 0;
            while (!targetReached)
            {
                float deltaDist = Time.deltaTime * _shootSpeed;
                float maxMoveDist = maxDist - movedDistance;
                
                // update stop condition
                targetReached = deltaDist > maxMoveDist;
                
                // clamp move distance
                deltaDist = Mathf.Min(maxMoveDist, deltaDist);
                
                connected = CheckConnection(
                    ropeTip.position,
                    normDir,
                    deltaDist,
                    out connectionPoint);

                if (connected)
                    targetReached = true;

                MoveRopeTip(ropeTip, deltaDist, normDir);
                movedDistance += deltaDist;

                yield return null;
            }

            if (connected)
            {
                var hooked = _raycastsBuffer[0].collider;
                Connect(ropeTip, hooked, connectionPoint);

                if (BothTipsConnected)
                {
                    OnBothTipsConnected?.Invoke();
                    yield return RetractRope();
                }
            }
            else
            {
                ropeTip.Weight = 0.1f;
//                yield return KeepMovingWithDrag(ropeTip, normDir);
            }

            if (BothTipsShot)
            {
                yield return DisappearRope();
            }
        }

        private void Connect(RopeTip ropeTip, Collider2D coll, Vector2 connectionPoint)
        {
            var hooked = coll.GetComponent<Hookable>();
            hooked.OnConnected();
            ropeTip.position = connectionPoint;
            ropeTip.transform.SetParent(hooked.Transform, true);
            _connectedTips.Add(new Connection(ropeTip, hooked));
            OnTipConnected?.Invoke();
        }

        private void Disconnect(RopeTip tip)
        {
            tip.SetParent(null);
        }

        private static void MoveRopeTip(RopeTip ropeTip, float deltaDist, Vector2 normDir)
        {
            var currentPos = ropeTip.position;
            var movement = deltaDist * normDir;
            var newPos = currentPos + movement;
            ropeTip.position = newPos;
        }

        private IEnumerator KeepMovingWithDrag(RopeTip ropeTip, Vector2 normDir)
        {
            float speed = _shootSpeed;

            while (speed > 0)
            {
                var deltaDist = Time.deltaTime * speed;
                MoveRopeTip(ropeTip, deltaDist, normDir);
                speed -= _drag;
                yield return null;
            }
        }

        private IEnumerator RetractRope()
        {
            OnStartRetracting?.Invoke();
            FirstConnected.OnStartRetracting();
            SecondConnected.OnStartRetracting();
            
            float length = _rope.Length;
            while (_rope.Tension < _tensionToJoin)
            {
                length -= _retractSpeed;
                _rope.Length = length;
                yield return null;
            }

            _rope.Length = 0;
            
            // start retracting
            bool areSame = FirstConnection.Hooked == SecondConnection.Hooked;

            if (!areSame)
            {
                yield return _retractionController.Retract(FirstConnection, SecondConnection);
            }

            ;OnFinishedRetracting?.Invoke();
        }

        private IEnumerator DisappearRope()
        {
            yield return TweeningUtils.TweenTimeCoroutine(
                time =>
                {
                    var material = _lineRenderer.material;
                    var color = material.color;
                    color.a = 1 - time;
                    material.color = color;
                },
                _disappearTime,
                Curves.Linear);

            DestroySelf();
        }

        private void DestroySelf()
        {
            Destroy(gameObject);
            Destroy(_rope.StartPoint.gameObject);
            Destroy(_rope.EndPoint.gameObject);
        }

        private bool CheckConnection(
            Vector2 tipPosition,
            Vector2 dir,
            float moveDistance,
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

            bool connected = false;
            if (hits > 0)
            {
                connected = true;
                var hit = _raycastsBuffer[0];
                connectionPoint = hit.point;
            }

            return connected;
        }
    }
}