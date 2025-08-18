using System;
using GameDrive.Network;
using Newtonsoft.Json;
using UnityEngine;

namespace GameDrive
{
    public class Endpoint
    {
        const string TEXT_SEND_REQUEST_TO_ENDPOINT_ERROR = "SEND_REQUEST_TO_ENDPOINT_ERROR";
        const string TEXT_ENDPOINT_DESERIALIZE_ERROR = "ENDPOINT_DESERIALIZE_ERROR";

        string _name;

        Client _client;
        public Client Client
        {
            get
            {
                return _client;
            }
        }

        public Endpoint(string name, Client client = null)
        {
            _name = name;
            _client = ClientDefaultResolver.Resolve(client);
        }

        const string pathRun = "/endpoints/v2/run";
        const string pathRunIncognito2 = "/endpoints/v2/run-incognito";

        private void _SendRequestBase<T>(string path, Byte[] bytes, Action<T> callback, Action<ErrorSimple> callbackError)
        {
            HttpPutRequest.Instance().RequestJson(_client, path,
                bytes, (result) =>
                {
                    try
                    {
                        T data = JsonConvert.DeserializeObject<T>(result);
                        callback(data);
                    }
                    catch (System.Exception exception)
                    {
                        ErrorSimple error = new ErrorSimple(TEXT_ENDPOINT_DESERIALIZE_ERROR, exception.ToString());
                        callbackError(error);
                    }
                }, callbackError);
        }

        dynamic[] _args;
        public Endpoint SetArguments(params dynamic[] args)
        {
            _args = args;
            return this;
        }

        public void SendRequestErrorData<T>(Action<ErrorSimple> callbackFailed, Action<T> callbackSuccess)
        {
            if (string.IsNullOrEmpty(_client.GetTokenManager().GetAccessToken()))
            {
                var error = new ErrorSimple(TEXT_SEND_REQUEST_TO_ENDPOINT_ERROR, "Need to do authentication before make this request");
                callbackFailed(error);
                return;
            }

            var settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            string endpointRequestRawBodyJson = JsonConvert.SerializeObject(new EndpointRunRawBody(_name, _args), settings);

            _SendRequestBase(pathRun, System.Text.Encoding.UTF8.GetBytes(endpointRequestRawBodyJson), callbackSuccess, callbackFailed);
        }

        public void SendRequest<T>(Action<T> callbackSuccess, Action<ErrorSimple> callbackFailed)
        {
            SendRequestErrorData(callbackFailed, callbackSuccess);
        }

        public void SendRequestIncognitoErrorData<T>(Action<ErrorSimple> callbackFailed, Action<T> callbackSuccess)
        {
            var settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            string endpointRequestRawBodyJson = JsonConvert.SerializeObject(new EndpointRunIncognitoRawBody(_name, _args), settings);
            _SendRequestBase(pathRunIncognito2, System.Text.Encoding.UTF8.GetBytes(endpointRequestRawBodyJson), callbackSuccess, callbackFailed);
        }

        public void SendRequestIncognito<T>(Action<T> callbackSuccess, Action<ErrorSimple> callbackFailed)
        {
            SendRequestIncognitoErrorData(callbackFailed, callbackSuccess);
        }
    }

}