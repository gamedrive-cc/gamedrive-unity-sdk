using UnityEngine;

namespace GameDriveSample
{
    public class PanelPlayerInfo : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TMP_InputField _inputFieldId;

        [SerializeField]
        private TMPro.TextMeshProUGUI _textPlayerName;

        public void SetPlayerInfo(GameDrive.Player player)
        {
            _inputFieldId.text = player._id;
            _textPlayerName.text = player.name;
        }
    }
}