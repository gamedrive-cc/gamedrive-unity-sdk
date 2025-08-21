using System;
using UnityEngine.Networking;
using System.Collections;

namespace GameDrive.Network
{
    public class HttpGetRequest
    {
        private static HttpGetRequest _instance = null;
        public static HttpGetRequest Instance()
        {
            if (_instance == null)
            {
                _instance = new HttpGetRequest();
            }
            return _instance;
        }

        public void Request(Client client, string segments, Action<string> callback, HttpQuery query = null)
        {
            UriBuilder urlBuilder = UriHelper.BuildUri(segments);
            QueryStringManager.AddQueryString(urlBuilder, query);

            NetworkInstaller.Mono.StartCoroutine(GetRequestCoroutine(client, urlBuilder.Uri, callback));
        }


        IEnumerator GetRequestCoroutine(Client client, Uri uri, Action<string> callback)
        {
            yield return client.ClientReadyToMakeRequest.WaitReadyToRequest();

            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                WebRequestHeaderManager.AddHeaders(client, webRequest);
                CertificateManager.AddCertificate(webRequest);

                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError)
                {
                    callback(UnityEngine.JsonUtility.ToJson(new ConectionError()));
                }
                else
                {
                    //UnityWebRequest.Result.ProtocolError
                    callback(webRequest.downloadHandler.text);
                }
            }
        }


        public void RequestJson(Client client, string segments, Action<string> callback, Action<ErrorSimple> errorCallback, HttpQuery query = null)
        {
            UriBuilder uriBuilder = UriHelper.BuildUri(segments);
            QueryStringManager.AddQueryString(uriBuilder, query);
            NetworkInstaller.Mono.StartCoroutine(GetRequestJsonCoroutine(client, uriBuilder.Uri, callback, errorCallback));
        }

        IEnumerator GetRequestJsonCoroutine(Client client, Uri uri, Action<string> callback, Action<ErrorSimple> errorCallback)
        {

            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                WebRequestHeaderManager.AddHeaders(client, webRequest);
                CertificateManager.AddCertificate(webRequest);

                yield return webRequest.SendWebRequest();
                HttpResultHelper.HandleJsonResult(client, webRequest, callback, errorCallback);
            }
        }
    }
}