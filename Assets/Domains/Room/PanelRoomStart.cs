using UnityEngine;
using UnityEngine.UI;

namespace GameDriveSample
{
    public class PanelRoomStart : MonoBehaviour
    {
        [SerializeField] private Button _buttonStart;

        private void OnEnable()
        {
            _buttonStart.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _buttonStart.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            RoomPanelController.Instance.SetPanel(RoomPanels.Client);
        }
    }
}