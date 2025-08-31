using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameDriveSample
{
    public class PanelClientInfo : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        [SerializeField]
        private Button _loginButton;

        [SerializeField]
        private PanelPlayerInfo _panelPlayerInfo;

        [SerializeField]
        private PanelClient _panelClient;

        [SerializeField] private ClientRoomStateManager _clientRoomStateManager;

        public Action ActionOnLoggedIn { get; set; }

        private void OnEnable()
        {
            _loginButton.onClick.AddListener(OnButtonLoginClick);
        }

        private void OnDisable()
        {
            _loginButton.onClick.RemoveListener(OnButtonLoginClick);
        }

        private void Start()
        {
            UpdateClientSelected();
        }
        private void UpdateClientSelected()
        {
            _loginButton.gameObject.SetActive(false);
            _panelPlayerInfo.gameObject.SetActive(false);

            if (IsLoggedIn())
            {
                //Show loggined button
                ShowPlayerInfo();
            }
            else
            {
                _loginButton.gameObject.SetActive(true);
            }
        }

        public bool IsLoggedIn()
        {
            var client = _panelClient.ClientsHolder;
            return client.Client.Player != null;

        }
        private void ShowPlayerInfo()
        {
            _panelPlayerInfo.gameObject.SetActive(true);
            var clientHelper = _panelClient.ClientsHolder;
            _panelPlayerInfo.SetPlayerInfo(clientHelper.Client.Player);
        }

        private void OnButtonLoginClick()
        {
            var clients = _panelClient.ClientsHolder;
            PanelLoadingController.Instance.ShowLoading(gameObject.name);
            GameDrive.Authentication.LoginWithDevice((loggedInPlayer) =>
            {
                PanelLoadingController.Instance.HideLoading(gameObject.name);
                ShowPlayerInfo();
                _loginButton.gameObject.SetActive(false);
                _clientRoomStateManager.TriggerFindClientState();
                ActionOnLoggedIn?.Invoke();
            }, (error) =>
            {
                PanelLoadingController.Instance.HideLoading(gameObject.name);
                Debug.LogError(error);
                PanelError.Instance.ShowErrorMessage(error.ToString(), 3.0f);
            }, clients.Client);
        }
    }
}