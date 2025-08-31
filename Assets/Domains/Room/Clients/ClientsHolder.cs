using Colyseus;
using GameDrive.Room;
using System;

namespace GameDriveSample
{

    public class ClientsHolder
    {
        public GameDrive.Client Client { get; private set; }
        public RoomClient<MainState> RoomClient { get; private set; }
        public ClientsHolder(string id)
        {
            Client = new GameDrive.Client(id);
            RoomClient = new RoomClient<MainState>("cool-game-room", Client);
        }

        public bool IsDisconnected { get; private set; }
        public void SetDisconnected(bool disconnected)
        {
            IsDisconnected = disconnected;
        }

    }
}