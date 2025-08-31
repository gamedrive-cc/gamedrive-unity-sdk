using Colyseus;
using Colyseus.Schema;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GameDrive.Room
{
    public class RoomClient<T> : IRoomClient<T> where T : Schema
    {
        ColyseusClient colyseusClient;

        public ColyseusClient GetColyseusClient()
        {
            return colyseusClient;
        }

        RoomService _roomService;

        const string TEXT_ROOM_SERVER_ERROR = "ROOM_SERVER_ERROR";

        string _roomName;
        public Client Client { get; private set; }
        public string RoomServiceAddress { get; private set; }

        private ColyseusRoom<T> _colyseusRoom;

        public ColyseusRoom<T> ColyseusRoom => _colyseusRoom;
        public RoomSession LatestRoomSession { get; private set; }

        public RoomClient(string roomName, Client client_ = null)
        {
            _roomName = roomName;
            Client = ClientDefaultResolver.Resolve(client_);
            _roomService = new RoomService(_roomName, Client);
        }

        private ColyseusSettings CreateColyseusSettings(string roomServiceAddress)
        {
            RoomServiceAddress = roomServiceAddress;

            ColyseusSettings colyseusSettings = ScriptableObject.CreateInstance<ColyseusSettings>();
            colyseusSettings.colyseusServerAddress = GameDriveInitializer.Settings.apiInfo.serverAddress;
            colyseusSettings.colyseusSubPath = "/" + roomServiceAddress;
            colyseusSettings.colyseusServerPort = ProtocalManager.GetPort();
            colyseusSettings.useSecureProtocol = ProtocalManager.GetUseSecure();

            return colyseusSettings;
        }

        private async Task RunColyseusRoomTask(Task<ColyseusRoom<T>> task, Action<ErrorSimple> onError, Action<ColyseusRoom<T>> onJoin, string roomServiceAddress)
        {
            try
            {
                var result = await task;
                _colyseusRoom = result;
                LatestRoomSession = new RoomSession();
                LatestRoomSession.sessionId = _colyseusRoom.SessionId;
                LatestRoomSession.roomId = _colyseusRoom.Id;
                LatestRoomSession.roomServiceAddress = roomServiceAddress;
                onJoin.Invoke(result);
            }
            catch (System.Exception exception)
            {
                LatestRoomSession = null;
                onError.Invoke(CreateErrorSimple(exception));
            }
        }

        private async Task RunTask(Task task, Action<ErrorSimple> onError, Action onSuccess)
        {
            bool runTaskPassed = false;
            try
            {
                await task;
                runTaskPassed = true;
            }
            catch (System.Exception exception)
            {
                onError.Invoke(CreateErrorSimple(exception));
            }

            if (runTaskPassed)
            {
                onSuccess.Invoke();
            }
        }

        public void JoinOrCreate(Dictionary<string, object> options, Action<ColyseusRoom<T>> onJoin, Action<ErrorSimple> onError)
        {
            var toSendOptions = CreateToSendOptions(options);
            _roomService.RequestJoinOrCreateRoom(toSendOptions, (joinOrCreateResult) =>
             {

                 if (joinOrCreateResult.isCreate)
                 {
                     colyseusClient = new ColyseusClient(CreateColyseusSettings(joinOrCreateResult.createRoomResult.roomServiceAddress), true);

                     string roomServiceRoomName = joinOrCreateResult.createRoomResult.roomName;

                     Task<ColyseusRoom<T>> taskJoinRoom = colyseusClient.Create<T>(roomServiceRoomName, toSendOptions);

                     var task = RunColyseusRoomTask(taskJoinRoom, onError, onJoin, joinOrCreateResult.createRoomResult.roomServiceAddress);
                     task.ContinueWith(t =>
                     {
                         onError.Invoke(CreateErrorSimple(t.Exception));
                     }, TaskContinuationOptions.OnlyOnFaulted);
                 }
                 else
                 {
                     colyseusClient = new ColyseusClient(CreateColyseusSettings(joinOrCreateResult.joinReservation.roomAddress.roomServiceAddress), true);

                     var joinReservation = joinOrCreateResult.joinReservation;

                     //TODO consume reservation
                     ColyseusMatchMakeResponse colyseusMatchMakeResponse = new ColyseusMatchMakeResponse();
                     colyseusMatchMakeResponse.sessionId = joinReservation.roomReservation.sessionId;
                     colyseusMatchMakeResponse.room = new ColyseusRoomAvailable();
                     colyseusMatchMakeResponse.room.clients = (uint)joinReservation.roomReservation.roomInfo.clients;
                     colyseusMatchMakeResponse.room.maxClients = (uint)joinReservation.roomReservation.roomInfo.maxClients;
                     if (joinReservation.roomReservation.roomInfo.maxClients < 0)// -1 mean Infinity in client side
                     {
                         colyseusMatchMakeResponse.room.maxClients = uint.MaxValue;
                     }

                     colyseusMatchMakeResponse.room.name = joinReservation.roomReservation.roomInfo.name;
                     colyseusMatchMakeResponse.room.processId = joinReservation.roomReservation.roomInfo.processId;
                     colyseusMatchMakeResponse.room.roomId = joinReservation.roomReservation.roomInfo.roomId;

                     Task<ColyseusRoom<T>> taskJoinRoom = colyseusClient.ConsumeSeatReservation<T>(colyseusMatchMakeResponse);

                     var task = RunColyseusRoomTask(taskJoinRoom, onError, onJoin, joinOrCreateResult.joinReservation.roomAddress.roomServiceAddress);
                     task.ContinueWith(t =>
                     {
                         onError.Invoke(CreateErrorSimple(t.Exception));
                     }, TaskContinuationOptions.OnlyOnFaulted);
                 }
             },
             (err) =>
             {
                 onError(err);
             }
             );
        }

        public void Create(Dictionary<string, object> options, Action<ColyseusRoom<T>> onCreated, Action<ErrorSimple> onError)
        {
            _roomService.RequestCreateRoom((roomAddress) =>
            {
                if (roomAddress != null)
                {
                    colyseusClient = new ColyseusClient(CreateColyseusSettings(roomAddress.roomServiceAddress), true);

                    string roomServiceRoomName = roomAddress.roomName;

                    var toSendOptions = CreateToSendOptions(options);
                    Task<ColyseusRoom<T>> taskJoinRoom = colyseusClient.Create<T>(roomServiceRoomName, toSendOptions);

                    var task = RunColyseusRoomTask(taskJoinRoom, onError, onCreated, roomAddress.roomServiceAddress);
                    task.ContinueWith(t =>
                    {
                        onError.Invoke(CreateErrorSimple(t.Exception));
                    }, TaskContinuationOptions.OnlyOnFaulted);
                }
                else
                {
                    // show room service not avaialble
                    onError.Invoke(new ErrorSimple(TEXT_ROOM_SERVER_ERROR, " there is no room service available"));
                }
            },
            (err) =>
            {
                onError(err);
            });
        }

        public void Join(Dictionary<string, object> options, Action<ColyseusRoom<T>> onJoin, Action<ErrorSimple> onError)
        {
            //find roomServiceRoomName by send request to server player gateway
            var toSendOptions = CreateToSendOptions(options);
            _roomService.RequestJoinRoom(toSendOptions, (joinRoomReserve) =>
            {
                if (joinRoomReserve == null)
                {
                    onError.Invoke(new ErrorSimple(TEXT_ROOM_SERVER_ERROR, "there is no available room to join"));
                }
                else
                {
                    //join room
                    colyseusClient = new ColyseusClient(CreateColyseusSettings(joinRoomReserve.roomAddress.roomServiceAddress), true);

                    ColyseusMatchMakeResponse colyseusMatchMakeResponse = new ColyseusMatchMakeResponse();
                    colyseusMatchMakeResponse.sessionId = joinRoomReserve.roomReservation.sessionId;
                    colyseusMatchMakeResponse.room = new ColyseusRoomAvailable();
                    colyseusMatchMakeResponse.room.name = joinRoomReserve.roomReservation.roomInfo.name;
                    colyseusMatchMakeResponse.room.roomId = joinRoomReserve.roomReservation.roomInfo.roomId;
                    colyseusMatchMakeResponse.room.processId = joinRoomReserve.roomReservation.roomInfo.processId;

                    Task<ColyseusRoom<T>> taskJoinRoom = colyseusClient.ConsumeSeatReservation<T>(colyseusMatchMakeResponse);

                    var task = RunColyseusRoomTask(taskJoinRoom, onError, onJoin, joinRoomReserve.roomAddress.roomServiceAddress);
                    task.ContinueWith(t =>
                    {
                        onError.Invoke(CreateErrorSimple(t.Exception));
                    }, TaskContinuationOptions.OnlyOnFaulted);
                }
            }, (err) =>
            {
                onError(err);
            });
        }

        public void JoinById(string roomId, string roomServiceAddress, Dictionary<string, object> options, Action<ColyseusRoom<T>> onJoin, Action<ErrorSimple> onError)
        {
            //find roomServiceRoomName by send request to server roomName
            colyseusClient = new ColyseusClient(CreateColyseusSettings(roomServiceAddress), true);

            var toSendOptions = CreateToSendOptions(options);
            Task<ColyseusRoom<T>> taskJoinRoom = colyseusClient.JoinById<T>(roomId, toSendOptions);

            var task = RunColyseusRoomTask(taskJoinRoom, onError, onJoin, roomServiceAddress);
            task.ContinueWith(t =>
            {
                onError.Invoke(CreateErrorSimple(t.Exception));
            }, TaskContinuationOptions.OnlyOnFaulted);
        }

        public void GetPlayerCurrentRoom(Action<RoomSession> onData, Action<ErrorSimple> onError)
        {
            _roomService.GetPlayerCurrentRoom(result =>
            {
                LatestRoomSession = result;
                onData(result);
            }, (err) =>
            {
                onError(err);
            });
        }


        public void ForcePlayerLeaveCurrentRoom(Action<ForceLeave> onData, Action<ErrorSimple> onError)
        {
            _roomService.ForcePlayerLeaveCurrentRoom(result =>
            {
                onData(result);
            }, (err) =>
            {
                onError(err);
            });
        }

        public void Disconnect(Action onLeavedRoomCalled, Action<ErrorSimple> onError)
        {
            Task task = RunTask(ColyseusRoom.Leave(false), onError, onLeavedRoomCalled);

            task.ContinueWith(t =>
            {
                onError.Invoke(CreateErrorSimple(t.Exception));
            }, TaskContinuationOptions.OnlyOnFaulted);
        }

        public void Leave(Action onLeavedRoomCalled, Action<ErrorSimple> onError)
        {
            Task task = RunTask(ColyseusRoom.Leave(true), onError, () =>
            {
                LatestRoomSession = null;
                _colyseusRoom = null;
                onLeavedRoomCalled.Invoke();
            });

            task.ContinueWith(t =>
            {
                onError.Invoke(CreateErrorSimple(t.Exception));
            }, TaskContinuationOptions.OnlyOnFaulted);
        }

        public void Reconnect(RoomSession roomSession, Action<ColyseusRoom<T>> onReconnected, Action<ErrorSimple> onError)
        {
            colyseusClient = new ColyseusClient(CreateColyseusSettings(roomSession.roomServiceAddress), true);
            //colyseusClient.rec

            Task<ColyseusRoom<T>> taskJoinRoom = colyseusClient.Reconnect<T>(roomSession.roomId, roomSession.sessionId);

            var task = RunColyseusRoomTask(taskJoinRoom, onError, onReconnected, roomSession.roomServiceAddress);

            task.ContinueWith(t =>
            {
                onError.Invoke(CreateErrorSimple(t.Exception));
            }, TaskContinuationOptions.OnlyOnFaulted);
        }

        private ErrorSimple CreateErrorSimple(System.Exception exception)
        {
            return new ErrorSimple(TEXT_ROOM_SERVER_ERROR, exception.Message);
        }

        private Dictionary<string, object> CreateToSendOptions(Dictionary<string, object> options)
        {
            Dictionary<string, object> toSendOptions = new Dictionary<string, object>();
            toSendOptions["accessToken"] = Client.GetTokenManager().GetAccessToken();

            if (options != null)
            {
                foreach (var kvp in options)
                {
                    toSendOptions[kvp.Key] = kvp.Value;
                }
            }

            return toSendOptions;
        }

        public void PrepareRoom(Action preparedRoomCallback, Action<ErrorSimple> onError)
        {
            _roomService.RequestPrepareRoom((errorsData) =>
            {
                preparedRoomCallback();
            }, (err) =>
            {
                onError(err);
            });
        }
    }
}
