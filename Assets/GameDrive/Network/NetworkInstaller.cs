using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDrive.Network
{
    public class NetworkInstaller
    {
        private static MonoBehaviour _mono = null;
        public static MonoBehaviour Mono
        {
            get
            {
                if (_mono == null)
                {
                    throw new System.Exception("Please add GameDriveInstaller to your scene before calling any request");
                }

                return _mono;
            }
        }
        // Start is called before the first frame update
        public static void Install(GameObject gameOject)
        {
            MonoBehaviour newMono = gameOject.AddComponent<NetworkMono>();
            _mono = newMono;
        }
    }

    public class NetworkMono : MonoBehaviour
    {

    }
}
