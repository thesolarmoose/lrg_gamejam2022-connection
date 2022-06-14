using System;
using UnityEngine;
using UnityEngine.Events;

namespace Hook
{
    public class Hookable : MonoBehaviour
    {
        [SerializeField] private bool _isMovable;
        [SerializeField] private Collider2D _collider;
        
        [SerializeField] private UnityEvent onConnected;
        [SerializeField] private UnityEvent onStartRetracting;
        [SerializeField] private UnityEvent<Hookable> onCollided;

        public bool IsMovable => _isMovable;
        public Collider2D Collider => _collider;

        public void OnConnected()
        {
            onConnected?.Invoke();
        }

        public void OnStartRetracting()
        {
            onStartRetracting?.Invoke();
        }

        public void OnCollided(Hookable other)
        {
            onCollided?.Invoke(other);
        }
    }
}