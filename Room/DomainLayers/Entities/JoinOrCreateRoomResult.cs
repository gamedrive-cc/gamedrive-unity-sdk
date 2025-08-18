using System;

namespace GameDrive.Room
{
    [Serializable]
    public class JoinOrCreateRoomResult
    {
        public bool isCreate;
        public CreateRoomResult createRoomResult;

        public JoinRoomReservation joinReservation;
    }

    [Serializable]
    public class JoinOrCreateRoomRequest
    {
        public bool isCreate;
        public CreateRoomResult createRoomResult;

        public JoinRoomReservation joinReservation;

    }
}
