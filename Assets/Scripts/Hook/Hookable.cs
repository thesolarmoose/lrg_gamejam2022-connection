using System;
using UnityEngine;
using UnityEngine.Events;

namespace Hook
{
    public class Hookable : MonoBehaviour, IHookable
    {
        [SerializeField] private bool _isMovable;
        
        public UnityEvent onConnected;
        public UnityEvent onStartRetracting;
        public UnityEvent<IHookable> onCollided;
        
        private Collider2D _collider;

        private void Start()
        {
            _collider = GetComponent<Collider2D>();
        }

        public bool IsMovable()
        {
            return _isMovable;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public Collider2D GetCollider()
        {
            return _collider;
        }

        public void OnConnected()
        {
            onConnected?.Invoke();
        }

        public void OnStartRetracting()
        {
            onStartRetracting?.Invoke();
        }

        public void OnCollided(IHookable other)
        {
            onCollided?.Invoke(other);
        }
    }
}