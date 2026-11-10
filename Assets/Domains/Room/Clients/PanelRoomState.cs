
using Colyseus;
using UnityEngine;

namespace GameDriveSample
{
    public class PanelRoomState : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TextMeshProUGUI _textTimerValue;
        [SerializeField]
        private TMPro.TextMeshProUGUI _text;
        [SerializeField] private RectTransform _content;

        [SerializeField] PanelClient _panelClient;

        [SerializeField] ClientRoomStateManager _clientRoomStateManager;

        [SerializeField]
        private TMPro.TextMeshProUGUI _textRoomId;
        private Colyseus.Room<MainState> _colyseusRoom;

        private void Start()
        {
            _clientRoomStateManager.ActionStateChanged += UpdateClientColyseusRoom;
        }

        private void OnDestroy()
        {
            _clientRoomStateManager.ActionStateChanged -= UpdateClientColyseusRoom;

            if (_colyseusRoom != null)
            {
                _colyseusRoom.OnStateChange -= OnMainState_OnChange;
            }
        }


        ClientsHolder _currentClientsHolder;
        private void UpdateClientColyseusRoom()
        {
            _text.text = "";
            _textTimerValue.text = "";
            _textRoomId.text = "";

            _currentClientsHolder = _panelClient.ClientsHolder;
            ListentoMessage(_currentClientsHolder?.RoomClient?.ColyseusRoom);
            UpdateRoomIdText();
        }

        private void UpdateRoomIdText() { 
            _textRoomId.text = _currentClientsHolder?.RoomClient?.ColyseusRoom?.RoomId ?? "";
        }

        private void ListentoMessage(Colyseus.Room<MainState> colyseusRoom)
        {
            //if the old message not null, unlisten
            if (_colyseusRoom == colyseusRoom)
            {
                return;
            }

            if (_colyseusRoom != null)
            {
                _colyseusRoom.OnStateChange -= OnMainState_OnChange;
            }

            _colyseusRoom = colyseusRoom;

            if (_colyseusRoom != null)
            {
                _colyseusRoom.OnStateChange += OnMainState_OnChange;
            }

            UpdateTimer();
            UpdateRenderStateObject();
        }

        private void OnMainState_OnChange(MainState state, bool isFirstState)
        {
            //update 
            UpdateTimer();
            UpdateRenderStateObject();
        }

        private void UpdateTimer()
        {
            _textTimerValue.text = "";
            if (_colyseusRoom == null)
            {
                return;
            }
            _textTimerValue.text = _colyseusRoom.State.timer.ToString("G4");
        }

        private void UpdateRenderStateObject()
        {
            _text.text = "[Join room to see state]";
            if (_colyseusRoom == null)
            {
                return;
            }
            var newText = _colyseusRoom.State.ToString();
            _text.text += newText;
            var textHeight = _text.preferredHeight;
            _content.sizeDelta = new Vector2(_content.sizeDelta.x, textHeight);
        }
    }
}
