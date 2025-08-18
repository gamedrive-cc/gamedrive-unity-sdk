using System;
using UnityEngine;

namespace GameDrive
{
    public class ClientTokensManager
    {
        string _accessToken;
        string _refreshToken;

        const string REFRESH_TOKEN_KEY = "GameDriveRefreshToken";
        const string ACCESS_TOKEN_KEY = "GameDriveAccessToken";

        string _accessTokenKey;
        string _refreshTokenKey;

        string _encryptionKeyRefreshToken = SystemInfo.deviceUniqueIdentifier;
        public ClientAutoRotateTokensManager ClientAutoRotateTokensManager { get; private set; }
        public ClientTokensManager(string clientId, Client client)
        {
            _refreshTokenKey = clientId + REFRESH_TOKEN_KEY;
            _accessTokenKey = clientId + ACCESS_TOKEN_KEY;
            ClientAutoRotateTokensManager = new ClientAutoRotateTokensManager(client);
            if (string.IsNullOrEmpty(_encryptionKeyRefreshToken))
            {
                _encryptionKeyRefreshToken = "enocrypt_key_" + clientId;
            };

            Initialize();
        }
        private void Initialize()
        {
            //load from player prefs
            _accessToken = PlayerPrefs.GetString(_accessTokenKey);
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
                Debug.LogError("Error saving token: " + ex);
            }
        }

        public string GetRefreshToken()
        {
            if (String.IsNullOrEmpty(_refreshToken))
            {
                string encryptRefreshToken = PlayerPrefs.GetString(_refreshTokenKey);
                if (!String.IsNullOrEmpty(encryptRefreshToken))
                {
                    string refreshTokenEncrypt = EncryptHelper.Decrypt(encryptRefreshToken, _encryptionKeyRefreshToken);
                    _refreshToken = refreshTokenEncrypt;
                }
            }
            return _refreshToken;
        }
    }

}
