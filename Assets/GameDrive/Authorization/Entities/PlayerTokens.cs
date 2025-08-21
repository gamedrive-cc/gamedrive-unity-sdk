using System;

namespace GameDrive
{
    [Serializable]
    public class PlayerTokens
    {
        public string refreshToken;
        public string accessToken;
        public int accessTokenExpiresInSeconds;
    }
}
