using System;

namespace GameDrive.Room
{
    [Serializable]
    public class JoinRoomReservation
    {
        public RoomReservation roomReservation;
        public RoomAddress roomAddress;
    }
}
