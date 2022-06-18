using System;
using UnityEngine;
using UnityEngine.Events;

namespace Hook.ConnectionsResponses
{
    [Serializable]
    public class UnityEventCollisionResponse : ICollisionResponse
    {
        [SerializeField] private UnityEvent _event;
        
        public void Execute(Collision collision)
        {
            _event?.Invoke();
        }
    }
}