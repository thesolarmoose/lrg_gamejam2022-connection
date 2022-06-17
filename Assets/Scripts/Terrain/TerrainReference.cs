using UnityEngine;

namespace Terrain
{
    
    /// <summary>
    /// Yes, I know mesh navigation exists
    /// </summary>
    [CreateAssetMenu(fileName = "TerrainReference", menuName = "Terrain/TerrainReference", order = 0)]
    public class TerrainReference : ScriptableObject
    {
        private RectsTerrain _terrain;

        public RectsTerrain Terrain
        {
            get => _terrain;
            set => _terrain = value;
        }
    }
}