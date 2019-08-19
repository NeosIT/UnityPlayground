using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Karambolo.PO;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.Globalization;
using static UnityEngine.Globalization.Translation;

namespace UnityEditor.Globalization
{
    public class EditorTranslation : IPreprocessBuildWithReport
    {
        private static FileSystemWatcher _fileSystemWatcher;
        private const string EDITOR_LANGUAGE_KEY = "Editor.kEditorLocale";

        private static void BuildKeyCache()
        {
            if (!Directory.Exists(LOCALIZATION_RESOURCES_DIR)) return;

            var parser = new POParser();
            var files = Directory.GetFiles(LOCALIZATION_RESOURCES_DIR, "*.po", SearchOption.TopDirectoryOnly);
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

            if (AssetDatabase.LoadAssetAtPath<TextAsset>(LOCALIZATION_ALL_PATH))
            {
                AssetDatabase.DeleteAsset(LOCALIZATION_ALL_PATH);
            }

            var assetPath = $"{LOCALIZATION_ALL_PATH}.txt";
            File.WriteAllText(assetPath, string.Join("\n", keys));
            AssetDatabase.Refresh();
        }

        [InitializeOnLoadMethod]
        internal static void Init()
        {
            var str = EditorPrefs.GetString(EDITOR_LANGUAGE_KEY);
            BuildKeyCache();
            Language = Enum.TryParse<SystemLanguage>(str, out var lang) ? lang : SystemLanguage.Unknown;

            if (Directory.Exists(LOCALIZATION_RESOURCES_DIR))
            {
                ReloadLanguagesMenu();
            }

            _fileSystemWatcher?.Dispose();
            _fileSystemWatcher = new FileSystemWatcher
            {
                Path = LOCALIZATION_RESOURCES_DIR,
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
            ReloadLanguages();
        }

        [MenuItem("Help/I18N/Reload Translations", true)]
        internal static bool ReloadLanguagesValidate()
        {
            return Directory.Exists(LOCALIZATION_RESOURCES_DIR);
        }

        [MenuItem("Help/I18N/Build Translation Key Cache", true)]
        internal static bool BuildKeyCacheMenuValidate()
        {
            return Directory.Exists(LOCALIZATION_RESOURCES_DIR);
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

        public static void PropertyField(SerializedProperty prop, string tooltip = null)
        {
            // TODO test this. thanks to unity this has to be very dirty and must be tested!

            Type GetFieldType()
            {
                // do some hacking because serializedproperty does not expose its objectreference type
                return prop.serializedObject.targetObject.GetType().GetField(prop.name).FieldType;
            }

            var label = new GUIContent(_(prop.displayName), string.IsNullOrWhiteSpace(tooltip) ? null : _(tooltip));

            switch (prop.propertyType)
            {
                case SerializedPropertyType.Boolean:
                    prop.boolValue = EditorGUILayout.Toggle(label, prop.boolValue);
                    break;
                case SerializedPropertyType.Vector2:
                    prop.vector2Value = EditorGUILayout.Vector2Field(label, prop.vector2Value);
                    break;
                case SerializedPropertyType.Float:
                    prop.floatValue = EditorGUILayout.FloatField(label, prop.floatValue);
                    break;
                case SerializedPropertyType.Enum:
                    var displayedOptions = Enum.GetNames(GetFieldType()).Select(_).ToArray();
                    prop.enumValueIndex = EditorGUILayout.Popup(label, prop.enumValueIndex, displayedOptions);
                    break;
                case SerializedPropertyType.ObjectReference:
                    prop.objectReferenceValue = EditorGUILayout.ObjectField(label, prop.objectReferenceValue, GetFieldType(), true);
                    break;
                default:
                    Debug.LogWarning("Unknown property type: " + prop.propertyType);
                    break;
            }
        }

        public static bool PropertyField<T>(SerializedProperty prop)
        {
            return EditorGUILayout.Toggle(_(prop.displayName), prop.boolValue);
        }

        public static void PropertyTagField(SerializedProperty prop)
        {
            prop.stringValue = EditorGUILayout.TagField(_(prop.displayName), prop.stringValue);

        }
    }
}
