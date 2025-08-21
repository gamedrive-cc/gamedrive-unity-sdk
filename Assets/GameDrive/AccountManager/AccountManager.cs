using GameDrive.Network;
using System;
using Newtonsoft.Json;

namespace GameDrive
{
    [Serializable]
    class BindFacebookInput
    {
        public string accessToken;

        public BindFacebookInput(string accessToken)
        {
            this.accessToken = accessToken;
        }
    }

    [Serializable]
    class BindGoogleInput
    {
        public string googleClientId;
        public string googleIdToken;

        public BindGoogleInput(string googleClientId, string googleIdToken)
        {
            this.googleClientId = googleClientId;
            this.googleIdToken = googleIdToken;
        }
    }

    public static class AccountManager
    {
        const string playerAccountV2 = "/player/account/v2";

        public static void BindWithFacebook(string accessToken, Action<SocialAccounts> callbackSuccess, Action<ErrorSimple> callbackFailed, Client client_ = null)
        {
            var client = ClientDefaultResolver.Resolve(client_);
            var body = new BindFacebookInput(accessToken);
            var jsonBody = JsonConvert.SerializeObject(body);
            HttpPostRequest.Instance().RequestJson(client, playerAccountV2 + "/bind-with-facebook", jsonBody, (stringData) =>
            {
                SocialAccounts socialAccount = JsonConvert.DeserializeObject<SocialAccounts>(stringData);
                callbackSuccess(socialAccount);
            }, callbackFailed);
        }


        public static void BindWithGoogle(string googleClientId, string googleIdToken, Action<SocialAccounts> callbackSuccess, Action<ErrorSimple> callbackFailed, Client client_ = null)
        {
            var client = ClientDefaultResolver.Resolve(client_);
            var body = new BindGoogleInput(googleClientId, googleIdToken);
            var jsonBody = JsonConvert.SerializeObject(body);
            HttpPostRequest.Instance().RequestJson(client, playerAccountV2 + "/bind-with-google", jsonBody, (stringData) =>
            {
                SocialAccounts socialAccount = JsonConvert.DeserializeObject<SocialAccounts>(stringData);
                callbackSuccess(socialAccount);
            }, callbackFailed);
        }
    }
}
