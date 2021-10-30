using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcuFix
{
    internal static class AswHelper
    {
        private static void CheckAsw()
        {
            if (!Configuration.PluginConfig.Instance.DisableASW) 
                return;

            if (!DebugToolControl.ASWEnabled()) 
                return;

            var status = DebugToolControl.DisableASW();
            if (status)
                Plugin.Log.Info("Disabled ASW");
            else
                Plugin.Log.Error("Failed to disable ASW");
        }

        public static void CheckAswWrapper()
        {
            try
            {
                CheckAsw();
            }
            catch (Exception ex)
            {
                Plugin.Log.Error("Failed to check ASW! Exception: " + ex.Message);
            }
        }
    }
}
