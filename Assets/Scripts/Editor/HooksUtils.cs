using Hook;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class HooksUtils
    {
        [MenuItem("Connection/Remove hooks")]
        public static void RemoveHooks()
        {
            var hooks = Object.FindObjectsOfType<RetractableHook>();
            foreach (var hook in hooks)
            {
                var startTip = hook.Rope.StartPoint;
                var endTip = hook.Rope.EndPoint;
                if (Application.isPlaying)
                {
                    Object.Destroy(hook.gameObject);
                    Object.Destroy(startTip.gameObject);
                    Object.Destroy(endTip.gameObject);
                }
                else
                {
                    Object.DestroyImmediate(hook.gameObject);
                    Object.DestroyImmediate(startTip.gameObject);
                    Object.DestroyImmediate(endTip.gameObject);
                }
            }
        }
    }
}