using UnityEngine;

namespace GameDrive
{
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
            string platform;
#if UNITY_EDITOR
            platform = Device.Editor;
#elif UNITY_ANDROID
            platform = Device.Android;
#elif UNITY_IOS
            platform = Device.IOS;
#endif

            return platform;
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
