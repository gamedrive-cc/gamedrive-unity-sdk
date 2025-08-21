using System;

namespace GameDrive
{

    [Serializable]
    class LoginBodyGooogle : LoginBodyBase
    {
        public string googleIdToken;
        public string googleClientId;

        public LoginBodyGooogle(string projectId, string apiSecret, string stage, Device device, string googleClientId, string googleIdToken) : base(projectId, apiSecret, stage, device)
        {
            this.googleIdToken = googleClientId;
            this.googleClientId = googleIdToken;
        }
    }
}
