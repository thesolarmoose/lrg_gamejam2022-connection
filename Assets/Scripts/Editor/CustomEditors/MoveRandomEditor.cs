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

            var globalWorld = obj.GlobalBounds;
            var localWorld = obj.LocalBounds;
            var pos = obj.transform.position;
            localWorld.x += pos.x;
            localWorld.y += pos.y;

            var globalColor = new Color(0, 0.7f, 0, 0.2f);
            var localColor = new Color(0, 0.3f, 0.8f, 0.2f);
            Handles.DrawSolidRectangleWithOutline(globalWorld, globalColor, globalColor);
            Handles.DrawSolidRectangleWithOutline(localWorld, localColor, localColor);
        }
    }
}