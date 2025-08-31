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

        [SerializeField]
        private PanelClient[] _panelClients;

        Dictionary<string, ClientsHolder> _clients = new Dictionary<string, ClientsHolder>();
        void Awake()
        {
            Instance = this;
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
            for (int i = 0; i < _panelClients.Length; i++)
            {
                _panelClients[i].gameObject.SetActive(false);
            }

            if (!_clients.ContainsKey(clientId))
            {
                _clients[clientId] = new ClientsHolder(clientId);
                _panelClients[id].SetClientsHolder(_clients[clientId]);
            }

            _panelClients[id].gameObject.SetActive(true);
        }
    }
}