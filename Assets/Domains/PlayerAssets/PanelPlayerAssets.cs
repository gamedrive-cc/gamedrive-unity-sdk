using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameDriveSample
{
    [Serializable]
    public class PlayerAssetData
    {
        public string status;
        public string coin;
    }

    public class PanelPlayerAssets : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TMP_InputField _inputFieldCoin;

        [SerializeField]
        private TMPro.TMP_InputField _inputFieldStatus;

        [SerializeField]
        private Button _buttonCreateOrUpdateAsset;

        [SerializeField]
        private TMPro.TMP_Text _textPlayerAssets;

        private PlayerAssetData _playerAssetsData;

        private void Start()
        {
            PlayerDataHolder.Instance.ActionPlayerDataUpdated += ActionPlayerDataUpdatedCalled;
        }

        private void OnDestroy()
        {
            PlayerDataHolder.Instance.ActionPlayerDataUpdated -= ActionPlayerDataUpdatedCalled;
        }

        private void OnEnable()
        {
            _buttonCreateOrUpdateAsset.onClick.AddListener(OnButtonSetScoreClicked);
            LoadGetPlayerAssets();
        }

        private void OnDisable()
        {
            _buttonCreateOrUpdateAsset.onClick.RemoveListener(OnButtonSetScoreClicked);
        }

        private void OnButtonSetScoreClicked()
        {
            //call game drive
            if (PlayerDataHolder.Instance.Client == null)
            {
                Debug.LogError("client not found");
                return;
            }

            if (string.IsNullOrEmpty(_inputFieldStatus.text) || string.IsNullOrEmpty(_inputFieldCoin.text))
            {
                PanelError.Instance.ShowErrorMessage("Please insert status and coin value", 3.0f);
                return;
            }

            //For default client, each Endpoint object should be created once and reuse. But in this case we re-create for dynamic Client
            GameDrive.Endpoint createOrUpdatePlayerAssetsEndpoint = new GameDrive.Endpoint("createOrUpdatePlayerAssets", PlayerDataHolder.Instance.Client);

            string status = _inputFieldStatus.text;

            int coin;
            if (!int.TryParse(_inputFieldCoin.text, out coin))
            {
                Debug.LogError("_inputFieldCoin's value is not int");
                return;
            }


            //Since our Endpoints on the web console definded 2 params be status is the first and coint is the second. We need to set arguments follow by the order
            createOrUpdatePlayerAssetsEndpoint.SetArguments(status, coin);

            PanelLoadingController.Instance.ShowLoading(gameObject.name);
            createOrUpdatePlayerAssetsEndpoint.SendRequest<PlayerAssetData>((data) =>
            {
                SetPlayerAssetData(data);
                PanelLoadingController.Instance.HideLoading(gameObject.name);
            }, (GameDrive.ErrorSimple error) =>
            {
                PanelLoadingController.Instance.HideLoading(gameObject.name);
                Debug.LogError(error);
            });
        }

        private void ActionPlayerDataUpdatedCalled()
        {
            ResetTextInput();
            SetPlayerAssetData(null);
        }

        private void ResetTextInput()
        {
            _inputFieldStatus.text = "";
            _inputFieldCoin.text = "";
        }

        private void LoadGetPlayerAssets()
        {
            if (PlayerDataHolder.Instance.Client == null)
            {
                Debug.LogError("client not found");
                return;
            }

            //For default client, each Endpoint object should be created once and reuse. But in this case we re-create for dynamic Client
            GameDrive.Endpoint getPlayerAssetsEndpoint = new GameDrive.Endpoint("getPlayerAssets", PlayerDataHolder.Instance.Client);
            PanelLoadingController.Instance.ShowLoading(gameObject.name);
            getPlayerAssetsEndpoint.SendRequest<PlayerAssetData>((data) =>
            {
                SetPlayerAssetData(data);
                PanelLoadingController.Instance.HideLoading(gameObject.name);
            }, (GameDrive.ErrorSimple error) =>
            {
                PanelLoadingController.Instance.HideLoading(gameObject.name);
                Debug.LogError(error);
            });
        }

        private void SetPlayerAssetData(PlayerAssetData playerAssetData)
        {
            _playerAssetsData = playerAssetData;
            UpdateGameObjectNotFound();

        }

        private void UpdateGameObjectNotFound()
        {
            if (_playerAssetsData == null)
            {
                _textPlayerAssets.text = "Player's asset not created yet";
            }
            else
            {
                _textPlayerAssets.text = JsonUtility.ToJson(_playerAssetsData);
            }
        }
    }
}