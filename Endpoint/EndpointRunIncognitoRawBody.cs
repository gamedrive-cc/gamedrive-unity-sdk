using System;

namespace GameDrive
{
    [Serializable]
    public class EndpointRunIncognitoRawBody
    {
        public string endpointName;
        public dynamic[] arguments;
        public string projectId;
        public string apiSecret;

        public EndpointRunIncognitoRawBody(string endpointName, dynamic[] args)
        {
            this.endpointName = endpointName;
            this.arguments = args;
            this.projectId = GameDriveInitializer.Config.projectId;
            this.apiSecret = GameDriveInitializer.Config.apiSecret;
        }
    }

}