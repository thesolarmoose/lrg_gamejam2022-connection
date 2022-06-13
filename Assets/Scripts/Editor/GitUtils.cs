using UnityEditor;

namespace Editor
{
    public static class GitUtils
    {
        [MenuItem("TSM/Version control/Pull")]
        public static void Pull()
        {
            Utils.Editor.GitUtils.Restore();
            Utils.Editor.GitUtils.Pull();
        }
    }
}