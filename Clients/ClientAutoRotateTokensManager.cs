using System;
using System.Collections;
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

        public void SetAutoRotateInNextSeconds(int expiresInSeconds)
        {
            _checkInSeconds = expiresInSeconds - offsetBeforeExpires;
            if (_checkInSeconds <= 0)
            {
                Debug.LogError("SetAutoRotateInNextSeconds _checkInSeconds <= 0:" + _checkInSeconds);
                return;
            }
            DateTime now = DateTime.Now;
            _toRotateDateTime = now.AddSeconds(_checkInSeconds);
            CoroutineHelper.Instance.RestartCoroutine(ref _rotateTokenCoroutine, CheckAndRotateTokenCoroutine(_checkInSeconds + 1));
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
