using System;
using UnityEngine.Networking;
using System.Collections;

namespace GameDrive.Network
{
    public class HttpPutRequest
    {
        private static HttpPutRequest _instance = null;
        public static HttpPutRequest Instance()
        {
            if (_instance == null)
            {
                _instance = new HttpPutRequest();
            }

            return _instance;
        }

        public void RequestJson(Client client, string segments, byte[] bytes, Action<string> callback, Action<ErrorSimple> calbackFailed, HttpQuery query = null)
        {
            UriBuilder uriBuilder = UriHelper.BuildUri(segments);
            QueryStringManager.AddQueryString(uriBuilder, query);
            NetworkInstaller.Mono.StartCoroutine(PutRequestCoroutine(client, uriBuilder.Uri, bytes, callback, calbackFailed));
        }

        IEnumerator PutRequestCoroutine(Client client, Uri uri, byte[] bytes, Action<string> callback, Action<ErrorSimple> errorCallback)
        {
            yield return client.ClientReadyToMakeRequest.WaitReadyToRequest();

            using (UnityWebRequest webRequest = UnityWebRequest.Put(uri, bytes))
            {
                WebRequestHeaderManager.AddHeadersPut(webRequest, client);
                CertificateManager.AddCertificate(webRequest);

                yield return webRequest.SendWebRequest();
                HttpResultHelper.HandleJsonResult(client, webRequest, callback, errorCallback);
            }
        }
    }
}