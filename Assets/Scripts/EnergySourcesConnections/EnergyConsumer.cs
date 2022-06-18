using Hook;
using Hook.ConnectionsResponses;
using UnityEngine;
using UnityEngine.Events;

namespace EnergySourcesConnections
{
    public class EnergyConsumer : MonoBehaviour, IConnectionResponse
    {
        [SerializeField] private UnityEvent _onEnergyReceived;
        
        public void OnEnergyReceived()
        {
            _onEnergyReceived?.Invoke();
        }

        public void Execute(Connection connection)
        {
            
        }
    }
}