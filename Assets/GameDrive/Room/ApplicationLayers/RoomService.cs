using GameDrive.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GameDrive.Room
{
    public class RoomService
    {
        const string roomServicePathV1 = "/room-service/v1";

        string _roomName;

        Client _client;
        internal RoomService(string roomName, Client client_)
        {
            _roomName = roomName;
            _client = ClientDefaultResolver.Resolve(client_);
        }

        public void RequestJoinOrCreateRoom(Dictionary<string, object> options, Action<JoinOrCreateRoomResult> callbackSuccess, Action<ErrorSimple> callbackFailed)
        {
            var body = new RoomOptionsRequestion(_roomName, CreateJsonString(options));
            var jsonBody = JsonConvert.SerializeObject(body);
            HttpPostRequest.Instance().RequestJson(_client, roomServicePathV1 + "/join-or-create-room", jsonBody, (stringData) =>
            {
                JoinOrCreateRoomResult result = JsonConvert.DeserializeObject<JoinOrCreateRoomResult>(stringData);
                callbackSuccess(result);
            }, callbackFailed);
        }

        public void RequestJoinRoom(Dictionary<string, object> options, Action<JoinRoomReservation> callbackSuccess, Action<ErrorSimple> callbackFailed)
        {
            var body = new RoomOptionsRequestion(_roomName, CreateJsonString(options));
            var jsonBody = JsonConvert.SerializeObject(body);
            HttpPostRequest.Instance().RequestJson(_client, roomServicePathV1 + "/join-room", jsonBody, (stringData) =>
            {
                JoinRoomReservation result = JsonConvert.DeserializeObject<JoinRoomReservation>(stringData);
                callbackSuccess(result);
            }, callbackFailed);
        }

        public void GetAvailableRooms(Dictionary<string, object> options, Action<AvailableRoom[]> callbackSuccess, Action<ErrorSimple> callbackFailed)
        {
            var body = new RoomOptionsRequestion(_roomName, CreateJsonString(options));
            var jsonBody = JsonConvert.SerializeObject(body);
            HttpPostRequest.Instance().RequestJson(_client, roomServicePathV1 + "/get-available-rooms", jsonBody, (stringData) =>
            {
                AvailableRoom[] result = JsonConvert.DeserializeObject<AvailableRoom[]>(stringData);
                callbackSuccess(result);
            }, callbackFailed);
        }

        public void RequestCreateRoom(Action<RoomAddress> callbackSuccess, Action<ErrorSimple> callbackFailed)
        {
            var body = new RoomRequest(_roomName);
            var jsonBody = JsonConvert.SerializeObject(body);
            HttpPostRequest.Instance().RequestJson(_client, roomServicePathV1 + "/create-room", jsonBody, (stringData) =>
            {
                RoomAddress result = JsonConvert.DeserializeObject<RoomAddress>(stringData);
                callbackSuccess(result);
            }, callbackFailed);
        }

        public void RequestPrepareRoom(Action<RoomAddress> callbackSuccess, Action<ErrorSimple> callbackFailed)
        {
            var body = new RoomRequest(_roomName);
            var jsonBody = JsonConvert.SerializeObject(body);
            HttpPostRequest.Instance().RequestJson(_client, roomServicePathV1 + "/prepare-room", jsonBody, (stringData) =>
            {
                RoomAddress result = JsonConvert.DeserializeObject<RoomAddress>(stringData);
                callbackSuccess(result);
            }, callbackFailed);
        }

        private string CreateJsonString(Dictionary<string, object> options)
        {
            string jsonString = JsonConvert.SerializeObject(options);
            return jsonString.Replace('"', '\'');
        }

        public void GetPlayerCurrentRoom(Action<RoomSession> callbackSuccess, Action<ErrorSimple> callbackFailed)
        {
            HttpQuery httpQuery = new HttpQuery();
            httpQuery["roomName"] = _roomName;
            HttpGetRequest.Instance().RequestJson(_client, roomServicePathV1 + "/player-current-room", (stringData) =>
            {
                RoomSession result = JsonConvert.DeserializeObject<RoomSession>(stringData);
                callbackSuccess(result);
            }, callbackFailed, httpQuery);
        }

        public void ForcePlayerLeaveCurrentRoom(Action<ForceLeave> callbackSuccess, Action<ErrorSimple> callbackFailed)
        {
            var body = new RoomRequest(_roomName);
            var jsonBody = JsonConvert.SerializeObject(body);
            HttpPostRequest.Instance().RequestJson(_client, roomServicePathV1 + "/force-leave", jsonBody, (stringData) =>
            {
                ForceLeave result = JsonConvert.DeserializeObject<ForceLeave>(stringData);
                callbackSuccess(result);
            }, callbackFailed);
        }

    }
}
