namespace ArtArmin.BoardGamesVR
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System;
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(LibraryItemSO))]
    public class LibraryItemManifestModelEditor : Editor
    {
        private const string manifestFileName = "manifest.asset";

        private TextAsset manifestTextAssetToLoadFrom;

        private static readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
            Formatting = Formatting.Indented,
        };

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space(32);

            LibraryItemSO myTarget = (LibraryItemSO)target;

            EditorGUILayout.HelpBox("Use the load button to load an existing manifest file content here", MessageType.Info);

            manifestTextAssetToLoadFrom = (TextAsset)EditorGUILayout.ObjectField("Load from manifest file", manifestTextAssetToLoadFrom, typeof(TextAsset), true);

            EditorGUI.BeginDisabledGroup(manifestTextAssetToLoadFrom == null);

            if (GUILayout.Button("Load"))
            {
                myTarget.manifest = LoadManifestFromAsset(manifestTextAssetToLoadFrom);
                EditorUtility.SetDirty(myTarget);
            }

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space(16);

            EditorGUILayout.HelpBox("Use the Save button to save the current info into a manifest file in the same path", MessageType.Info);

            if (GUILayout.Button($"Save {manifestFileName}"))
            {
                SaveManifest(myTarget.manifest);
            }
        }

        public void SaveManifest(LibraryItemManifestModel manifest)
        {
            var manifestString = JsonConvert.SerializeObject(manifest, jsonSerializerSettings);
            var textAsset = new TextAsset(manifestString);
            var targetPath = AssetDatabase.GetAssetPath(target);
            var folderPath = Path.GetDirectoryName(targetPath);

            AssetDatabase.Refresh();
            AssetDatabase.CreateAsset(textAsset, $"{folderPath}/{manifestFileName}");
            AssetDatabase.Refresh();
        }

        public LibraryItemManifestModel LoadManifestFromAsset(TextAsset textAsset)
        {
            var manifestString = textAsset.text;
            try
            {
                var manifest = JsonConvert.DeserializeObject<LibraryItemManifestModel>(manifestString, jsonSerializerSettings);
                return manifest;
            }
            catch (Exception) { }
            return new LibraryItemManifestModel();
        }
    }
}