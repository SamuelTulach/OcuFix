using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcuFix
{
    internal static class AswHelper
    {
        private static DebugToolControl.ASWMode _lastMode = DebugToolControl.ASWMode.Unknown;
        
        private static void DisableAsw()
        {
            if (!Configuration.PluginConfig.Instance.DisableASW) 
                return;

            _lastMode = DebugToolControl.GetASWMode();
            if (_lastMode == DebugToolControl.ASWMode.Disabled) 
                return;

            var status = DebugToolControl.SetASWMode(DebugToolControl.ASWMode.Disabled);
            if (status)
                Plugin.Log.Info("Disabled ASW");
            else
                Plugin.Log.Error("Failed to disable ASW");
        }

        public static void DisableAswWrapper()
        {
            try
            {
                DisableAsw();
            }
            catch (Exception ex)
            {
                Plugin.Log.Error("Failed to check ASW! Exception: " + ex.Message);
            }
        }

        private static void RestoreAsw()
        {
            if (_lastMode == DebugToolControl.ASWMode.Unknown)
                return;

            var status = DebugToolControl.SetASWMode(_lastMode);
            if (status)
                Plugin.Log.Info("Restored ASW");
            else
                Plugin.Log.Error("Failed to restore ASW");
        }

        public static void RestoreAswWrapper()
        {
            try
            {
                RestoreAsw();
            }
            catch (Exception ex)
            {
                Plugin.Log.Error("Failed to restore ASW! Exception: " + ex.Message);
            }
        }
    }
}
