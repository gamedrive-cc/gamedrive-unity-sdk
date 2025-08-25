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

        public Action ActionClientSelect { get; set; }

        Dictionary<string, ClientHolder> _clients = new Dictionary<string, ClientHolder>();
        public ClientHolder CurrentClient { get; private set; }
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
                _clients[clientId] = new ClientHolder(clientId);
            }

            CurrentClient = _clients[clientId];
            ActionClientSelect?.Invoke();
        }
    }
}