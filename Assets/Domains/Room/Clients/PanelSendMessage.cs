using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameDriveSample
{

    [Serializable]
    public class MessageBodyMovePlayer
    {
        public int x;
        public int y;
    }

    [Serializable]
    public class MessageBodyAddItem
    {
        public string id;
        public string name;
        public int level;
    }

    [Serializable]
    public class MessageBodyDM
    {
        public string toPlayerId;
        public string message;
    }

    public class PanelSendMessage : MonoBehaviour
    {
        [SerializeField] private Button _buttonSent;

        [SerializeField] private TMPro.TMP_Dropdown _dropdownMessageType;

        [SerializeField] private TMPro.TMP_InputField _inputMessage;

        [SerializeField] private PanelClient _panelClient;

        [SerializeField] private ClientRoomStateManager _clientRoomStateManager;

        private void Awake()
        {
            InitMessageTypeDropdown();
        }

        private void InitMessageTypeDropdown()
        {
            _dropdownMessageType.options = new System.Collections.Generic.List<TMPro.TMP_Dropdown.OptionData>();
            var newOptions = new System.Collections.Generic.List<TMPro.TMP_Dropdown.OptionData>();
            for (int i = 0; i < MessageTypes.Types.Length; i++)
            {
                TMPro.TMP_Dropdown.OptionData options = new TMPro.TMP_Dropdown.OptionData();
                options.text = MessageTypes.Types[i];
                newOptions.Add(options);
            }
            _dropdownMessageType.AddOptions(newOptions);
        }

        private void OnEnable()
        {
            _clientRoomStateManager.ActionStateChanged += OnStateChange;
            OnStateChange();

            _dropdownMessageType.onValueChanged.AddListener(OnMessageTypeChanged);

            _buttonSent.onClick.AddListener(ButtonSendClicked);
        }

        private void OnDisable()
        {
            _clientRoomStateManager.ActionStateChanged -= OnStateChange;
            _dropdownMessageType.onValueChanged.RemoveListener(OnMessageTypeChanged);

            _buttonSent.onClick.RemoveListener(ButtonSendClicked);
        }

        private void Start()
        {
            OnStateChange();
            OnMessageTypeChanged(_dropdownMessageType.value);
        }

        private void OnStateChange()
        {
            _buttonSent.interactable = _clientRoomStateManager.CurrentState == ClientRoomState.Connecting;
        }

        string _messageType = string.Empty;
        private void OnMessageTypeChanged(int val)
        {
            _messageType = MessageTypes.Types[val];

            string str = "";
            switch (_messageType)
            {
                case MessageTypes.MESSAGE_TYPE_MOVE_PLAYER:
                    str = JsonUtility.ToJson(new MessageBodyMovePlayer());
                    break;
                case MessageTypes.MESSAGE_TYPE_ADD_ITEM:
                    str = JsonUtility.ToJson(new MessageBodyAddItem());
                    break;
                case MessageTypes.MESSAGE_TYPE_REMOVE_ITEM:
                    str = "itemId";
                    break;
                case MessageTypes.MESSAGE_CHAT_DM:
                    str = JsonUtility.ToJson(new MessageBodyDM());
                    break;
                case MessageTypes.MESSAGE_CHAT_BC:
                    str = "broadcase message";
                    break;
            }
            SetMessageInput(str);
        }

        private void SetMessageInput(string str)
        {
            _inputMessage.text = str;
        }

        private async void ButtonSendClicked()
        {
            var message = _inputMessage.text;
            System.Object messageObj = null;
            switch (_messageType)
            {
                case MessageTypes.MESSAGE_TYPE_MOVE_PLAYER:
                    messageObj = JsonUtility.FromJson<MessageBodyMovePlayer>(message);
                    break;
                case MessageTypes.MESSAGE_TYPE_ADD_ITEM:
                    var addItemBody = JsonUtility.FromJson<MessageBodyAddItem>(message);
                    if (string.IsNullOrEmpty(addItemBody.id))
                    {
                        PanelError.Instance.ShowErrorMessage("item string can not be empty or null", 3.0f);
                        Debug.LogError("item string can not be empty or null");
                        return;
                    }
                    messageObj = addItemBody;
                    break;
                case MessageTypes.MESSAGE_TYPE_REMOVE_ITEM:
                    if (string.IsNullOrEmpty(message))
                    {
                        PanelError.Instance.ShowErrorMessage("item string can not be empty or null", 3.0f);
                        Debug.LogError("item string can not be empty or null");
                        return;
                    }
                    messageObj = message;
                    break;
                case MessageTypes.MESSAGE_CHAT_DM:
                    messageObj = JsonUtility.FromJson<MessageBodyDM>(message);
                    break;
                case MessageTypes.MESSAGE_CHAT_BC:
                    messageObj = message;
                    break;
            }

            try
            {
                var colyseusRoom = _panelClient.ClientsHolder.RoomClient.ColyseusRoom;
                await colyseusRoom.Send(_messageType, messageObj);
            }
            catch (System.Exception err)
            {
                Debug.LogError(err.ToString());
            }
        }
    }
}