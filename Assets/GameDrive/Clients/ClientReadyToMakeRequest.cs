using System;
using System.Collections;

namespace GameDrive
{
    public class ClientReadyToMakeRequest
    {
        ClientTokensManager _clientTokensManager;
        public ClientReadyToMakeRequest(ClientTokensManager clientTokensManager)
        {
            _clientTokensManager = clientTokensManager;
        }

        public IEnumerator WaitReadyToRequest()
        {
            while (_clientTokensManager.ClientAutoRotateTokensManager.IsRotating)
            {
                yield return null;
            }
        }

    }
}
