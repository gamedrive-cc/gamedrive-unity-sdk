using System;

namespace GameDrive
{
    public enum Stage
    {
        PREVIEW,
        LIVE,
    }

    public enum Region
    {
        SOUTHEAST_ASIA,
    }

    [Serializable]
    public class Settings
    {
        public Settings()
        {
            apiInfo = new ApiInfo();
        }

        public ApiInfo apiInfo;
        public bool UseSecureProtocal = true;
        public string HttpPort = "80";
        public string HttpsPort = "443";
    }
}
