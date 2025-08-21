using GameDrive.Network;
using Newtonsoft.Json;
using System;

namespace GameDrive
{
    public class Authentication
    {
        private static Authentication _instance = null;

        const string loginV3Path = "/player/auth/v3";
        public static Authentication Instance()
        {
            if (_instance == null)
            {
                _instance = new Authentication();
            }
            return _instance;
        }

        public static void LoginWithDevice(Action<LoggedInPlayer> callbackSuccecss, Action<ErrorSimple> callbackFailed, Client client_ = null)
        {
            var client = ClientDefaultResolver.Resolve(client_);
            var device = client.GetDeviceManager().GetDevice();

            var body = new LoginBodyBase(
            GameDriveInitializer.Config.projectId,
            GameDriveInitializer.Config.apiSecret, StageHelper.GetStageBody(), device);

            var jsonBody = JsonConvert.SerializeObject(body);

            HttpPostRequest.Instance().RequestJson(client, loginV3Path + "/login-by-device", jsonBody, (stringData) =>
            {
                LoggedInPlayer loggedInPlayer = JsonConvert.DeserializeObject<LoggedInPlayer>(stringData);
                ManageLoggedInCommon(loggedInPlayer, client);
                callbackSuccecss(loggedInPlayer);
            }, callbackFailed);
        }


        public static void LoginWithGoogle(string googleClientId, string googleIdToken, Action<LoggedInPlayer> callbackSuccecss, Action<ErrorSimple> callbackFailed, Client client_ = null)
        {
            var client = ClientDefaultResolver.Resolve(client_);
            var device = client.GetDeviceManager().GetDevice();

            var body = new LoginBodyGooogle(
            GameDriveInitializer.Config.projectId,
            GameDriveInitializer.Config.apiSecret,
            StageHelper.GetStageBody(),
            device,
            googleClientId,
            googleIdToken);

            var jsonBody = JsonConvert.SerializeObject(body);

            HttpPostRequest.Instance().RequestJson(client, loginV3Path + "/login-with-google", jsonBody, (stringErrorData) =>
            {
                LoggedInPlayer loggedInPlayer = JsonConvert.DeserializeObject<LoggedInPlayer>(stringErrorData);
                ManageLoggedInCommon(loggedInPlayer, client);
                callbackSuccecss(loggedInPlayer);
            }, callbackFailed);
        }

        public static void LoginWithFacebook(string facebookAccessToken, Action<LoggedInPlayer> callbackSuccecss, Action<ErrorSimple> callbackFailed, Client client_ = null)
        {
            var client = ClientDefaultResolver.Resolve(client_);
            var device = client.GetDeviceManager().GetDevice();

            var body = new LoginBodyFacebook(
            GameDriveInitializer.Config.projectId,
            GameDriveInitializer.Config.apiSecret,
            StageHelper.GetStageBody(),
            device,
            facebookAccessToken);

            var jsonBody = JsonConvert.SerializeObject(body);

            HttpPostRequest.Instance().RequestJson(client, loginV3Path + "/login-with-facebook", jsonBody, (stringErrorData) =>
            {
                LoggedInPlayer loggedInPlayer = JsonConvert.DeserializeObject<LoggedInPlayer>(stringErrorData);
                ManageLoggedInCommon(loggedInPlayer, client);
                callbackSuccecss(loggedInPlayer);
            }, callbackFailed);
        }


        public static void Logout(Client client_ = null)
        {
            Client client = ClientDefaultResolver.Resolve(client_);
            client.GetTokenManager().SetRefreshToken(null);
            client.GetTokenManager().SetAccessToken(null);
            client.SetPlayer(null);
        }

        private static void ManageLoggedInCommon(LoggedInPlayer loggedInPlayer, Client client)
        {
            client.SetPlayer(loggedInPlayer.player);
            client.GetTokenManager().SetRefreshToken(loggedInPlayer.refreshToken);
            client.GetTokenManager().SetAccessToken(loggedInPlayer.accessToken);
            client.GetTokenManager().SetAccessTokenExpiresInSeconds(loggedInPlayer.accessTokenExpiresInSeconds);
        }
    }
}
