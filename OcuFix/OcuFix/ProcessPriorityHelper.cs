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
        private static ProcessPriorityClass _targetGamePriorityClass = ProcessPriorityClass.High;
        private static void SwapGame()
        {
            var currentProcess = Process.GetCurrentProcess();
            if (currentProcess.PriorityClass != _targetGamePriorityClass)
            {
                (currentProcess.PriorityClass, _targetGamePriorityClass) = (_targetGamePriorityClass, currentProcess.PriorityClass);
                Plugin.Log.Info($"Game priority set");
            }
        }

        private static ProcessPriorityClass _targetRuntimePriorityClass = ProcessPriorityClass.AboveNormal;
        private static void SwapRuntime()
        {
            var runtimeProcesses = Process.GetProcessesByName("oculus-platform-runtime");
            if (runtimeProcesses.Length == 0)
                return;

            var runtimeProcess = runtimeProcesses[0];
            if (runtimeProcess.PriorityClass != _targetRuntimePriorityClass)
            {
                (runtimeProcess.PriorityClass, _targetRuntimePriorityClass) = (_targetRuntimePriorityClass, runtimeProcess.PriorityClass);
                Plugin.Log.Info("Runtime priority set");
            }
        }

        private static ProcessPriorityClass _targetServerPriorityClass = ProcessPriorityClass.AboveNormal;
        private static void SwapServer()
        {
            var serverProcesses = Process.GetProcessesByName("OVRServer_x64");
            if (serverProcesses.Length == 0)
                return;

            var serverProcess = serverProcesses[0];
            if (serverProcess.PriorityClass != _targetServerPriorityClass)
            {
                (serverProcess.PriorityClass, _targetServerPriorityClass) = (_targetServerPriorityClass, serverProcess.PriorityClass);
                Plugin.Log.Info("Server priority set");
            }
        }
        
        private static void SwapPriorities()
        {
            if (Configuration.PluginConfig.Instance.GamePriority)
                SwapGame();

            if (Configuration.PluginConfig.Instance.SetPriority)
            {
                SwapRuntime();
                SwapServer();
            }
        }

        public static void SwapPrioritiesWrapper()
        {
            try
            {
                SwapPriorities();
            }
            catch (Exception ex)
            {
                Plugin.Log.Error("Failed to set priority! Exception: " + ex.Message);
            }
        }
    }
}
