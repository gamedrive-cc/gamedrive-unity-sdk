using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDrive
{
    public static class ProtocalManager
    {

        public static string GetPort()
        {
            if (GameDriveInitializer.Settings.UseSecureProtocal)
            {
                return GameDriveInitializer.Settings.HttpsPort;
            }
            else
            {
                return GameDriveInitializer.Settings.HttpPort;
            }
        }

        public static bool GetUseSecure()
        {
            return GameDriveInitializer.Settings.UseSecureProtocal;
        }
    }
}
