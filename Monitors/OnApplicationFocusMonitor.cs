using System;
using UnityEngine;

namespace GameDrive
{
    public class OnApplicationFocusMonitor : MonoBehaviour
    {
        public static Action<bool> ActionApplicationFocus { get; set; }

        void OnApplicationFocus(bool focus)
        {
            ActionApplicationFocus?.Invoke(focus);
        }
    }
}
