using UnityEngine;

namespace GameDriveSample
{
    public enum RoomPanels
    {
        Start,
        Client
    }
    public class RoomPanelController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _panelStart;

        [SerializeField]
        private GameObject _panelClient;

        public static RoomPanelController Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            SetPanel(RoomPanels.Start);
        }

        public void SetPanel(RoomPanels roomPanel)
        {
            _panelStart.SetActive(roomPanel == RoomPanels.Start);
            _panelClient.SetActive(roomPanel == RoomPanels.Client);
        }
    }
}