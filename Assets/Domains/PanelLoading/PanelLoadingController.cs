using System.Collections.Generic;
using UnityEngine;

namespace GameDriveSample
{
    public class PanelLoadingController : MonoBehaviour
    {
        public static PanelLoadingController Instance;

        [SerializeField] private GameObject panel;
        private void Awake()
        {
            Instance = this;
        }

        private HashSet<string> keys = new HashSet<string>();

        public void ShowLoading(string key)
        {
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