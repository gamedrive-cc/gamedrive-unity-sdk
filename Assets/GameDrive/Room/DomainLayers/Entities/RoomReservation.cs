using System;

namespace GameDrive.Room
{
    [Serializable]
    public class RoomReservation
    {
        public string name;
        public string sessionId;
        public string roomId;
        public string processId;
        public string reconnectionToken;
    }
}
