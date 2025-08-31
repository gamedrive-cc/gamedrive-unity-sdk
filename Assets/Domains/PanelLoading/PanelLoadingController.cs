using System.Collections.Generic;
using UnityEngine;

namespace GameDriveSample
{
    public class PanelLoadingController : MonoBehaviour
    {
        public static PanelLoadingController Instance;

        [SerializeField] private GameObject panel;
        [SerializeField] private TMPro.TextMeshProUGUI _text;
        string _defaultText = "";
        private void Awake()
        {
            Instance = this;
            if (_text != null)
            {
                _defaultText = _text.text;
            }
        }

        private HashSet<string> keys = new HashSet<string>();

        public void ShowLoading(string key, string text = "")
        {
            string newText = text;
            if (string.IsNullOrEmpty(newText))
            {
                newText = _defaultText;
            }

            _text.text = newText;
            keys.Add(key);
            UpdateShowingPanel();
        }

        public void HideLoading(string key)
        {
            keys.Remove(key);
            UpdateShowingPanel();
        }

        private void UpdateShowingPanel()
        {
            panel.SetActive(keys.Count > 0);
        }
    }

}