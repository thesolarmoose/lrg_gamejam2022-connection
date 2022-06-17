using AI.Behaviours;
using UnityEditor;
using UnityEngine;

namespace Editor.CustomEditors
{
    [CustomEditor(typeof(MoveRandom))]
    public class MoveRandomEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            var obj = (MoveRandom) target;

            var localWorld = obj.LocalBounds;
            var pos = obj.transform.position;
            localWorld.x += pos.x;
            localWorld.y += pos.y;

            var localColor = new Color(0, 0.3f, 0.8f, 0.2f);
            Handles.DrawSolidRectangleWithOutline(localWorld, localColor, localColor);
        }
    }
}