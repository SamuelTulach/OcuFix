using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcuFix
{
    internal static class ProcessPriorityHelper
    {
        private static void CheckGame()
        {
            var currentProcess = Process.GetCurrentProcess();
            if (currentProcess.PriorityClass != ProcessPriorityClass.High)
            {
                currentProcess.PriorityClass = ProcessPriorityClass.High;
                Plugin.Log.Info("Game priority set to high");
            }
        }

        private static void CheckRuntime()
        {
            var runtimeProcesses = Process.GetProcessesByName("oculus-platform-runtime");
            if (runtimeProcesses.Length == 0)
                return;

            var runtimeProcess = runtimeProcesses[0];
            if (runtimeProcess.PriorityClass != ProcessPriorityClass.AboveNormal)
            {
                runtimeProcess.PriorityClass = ProcessPriorityClass.AboveNormal;
                Plugin.Log.Info("Runtime priority set to be above normal");
            }
        }

        private static void CheckServer()
        {
            var serverProcesses = Process.GetProcessesByName("OVRServer_x64");
            if (serverProcesses.Length == 0)
                return;

            var serverProcess = serverProcesses[0];
            if (serverProcess.PriorityClass != ProcessPriorityClass.AboveNormal)
            {
                serverProcess.PriorityClass = ProcessPriorityClass.AboveNormal;
                Plugin.Log.Info("Server priority set to be above normal");
            }
        }
        
        private static void CheckPriorities()
        {
            if (Configuration.PluginConfig.Instance.GamePriority)
                CheckGame();

            if (Configuration.PluginConfig.Instance.SetPriority)
            {
                CheckRuntime();
                CheckServer();
            }
        }

        public static void CheckPrioritiesWrapper()
        {
            try
            {
                CheckPriorities();
            }
            catch (Exception ex)
            {
                Plugin.Log.Error("Failed to set priority! Exception: " + ex.Message);
            }
        }
    }
}
