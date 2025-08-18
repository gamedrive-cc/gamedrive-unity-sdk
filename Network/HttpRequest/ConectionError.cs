using System.Text;
using System.Collections.Specialized;
using System.Net;
using System;

namespace GameDrive.Network
{
    [Serializable]
    public class ConectionError : ErrorSimple
    {
        public ConectionError() : base("CONNECTION_ERROR", "Please check your connection")
        {
        }
    }
}
