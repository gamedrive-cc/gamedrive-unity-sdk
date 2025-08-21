using System;
using UnityEngine;
using System.Collections.Generic;

namespace GameDrive
{
    public class GUIDManager
    {
        private static GUIDManager _instance;
        public static GUIDManager Instance()
        {
            if (_instance == null)
            {
                _instance = new GUIDManager();
            }
            return _instance;
        }
        const string guidKey = "GameDriveDeiveGUID";

        private string _guid;
    
        Dictionary<string, string> clientGuidDict = new Dictionary<string, string>();
        public string GetGUID(string clientId)
        {
            if (!clientGuidDict.ContainsKey(clientId))
            {
                string clientGuidKey = clientId + guidKey;
                _guid = PlayerPrefs.GetString(clientGuidKey);
                if (string.IsNullOrEmpty(_guid))
                {
                    _guid = Guid.NewGuid().ToString();
                    PlayerPrefs.SetString(clientGuidKey, _guid);
                }

                clientGuidDict.Add(clientGuidKey, _guid);
            }

            return _guid;
        }

    }
}