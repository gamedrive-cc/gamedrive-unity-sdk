using System;
using UnityEngine.Networking;
using Zenject;
using System.Collections;
using System.Text;
using UnityEngine;
using System.Net;

namespace GameDrive.Network
{
    public class HttpResultHelper
    {

        public static void HandleJsonResult(Client client, UnityWebRequest webRequest, Action<string> callback, Action<ErrorSimple> errorCallback)
        {
            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                errorCallback(new ConectionError());
            }
            else
            {
                string text = webRequest.downloadHandler.text;
                if (webRequest.responseCode >= 400)
                {
                    string errorMessage = "url:" + webRequest.uri.ToString() + ",\ntext:" + text;
                    Debug.LogError(errorMessage);
                    ErrorSimple errorSimple;
                    try
                    {
                        errorSimple = JsonUtility.FromJson<ErrorSimple>(text);
                    }
                    catch (System.Exception err)
                    {
                        errorSimple = new ErrorSimple(ErrorCodes.UNKNOW_ERROR_CODE, text);
                    }

                    if (errorSimple.code == ErrorCodes.INVALID_TOKEN)
                    {
                        client.ActionErrorInvalidToken?.Invoke();
                    }

                    errorCallback(errorSimple);
                }
                else
                {
                    callback(text);
                }
            }
        }

    }
}