using UnityEngine;

namespace GameDrive
{
    public class GameDriveInitializerGameObject : MonoBehaviour
    {
        [SerializeField]
        private string _gameDriveConfigsObjectPath =
            GamedriveConfigsObject.CONFIGS_ASSET_PATH;

        [SerializeField]
        private GamedriveConfigsObject _gameDriveConfigsObject;

        private static bool _isInitialized = false;

        private void Awake()
        {
            if (_isInitialized)
            {
                // Prevent duplicate initialization
                Destroy(gameObject);
                Debug.LogWarning(
                    "You are trying to spawn GameDriveInitializerGameObject more than once"
                );
                return;
            }

            if (_gameDriveConfigsObject == null)
            {
                Debug.LogError(
                    "GameDrive's config not found. " +
                    "Open Window->GameDrive to auto generate one."
                );
                return;
            }

            GameDriveInitializer.Initial(_gameDriveConfigsObject);

            _isInitialized = true;

            DontDestroyOnLoad(gameObject);
        }
    }
}