using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Karambolo.PO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace _INTERNAL_.Scripts.Utilities
{
    public class EditorTranslation : IPreprocessBuildWithReport
    {
        public const string EDITOR_LANGUAGE_KEY = "Editor.kEditorLocale";

        [InitializeOnLoadMethod]
        public static void Init()
        {
            var str = EditorPrefs.GetString(EDITOR_LANGUAGE_KEY);
            BuildKeyCache();
            Translation.Language = Enum.TryParse<SystemLanguage>(str, out var lang) ? lang : SystemLanguage.Unknown;

            if (Directory.Exists(Translation.LOCALIZATION_RESOURCES_DIR))
            {
                ReloadLanguages();
            }
        }

        [MenuItem("Help/I18N/Reload Translations")]
        public static void ReloadLanguages()
        {
            Translation.ReloadLanguages();
        }

        [MenuItem("Help/I18N/Reload Translations", true)]
        public static bool ReloadLanguagesValidate()
        {
            return Directory.Exists(Translation.LOCALIZATION_RESOURCES_DIR);
        }

        public int callbackOrder { get; }

        public void OnPreprocessBuild(BuildReport report)
        {
            BuildKeyCache();
        }

        [MenuItem("Help/I18N/Build Translation Key Cache")]
        public static void BuildKeyCacheMenu()
        {
            BuildKeyCache();
        }

        [MenuItem("Help/I18N/Build Translation Key Cache", true)]
        public static bool BuildKeyCacheMenuValidate()
        {
            return Directory.Exists(Translation.LOCALIZATION_RESOURCES_DIR);
        }

        public static void BuildKeyCache()
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
    }
}
