using UnityEngine;
using Utils;

namespace EnergySourcesConnections
{
    [CreateAssetMenu(fileName = "EnergyConnectionsReference", menuName = "EnergyConnectionsReference", order = 0)]
    public class EnergyConnectionsReference : ScriptableObjectSingleton<EnergyConnectionsReference>
    {
        private EnergyConnectionsManager _manager;

        public EnergyConnectionsManager Manager
        {
            get => _manager;
            set => _manager = value;
        }
    }
}