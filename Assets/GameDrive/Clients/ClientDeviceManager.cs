using UnityEngine;

namespace GameDrive
{

    public class DevicePlatform {
        public const string Android = "Android";
        public const string IOS = "iOS";
        public const string Windows = "Windows";
        public const string MacOS = "macOS";
        public const string Linux = "Linux";

        public const string Web = "Web";
        public const string TVOS = "tvOS";
        public const string VisionOS = "visionOS";
        public const string UWP = "UWP";

        public const string PlayStation = "PlayStation";
        public const string Xbox = "Xbox";
        public const string NintendoSwitch = "NintendoSwitch";

        public const string EmbeddedLinux = "EmbeddedLinux";
        public const string QNX = "QNX";
        public const string Server = "Server";

        public const string Editor = "Editor";
        public const string Unknown = "Unknown";

    }
    public class ClientDeviceManager
    {
        string _clientId;
        public ClientDeviceManager(string clientId)
        {
            _clientId = clientId;
            Initialize();
        }

        Device _device;
        public Device GetDevice()
        {
            return _device;
        }

        private void Initialize()
        {
            if (_clientId == Client.DefaultId)
            {
                CreateDefaultDevice();
            }
            else
            {
                CreateDevice();
            }
        }

        private void CreateDevice()
        {
            string deviceName = GetDeviceName(_clientId);
            string deviceId = GUIDManager.Instance().GetGUID(_clientId);
            _device = new Device(deviceName, deviceId, GetPlatform());
        }

        private void CreateDefaultDevice()
        {
            string deviceName = GetDeviceName();

            string deviceId = SystemInfo.deviceUniqueIdentifier;

            if (string.IsNullOrEmpty(deviceId) || deviceId == SystemInfo.unsupportedIdentifier)
            {
                deviceId = GUIDManager.Instance().GetGUID(_clientId);
            }

            _device = new Device(deviceName, deviceId, GetPlatform());
        }

        private string GetPlatform()
        {
#if UNITY_EDITOR
            return DevicePlatform.Editor;
#elif UNITY_SERVER
    return DevicePlatform.Server;

#elif UNITY_ANDROID
    return DevicePlatform.Android;

#elif UNITY_IOS
    return DevicePlatform.IOS;

#elif UNITY_TVOS
    return DevicePlatform.TVOS;

#elif UNITY_VISIONOS
    return DevicePlatform.VisionOS;

#elif UNITY_STANDALONE_WIN
    return DevicePlatform.Windows;

#elif UNITY_STANDALONE_OSX
    return DevicePlatform.MacOS;

#elif UNITY_STANDALONE_LINUX
    return DevicePlatform.Linux;

#elif UNITY_WEBGL
    return DevicePlatform.Web;

#elif UNITY_WSA
    return DevicePlatform.UWP;

#elif UNITY_PS5 || UNITY_PS4
    return DevicePlatform.PlayStation;

#elif UNITY_GAMECORE || UNITY_XBOXONE
    return DevicePlatform.Xbox;

#elif UNITY_SWITCH
    return DevicePlatform.NintendoSwitch;

#elif UNITY_EMBEDDED_LINUX
    return DevicePlatform.EmbeddedLinux;

#elif UNITY_QNX
    return DevicePlatform.QNX;
#else
    return DevicePlatform.Unknown;
#endif
        }


        private string GetDeviceName(string suffix = "")
        {
            string deviceName;
            if (SystemInfo.unsupportedIdentifier != SystemInfo.deviceName)
            {
                deviceName = SystemInfo.deviceName;
            }
            else
            {
                deviceName = "Undefined Name";
            }
            deviceName += "_" + suffix;
            return deviceName;
        }
    }
}
