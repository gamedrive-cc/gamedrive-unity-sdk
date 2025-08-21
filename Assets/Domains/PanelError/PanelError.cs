using System.Collections;
using UnityEngine;

namespace GameDriveSample
{
    public class PanelError : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TMP_Text _textError;

        public static PanelError Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        Coroutine hideTextCoroutine;
        public void ShowErrorMessage(string text, float hideInSecond)
        {
            _textError.gameObject.SetActive(true);
            _textError.text = text;
            if (hideTextCoroutine != null)
            {
                StopCoroutine(hideTextCoroutine);
            }
            hideTextCoroutine = StartCoroutine(HideTextInt(hideInSecond));
        }

        private IEnumerator HideTextInt(float hideInSecond)
        {
            yield return new WaitForSeconds(hideInSecond);
            _textError.gameObject.SetActive(false);
            hideTextCoroutine = null;
        }
    }

}