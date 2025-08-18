using System;
using System.Collections.Generic;

namespace GameDrive
{
    public class Client
    {
        public const string DefaultId = "defaultClient";

        private static Dictionary<string, Client> clientDict = new Dictionary<string, Client>();

        ClientTokensManager _clientTokensManager;
        ClientDeviceManager _clientDeviceManager;
        public ClientReadyToMakeRequest ClientReadyToMakeRequest { get; private set; }
        public string ClientId { get; private set; }

        public Player Player { get; private set; }

        public Action ActionErrorInvalidToken { get; private set; }

        public Client(string clientId)
        {
            if (clientDict.ContainsKey(clientId))
            {
                throw new System.Exception("Cannot create new Client with already exist clientId:" + clientId);
            }

            clientDict.Add(clientId, this);

            ClientId = clientId;
            _clientTokensManager = new ClientTokensManager(ClientId, this);
            _clientDeviceManager = new ClientDeviceManager(ClientId);
            ClientReadyToMakeRequest = new ClientReadyToMakeRequest(_clientTokensManager);
        }

        public static Client New(string id)
        {
            Client client = new Client(id);
            return client;
        }

        public ClientTokensManager GetTokenManager()
        {
            return _clientTokensManager;
        }

        public ClientDeviceManager GetDeviceManager()
        {
            return _clientDeviceManager;
        }

        private static Client defaultClient;

        public static Client GetDefaultClient()
        {
            if (defaultClient == null)
            {
                defaultClient = New(DefaultId);
            }

            return defaultClient;
        }

        public void SetPlayer(Player player)
        {
            Player = player;
        }


    }
}
