using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Omegasis.HappyBirthday.Framework.ContentPack;
using Omegasis.HappyBirthday.Framework.Utilities;
using StardewValley;

namespace Omegasis.HappyBirthday.Framework
{
    /// <summary>A class which deals with handling different translations for Vocalization should other voice teams ever wish to voice act for that language.</summary>
    public class TranslationInfo
    {

        public enum FileType
        {
            XNB,
            JSON
        }
        /*********
        ** Accessors
        *********/


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        public TranslationInfo()
        {
        }

        /// <summary>Gets the proper file extension for the current translation.</summary>
        /// <param name="language">The translation language name.</param>
        protected string getFileExtentionForTranslation(string languageCode, FileType fileType)
        {
            if (string.IsNullOrEmpty(languageCode)) {
                return this.getFileExtensionForFileType(fileType);
            }

            return "."+LocalizedContentManager.CurrentLanguageString + this.getFileExtensionForFileType(fileType);
        }
        protected string getFileExtensionForFileType(FileType Type)
        {
            if (Type == FileType.JSON)
            {
                return ".json";
            }
            else
            {
                return ".xnb";
            }
        }

        public string loadStringFromXNBFile(string xnbFileName, string key)
        {
            return this.loadStringFromXNBFile(xnbFileName, key, LocalizedContentManager.CurrentLanguageString);
        }

        /// <summary>Loads an XNB file from StardewValley/Content</summary>
        public string loadStringFromXNBFile(string xnbFileName, string key, string languageCode)
        {
            if (string.IsNullOrEmpty(languageCode))
            {
                languageCode = LocalizedContentManager.CurrentLanguageString;
            }

            string xnb = xnbFileName + this.getFileExtentionForTranslation(languageCode, FileType.XNB);

            Dictionary<string, string> loadedDict = Game1.content.Load<Dictionary<string, string>>(xnb);

            if (!loadedDict.TryGetValue(key, out string loaded))
            {
                Omegasis.HappyBirthday.HappyBirthdayModCore.Instance.Monitor.Log("Big issue: Key not found in file:" + xnb + " " + key);
                return "";
            }
            return loaded;
        }

        public virtual string getTranslatedBaseGameString(string Key)
        {
            if (string.IsNullOrEmpty(Key)) return "";
            if (Key.Equals("Birthday"))
            {
                string s = Game1.content.LoadString("Strings\\UI:Profile_Birthday");
                return s;
            }
            if (Key.Equals("Spring") || Key.Equals("spring"))
            {
                string file = Path.Combine("Strings", "StringsFromCSFiles");
                return HappyBirthdayModCore.Instance.translationInfo.loadStringFromXNBFile(file, "Utility.cs.5680");
            }
            if (Key.Equals("Summer") || Key.Equals("summer"))
            {
                string file = Path.Combine("Strings", "StringsFromCSFiles");
                return HappyBirthdayModCore.Instance.translationInfo.loadStringFromXNBFile(file, "Utility.cs.5681");
            }
            if (Key.Equals("Fall") || Key.Equals("fall"))
            {
                string file = Path.Combine("Strings", "StringsFromCSFiles");
                return HappyBirthdayModCore.Instance.translationInfo.loadStringFromXNBFile(file, "Utility.cs.5682");
            }
            if (Key.Equals("Winter") || Key.Equals("winter"))
            {
                string file = Path.Combine("Strings", "StringsFromCSFiles");
                return HappyBirthdayModCore.Instance.translationInfo.loadStringFromXNBFile(file, "Utility.cs.5683");
            }
            return "";
        }

        /// <summary>
        /// Gets an event string from a Happy Birthday content pack.
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public virtual string getEventString(string Key)
        {
            return this.getEventString(Key, LocalizedContentManager.CurrentLanguageString, true);
        }

