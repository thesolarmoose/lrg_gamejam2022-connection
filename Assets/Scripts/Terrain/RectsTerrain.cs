using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Utils.Extensions;

namespace Terrain
{
    public class RectsTerrain : MonoBehaviour
    {
        [SerializeField] private TerrainReference _referenceContainer;
        [SerializeField] private List<Rect> _rects;

        public ReadOnlyCollection<Rect> Rects => _rects.AsReadOnly();

        private void Start()
        {
            _referenceContainer.Terrain = this;
        }

        public Vector2 GetRandomPointInside()
        {
            return _rects.GetRandom().RandomPositionInside();
        }
    }
}