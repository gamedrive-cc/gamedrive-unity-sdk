using System;
using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;

namespace GameDrive
{
    public class ClientAutoRotateTokensManager
    {
        Client _client;
        public ClientAutoRotateTokensManager(Client client)
        {
            _client = client;
            OnApplicationFocusMonitor.ActionApplicationFocus += OnApplicationFocus;
        }

        DateTime _toRotateDateTime;
        const int offsetBeforeExpires = 15;
        Coroutine _rotateTokenCoroutine;
        int _checkInSeconds;
        public bool IsRotating { get; private set; }

        public void Initialize()
        {
            LoadToRotateDateTime();
            CheckToRotate();
        }

        private string GenerateToRotateDateTimeKey()
        {
            return _client.ClientId + "_to_rotate_date_time";
        }

        public void SetAutoRotateInNextSeconds(int expiresInSeconds)
        {
            _checkInSeconds = expiresInSeconds - offsetBeforeExpires;
            if (_checkInSeconds <= 0)
            {
                Debug.LogError("SetAutoRotateInNextSeconds _checkInSeconds <= 0:" + _checkInSeconds);
                return;
            }
            DateTime now = DateTime.Now;
            SetToRotateDateTime(now.AddSeconds(_checkInSeconds));
            CoroutineHelper.Instance.RestartCoroutine(ref _rotateTokenCoroutine, CheckAndRotateTokenCoroutine(_checkInSeconds + 1));
        }

        private void SetToRotateDateTime(DateTime dateTime)
        {
            _toRotateDateTime = dateTime;
            string key = GenerateToRotateDateTimeKey();
            PlayerPrefs.SetString(key, dateTime.ToString());
        }

        private void LoadToRotateDateTime()
        {
            string key = GenerateToRotateDateTimeKey();
            string _toRotateDateTimeString = PlayerPrefs.GetString(key);
            if (!string.IsNullOrEmpty(_toRotateDateTimeString))
            {
                DateTime parsed = DateTime.Parse(_toRotateDateTimeString);
                SetToRotateDateTime(parsed);
            }
            else
            {
                //No rotate if not found so add to rotate higher
                SetToRotateDateTime(DateTime.Now.AddSeconds(60 * 60));
            }
        }

        IEnumerator CheckAndRotateTokenCoroutine(int checkInSeconds)
        {
            while (true)
            {
                yield return new WaitForSeconds(checkInSeconds);

                //check the
                CheckToRotate();
            }
        }

        private void OnApplicationFocus(bool focus)
        {
            if (focus && _checkInSeconds > 0)
            {
                CheckToRotate();
            }
        }

        private void CheckToRotate()
        {
            DateTime nowTick = DateTime.Now;
            if (nowTick >= _toRotateDateTime)
            {
                // do rotouen
                CoroutineHelper.Instance.StopCoroutineAndSetNull(ref _rotateTokenCoroutine);
                RotateToken();
            }
        }

        private void RotateToken()
        {
            if (string.IsNullOrEmpty(_client.GetTokenManager().GetRefreshToken()))
            {
                return;
            }
            SetRotating(false);
            Authorization.RotateTokens(() =>
            {
                //success
                SetRotating(false);
            }, (err) =>
            {
                SetRotating(false);
                Debug.LogError("ClientAutoRotateTokensManager RotateToken error" + err.ToString());
            }, _client);

            //need to set after because in side Authorization.RotateTokens have yield this condition
            SetRotating(true);
        }

        private void SetRotating(bool rotating)
        {
            IsRotating = rotating;
        }
    }
}
