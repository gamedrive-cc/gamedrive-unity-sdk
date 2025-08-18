using UnityEngine;

namespace GameDrive
{
    public class ShowPreviewTextComponent : MonoBehaviour
    {

        private GUIStyle guiStyle = new GUIStyle();

        private void OnEnable()
        {
            float screenHeight = Screen.height;
            float desiredFontSize = screenHeight * 0.035f;
            guiStyle.fontSize = Mathf.FloorToInt(desiredFontSize);
            guiStyle.fontStyle = FontStyle.Italic;
        }

        void OnGUI()
        {
            GUILayout.Label("GameDrive - PREVIEW", guiStyle);
        }
    }
}
