using UnityEditor;
using UnityEngine;

namespace GameDrive
{
    public class GameDriveInitializerGameObject : MonoBehaviour
    {
        [SerializeField]
        private string _gameDriveConfigsObjectPath = GamedriveConfigsObject.CONFIGS_ASSET_PATH;

        private static bool _isInitialized = false;
        private void Awake()
        {
            if (_isInitialized)
            {
                //Protect to go back to the same scene and recreate this script gameobject again
                GameObject.Destroy(gameObject);
                Debug.LogWarning("you are trying to spawn GameDriveInitializerGameObject more than once");
                return;
            }

            GameObject.DontDestroyOnLoad(this);
            string path = _gameDriveConfigsObjectPath;
            var config = AssetDatabase.LoadAssetAtPath<GamedriveConfigsObject>(path);
            if (!config)
            {
                Debug.LogError("GameDrive's config not found by path, open Window->GameDrive to auto generate one");
            }
            else
            {
                GameDriveInitializer.Initial(config);
                _isInitialized = true;
            }
        }
    }
}
