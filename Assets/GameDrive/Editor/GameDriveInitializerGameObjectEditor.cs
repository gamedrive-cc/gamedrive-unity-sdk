using UnityEditor;
using UnityEngine;

namespace GameDrive
{
    [CustomEditor(typeof(GameDriveInitializerGameObject))]
    public class GameDriveInitializerGameObjectEditor : Editor
    {
        private SerializedProperty _configPathProperty;
        private SerializedProperty _configProperty;

        private void OnEnable()
        {
            _configPathProperty = serializedObject.FindProperty(
                "_gameDriveConfigsObjectPath"
            );

            _configProperty = serializedObject.FindProperty(
                "_gameDriveConfigsObject"
            );

            LoadConfigIfMissing();
        }

        private void LoadConfigIfMissing()
        {
            serializedObject.Update();

            // Already assigned, do nothing
            if (_configProperty.objectReferenceValue != null)
                return;

            string path = _configPathProperty.stringValue;

            if (string.IsNullOrEmpty(path))
                return;

            var config =
                AssetDatabase.LoadAssetAtPath<GamedriveConfigsObject>(path);

            if (config == null) {
                config = CreateInstance<GamedriveConfigsObject>();
                AssetDatabase.CreateAsset(config, path);
                AssetDatabase.Refresh();
            }

            _configProperty.objectReferenceValue = config;

            serializedObject.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Show the default Script field (read-only)
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.ObjectField(
                    "Script",
                    MonoScript.FromMonoBehaviour(
                        (GameDriveInitializerGameObject)target
                    ),
                    typeof(GameDriveInitializerGameObject),
                    false
                );
            }

            EditorGUILayout.PropertyField(_configPathProperty);
            EditorGUILayout.PropertyField(_configProperty);

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Load Config From Path"))
            {
                LoadConfigIfMissing();
            }
        }
    }
}