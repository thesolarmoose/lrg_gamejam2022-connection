using DefaultNamespace;
using Terrain;
using UnityEditor;
using UnityEngine;

namespace Editor.CustomEditors
{
    [CustomEditor(typeof(RectsTerrain))]
    public class TerrainEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            var terrain = (RectsTerrain) target;

            var rects = terrain.Rects;
            var previousSeed = Random.state;
            for (int i = 0; i < rects.Count; i++)
            {
                var rect = rects[i];
                Random.InitState(i);
                float r = 0.7f;
                float g = Random.Range(0.0f, 0.3f);
                float b = Random.Range(0.0f, 0.3f);
                float a = 0.4f;
                var color = new Color(r, g, b, a);
                Handles.DrawSolidRectangleWithOutline(rect, color, color);
            }
            Random.state = previousSeed;
        }
    }
}