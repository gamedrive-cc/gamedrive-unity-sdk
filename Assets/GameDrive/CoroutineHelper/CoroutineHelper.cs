using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameDrive
{
    public class CoroutineHelper : MonoBehaviour
    {
        public static CoroutineHelper Instance;

        public bool IsDestroyed { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public void RestartCoroutine(ref Coroutine coroutine, IEnumerator enumerator)
        {
            StopCoroutineAndSetNull(ref coroutine);
            coroutine = StartCoroutine(enumerator);
        }

        public void RestartCoroutine(ref Coroutine coroutine, IEnumerator enumerator, MonoBehaviour mono)
        {
            StopCoroutineAndSetNull(ref coroutine, mono);
            coroutine = mono.StartCoroutine(enumerator);
        }

        public void StopCoroutineAndSetNull(ref Coroutine coroutine)
        {
            if (coroutine != null && !IsDestroyed)
            {
                StopCoroutine(coroutine);
            }

            coroutine = null;
        }

        public void StopCoroutineAndSetNull(ref Coroutine coroutine, MonoBehaviour mono)
        {
            if (!IsDestroyed && coroutine != null)
            {
                mono.StopCoroutine(coroutine);
            }

            coroutine = null;
        }

        public Coroutine StartCoroutineOnHelper(IEnumerator enumerator)
        {
            return StartCoroutine(enumerator);
        }

        public void StopCoroutineOnHelper(Coroutine coroutine)
        {
            if (!IsDestroyed && coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }

        private void OnDestroy()
        {
            IsDestroyed = true;
            StopAllCoroutines();
        }
    }
}