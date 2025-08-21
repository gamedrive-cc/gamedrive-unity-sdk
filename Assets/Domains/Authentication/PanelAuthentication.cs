using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace GameDriveSample
{
    enum ClientType
    {
        Default = 0,
        CustomClient = 1,
    }

    public class PanelAuthentication : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        [SerializeField]
        private TMPro.TMP_Dropdown _dropdownClientType;

        [SerializeField]
        private TMPro.TMP_InputField _inputFieldClientId;

        [SerializeField]
        private Button _buttonLoginWithDevice;

        private void OnEnable()
        {
            _buttonLoginWithDevice.onClick.AddListener(OnButtonSignWithDeviceClicked);
            _dropdownClientType.onValueChanged.AddListener(OnDropdownClientTypeChange);
            HideOrShowCLientIdInput();
        }

        private void OnDisable()
        {
            _buttonLoginWithDevice.onClick.RemoveListener(OnButtonSignWithDeviceClicked);
            _dropdownClientType.onValueChanged.RemoveListener(OnDropdownClientTypeChange);
        }

        private GameDrive.Client GetGameDriveClient()
        {
            if (_dropdownClientType.value == (int)ClientType.Default)
            {
                return GameDrive.Client.GetDefaultClient();
            }
            else
            {
                string clientId = _inputFieldClientId.text;
                if (string.IsNullOrEmpty(clientId))
                {
                    return GameDrive.Client.GetDefaultClient();
                }
                return new GameDrive.Client(clientId);
            }
        }

        private void OnDropdownClientTypeChange(int val)
        {
            HideOrShowCLientIdInput();
        }

        private void HideOrShowCLientIdInput()
        {
            _inputFieldClientId.gameObject.SetActive(_dropdownClientType.value != (int)ClientType.Default);
        }

        private void OnButtonSignWithDeviceClicked()
        {
            var client = GetGameDriveClient();
            //Show loading
            PanelLoadingController.Instance.ShowLoading(gameObject.name);

            GameDrive.Authentication.LoginWithDevice((GameDrive.LoggedInPlayer playerInfo) =>
            {
                PanelLoadingController.Instance.HideLoading(gameObject.name);

                PlayerDataHolder.Instance.SetPlayerData(playerInfo.player, client);
                PanelsController.Instance.SetPanel(Panels.PlayerAndScore);
            }, (GameDrive.ErrorSimple error) =>
            {
                PanelLoadingController.Instance.HideLoading(gameObject.name);

                Debug.LogError(error);
            }, client);
        }
    }

}