        /// <summary>
        /// Gets an event string from a Happy Birthday content pack.
        /// </summary>
        /// <param name="Key">The key in the .json file for the dialogue to load.</param>
        /// <param name="LanguageCode">The language code to check against content packs to appropriately match translations./</param>
        /// <param name="DefaultToEnglish">Should the mod default to the english localization if no content packs are found for the current mod translation?</param>
        /// <returns></returns>
        public virtual string getEventString(string Key, string LanguageCode, bool DefaultToEnglish)
        {
            if (LanguageCode == "" && DefaultToEnglish == true)
            {
                //Prevent infinite recursion.
                DefaultToEnglish = false;
            }
            List<HappyBirthdayContentPack> affectedContentPacks = HappyBirthdayModCore.Instance.happyBirthdayContentPackManager.getHappyBirthdayContentPacksForCurrentLanguageCode();
            List<string> potentialStrings = new List<string>();
            foreach (HappyBirthdayContentPack contentPack in affectedContentPacks)
            {
                string str = contentPack.getEventString(Key, false);
                if (string.IsNullOrEmpty(str)) continue;
                potentialStrings.Add(str);
            }
            if (potentialStrings.Count == 0)
            {
                if (DefaultToEnglish)
                {
                    return this.getEventString(Key, "", false);
                }
                return "";
            }
            else
            {
                int randomIndex = Game1.random.Next(0, potentialStrings.Count);
                return potentialStrings[randomIndex];
            }
        }

        public virtual string getMailString(string Key)
        {
            return this.getMailString(Key, LocalizedContentManager.CurrentLanguageString, true);
        }

        public virtual string getMailString(string Key, string LanguageCode, bool DefaultToEnglish)
        {
            if (LanguageCode == "" && DefaultToEnglish == true)
            {
                //Prevent infinite recursion.
                DefaultToEnglish = false;
            }
            List<HappyBirthdayContentPack> affectedContentPacks = HappyBirthdayModCore.Instance.happyBirthdayContentPackManager.getHappyBirthdayContentPacksForCurrentLanguageCode();
            List<string> potentialStrings = new List<string>();
            foreach (HappyBirthdayContentPack contentPack in affectedContentPacks)
            {
                string str = contentPack.getMailString(Key, false);
                if (string.IsNullOrEmpty(str)) continue;
                potentialStrings.Add(str);
            }
            if (potentialStrings.Count == 0)
            {

                if (DefaultToEnglish)
                {
                    return this.getMailString(Key, "", false);
                }

                return "";
            }
            else
            {
                int randomIndex = Game1.random.Next(0, potentialStrings.Count);
                return potentialStrings[randomIndex];
            }
        }

        /// <summary>
        /// Gets a translated string from a content pack.
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public virtual string getTranslatedContentPackString(string Key)
        {
            return this.getTranslatedContentPackString(Key, LocalizedContentManager.CurrentLanguageString, true);
        }

        /// <summary>
        /// Gets a translated string from a content pack.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="LanguageCode"></param>
        /// <param name="DefaultToEnglish"></param>
        /// <returns></returns>
        public virtual string getTranslatedContentPackString(string Key, string LanguageCode, bool DefaultToEnglish)
        {
            if (LanguageCode == "" && DefaultToEnglish == true)
            {
                //Prevent infinite recursion.
                DefaultToEnglish = false;
            }
            List<HappyBirthdayContentPack> affectedContentPacks = HappyBirthdayModCore.Instance.happyBirthdayContentPackManager.getHappyBirthdayContentPacksForCurrentLanguageCode();
            List<string> potentialStrings = new List<string>();
            foreach (HappyBirthdayContentPack contentPack in affectedContentPacks)
            {
                string str = contentPack.getTranslationString(Key, false);
                if (string.IsNullOrEmpty(str)) continue;
                potentialStrings.Add(str);
            }
            if (potentialStrings.Count == 0)
            {
                if (DefaultToEnglish)
                {
                    return this.getTranslatedContentPackString(Key, "", false);
                }
                return "";
            }
            else
            {
                int randomIndex = Game1.random.Next(0, potentialStrings.Count);
                return potentialStrings[randomIndex];
            }
        }
    }
}
