using UnityEngine;

namespace GameDriveSample
{
    public class PanelClient : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public ClientsHolder ClientsHolder { get; private set; }
        public void SetClientsHolder(ClientsHolder clientsHolder)
        {
            ClientsHolder = clientsHolder;
        }
    }
}