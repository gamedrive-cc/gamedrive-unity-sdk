using GameDriveSample;
using System;
using UnityEngine;

public class PanelCurrentRoom : MonoBehaviour
{
    [SerializeField]
    private TMPro.TMP_Text _textCurrentRoomValue;

    [SerializeField]
    private GameObject mainPanel;

    [SerializeField]
    private PanelClient _panelClient;

    [SerializeField]
    private PanelClientInfo _panelClientInfo;

    private void OnEnable()
    {
        _panelClientInfo.ActionOnLoggedIn += LoadAndShowCurrentRoom;
    }

    private void OnDisable()
    {
        _panelClientInfo.ActionOnLoggedIn -= LoadAndShowCurrentRoom;
    }

    private void Start()
    {
        _panelClient.ClientsHolder.ConnectionChanged += LoadAndShowCurrentRoom;
    }

    public void LoadAndShowCurrentRoom()
    {
        if (_panelClientInfo.IsLoggedIn())
        {
            _textCurrentRoomValue.text = ".....";
            this.mainPanel.SetActive(true);
            _panelClient.ClientsHolder.RoomClient.GetPlayerCurrentRoom((colyseusRoom) =>
            {
                if (colyseusRoom != null)
                {
                    _textCurrentRoomValue.text = JsonUtility.ToJson(colyseusRoom);
                    //mainPanel.SetActive(true);
                }
                else
                {
                    _textCurrentRoomValue.text = "No current room";
                    //mainPanel.SetActive(false);
                    Debug.Log("No current room found for the player:" + _panelClient.ClientsHolder.Client.Player._id);
                }
            }, (err) =>
            {
                Debug.LogError("Error occurred while fetching current room: " + err.ToString());
            });
        }
        else {
            this.mainPanel.SetActive(false);
        }
    }

    public void ButtonReloadCurrentRoomClicked() {
        LoadAndShowCurrentRoom();
    }
}
