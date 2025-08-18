using System;

namespace GameDrive.Room
{
    [Serializable]
    public class RoomInfo
    {
        public string roomId;
        public string processId;
        public int maxClients;
        public int clients;
        public bool locked;
        public bool isPrivate;
        public string name;
        public string metadataJsonString;
    }
}
