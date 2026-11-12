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
        Colyseus.Client colyseusClient;

        public Colyseus.Client GetColyseusClient()
        {
            return colyseusClient;
        }

        RoomService _roomService;

        const string TEXT_ROOM_SERVER_ERROR = "ROOM_SERVER_ERROR";

        string _roomName;
        public Client Client { get; private set; }

        private Colyseus.Room<T> _colyseusRoom;

        private string _roomServiceAddress;
        public string RoomServiceAddress => _roomServiceAddress;

        public Colyseus.Room<T> ColyseusRoom => _colyseusRoom;

        public RoomClient(string roomName, Client client_ = null)
        {
            _roomName = roomName;
            Client = ClientDefaultResolver.Resolve(client_);
            _roomService = new RoomService(_roomName, Client);
        }

        private Colyseus.Settings CreateColyseusSettings(string roomServiceAddress)
        {
            Colyseus.Settings colyseusSettings = ScriptableObject.CreateInstance<Colyseus.Settings>();
            colyseusSettings.colyseusServerAddress = GameDriveInitializer.Settings.apiInfo.serverAddress + "/" + roomServiceAddress;
            colyseusSettings.colyseusServerPort = ProtocalManager.GetPort();
            colyseusSettings.useSecureProtocol = ProtocalManager.GetUseSecure();
            return colyseusSettings;
        }

        private async Task RunColyseusRoomTask(Task<Colyseus.Room<T>> task, Action<ErrorSimple> onError, Action<Colyseus.Room<T>> onJoin, string roomServiceAddress)
        {
            try
            {
                var result = await task;
                _colyseusRoom = result;
                _roomServiceAddress = roomServiceAddress;
                onJoin.Invoke(result);
            }
            catch (System.Exception exception)
            {
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
        private  SeatReservation CreateSeatReservation(JoinRoomReservation joinReservation)
        {
            return new SeatReservation
            {
                name = joinReservation.roomReservation.name,
                roomId = joinReservation.roomReservation.roomId,
                processId = joinReservation.roomReservation.processId,
                reconnectionToken = joinReservation.roomReservation.reconnectionToken,
                sessionId = joinReservation.roomReservation.sessionId,

            };
        }

        public void JoinOrCreate(Dictionary<string, object> options, Action<Colyseus.Room<T>> onJoin, Action<ErrorSimple> onError)
        {
            var toSendOptions = CreateToSendOptions(options);
            _roomService.RequestJoinOrCreateRoom(toSendOptions, (joinOrCreateResult) =>
             {

                 if (joinOrCreateResult.isCreate)
                 {
                     colyseusClient = new Colyseus.Client(CreateColyseusSettings(joinOrCreateResult.createRoomResult.roomServiceAddress));

                     string roomServiceRoomName = joinOrCreateResult.createRoomResult.roomName;

                     Task<Colyseus.Room<T>> taskJoinRoom = colyseusClient.Create<T>(roomServiceRoomName, toSendOptions);

                     var task = RunColyseusRoomTask(taskJoinRoom, onError, onJoin, joinOrCreateResult.createRoomResult.roomServiceAddress);
                     task.ContinueWith(t =>
                     {
                         onError.Invoke(CreateErrorSimple(t.Exception));
                     }, TaskContinuationOptions.OnlyOnFaulted);
                 }
                 else
                 {
                     colyseusClient = new Colyseus.Client(CreateColyseusSettings(joinOrCreateResult.joinReservation.roomAddress.roomServiceAddress));

                     JoinRoomReservation joinReservation = joinOrCreateResult.joinReservation;
                     SeatReservation seat = CreateSeatReservation(joinReservation);
                     Task<Colyseus.Room<T>> taskJoinRoom = colyseusClient.ConsumeSeatReservation<T>(seat);

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

        public void Create(Dictionary<string, object> options, Action<Colyseus.Room<T>> onCreated, Action<ErrorSimple> onError)
        {
            _roomService.RequestCreateRoom((roomAddress) =>
            {
                if (roomAddress != null)
                {
                    colyseusClient = new Colyseus.Client(CreateColyseusSettings(roomAddress.roomServiceAddress));

                    string roomServiceRoomName = roomAddress.roomName;

                    var toSendOptions = CreateToSendOptions(options);
                    Task<Colyseus.Room<T>> taskJoinRoom = colyseusClient.Create<T>(roomServiceRoomName, toSendOptions);

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

        public void Join(Dictionary<string, object> options, Action<Colyseus.Room<T>> onJoin, Action<ErrorSimple> onError)
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
                    colyseusClient = new Colyseus.Client(CreateColyseusSettings(joinRoomReserve.roomAddress.roomServiceAddress));
                    SeatReservation seat = CreateSeatReservation(joinRoomReserve);
                    Task<Colyseus.Room<T>> taskJoinRoom = colyseusClient.ConsumeSeatReservation<T>(seat);

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

        public void JoinById(string roomId, string roomServiceAddress, Dictionary<string, object> options, Action<Colyseus.Room<T>> onJoin, Action<ErrorSimple> onError)
        {
            //find roomServiceRoomName by send request to server roomName
            colyseusClient = new Colyseus.Client(CreateColyseusSettings(roomServiceAddress));

            var toSendOptions = CreateToSendOptions(options);
            Task<Colyseus.Room<T>> taskJoinRoom = colyseusClient.JoinById<T>(roomId, toSendOptions);

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

        public async Task DisconnectAsync(Action onSuccess, Action<ErrorSimple> onError)
        {
            var room = ColyseusRoom;
            if (room == null)
            {
                onSuccess?.Invoke();
                return;
            }

            // An intentional disconnect keeps the seat for a manual Reconnect;
            // without this, an abnormal close (1006) triggers Colyseus auto-reconnect.
            room.Reconnection.Enabled = false;

            try
            {
                await room.Leave(false);
            }
            catch (System.Net.WebSockets.WebSocketException)
            {
                // Proxies in front of remote servers often drop the TCP connection instead of
                // echoing the close frame; the socket is closed (aborted) either way.
            }
            catch (System.Exception ex)
            {
                onError?.Invoke(CreateErrorSimple(ex));
                return;
            }

            onSuccess?.Invoke();
        }

        public void Disconnect(Action onSuccess, Action<ErrorSimple> onError)
        {
            _ = DisconnectAsync(onSuccess, onError);
        }

        public async Task LeaveAsync(Action onSuccess, Action<ErrorSimple> onError)
        {
            try
            {
                // Attempt the leave handshake
                await ColyseusRoom.Leave(true);
                onSuccess?.Invoke();
            }
            catch (System.Exception ex)
            {
                onError?.Invoke(CreateErrorSimple(ex));
            }
        }

        public void Leave(Action onSuccess, Action<ErrorSimple> onError)
        {
            _ = LeaveAsync(onSuccess, onError);
        }

        public void Reconnect(RoomSession roomSession, Action<Colyseus.Room<T>> onReconnected, Action<ErrorSimple> onError)
        {
            colyseusClient = new Colyseus.Client(CreateColyseusSettings(roomSession.roomServiceAddress));
            //colyseusClient.rec

            Task<Colyseus.Room<T>> taskJoinRoom = colyseusClient.Reconnect<T>(new ReconnectionToken {
                RoomId = roomSession.roomId,
                Token = roomSession.reconnectionToken
            });

            var task = RunColyseusRoomTask(taskJoinRoom, onError, onReconnected, roomSession.roomServiceAddress);

            task.ContinueWith(t =>
            {
                onError.Invoke(CreateErrorSimple(t.Exception));
            }, TaskContinuationOptions.OnlyOnFaulted);
        }

        public RoomSession CreateRoomSession()
        {
            if (_colyseusRoom == null)
            {
                return null;
            }

            return new RoomSession
            {
                roomId = _colyseusRoom.RoomId,
                roomServiceAddress = _roomServiceAddress,
                reconnectionToken = _colyseusRoom.ReconnectionToken.Token
            };
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
