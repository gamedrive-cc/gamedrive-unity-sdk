using UnityEngine;

namespace GameDriveSample
{
    public class PanelClientInfo : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void OnEnable()
        {
            OnClientSelected();
            ClientSelectController.Instance.ActionClientSelect += OnClientSelected;
        }

        private void OnDisable()
        {
            ClientSelectController.Instance.ActionClientSelect -= OnClientSelected;
        }

        private void OnClientSelected()
        {
            var client = ClientSelectController.Instance.CurrentClient;
           
        }
    }
}