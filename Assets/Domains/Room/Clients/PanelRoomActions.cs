using GameDevWare.Serialization;
using GameDrive.Room;
using Newtonsoft.Json;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameDriveSample
{

    public class PanelRoomActions : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        [SerializeField] private Button _buttonJoinOrCreate;
        [SerializeField] private Button _buttonReconnect;
        [SerializeField] private Button _buttonDisconnect;
        [SerializeField] private Button _buttonLeave;

        [SerializeField] private PanelClient _panelClient;

        [SerializeField] private ClientRoomStateManager _clientRoomStateManager;
        [SerializeField] private PanelClientInfo _panelClientInfo;
        [SerializeField] private TMPro.TMP_InputField _inputOptions;

        private void OnEnable()
        {
            _clientRoomStateManager.ActionStateChanged += OnRoomStateChanged;
            _buttonJoinOrCreate.onClick.AddListener(OnButtonJoinOrCreateClicked);
            _buttonReconnect.onClick.AddListener(OnButtonReconnectClicked);
            _buttonDisconnect.onClick.AddListener(OnButtonDisconnectClicked);
            _buttonLeave.onClick.AddListener(OnButtonLeaveClicked);
            _panelClientInfo.ActionOnLoggedIn += TryToLoadCurrentRoom;

        }

        private void OnDisable()
        {
            _clientRoomStateManager.ActionStateChanged -= OnRoomStateChanged;
            _buttonJoinOrCreate.onClick.RemoveListener(OnButtonJoinOrCreateClicked);
            _buttonReconnect.onClick.RemoveListener(OnButtonReconnectClicked);
            _buttonDisconnect.onClick.RemoveListener(OnButtonDisconnectClicked);
            _buttonLeave.onClick.RemoveListener(OnButtonLeaveClicked);
            _panelClientInfo.ActionOnLoggedIn -= TryToLoadCurrentRoom;
        }

        private void Start()
        {
            TryToLoadCurrentRoom();
        }

        private void TryToLoadCurrentRoom()
        {
            if (_panelClientInfo.IsLoggedIn())
            {
                LoadCurrentRoom();
            }
        }

        private void OnRoomStateChanged()
        {
            _buttonJoinOrCreate.gameObject.SetActive(false);
            _buttonReconnect.gameObject.SetActive(false);
            _buttonDisconnect.gameObject.SetActive(false);
            _buttonLeave.gameObject.SetActive(false);

            switch (_clientRoomStateManager.CurrentState)
            {
                case ClientRoomState.Disconnected:
                    _buttonReconnect.gameObject.SetActive(true);
                    //_buttonLeave.gameObject.SetActive(true);
                    break;
                case ClientRoomState.Connecting:
                    _buttonDisconnect.gameObject.SetActive(true);
                    _buttonLeave.gameObject.SetActive(true);
                    break;
                case ClientRoomState.Available:
                    _buttonJoinOrCreate.gameObject.SetActive(true);
                    //PrepareRoom();
                    break;
            }
        }

        private void PrepareRoom()
        {
            var clients = _panelClient.ClientsHolder;

            //after call prepare room will make join or create room take less time to load for the first time
            PanelLoadingController.Instance.ShowLoading(gameObject.name);
            clients.RoomClient.PrepareRoom(() =>
            {
                PanelLoadingController.Instance.HideLoading(gameObject.name);
            }, (error) =>
            {
                PanelLoadingController.Instance.HideLoading(gameObject.name);
                Debug.LogError(error.ToString());
            });
        }

        private Dictionary<string, object> GetOptionsFromInput() {
            string value = _inputOptions.text;

            if (string.IsNullOrWhiteSpace(value))
            {
                return new Dictionary<string, object>();
            }

            try
            {
                return JsonConvert.DeserializeObject<Dictionary<string, object>>(value)
                    ?? new Dictionary<string, object>();
            }
            catch (JsonException ex)
            {
                Debug.LogError($"Invalid JSON options: {ex.Message}");
                return new Dictionary<string, object>();
            }
        }
        private void OnButtonJoinOrCreateClicked()
        {
            var clients = _panelClient.ClientsHolder;
            var options = GetOptionsFromInput();
            PanelLoadingController.Instance.ShowLoading(gameObject.name);
            clients.RoomClient.JoinOrCreate(options, (colyseusRoom) =>
            {
                PanelLoadingController.Instance.HideLoading(gameObject.name);

                clients.SetLatestRoomSession(clients.RoomClient.CreateRoomSession());
                _clientRoomStateManager.TriggerFindClientState();
                clients.SetDisconnected(true);
            }, (error) =>
            {
                Debug.LogError(error);
                PanelError.Instance.ShowErrorMessage(error.ToString(), 3.0f);
                PanelLoadingController.Instance.HideLoading(gameObject.name);
            });
        }

        private void OnButtonReconnectClicked()
        {
            var clients = _panelClient.ClientsHolder;
            var options = GetOptionsFromInput();

            PanelLoadingController.Instance.ShowLoading(gameObject.name);
            clients.RoomClient.Reconnect(clients.LatestRoomSession, (colyseusRoom) =>
            {
                PanelLoadingController.Instance.HideLoading(gameObject.name);

                clients.SetLatestRoomSession(clients.RoomClient.CreateRoomSession());
                clients.SetDisconnected(false);
                _clientRoomStateManager.TriggerFindClientState();
            }, (error) =>
            {
                Debug.LogError(error);
                PanelError.Instance.ShowErrorMessage(error.ToString(), 3.0f);
                PanelLoadingController.Instance.HideLoading(gameObject.name);
            });
        }

        private void OnButtonDisconnectClicked()
        {
            var clients = _panelClient.ClientsHolder;
            PanelLoadingController.Instance.ShowLoading(gameObject.name);
            clients.RoomClient.Disconnect(() =>
            {
                PanelLoadingController.Instance.HideLoading(gameObject.name);
                clients.SetDisconnected(true);
                _clientRoomStateManager.TriggerFindClientState();
            }, (error) =>
            {
                Debug.LogError(error); 
                PanelLoadingController.Instance.HideLoading(gameObject.name);
            });
        }

        private void OnButtonLeaveClicked()
        {
            var clients = _panelClient.ClientsHolder;
            PanelLoadingController.Instance.ShowLoading(gameObject.name);
            clients.RoomClient.Leave(() =>
            {
                PanelLoadingController.Instance.HideLoading(gameObject.name);
                clients.SetLatestRoomSession(null);
                _clientRoomStateManager.TriggerFindClientState();
            }, (error) =>
            {
                Debug.LogError(error);
                PanelLoadingController.Instance.HideLoading(gameObject.name);
            });
        }
        //

        private void LoadCurrentRoom()
        {
            var clients = _panelClient.ClientsHolder;
            PanelLoadingController.Instance.ShowLoading(gameObject.name, "Loading current room...");
            clients.RoomClient.GetPlayerCurrentRoom((RoomSession roomSession) =>
            {
                PanelLoadingController.Instance.HideLoading(gameObject.name);
                clients.SetLatestRoomSession(roomSession);
                _clientRoomStateManager.TriggerFindClientState();
            }, (error) =>
            {
                PanelLoadingController.Instance.HideLoading(gameObject.name);
                Debug.LogError(error);
            });
        }
    }
}