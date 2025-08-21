using System;

namespace GameDrive
{

    [Serializable]
    class LoginBodyFacebook : LoginBodyBase
    {
        public string facebookAccessToken;
        public LoginBodyFacebook(string projectId, string apiSecret, string stage, Device device, string facebookAccessToken) : base(projectId, apiSecret, stage, device)
        {
            this.facebookAccessToken = facebookAccessToken;
        }
    }
}
