using GameDrive.Room;
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace GameDriveSample
{
    public enum ClientRoomState
    {
        Empty,
        NotLogin,
        Available,
        Connecting,
        Disconnected,
    }

    public class ClientRoomStateManager : MonoBehaviour
    {

        public ClientRoomState CurrentState { get; private set; }
        public Action ActionStateChanged { get; set; }

        [SerializeField] private PanelClient _panelClient;

        private void Start()
        {
            UpdateClientState();
        }

        public void TriggerFindClientState()
        {
            UpdateClientState();
        }

        private void UpdateClientState()
        {
            SetCurrentStus(ClientRoomState.Empty);
            var client = _panelClient.ClientsHolder;
            if (client.Client.Player == null)
            {
                SetCurrentStus(ClientRoomState.NotLogin);
            }
            else
            {
                if (client.RoomClient.ColyseusRoom == null)
                {
                    if (client.RoomClient.LatestRoomSession != null)
                    {
                        SetCurrentStus(ClientRoomState.Disconnected);
                    }
                    else
                    {
                        SetCurrentStus(ClientRoomState.Available);
                    }
                }
                else
                {

                    if (client.RoomClient.ColyseusRoom.colyseusConnection.IsOpen)
                    {
                        if (client.IsDisconnected)
                        {
                            if (client.RoomClient.LatestRoomSession != null)
                            {
                                SetCurrentStus(ClientRoomState.Disconnected);
                            }
                            else
                            {
                                SetCurrentStus(ClientRoomState.Available);
                            }
                        }
                        else
                        {
                            SetCurrentStus(ClientRoomState.Connecting);
                        }
                    }
                    else
                    {
                        if (client.RoomClient.LatestRoomSession != null)
                        {
                            SetCurrentStus(ClientRoomState.Disconnected);
                        }
                        else
                        {
                            SetCurrentStus(ClientRoomState.Available);
                        }
                    }
                }
            }
        }

        private void SetCurrentStus(ClientRoomState clientRroomState)
        {
            CurrentState = clientRroomState;
            ActionStateChanged?.Invoke();
        }
    }
}