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

        private ColyseusRoom<MainState> _colyseusRoom;

        private void Start()
        {
            _clientRoomStateManager.ActionStateChanged += UpdateClientColyseusRoom;
        }


        ClientsHolder _currentClientsHolder;
        private void UpdateClientColyseusRoom()
        {
            if (_clientRoomStateManager.CurrentState == ClientRoomState.Disconnected)
            {
                _textTimerValue.text = "";
                _text.text = "";
            }

            _currentClientsHolder = _panelClient.ClientsHolder;
            ListentoMessage(_currentClientsHolder.RoomClient.ColyseusRoom);
        }

        private void ListentoMessage(ColyseusRoom<MainState> colyseusRoom)
        {
            //if the old message not null, unlisten
            if (_colyseusRoom == colyseusRoom)
            {
                return;
            }

            if (_colyseusRoom != null)
            {
                for (int i = 0; i < MessageTypes.Types.Length; i++)
                {
                    var messageType = MessageTypes.Types[i];
                    _colyseusRoom.State.OnChange -= OnMainState_OnChange;
                    _colyseusRoom.State.players.OnChange -= Players_OnChange;
                    _colyseusRoom.State.players.OnAdd -= Players_OnAdd;

                    _colyseusRoom.State.itemMap.OnChange -= ItemMap_OnChange;
                }

            }

            _colyseusRoom = colyseusRoom;

            if (_colyseusRoom != null)
            {
                for (int i = 0; i < MessageTypes.Types.Length; i++)
                {
                    var messageType = MessageTypes.Types[i];
                    _colyseusRoom.State.OnChange += OnMainState_OnChange;
                    _colyseusRoom.State.players.OnChange += Players_OnChange;
                    _colyseusRoom.State.players.OnAdd += Players_OnAdd;
                    _colyseusRoom.State.players.OnRemove += Players_OnRemove;

                    _colyseusRoom.State.itemMap.OnChange += ItemMap_OnChange;
                    _colyseusRoom.State.itemMap.OnAdd += ItemMap_OnAdd;
                    _colyseusRoom.State.itemMap.OnRemove += ItemMap_OnRemove;
                }

                UpdateRenderStateObject();
            }
            else
            {
                UpdateRenderStateObject();
            }
        }

        private void ItemMap_OnRemove(string key, Item value)
        {
            UpdateRenderStateObject();
        }

        private void ItemMap_OnAdd(string key, Item value)
        {
            UpdateRenderStateObject();
            value.OnChange += OnItemChanged;
        }

        private void Players_OnAdd(int key, Player value)
        {
            UpdateRenderStateObject();
            value.OnChange += OnPlayerChanged;
        }

        private void Players_OnRemove(int key, Player value)
        {
            UpdateRenderStateObject();
        }

        private void OnMainState_OnChange(System.Collections.Generic.List<Colyseus.Schema.DataChange> changes)
        {
            //update 
            UpdateTimer();
        }

        private void UpdateTimer()
        {
            _textTimerValue.text = "";
            if (_colyseusRoom == null)
            {
                return;
            }
            _textTimerValue.text = _colyseusRoom.State.timer.ToString();
        }


        private void ItemMap_OnChange(string key, Item value)
        {
            UpdateRenderStateObject();
        }

        private void Players_OnChange(int key, Player value)
        {
            UpdateRenderStateObject();
        }

        private void OnItemChanged(System.Collections.Generic.List<Colyseus.Schema.DataChange> changes)
        {
            UpdateRenderStateObject();
        }

        private void OnPlayerChanged(System.Collections.Generic.List<Colyseus.Schema.DataChange> changes)
        {
            UpdateRenderStateObject();
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