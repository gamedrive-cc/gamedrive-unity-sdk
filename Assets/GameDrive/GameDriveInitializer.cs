using GameDrive.Network;
using UnityEngine;

namespace GameDrive
{
    public class GameDriveInitializer
    {
        public static GamedriveConfigsObject Config { get; private set; }
        private static bool isInitialized = false;

        public static void Initial(GamedriveConfigsObject configSO)
        {
            if (isInitialized)
            {
                throw new System.Exception("Initial() not allow to call multiple times");
            }
            Config = UnityEngine.GameObject.Instantiate(configSO);
            if (_isForcedStage)
            {
                Config.stage = _forcedStage;
            }

            GameObject gameOject = new GameObject();
            gameOject.name = "GameDrive Object Don't DestroyOnLoad";
            Object.DontDestroyOnLoad(gameOject);

            StartMonitors(gameOject);
            UpdateSettings();
            ShowPreviewText(gameOject);
            StartGameDriveCoroutineHelper(gameOject);
            NetworkInstaller.Install(gameOject);
            isInitialized = true;
        }

        public static Settings Settings { get; private set; }

        private static bool _isForcedStage = false;
        private static Stage _forcedStage;

        public static void ForceInitalizeStage(Stage stage)
        {
            _isForcedStage = true;
            _forcedStage = stage;
        }

        private static void UpdateSettings()
        {
            Settings = new Settings();
            Settings.apiInfo = new ApiInfo();
            Settings.apiInfo.secret = Config.apiSecret;
            switch (Config.region)
            {
                case Region.SOUTHEAST_ASIA:
                    Settings.apiInfo.serverAddress = "southeast-asia.gamedrive.cc";
                    break;
            }

#if GAMEDRIVE_TEST
            if (Config.testing)
            {
                Settings.apiInfo.serverAddress = Config.testConfig.serverAddress;
                Settings.UseSecureProtocal = Config.testConfig.useSecureProtocal;
                Settings.HttpPort = Config.testConfig.httpPort;
                Settings.HttpsPort = Config.testConfig.httpsPort;

                if (Config.testConfig.localhost)
                {
                    Settings.apiInfo.serverAddress = "localhost";
                    Settings.UseSecureProtocal = false;
                }
            }
#endif

        }

        private static void ShowPreviewText(GameObject gameObject)
        {
            if (Config.stage == Stage.PREVIEW)
            {
                gameObject.AddComponent<ShowPreviewTextComponent>();
            }
        }

        private static void StartGameDriveCoroutineHelper(GameObject gameObject)
        {
            gameObject.AddComponent<CoroutineHelper>();
        }

        private static void StartMonitors(GameObject gameObject)
        {
            gameObject.AddComponent<OnApplicationFocusMonitor>();
        }
    }
}
