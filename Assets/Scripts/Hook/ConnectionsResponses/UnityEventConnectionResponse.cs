using System;
using UnityEngine;
using UnityEngine.Events;

namespace Hook.ConnectionsResponses
{
    [Serializable]
    public class UnityEventConnectionResponse : IConnectionResponse
    {
        [SerializeField] private UnityEvent _response;
        
        public void Execute(Connection connection)
        {
            _response?.Invoke();
        }
    }
}