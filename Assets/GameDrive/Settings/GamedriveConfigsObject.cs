using UnityEngine;

namespace GameDrive
{
    public class GamedriveConfigsObject : ScriptableObject
    {
        public const string CONFIGS_ASSET_PATH = "Assets/GameDriveConfigs.asset";

        public string projectId;
        public string apiSecret;
        public Stage stage = Stage.LIVE;
        public Region region = Region.SOUTHEAST_ASIA;

#if GAMEDRIVE_TEST
        public bool testing;
        public TestConfig testConfig;
#endif
    }
}
