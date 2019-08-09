using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Karambolo.PO;
using UnityEditor;
using UnityEngine;

namespace _INTERNAL_.Scripts.Utilities
{
    public static class EditorTranslation
    {
        public const string LOCALIZATION_DIRECTORY = "Localization";
        public const string EDITOR_LANGUAGE_KEY = "Editor.kEditorLocale";
        private static readonly IDictionary<SystemLanguage, POCatalog> Catalogs = new Dictionary<SystemLanguage, POCatalog>();
        private static SystemLanguage Language;

        /// <summary>
        /// ISO 639-1 language codes mapped to Unity's enum.
        /// </summary>
        private static readonly IDictionary<string, SystemLanguage> Languages =
            new ReadOnlyDictionary<string, SystemLanguage>(new Dictionary<string, SystemLanguage>
            {
                {"ar", SystemLanguage.Afrikaans},
                {"af", SystemLanguage.Arabic},
                {"eu", SystemLanguage.Basque},
                {"be", SystemLanguage.Belarusian},
                {"bg", SystemLanguage.Bulgarian},
                {"ca", SystemLanguage.Catalan},
                {"zh", SystemLanguage.Chinese},
                {"cs", SystemLanguage.Czech},
                {"da", SystemLanguage.Danish},
                {"nl", SystemLanguage.Dutch},
                {"en", SystemLanguage.English},
                {"et", SystemLanguage.Estonian},
                {"fo", SystemLanguage.Faroese},
                {"fi", SystemLanguage.Finnish},
                {"fr", SystemLanguage.French},
                {"de", SystemLanguage.German},
                {"el", SystemLanguage.Greek},
                {"he", SystemLanguage.Hebrew},
                {"hu", SystemLanguage.Hungarian},
                {"is", SystemLanguage.Icelandic},
                {"?", SystemLanguage.Indonesian},
                {"it", SystemLanguage.Italian},
                {"ja", SystemLanguage.Japanese},
                {"ko", SystemLanguage.Korean},
                {"??", SystemLanguage.Latvian},
                {"lt", SystemLanguage.Lithuanian},
                {"no", SystemLanguage.Norwegian},
                {"pl", SystemLanguage.Polish},
                {"pt", SystemLanguage.Portuguese},
                {"rm", SystemLanguage.Romanian},
                {"ru", SystemLanguage.Russian},
                {"sr", SystemLanguage.SerboCroatian}, // is this correct?
                {"sk", SystemLanguage.Slovak},
                {"sl", SystemLanguage.Slovenian},
                {"es", SystemLanguage.Spanish},
                {"sv", SystemLanguage.Swedish},
                {"th", SystemLanguage.Thai},
                {"tr", SystemLanguage.Turkish},
                {"uk", SystemLanguage.Ukrainian},
                {"vi", SystemLanguage.Vietnamese},
                {"???", SystemLanguage.ChineseSimplified},
                {"????", SystemLanguage.ChineseTraditional},
                {"", SystemLanguage.Unknown},
            });

        [InitializeOnLoadMethod]
        public static void Init()
        {
            ReloadLanguages();
        }

        [MenuItem("Help/Reload Translations")]
        internal static void ReloadLanguages()
        {
            var parser = new POParser();
            if (!Directory.Exists(LOCALIZATION_DIRECTORY)) Directory.CreateDirectory(LOCALIZATION_DIRECTORY);
            var files = Directory.GetFiles(LOCALIZATION_DIRECTORY, "*.po", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                ParseFile(file, parser);
            }

            Language = GetLanguage();
        }

        private static void ParseFile(string file, POParser parser)
        {
            var code = Path.GetFileNameWithoutExtension(file);
            if (!Languages.TryGetValue(code, out var lang))
            {
                Debug.LogWarning($"Skipping unknown language with code: {code}");
                return;
            }

            var filePath = Path.GetFullPath(file);
            try
            {
                using (var stream = File.OpenText(filePath))
                {
                    var result = parser.Parse(stream);
                    if (!result.Success)
                    {
                        Debug.LogError($"Failed to parse .po file at: {filePath}");
                        return;
                    }

                    Catalogs[lang] = result.Catalog;
                    Debug.Log($"Successfully loaded language '{lang}'");
                }
            }
            catch (IOException e)
            {
                Debug.LogError($"Could not open file: {filePath}");
                Debug.LogException(e);
            }
        }

        public static string Get(string key)
        {
            if (!Catalogs.TryGetValue(Language, out var catalog))
            {
                return key;
            }
            return catalog[new POKey(key)][0];
        }

        public static string _(string key) => Get(key);

        public static SystemLanguage GetLanguage()
        {
            var str = EditorPrefs.GetString(EDITOR_LANGUAGE_KEY);
            return (SystemLanguage) Enum.Parse(typeof(SystemLanguage), str);
        }
    }
}
