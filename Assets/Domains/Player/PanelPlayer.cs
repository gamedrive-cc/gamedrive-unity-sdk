using GameDriveSample;
using UnityEngine;
using UnityEngine.UI;

public class PanelPlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    public TMPro.TMP_Text playerName;

    [SerializeField]
    public TMPro.TMP_Text playerId;

    [SerializeField]
    public TMPro.TMP_Text playerNumber;


    [SerializeField]
    public Button buttonLogout;

    void Start()
    {
        UpdatePlayerInfo();
    }

    private void OnEnable()
    {
        UpdatePlayerInfo();
        PlayerDataHolder.Instance.ActionPlayerDataUpdated += UpdatePlayerInfo;
        buttonLogout.onClick.AddListener(OnButtonLogoutClick);
    }

    private void OnDisable()
    {
        PlayerDataHolder.Instance.ActionPlayerDataUpdated -= UpdatePlayerInfo;
        buttonLogout.onClick.RemoveListener(OnButtonLogoutClick);
    }

    private void UpdatePlayerInfo()
    {
        var player = PlayerDataHolder.Instance.Player;
        if (player != null)
        {
            playerName.text = player.name;
            playerId.text = player._id;
            playerNumber.text = player.numberId + "";
        }
    }

    private void OnButtonLogoutClick()
    {
        GameDrive.Authentication.Logout(PlayerDataHolder.Instance.Client);
        PanelsController.Instance.SetPanel(Panels.Authentication);
    }
}
