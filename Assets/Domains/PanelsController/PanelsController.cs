using UnityEngine;

namespace GameDriveSample
{
    public enum Panels
    {
        Authentication,
        PlayerAssets,
    }

    public class PanelsController : MonoBehaviour
    {

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public static PanelsController Instance;

        [SerializeField] private GameObject _panelAuthentication;
        [SerializeField] private GameObject _panelPlayerAssets;
        [SerializeField] private GameObject _panelPlayerInfo;

        public void Awake()
        {
            Instance = this;
        }

        public void Start()
        {
            SetPanel(Panels.Authentication);
        }

        public void SetPanel(Panels panels)
        {
            _panelAuthentication.SetActive(false);
            _panelPlayerAssets.SetActive(false);
            _panelPlayerInfo.SetActive(false);
            switch (panels)
            {
                case Panels.Authentication:
                    _panelAuthentication.SetActive(true);
                    break;
                case Panels.PlayerAssets:
                    _panelPlayerAssets.SetActive(true);
                    _panelPlayerInfo.SetActive(true);
                    break;
            }
        }

    }

}