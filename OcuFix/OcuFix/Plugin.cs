using IPA;
using IPA.Config;
using IPA.Config.Stores;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Settings;
using OcuFix.Configuration;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using IPALogger = IPA.Logging.Logger;

namespace OcuFix
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }

        private bool ShouldIgnore()
        {
            if (!XRSettings.loadedDeviceName.ToLower().Contains("oculus") && PluginConfig.Instance.EnableChecks)
            {
                Plugin.Log.Warn("Oculus vrmode not set, ignoring");
                return true;
            }

            if (Environment.CommandLine.ToLower().Contains("fpfc") && PluginConfig.Instance.EnableChecks)
            {
                Plugin.Log.Warn("FPFC mode enabled, ignoring");
                return true;
            }

            return false;
        }

        [Init]
        public void Init(Config config, IPALogger logger)
        {
            Instance = this;
            Log = logger;

            PluginConfig.Instance = config.Generated<PluginConfig>();
            BSMLSettings.instance.AddSettingsMenu("OcuFix", "OcuFix.Views.Settings.bsml", Configuration.PluginConfig.Instance);
        }

        [OnStart]
        public void OnApplicationStart()
        {
            if (ShouldIgnore())
                return;

            AswHelper.DisableAswWrapper();
            ProcessPriorityHelper.CheckPrioritiesWrapper();
        }

        [OnExit]
        public void OnApplicationQuit()
        {
            if (ShouldIgnore())
                return;

            if (PluginConfig.Instance.RestoreASW)
                AswHelper.RestoreAswWrapper();


        }
    }
}
