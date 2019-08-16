using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Karambolo.PO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.Globalization;
using Object = UnityEngine.Object;

namespace UnityEditor.Globalization
{
    public class EditorTranslation : IPreprocessBuildWithReport
    {
        private static FileSystemWatcher _fileSystemWatcher;
        private const string EDITOR_LANGUAGE_KEY = "Editor.kEditorLocale";

        private static void BuildKeyCache()
        {
            if (!Directory.Exists(Translation.LOCALIZATION_RESOURCES_DIR)) return;

            var parser = new POParser();
            var files = Directory.GetFiles(Translation.LOCALIZATION_RESOURCES_DIR, "*.po", SearchOption.TopDirectoryOnly);
            var keys = new List<string>();
            foreach (var file in files)
            {
                using (var stream = File.OpenRead(file))
                {
                    var result = parser.Parse(stream);
                    foreach (var key in result.Catalog.Keys.ToArray())
                    {
                        if (!keys.Contains(key.Id))
                        {
                            keys.Add(key.Id);
                        }
                    }
                }
            }

            if (AssetDatabase.LoadAssetAtPath<TextAsset>(Translation.LOCALIZATION_ALL_PATH))
            {
                AssetDatabase.DeleteAsset(Translation.LOCALIZATION_ALL_PATH);
            }

            var assetPath = $"{Translation.LOCALIZATION_ALL_PATH}.txt";
            File.WriteAllText(assetPath, string.Join("\n", keys));
            AssetDatabase.Refresh();
        }

        [InitializeOnLoadMethod]
        internal static void Init()
        {
            var str = EditorPrefs.GetString(EDITOR_LANGUAGE_KEY);
            BuildKeyCache();
            Translation.Language = Enum.TryParse<SystemLanguage>(str, out var lang) ? lang : SystemLanguage.Unknown;

            if (Directory.Exists(Translation.LOCALIZATION_RESOURCES_DIR))
            {
                ReloadLanguagesMenu();
            }

            _fileSystemWatcher?.Dispose();
            _fileSystemWatcher = new FileSystemWatcher
            {
                Path = Translation.LOCALIZATION_RESOURCES_DIR,
                Filter = "*.po",
                EnableRaisingEvents = true
            };
            _fileSystemWatcher.Changed += FileSystemWatcherOnChanged;
        }

        private static void FileSystemWatcherOnChanged(object sender, FileSystemEventArgs e)
        {
            EditorApplication.update += Update;
        }

        private static void Update()
        {
            BuildKeyCacheMenu();
            ReloadLanguagesMenu();
            EditorApplication.update -= Update;
        }

        [MenuItem("Help/I18N/Reload Translations")]
        internal static void ReloadLanguagesMenu()
        {
            Translation.ReloadLanguages();
        }

        [MenuItem("Help/I18N/Reload Translations", true)]
        internal static bool ReloadLanguagesValidate()
        {
            return Directory.Exists(Translation.LOCALIZATION_RESOURCES_DIR);
        }

        [MenuItem("Help/I18N/Build Translation Key Cache", true)]
        internal static bool BuildKeyCacheMenuValidate()
        {
            return Directory.Exists(Translation.LOCALIZATION_RESOURCES_DIR);
        }

        [MenuItem("Help/I18N/Build Translation Key Cache")]
        internal static void BuildKeyCacheMenu()
        {
            BuildKeyCache();
        }

        public int callbackOrder { get; }

        public void OnPreprocessBuild(BuildReport report)
        {
            BuildKeyCache();
        }

        public static void PropertyField(SerializedProperty obj)
        {
            var label = Translation._(obj.displayName);

            switch (obj.propertyType)
            {
                case SerializedPropertyType.Boolean:
                    EditorGUILayout.Toggle(label, obj.boolValue);
                    break;
                case SerializedPropertyType.Vector2:
                    EditorGUILayout.Vector2Field(label, obj.vector2Value);
                    break;
                default:
                    Debug.LogWarning("Unknown property type: " + obj.propertyType);
                    break;
            }
        }
    }
}
