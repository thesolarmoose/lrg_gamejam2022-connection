using UnityEngine;
using UnityEngine.Tilemaps;

namespace Terrain
{
    public class TileMapRendererDisabler : MonoBehaviour
    {
        [SerializeField] private TilemapRenderer _renderer;

        private void Awake()
        {
            _renderer.enabled = false;
        }
    }
}