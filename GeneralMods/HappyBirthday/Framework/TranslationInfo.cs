using System.Collections.Generic;
using System.IO;
using StardewValley;

namespace Omegasis.HappyBirthday.Framework
{
    /// <summary>A class which deals with handling different translations for Vocalization should other voice teams ever wish to voice act for that language.</summary>
    public class TranslationInfo
    {
        /// <summary>The list of all supported translations by this mod.</summary>
        public List<string> translations;

        /// <summary>The current translation mode for the mod, so that it knows what files to load at the beginning of the game.</summary>
        public string currentTranslation;

        /// <summary>Holds the info for what translation has what file extension.</summary>
        public Dictionary<string, string> translationFileInfo;

        public Dictionary<string, LocalizedContentManager.LanguageCode> translationCodes;

        /// <summary>Construct an instance..</summary>
        public TranslationInfo()
        {
            this.translations = new List<string>();

            this.translationFileInfo = new Dictionary<string, string>();
            this.translationCodes = new Dictionary<string, LocalizedContentManager.LanguageCode>();
            this.translations.Add("English");
            this.translations.Add("Spanish");
            this.translations.Add("Chinese");
            this.translations.Add("Japanese");
            this.translations.Add("Russian");
            this.translations.Add("German");
            this.translations.Add("Brazillian Portuguese");

            this.currentTranslation = "English";

            this.translationFileInfo.Add("English", ".json");
            this.translationFileInfo.Add("Spanish", ".es-ES.json");
            this.translationFileInfo.Add("Chinese", ".zh-CN.json");
            this.translationFileInfo.Add("Japanese", ".ja-JP.json");
            this.translationFileInfo.Add("Russian", ".ru-RU.json");
            this.translationFileInfo.Add("German", ".de-DE.json");
            this.translationFileInfo.Add("Brazillian Portuguese", ".pt-BR.json");


            this.translationCodes.Add("English", LocalizedContentManager.LanguageCode.en);
            this.translationCodes.Add("Spanish", LocalizedContentManager.LanguageCode.es);
            this.translationCodes.Add("Chinese", LocalizedContentManager.LanguageCode.zh);
            this.translationCodes.Add("Japanese", LocalizedContentManager.LanguageCode.ja);
            this.translationCodes.Add("Russian", LocalizedContentManager.LanguageCode.ru);
            this.translationCodes.Add("German", LocalizedContentManager.LanguageCode.de);
            this.translationCodes.Add("Brazillian Portuguese", LocalizedContentManager.LanguageCode.pt);

        }

        public string getTranslationNameFromPath(string fullPath)
        {
            return Path.GetFileName(fullPath);
        }

        public void changeLocalizedContentManagerFromTranslation(string translation)
        {
            string tra = this.getTranslationNameFromPath(translation);
            bool f = this.translationCodes.TryGetValue(tra, out LocalizedContentManager.LanguageCode code);
            if (!f) LocalizedContentManager.CurrentLanguageCode = LocalizedContentManager.LanguageCode.en;
            else LocalizedContentManager.CurrentLanguageCode = code;
            return;
        }

        public void resetLocalizationCode()
        {
            LocalizedContentManager.CurrentLanguageCode = LocalizedContentManager.LanguageCode.en;
        }

        /// <summary>Gets the proper file extension for the current translation.</summary>
        /// <param name="path"></param>
        public string getFileExtentionForTranslation(string path)
        {
            /*
            bool f = translationFileInfo.TryGetValue(translation, out string value);
            if (!f) return ".json";
            else return value;
            */
            string translation = Path.GetFileName(path);
            try
            {
                return this.translationFileInfo[translation];
            }
            catch
            {
                HappyBirthday.ModMonitor.Log("WTF SOMETHING IS WRONG!", StardewModdingAPI.LogLevel.Warn);
                //Vocalization.ModMonitor.Log(err.ToString());
                //Vocalization.ModMonitor.Log("Attempted to get translation: " + translation);
                return ".json";
            }
        }

        /// <summary>Gets the proper json for Buildings (aka Blueprints) from the data folder.</summary>
        public string getBuildingjsonForTranslation(string translation)
        {
            string buildings = "Blueprints";
            return buildings + this.getFileExtentionForTranslation(translation);
        }

        /// <summary>Gets the proper json file for the name passed in. Combines the file name with it's proper translation extension.</summary>
        /// <param name="jsonFileName"></param>
        /// <param name="translation"></param>
        public string getjsonForTranslation(string jsonFileName, string translation)
        {
            return jsonFileName + this.getFileExtentionForTranslation(translation);
        }



        /// <summary>Loads an json file from StardewValley/Content</summary>
        /// <param name="jsonFileName"></param>
        /// <param name="key"></param>
        /// <param name="translation"></param>
        public string LoadjsonFile(string jsonFileName, string key, string translation)
        {
            string json = jsonFileName + this.getFileExtentionForTranslation(translation);
            Dictionary<string, string> loadedDict = Game1.content.Load<Dictionary<string, string>>(json);

            bool f = loadedDict.TryGetValue(key, out string loaded);
            if (!f)
            {
                //Vocalization.ModMonitor.Log("Big issue: Key not found in file:" + json + " " + key);
                return "";
            }
            else return loaded;
        }

        /// <summary>Loads a string dictionary from a json file.</summary>
        /// <param name="jsonFileName"></param>
        /// <param name="translation"></param>
        public Dictionary<string, string> LoadJsonFileDictionary(string jsonFileName, string translation)
        {
            string json = jsonFileName + this.getFileExtentionForTranslation(translation);
            Dictionary<string, string> loadedDict = Game1.content.Load<Dictionary<string, string>>(json);

            return loadedDict;
        }

        public virtual string LoadString(string path, string translation, object sub1, object sub2, object sub3)
        {
            string format = this.LoadString(path, translation);
            try
            {
                return string.Format(format, sub1, sub2, sub3);
            }
            catch { }

            return format;
        }

        public virtual string LoadString(string path, string translation, object sub1, object sub2)
        {
            string format = this.LoadString(path, translation);
            try
            {
                return string.Format(format, sub1, sub2);
            }
            catch { }

            return format;
        }

        public virtual string LoadString(string path, string translation, object sub1)
        {
            string format = this.LoadString(path, translation);
            try
            {
                return string.Format(format, sub1);
            }
            catch { }

            return format;
        }

        public virtual string LoadString(string path, string translation)
        {
            this.parseStringPath(path, out string assetName, out string key);
            return this.LoadjsonFile(assetName, key, translation);
        }

        public virtual string LoadString(string path, string translation, params object[] substitutions)
        {
            string format = this.LoadString(path, translation);
            if (substitutions.Length != 0)
            {
                try
                {
                    return string.Format(format, substitutions);
                }
                catch { }
            }
            return format;
        }

        private void parseStringPath(string path, out string assetName, out string key)
        {
            int length = path.IndexOf(':');
            assetName = path.Substring(0, length);
            key = path.Substring(length + 1, path.Length - length - 1);
        }
    }
}
