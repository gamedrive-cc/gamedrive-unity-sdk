using System;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace GameDrive.Network
{
    public class HttpPostRequest
    {
        private static HttpPostRequest _instance = null;
        public static HttpPostRequest Instance()
        {
            if (_instance == null)
            {
                _instance = new HttpPostRequest();
            }

            return _instance;
        }

        public void Request(Client client, string segments, Action<string> callback, Dictionary<string, string> formFields = null, HttpQuery query = null)
        {
            UriBuilder uriBuilder = UriHelper.BuildUri(segments);
            QueryStringManager.AddQueryString(uriBuilder, query);
            NetworkInstaller.Mono.StartCoroutine(PostRequestCoroutine(client, uriBuilder.Uri, callback, formFields));
        }


        IEnumerator PostRequestCoroutine(Client client, Uri uri, Action<string> callback, Dictionary<string, string> formFields)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, formFields))
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
                    //even UnityWebRequest.Result.ProtocolError
                    //Debug.Log("webRequest.downloadHandler.text:" + webRequest.downloadHandler.text);
                    callback(webRequest.downloadHandler.text);
                }
            }
        }

        public void RequestJson(Client client, string segments, string jsonBody, Action<string> callback, Action<ErrorSimple> errorCallback, HttpQuery query = null)
        {
            UriBuilder uriBuilder = UriHelper.BuildUri(segments);
            QueryStringManager.AddQueryString(uriBuilder, query);
            NetworkInstaller.Mono.StartCoroutine(PostRequestJsonCoroutine(client, uriBuilder.Uri, jsonBody, callback, errorCallback));
        }

        IEnumerator PostRequestJsonCoroutine(Client client, Uri uri, string jsonBody, Action<string> callback, Action<ErrorSimple> errorCallback)
        {

            yield return client.ClientReadyToMakeRequest.WaitReadyToRequest();

            using (UnityWebRequest webRequest = new UnityWebRequest(uri, "POST"))
            {
                string jsonFields = jsonBody;

                webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonFields));
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                WebRequestHeaderManager.AddAuthorizationHeader(client, webRequest);
                webRequest.SetRequestHeader("Content-Type", "application/json");
                webRequest.SetRequestHeader("Accept", "application/json");

                CertificateManager.AddCertificate(webRequest);

                yield return webRequest.SendWebRequest();
                HttpResultHelper.HandleJsonResult(client, webRequest, callback, errorCallback);
            }
        }
    }
}