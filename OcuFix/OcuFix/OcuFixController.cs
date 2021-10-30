using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace OcuFix
{
    public class OcuFixController : MonoBehaviour
    {
        public static OcuFixController Instance { get; private set; }

        private Thread _checkThread;
        private bool _running;

        private void Awake()
        {
            if (Instance != null)
            {
                GameObject.DestroyImmediate(this);
                return;
            }
            GameObject.DontDestroyOnLoad(this);
            Instance = this;

            _running = true;
            _checkThread = new Thread(CheckThread);
            _checkThread.Start();
        }

        private void OnDestroy()
        {
            _running = false;
            _checkThread.Abort();
            
            if (Instance == this)
                Instance = null;
        }

        private void CheckThread()
        {
            Plugin.Log.Info("Started check thread");

            while (_running)
            {
                AswHelper.CheckAswWrapper();
                ProcessPriorityHelper.CheckPrioritiesWrapper();

                Thread.Sleep(20000);
            }
        }
    }
}
