using IPA;
using IPA.Config;
using IPA.Config.Stores;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Settings;
using UnityEngine;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;

namespace OcuFix
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }

        [Init]
        public void Init(Config config, IPALogger logger)
        {
            Instance = this;
            Log = logger;

            Configuration.PluginConfig.Instance = config.Generated<Configuration.PluginConfig>();
            BSMLSettings.instance.AddSettingsMenu("OcuFix", "OcuFix.Views.Settings.bsml", Configuration.PluginConfig.Instance);
        }

        [OnStart]
        public void OnApplicationStart()
        {
            new GameObject("OcuFixController").AddComponent<OcuFixController>();
        }

        [OnExit]
        public void OnApplicationQuit()
        {
            /**/
        }
    }
}
