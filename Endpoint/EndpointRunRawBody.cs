using System;

namespace GameDrive
{
    [Serializable]
    public class EndpointRunRawBody
    {
        public string endpointName;
        public dynamic[] arguments;

        public EndpointRunRawBody(string endpointName, dynamic[] args)
        {
            this.endpointName = endpointName;
            this.arguments = args;
        }
    }

}