using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameDrive
{
    public class GameDriveWindow : EditorWindow
    {
        private GamedriveConfigsObject _config;

        Texture2D gameDriveIcon;

        public bool _test;
        private Stage _currentStage;

        // Add menu item named "My Window" to the Window menu
        [MenuItem("Window/GameDrive")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            EditorWindow.GetWindow(typeof(GameDriveWindow), false, "GameDrive");
        }

        protected void OnEnable()
        {
            LoadConfigSO();
            gameDriveIcon = Resources.Load("GameDriveLogo", typeof(Texture2D)) as Texture2D;

        }

        private void LoadConfigSO()
        {
            string path = GamedriveConfigsObject.CONFIGS_ASSET_PATH;
            _config = AssetDatabase.LoadAssetAtPath<GamedriveConfigsObject>(path);
            if (!_config)
            {
                _config = CreateInstance<GamedriveConfigsObject>();
                AssetDatabase.CreateAsset(_config, path);
                AssetDatabase.Refresh();
                _currentStage = _config.stage;
            }

        }

        protected void OnDisable()
        {

        }


        void OnGUI()
        {
            GUILayout.Label("GameDrive Configs", EditorStyles.boldLabel);

            GUILayout.Box(gameDriveIcon);
            _config.projectId = EditorGUILayout.TextField("ProjectId", _config.projectId);
            _config.apiSecret = EditorGUILayout.PasswordField("Api Secret", _config.apiSecret);
            _config.region = (Region)EditorGUILayout.EnumPopup("Region", _config.region);
            _config.stage = (Stage)EditorGUILayout.EnumPopup("Stage", _config.stage);

            if (_currentStage != _config.stage)
            {
                _currentStage = _config.stage;
                Authentication.Logout();
            }

            if (GUILayout.Button("Logout default player", GUILayout.Width(250)))
            {
                Authentication.Logout();
            }

#if GAMEDRIVE_TEST
            _config.testing = EditorGUILayout.BeginToggleGroup("Testing", _config.testing);
            _config.testConfig.localhost = EditorGUILayout.Toggle("Localhost", _config.testConfig.localhost);
            if (!_config.testConfig.localhost)
            {
                _config.testConfig.serverAddress = EditorGUILayout.TextField("ServiceAddress", _config.testConfig.serverAddress);
                _config.testConfig.useSecureProtocal = EditorGUILayout.Toggle("UseSecureProtocal", _config.testConfig.useSecureProtocal);
                if (_config.testConfig.useSecureProtocal)
                {
                    _config.testConfig.httpsPort = EditorGUILayout.TextField("HttpsPort", _config.testConfig.httpsPort);
                }
                else
                {
                    _config.testConfig.httpPort = EditorGUILayout.TextField("HttpPort", _config.testConfig.httpPort);
                }
            }
            else
            {
                _config.testConfig.httpPort = EditorGUILayout.TextField("HttpPort", _config.testConfig.httpPort);
            }

            EditorGUILayout.EndToggleGroup();
#endif

            EditorUtility.SetDirty(_config);
        }
    }
}
