using System.Collections;
using UnityEngine;

namespace GameDriveSample
{
    public class PanelError : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TMP_Text _textError;

        [SerializeField]
        private GameObject _panelTextBg;

        public static PanelError Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        Coroutine hideTextCoroutine;
        public void ShowErrorMessage(string text, float hideInSecond)
        {
            _panelTextBg.SetActive(true);
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
            _panelTextBg.SetActive(false);
            hideTextCoroutine = null;
        }
    }

}