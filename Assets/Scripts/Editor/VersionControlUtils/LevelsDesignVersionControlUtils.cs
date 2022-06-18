﻿using System.IO;
using UnityEditor;

namespace Editor.VersionControlUtils
{
    public static class LevelsDesignVersionControlUtils
    {
        [MenuItem("TSM/Version control/Pull")]
        public static void Pull()
        {
            RunScript("./_pull.sh");
        }
        
        [MenuItem("TSM/Version control/Push")]
        public static void Push()
        {
            RunScript("./_push.sh");
        }

        private static void RunScript(string script)
        {
            string workingDir = $"{Directory.GetCurrentDirectory()}/_git_scripts/";
            string scriptName = "C:/Program Files/Git/git-bash.exe";  // TODO find a better way to get this

            var process = new System.Diagnostics.ProcessStartInfo
            {
                UseShellExecute = true,
                WorkingDirectory = workingDir,
                FileName = scriptName,
                Arguments = script,
            };

            var cmd =  System.Diagnostics.Process.Start(process);
            cmd.WaitForExit();
        }
    }
}