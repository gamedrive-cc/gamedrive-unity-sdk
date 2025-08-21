using System.Text;
using System.Collections.Specialized;
using System.Net;

namespace GameDrive.Network
{
    public class HttpQuery : NameValueCollection
    {
        public string ToQueryString()
        {
            int count = Count;
            if (count == 0)
                return "";
            StringBuilder sb = new StringBuilder();
            string[] keys = AllKeys;
            for (int i = 0; i < count; i++)
            {
                sb.AppendFormat("{0}={1}&", keys[i], WebUtility.UrlEncode(this[keys[i]]));
            }
            if (sb.Length > 0)
                sb.Length--;
            return sb.ToString();
        }
    }
}
