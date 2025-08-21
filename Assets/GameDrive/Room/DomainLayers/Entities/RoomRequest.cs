using System;

namespace GameDrive.Room
{
    [Serializable]
    public class RoomRequest
    {
        public string roomName;
        public RoomRequest(string roomName)
        {
            this.roomName = roomName;
        }
    }
}
