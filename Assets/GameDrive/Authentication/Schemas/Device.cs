using System;

namespace GameDrive
{
    [Serializable]
    public class Device
    {
        public string name;

        public string guid;

        public string platform;

        public Device(string name, string guid, string platform)
        {
            this.name = name;
            this.guid = guid;
            this.platform = platform;
        }
    }
}
