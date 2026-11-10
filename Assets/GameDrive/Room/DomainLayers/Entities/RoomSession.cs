using System;

namespace GameDrive.Room
{
    [Serializable]
    public class RoomSession
    {
        public string roomId;
        public string reconnectionToken;
        public string roomServiceAddress;
    }
}
