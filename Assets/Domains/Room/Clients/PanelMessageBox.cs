using Colyseus;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

namespace GameDriveSample
{
    class Message
    {
        string type;
        string message;
        public Message(string type, string message)
        {
            this.type = type;
            this.message = message;
        }

        public override string ToString()
        {
            return "Type:" + type + ", Message:" + message;
        }
    }

    [Serializable]
    class MessageObject
    {
        public string fromPlayerId;
        public string message;

        public override string ToString()
        {
            return "fromPlayerId:" + this.fromPlayerId + ", message:" + this.message;
        }
    }

    public class PanelMessageBox : MonoBehaviour
    {

        [SerializeField] private TMPro.TextMeshProUGUI _textMessages;
        [SerializeField] private RectTransform _content;
        [SerializeField] private Scrollbar _scrollbar;

        List<Message> messages = new List<Message>();

        [SerializeField] PanelClient _panelClient;

        private ColyseusRoom<MainState> _colyseusRoom;
        [SerializeField] ClientRoomStateManager _clientRoomStateManager;


        private void Start()
        {
            UpdateRenderMessage();
            _currentClientsHolder = _panelClient.ClientsHolder;
            _clientRoomStateManager.ActionStateChanged += OnColyseusRoomChanged;
        }


        ClientsHolder _currentClientsHolder;
        private void OnColyseusRoomChanged()
        {
            if (_clientRoomStateManager.CurrentState == ClientRoomState.Disconnected)
            {
                _textMessages.text = "";
            }
            ListentoMessage(_currentClientsHolder.RoomClient.ColyseusRoom);
        }

        private void ListentoMessage(ColyseusRoom<MainState> colyseusRoom)
        {
            //if the old message not null, unlisten
            if (_colyseusRoom == colyseusRoom)
            {
                UpdateRenderMessage();
                return;
            }

            if (_colyseusRoom != null)
            {
                for (int i = 0; i < MessageTypes.Types.Length; i++)
                {
                    var messageType = MessageTypes.Types[i];
                    _colyseusRoom.RemoveOnMessage<string>(messageType);
                }

            }

            _colyseusRoom = colyseusRoom;

            if (_colyseusRoom != null)
            {
                for (int i = 0; i < MessageTypes.Types.Length; i++)
                {
                    var messageType = MessageTypes.Types[i];
                    _colyseusRoom.OnMessage<MessageObject>(messageType, (MessageObject message) =>
                    {
                        //
                        messages.Add(new Message(messageType, message.ToString()));
                        UpdateRenderMessage();
                    });
                }

            }
            else
            {
                UpdateRenderMessage();
            }
        }

        private void UpdateRenderMessage()
        {
            _textMessages.text = "[Join room to see message]";
            if (_colyseusRoom == null)
            {
                return;
            }
            var messages = this.messages;
            string messageStr = "";
            for (int i = 0; i < messages.Count; i++)
            {
                messageStr += messages[i].ToString();
                messageStr += "\n";
            }

            _textMessages.text = messageStr;
            var textHeight = _textMessages.preferredHeight;
            _content.sizeDelta = new Vector2(_content.sizeDelta.x, textHeight);
            _scrollbar.value = 0;
        }
    }
}