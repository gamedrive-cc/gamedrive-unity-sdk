using System;

namespace GameDrive.Room
{
    [Serializable]
    public class RoomOptionsRequestion
    {
        public string roomName;
        public string optionsJsonString;
        public RoomOptionsRequestion(string roomName, string optionsJsonString)
        {
            this.roomName = roomName;
            this.optionsJsonString = optionsJsonString;
        }
    }
}
