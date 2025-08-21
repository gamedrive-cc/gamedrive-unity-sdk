using System;

namespace GameDrive
{
    [Serializable]
    public class Account
    {
        public SocialAccounts socialAccounts;
        public Device[] devices;
    }
}
