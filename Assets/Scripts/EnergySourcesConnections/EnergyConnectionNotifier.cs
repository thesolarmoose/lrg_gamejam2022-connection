using System;
using Hook;
using Hook.ConnectionsResponses;
using UnityEngine;

namespace EnergySourcesConnections
{
    [Serializable]
    public class EnergyConnectionNotifier : IConnectionResponse
    {
        [SerializeField] private EnergyConsumer _consumer;
        
        public void Execute(Connection connection)
        {
            var otherConsumer = connection.Hooked.GetComponent<EnergyConsumer>();
            if (otherConsumer != null)
            {
                var manager = EnergyConnectionsReference.Instance.Manager;
                manager.OnEnergyConsumersConnected(_consumer, otherConsumer);
            }
        }
    }
}