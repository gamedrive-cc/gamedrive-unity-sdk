using Newtonsoft.Json;
using System;

namespace GameDrive
{

    [Serializable]

    class LoginBodyBase
    {
        public string projectId;
        public string apiSecret;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string stage;
        public Device device;

        public LoginBodyBase(string projectId, string apiSecret, string stage, Device device)
        {
            this.projectId = projectId;
            this.apiSecret = apiSecret;
            this.stage = stage;
            this.device = device;
        }
    }
}
