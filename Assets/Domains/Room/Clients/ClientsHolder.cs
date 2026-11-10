using Colyseus;
using GameDrive.Room;
using System;

namespace GameDriveSample
{

    public class ClientsHolder
    {
        public GameDrive.Client Client { get; private set; }
        public RoomClient<MainState> RoomClient { get; private set; }
        public Action ConnectionChanged { get; set; }
        public ClientsHolder(string id)
        {
            Client = new GameDrive.Client(id);
            RoomClient = new RoomClient<MainState>("cool-game-room", Client);
        }

        public RoomSession LatestRoomSession { get; private set; }
        public void SetLatestRoomSession(RoomSession roomSession)
        {
            LatestRoomSession = roomSession;
        }

        public bool IsDisconnected { get; private set; }
        public void SetDisconnected(bool disconnected)
        {
            IsDisconnected = disconnected;
            ConnectionChanged?.Invoke();
        }

    }
}