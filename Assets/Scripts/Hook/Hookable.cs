using System;
using Hook.ConnectionsPredicates;
using Hook.ConnectionsResponses;
using UnityEngine;
using UnityEngine.Events;

namespace Hook
{
    public class Hookable : MonoBehaviour
    {
        [SerializeField] private bool _isMovable;
        [SerializeField] private bool _shouldKeepRopeAlive;
        [SerializeField] private Transform _transform;
        [SerializeField] private Collider2D _collider;
        
        [SerializeField] private UnityEvent onConnected;
        [SerializeField] private UnityEvent onStartRetracting;
        [SerializeField] private UnityEvent<Hookable> onCollided;
        [SerializeField] private ConnectionEvent _connectedToOtherEvent;

        public bool IsMovable => _isMovable;

        public bool ShouldKeepRopeAlive => _shouldKeepRopeAlive;

        public Transform Transform => _transform;
        
        public Collider2D Collider => _collider;

        public void OnConnected()
        {
            onConnected?.Invoke();
        }

        public void OnConnectedWithOther(Connection other)
        {
            _connectedToOtherEvent.ExecuteIfMetConditions(other);
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

    [Serializable]
    public class ConnectionEvent
    {
        [SerializeReference, SubclassSelector] private IConnectionPredicate _connectedCondition;
        [SerializeReference, SubclassSelector] private IConnectionResponse _response;

        public bool IsMet(Connection other)
        {
            if (_connectedCondition != null)
            {
                return _connectedCondition.IsMet(other);
            }
            return false;
        }

        public void Execute(Connection other)
        {
            _response?.Execute(other);
        }

        public void ExecuteIfMetConditions(Connection other)
        {
            if (IsMet(other))
            {
                Execute(other);
            }
        }
    }
}