using System;
using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;

namespace GameDrive
{
    public class ClientTokensManager
    {
        string _accessToken;
        string _refreshToken;

        const string REFRESH_TOKEN_KEY = "GameDriveRefreshToken2";
        const string ACCESS_TOKEN_KEY = "GameDriveAccessToken2";

        string _accessTokenKey;
        string _refreshTokenKey;

        string _encryptionKeyRefreshToken = SystemInfo.deviceUniqueIdentifier;
        public ClientAutoRotateTokensManager ClientAutoRotateTokensManager { get; private set; }
        public string ClientId { get; private set; }
        public ClientTokensManager(string clientId, Client client)
        {
            _refreshTokenKey = clientId + REFRESH_TOKEN_KEY;
            _accessTokenKey = clientId + ACCESS_TOKEN_KEY;
            ClientId = clientId;
            ClientAutoRotateTokensManager = new ClientAutoRotateTokensManager(client);

            Initialize();
        }
        private void Initialize()
        {
            if (string.IsNullOrEmpty(_encryptionKeyRefreshToken))
            {
                _encryptionKeyRefreshToken = "enocrypt_key_" + ClientId;
            };

            //load from player prefs
            _accessToken = PlayerPrefs.GetString(_accessTokenKey);
            LoadAccessToken();

            //isPlaying only to support editor mode
            if (Application.isPlaying)
            {
                CoroutineHelper.Instance.StartCoroutine(InitialAutoRotateRefreshTokenCoroutine());
            }

        }

        IEnumerator InitialAutoRotateRefreshTokenCoroutine()
        {
            yield return null;
            ClientAutoRotateTokensManager.Initialize();
        }


        public void SetAccessToken(string accessToken)
        {
            _accessToken = accessToken;
            PlayerPrefs.SetString(_accessTokenKey, accessToken);
        }

        public void SetAccessTokenExpiresInSeconds(int accessTokenExpiresInSeconds)
        {
            ClientAutoRotateTokensManager.SetAutoRotateInNextSeconds(accessTokenExpiresInSeconds);
        }

        public string GetAccessToken()
        {
            return _accessToken;
        }

        public void SetRefreshToken(string refreshToken)
        {
            try
            {
                string refreshTokenEncrypt = EncryptHelper.Encrypt(refreshToken, _encryptionKeyRefreshToken);
                PlayerPrefs.SetString(_refreshTokenKey, refreshTokenEncrypt);
                _refreshToken = refreshToken;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error Encrypt refresh token: " + ex);
            }
        }

        public string GetRefreshToken()
        {
            return _refreshToken;
        }

        private void LoadAccessToken()
        {
            string encryptRefreshToken = PlayerPrefs.GetString(_refreshTokenKey);
            if (!String.IsNullOrEmpty(encryptRefreshToken))
            {
                try
                {
                    string refreshTokenEncrypt = EncryptHelper.Decrypt(encryptRefreshToken, _encryptionKeyRefreshToken);
                    _refreshToken = refreshTokenEncrypt;
                }
                catch (Exception ex)
                {
                    Debug.LogError("Error Decrypt refresh token: " + ex);
                    _refreshToken = null;
                }
            }
        }
    }

}
