using System;

namespace GameDrive
{
    [Serializable]
    public class TestConfig
    {
        public bool localhost;
        public string serverAddress;
        public bool useSecureProtocal = false;
        public string httpPort = "8000";
        public string httpsPort = "8443";
    }
}
