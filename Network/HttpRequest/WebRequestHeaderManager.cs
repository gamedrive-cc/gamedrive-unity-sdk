using UnityEngine.Networking;
using Zenject;

namespace GameDrive.Network
{
    public class WebRequestHeaderManager
    {
#pragma warning disable 0649
#pragma warning restore
        public static void AddHeaders(Client client, UnityWebRequest webRequest)
        {
            AddAuthorizationHeader(client, webRequest);

            webRequest.SetRequestHeader("Accept", "application/json");
        }

        public static void AddAuthorizationHeader(Client client, UnityWebRequest webRequest)
        {
            if (!string.IsNullOrEmpty(client.GetTokenManager().GetAccessToken()))
            {
                webRequest.SetRequestHeader("authorization", "Bearer " + client.GetTokenManager().GetAccessToken());
            }
        }

        public static void AddHeadersPut(UnityWebRequest webRequest, Client client_)
        {
            AddHeaders(client_, webRequest);
            webRequest.SetRequestHeader("content-type", "text/plain");
        }
    }
}