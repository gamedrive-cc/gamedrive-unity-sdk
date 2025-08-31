using GameDrive.Room;
using System.Collections.Generic;
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

        private void OnButtonJoinOrCreateClicked()
        {
            var clients = _panelClient.ClientsHolder;
            var options = new Dictionary<string, object>();
            PanelLoadingController.Instance.ShowLoading(gameObject.name);
            clients.RoomClient.JoinOrCreate(options, (colyseusRoom) =>
            {
                PanelLoadingController.Instance.HideLoading(gameObject.name);

                _clientRoomStateManager.TriggerFindClientState();
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
            var options = new Dictionary<string, object>();

            PanelLoadingController.Instance.ShowLoading(gameObject.name);
            clients.RoomClient.Reconnect(clients.RoomClient.LatestRoomSession, (colyseusRoom) =>
            {
                PanelLoadingController.Instance.HideLoading(gameObject.name);

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
                _clientRoomStateManager.TriggerFindClientState();
            }, (error) =>
            {
                PanelLoadingController.Instance.HideLoading(gameObject.name);
                Debug.LogError(error);
            });
        }
    }
}