using UnityEngine;

namespace GameDriveSample
{
    public enum Panels
    {
        Authentication,
        PlayerAndScore,
    }

    public class PanelsController : MonoBehaviour
    {

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public static PanelsController Instance;

        [SerializeField] private GameObject _panelAuthentication;
        [SerializeField] private GameObject _panelPlayerAndScore;

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
            _panelPlayerAndScore.SetActive(false);
            switch (panels)
            {
                case Panels.Authentication:
                    _panelAuthentication.SetActive(true);
                    break;
                case Panels.PlayerAndScore:
                    _panelPlayerAndScore.SetActive(true);
                    break;
            }
        }

    }

}