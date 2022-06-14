﻿using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterMovement : MonoBehaviour, IMover, ILooker
    {
        [SerializeField] private List<BaseLooker> _lookers;
        
        [SerializeField] private float _speed;
        [SerializeField, Range(0.0f, 1.0f)] private float _inertia;

        private Rigidbody2D _rigidbody;
        
        private Vector2 _currentDesiredVelocity;
        private Vector2 _currentVelocity;
        public Vector2 CurrentLookDirection { get; private set; }

        public float CurrentSpeed => _currentVelocity.sqrMagnitude;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public Vector2 GetCurrentPosition()
        {
            return transform.position;
        }

        public void Move(Vector2 moveDir)
        {
            _currentDesiredVelocity = moveDir * _speed;
        }

        public void Look(Vector2 lookDir)
        {
            CurrentLookDirection = lookDir;
            float sign = Mathf.Sign(lookDir.x);
            var t = transform;
            var scale = t.localScale;
            scale.x = sign;
            t.localScale = scale;

            foreach (var looker in _lookers)
            {
                looker.Look(lookDir);
            }
        }

        private void FixedUpdate()
        {
            // move
            var t = transform;
            var pos = t.position;

            _currentVelocity = Vector2.Lerp(_currentVelocity, _currentDesiredVelocity, _inertia);
            pos += (Vector3) _currentVelocity;
            _rigidbody.MovePosition(pos);
        }
    }
}