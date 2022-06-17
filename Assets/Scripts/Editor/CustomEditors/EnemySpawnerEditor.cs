using DefaultNamespace;
using UnityEditor;
using UnityEngine;

namespace Editor.CustomEditors
{
    [CustomEditor(typeof(EnemySpawner))]
    public class EnemySpawnerEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            var obj = (EnemySpawner) target;

            var rect = obj.SpawnSpace;
            var color = new Color(0, 0.7f, 0, 0.2f);
            Handles.DrawSolidRectangleWithOutline(rect, color, color);
        }
    }
}