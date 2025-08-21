using GameDrive.Network;
using System;
using Newtonsoft.Json;

namespace GameDrive
{

    [Serializable]
    class UpdatePlayerNameInput
    {
        public string name;

        public UpdatePlayerNameInput(string name)
        {
            this.name = name;
        }
    }
    public static class PlayerManager
    {
        const string playerV2Path = "/player/v2";
        public static void UpdateName(string name, Action<Player> callbackSuccess, Action<ErrorSimple> callbackFailed, Client client_ = null)
        {
            var client = ClientDefaultResolver.Resolve(client_);
            var body = new UpdatePlayerNameInput(name);
            var jsonBody = JsonConvert.SerializeObject(body);
            HttpPostRequest.Instance().RequestJson(client, playerV2Path + "/name", jsonBody, (stringData) =>
            {
                Player player = JsonConvert.DeserializeObject<Player>(stringData);
                callbackSuccess(player);
            }, callbackFailed);
        }
    }
}
