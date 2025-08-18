using System;
using Zenject;

namespace GameDrive.Network
{
    public class UriHelper
    {
        private static string _gatewayUrl;
        public static string GatewayUrl
        {
            get
            {
                if (String.IsNullOrEmpty(_gatewayUrl))
                {
                    if (ProtocalManager.GetUseSecure())
                    {
                        _gatewayUrl = "https://" + GameDriveInitializer.Settings.apiInfo.serverAddress + ":" + GameDriveInitializer.Settings.HttpsPort;
                    }
                    else
                    {
                        _gatewayUrl = "http://" + GameDriveInitializer.Settings.apiInfo.serverAddress + ":" + GameDriveInitializer.Settings.HttpPort;
                    }
                }

                return _gatewayUrl;
            }
        }

        public static UriBuilder BuildUri(string segments)
        {
            UriBuilder uriBuilder = new UriBuilder(GatewayUrl);

            string gatewayPath = Consts.GATEWAY_PATH;

            uriBuilder.Path = gatewayPath + segments;
            return uriBuilder;
        }
    }
}