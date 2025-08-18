using System;

namespace GameDrive
{
    [Serializable]
    public class LoggedInPlayer
    {
        public Player player;
        public string refreshToken;
        public string accessToken;
        public int accessTokenExpiresInSeconds;
    }
}
