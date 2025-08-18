using GameDrive.Network;
using System;
using Newtonsoft.Json;

namespace GameDrive
{
    [Serializable]
    class RotateTokensInput
    {
        public string projectId;
        public string stage;
        public string refreshToken;


        public RotateTokensInput(string projectId, string stage, string refreshToken)
        {
            this.projectId = projectId;
            this.stage = stage;
            this.refreshToken = refreshToken;
        }
    }

    public class Authorization
    {
        const string authorizationPathV2 = "/authorization/v2";

        public static void RotateTokens(Action callbackSuccess, Action<ErrorSimple> callbackFailed, Client client_ = null)
        {
            Client client = ClientDefaultResolver.Resolve(client_);
            string refreshToken = client.GetTokenManager().GetRefreshToken();
            var body = new RotateTokensInput(
             GameDriveInitializer.Config.projectId,
             StageHelper.GetStageBody(),
             client.GetTokenManager().GetRefreshToken());

            var jsonBody = JsonConvert.SerializeObject(body);

            HttpPostRequest.Instance().RequestJson(client, authorizationPathV2 + "/rotate-tokens", jsonBody, (stringData) =>
            {
                PlayerTokens playerTokens = JsonConvert.DeserializeObject<PlayerTokens>(stringData);
                client.GetTokenManager().SetRefreshToken(playerTokens.refreshToken);
                client.GetTokenManager().SetAccessToken(playerTokens.accessToken);
                client.GetTokenManager().SetAccessTokenExpiresInSeconds(playerTokens.accessTokenExpiresInSeconds);
                callbackSuccess();
            }, callbackFailed);
        }
    }
}
