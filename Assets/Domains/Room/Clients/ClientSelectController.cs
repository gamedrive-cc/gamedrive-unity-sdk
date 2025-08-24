using System;
using System.Collections.Generic;
using System.IO;
using Unity.Collections;
using UnityEngine;

namespace GameDriveSample
{
    public class ClientSelectController : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TMP_Dropdown _drodownClients;

        public static ClientSelectController Instance;

        public Action<ClientWrapper> ActionClientSelect { get; set; }

        Dictionary<string, ClientWrapper> _clients = new Dictionary<string, ClientWrapper>();
        public ClientWrapper CurrentClientWrapper { get; private set; }
        void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            SetClient(_drodownClients.value);
        }

        private void OnEnable()
        {
            _drodownClients.onValueChanged.AddListener(OnDropdownValueChanged);
        }

        private void OnDisable()
        {
            _drodownClients.onValueChanged.RemoveListener(OnDropdownValueChanged);
        }

        private void OnDropdownValueChanged(int value)
        {
            SetClient(value);
        }


        const string clientIdPrefix = "room_client_";

        private void SetClient(int id)
        {
            string clientId = clientIdPrefix + id.ToString();
            if (!_clients.ContainsKey(clientId))
            {
                _clients[clientId] = new ClientWrapper(clientId);
            }

            CurrentClientWrapper = _clients[clientId];
            ActionClientSelect?.Invoke(CurrentClientWrapper);
        }
    }
}