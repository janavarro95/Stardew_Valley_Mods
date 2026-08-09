using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.HappyBirthday.Framework.Events;
using Omegasis.HappyBirthday.Framework.Gifts;
using Omegasis.StardustCore.Events;
using StardewModdingAPI;

namespace Omegasis.HappyBirthday.Framework.ContentPack
{
    /// <summary>
    /// A wrapper class that wraps <see cref="IContentPack"/> for <see cref="HappyBirthdayModCore"/> loaded in from SMAPI.
    /// </summary>
    public class HappyBirthdayContentPack
    {
        /// <summary>
        /// The base content pack that was loaded in from SMAPI for happy birthday.
        /// </summary>
        public IContentPack baseContentPack;

        /// <summary>
        /// The language code for this content pack.
        /// </summary>
        public string languageCode;

        /// <summary>
        /// Includes default birthday wishes.
        /// </summary>
        public Dictionary<string, string> birthdayWishes;

        /// <summary>
        /// Includes translated strings for the various birthday events.
        /// </summary>
        public Dictionary<string, string> eventStrings;

        /// <summary>
        /// A dictionary of events that have been loaded in for this content pack.
        /// </summary>
        public Dictionary<string, EventHelper> events;

        /// <summary>
        /// Includes mail strings which are displayed to the player.
        /// </summary>
        public Dictionary<string, string> mail;

        /// <summary>
        /// Includes birthday wishes from your spouse.
        /// </summary>
        public Dictionary<string, string> spouseBirthdayWishes;

        /// <summary>
        /// Includes miscellaneous translated strings.
        /// </summary>
        public Dictionary<string, string> translatedStrings;


        /// <summary>
        /// The npc birthday gifts added by the content pack.
        /// </summary>
        public Dictionary<string, List<GiftInformation>> npcBirthdayGifts;
        /// <summary>
        /// The spouse birthday gifts added by the content pack.
        /// </summary>
        public Dictionary<string, List<GiftInformation>> spouseBirthdayGifts;
        /// <summary>
        /// The default birthday gifts added by the content pack.
        /// </summary>
        public List<GiftInformation> defaultBirthdayGifts;


        /// <summary>
        /// The unique id for this content pack.
        /// </summary>
        public string UniqueId
        {
            get
            {
                return this.baseContentPack.Manifest.UniqueID;
            }
        }

        /// <summary>
        /// Default constuctor.
        /// </summary>
        public HappyBirthdayContentPack()
        {
            this.birthdayWishes = new Dictionary<string, string>();
            this.eventStrings = new Dictionary<string, string>();
            this.mail = new Dictionary<string, string>();
            this.spouseBirthdayWishes = new Dictionary<string, string>();
            this.translatedStrings = new Dictionary<string, string>();
        }

        /// <summary>
        /// Loads all of the information from a given content pack from HappyBirthday into this helper class.
        /// </summary>
        /// <param name="contentPack"></param>
        public virtual void load(StardewModdingAPI.IContentPack contentPack)
        {
            this.baseContentPack = contentPack;
            this.languageCode = this.loadFirstString("TranslationInfo.json");

            this.birthdayWishes = this.loadStringDictionary(this.getContentPackStringsPath(), "BirthdayWishes.json");
            this.eventStrings = this.loadStringDictionary(this.getContentPackStringsPath(), "Events.json");
            this.mail = this.loadStringDictionary(this.getContentPackStringsPath(), "Mail.json");
            this.spouseBirthdayWishes = this.loadStringDictionary(this.getContentPackStringsPath(), "SpouseBirthdayWishes.json");
            this.translatedStrings = this.loadStringDictionary(this.getContentPackStringsPath(), "TranslatedStrings.json");

            this.defaultBirthdayGifts = new List<GiftInformation>();
            this.npcBirthdayGifts = new Dictionary<string, List<GiftInformation>>();
            this.spouseBirthdayGifts = new Dictionary<string, List<GiftInformation>>();

            this.loadDefaultBirthdayGifts();
            this.loadVillagerBirthdayGifts();
            this.loadSpouseBirthdayGifts();
        }

        /// <summary>
        /// Gets a birthday wish string from this content pack.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="LogError"></param>
        /// <returns></returns>
        public virtual string getBirthdayWish(string Key, bool LogError=true)
        {
            if (this.birthdayWishes.ContainsKey(Key))
            {
                return this.birthdayWishes[Key];
            }
            if (LogError)
            {
                HappyBirthdayModCore.Instance.Monitor.Log(string.Format("Error: Attempted to get a birthday wish with key {0} from Happy Birthday content pack, but the given birthday wish key does not exist for the given content pack {1}.", Key, this.UniqueId), LogLevel.Error);
            }
            return "";
        }

        /// <summary>
        /// Gets a spouse birthday wish string from this content pack.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="LogError"></param>
        /// <returns></returns>
        public virtual string getSpouseBirthdayWish(string Key, bool LogError=true)
        {
            if (this.spouseBirthdayWishes.ContainsKey(Key) && string.IsNullOrEmpty(this.spouseBirthdayWishes[Key]) == false)
            {
                return this.spouseBirthdayWishes[Key];
            }
            if (LogError)
            {
                HappyBirthdayModCore.Instance.Monitor.Log(string.Format("Error: Attempted to get a spouse birthday wish with key {0} from Happy Birthday content pack, but the given spouse birthday wish key does not exist for the given content pack {1}.", Key, this.UniqueId), LogLevel.Error);
            }
            return "";
        }

        /// <summary>
        /// Gets a event string from this content pack.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="LogError"></param>
        /// <returns></returns>
        public virtual string getEventString(string Key, bool LogError = true)
        {
            if (this.eventStrings.ContainsKey(Key) && string.IsNullOrEmpty(this.eventStrings[Key]) == false)
            {
                return this.eventStrings[Key];
            }

            if(this.eventStrings.ContainsKey(Key)==false && Key.StartsWith("SpouseBirthdayEvent_"))
            {
                string fallbackKey = "SpouseBirthdayEvent_" + Key.Split('_').Last();
                if (this.eventStrings.ContainsKey(fallbackKey) && string.IsNullOrEmpty(this.eventStrings[fallbackKey])==false)
                {
                    return this.eventStrings[fallbackKey];
                }
            }

            if (this.eventStrings.ContainsKey(Key) == false && Key.StartsWith("SpouseAskPlayerForFavoriteGift_"))
            {
                string fallbackKey = "SpouseAskPlayerForFavoriteGift_" + Key.Split('_').Last();
                if (this.eventStrings.ContainsKey(fallbackKey) && string.IsNullOrEmpty(this.eventStrings[fallbackKey]) == false)
                {
                    return this.eventStrings[fallbackKey];
                }
            }

            if (LogError)
            {
                HappyBirthdayModCore.Instance.Monitor.Log(string.Format("Error: Attempted to get an event string with key {0} from Happy Birthday content pack, but the given event string key does not exist for the given content pack {1}.", Key, this.UniqueId), LogLevel.Error);
            }
            return "";
        }

        /// <summary>
        /// Gets a mail string from this content pack.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="LogError"></param>
        /// <returns></returns>
        public virtual string getMailString(string Key, bool LogError = true)
        {
            if (this.mail.ContainsKey(Key) && string.IsNullOrEmpty(this.mail[Key]) == false)
            {
                return this.mail[Key];
            }
            if (LogError)
            {
                HappyBirthdayModCore.Instance.Monitor.Log(string.Format("Error: Attempted to get a mail string with key {0} from Happy Birthday content pack, but the given mail string key does not exist for the given content pack {1}.", Key, this.UniqueId), LogLevel.Error);
            }
            return "";
        }

        /// <summary>
        /// Gets a mail string from this content pack.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="LogError"></param>
        /// <returns></returns>
        public virtual string getTranslationString(string Key, bool LogError = true)
        {
            if (this.translatedStrings.ContainsKey(Key) && string.IsNullOrEmpty(this.translatedStrings[Key]) == false)
            {
                return this.translatedStrings[Key];
            }
            if (LogError)
            {
                HappyBirthdayModCore.Instance.Monitor.Log(string.Format("Error: Attempted to get a translation string with key {0} from Happy Birthday content pack, but the given translation string key does not exist for the given content pack {1}.", Key, this.UniqueId), LogLevel.Error);
            }
            return "";
        }

        /// <summary>
        /// Gets the path to the content pack's strings folder.
        /// </summary>
        /// <returns></returns>
        public virtual string getContentPackStringsPath()
        {
            return Path.Combine("ModAssets", "Strings");
        }

        /// <summary>
        /// Loads a string Dictionary from a file.
        /// </summary>
        /// <param name="RelativePathToFile"></param>
        /// <returns></returns>
        protected virtual Dictionary<string, string> loadStringDictionary(params string[] RelativePathToFile)
        {
            return this.baseContentPack.ModContent.Load<Dictionary<string, string>>(Path.Combine(RelativePathToFile));
        }

        /// <summary>
        /// Loads a string from a string dictionary json file with the given key. Throws an exception if either file or key are not found.
        /// </summary>
        /// <param name="Key">The key of the string in the .json file.</param>
        /// <param name="RelativePathToFile">The relative path to the dictionary file starting at the root of the content pack directory.</param>
        /// <returns>A string stored in a content file.</returns>
        protected virtual string loadString(string Key, string RelativePathToFile)
        {
            Dictionary<string, string> dictionaryContentFile = this.loadStringDictionary(RelativePathToFile);
            if (dictionaryContentFile == null)
            {
                throw new FileNotFoundException(string.Format("The given string dictionary file does not exist for the given HappyBirthday content pack {0} at path {1}.", this.UniqueId, RelativePathToFile));
            }
            if (dictionaryContentFile.ContainsKey(Key) == false)
            {

                throw new ArgumentException(string.Format("The given key does not exist in the dictionary file for the given HappyBirthday content pack {0} at path {1} with key {2}.", this.UniqueId, RelativePathToFile,Key));
            }
            return dictionaryContentFile[Key];
        }

        /// <summary>
        /// Loads the first string from a string dictionary json file. Used when only one string key should be expected.
        /// </summary>
        /// <param name="RelativePathToFile"></param>
        /// <returns></returns>
        protected virtual string loadFirstString(string RelativePathToFile)
        {
            Dictionary<string, string> dictionaryContentFile = this.loadStringDictionary(RelativePathToFile);
            if (dictionaryContentFile == null)
            {
                throw new FileNotFoundException(string.Format("The given string dictionary file does not exist for the given HappyBirthday content pack {0} at path {1}.", this.UniqueId, RelativePathToFile));
            }
            if (dictionaryContentFile.Count == 0)
            {
                throw new ArgumentException(string.Format("There are zero string keys in the the dictionary file for the given HappyBirthday content pack {0} at path {1}", this.UniqueId, RelativePathToFile));
            }
            return dictionaryContentFile.First().Value;
        }

        /// <summary>
        /// Gets the <see cref="GiftInformation"/> for a given <see cref="StardewValley.NPC"/>
        /// </summary>
        /// <param name="NpcName">The name of the npc</param>
        /// <returns></returns>
        public virtual List<GiftInformation> getGiftsForNpc(string NpcName)
        {
            if (this.npcBirthdayGifts.ContainsKey(NpcName))
            {
                return this.npcBirthdayGifts[NpcName];
            }
            return new List<GiftInformation>();
        }

        /// <summary>
        /// Gets the <see cref="GiftInformation"/> for a given <see cref="StardewValley.NPC"/> if they are the spouse.
        /// </summary>
        /// <param name="NpcName">The name of the npc</param>
        /// <returns></returns>
        public virtual List<GiftInformation> getGiftsForSpouse(string NpcName)
        {
            if (this.spouseBirthdayGifts.ContainsKey(NpcName))
            {
                return this.spouseBirthdayGifts[NpcName];
            }
            return new List<GiftInformation>();
        }

        /// <summary>
        /// Gets the <see cref="GiftInformation"/> if no npc specific gift was selected.
        /// </summary>
        /// <param name="NpcName">The name of the npc</param>
        /// <returns></returns>
        public virtual List<GiftInformation> getDefaultBirthdayGifts()
        {
            return this.defaultBirthdayGifts;
        }



        public void loadDefaultBirthdayGifts()
        {

            if (File.Exists(Path.Combine(this.baseContentPack.DirectoryPath,"ModAssets", "Data", "Gifts", "DefaultGifts.json")))
            {
                HappyBirthdayModCore.Instance.Monitor.Log("Loading in default birthday gifts for content pack: " + this.UniqueId);
                this.defaultBirthdayGifts = this.baseContentPack.ReadJsonFile<List<GiftInformation>>(Path.Combine("ModAssets", "Data", "Gifts", "DefaultGifts.json"));

                if (this.defaultBirthdayGifts == null)
                {
                    throw new Exception("Default birthday gifts file is null????");
                }
            }

        }

        /// <summary>Load birthday gift information from disk. Preferably from BirthdayGift.json in the mod's directory.</summary>
        public void loadVillagerBirthdayGifts()
        {
            string[] files = Directory.GetFiles(Path.Combine(this.baseContentPack.DirectoryPath, "ModAssets", "Data", "Gifts"));
            foreach (string File in files)
            {
                if (!Path.GetExtension(File).Contains("json")) continue;

                try
                {
                    if (Path.GetFileNameWithoutExtension(File).Equals("RegisteredGifts"))
                    {
                        continue;
                    }

                    List<GiftInformation> giftInfo = this.baseContentPack.ReadJsonFile<List<GiftInformation>>(Path.Combine("ModAssets", "Data", "Gifts", Path.GetFileNameWithoutExtension(File) +".json"));

                    if (giftInfo == null)
                    {
                        throw new Exception("NPC GIFT INFO CAN NOT BE NULL!!!! Errored file: "+ Path.GetFileNameWithoutExtension(File) + ".json");
                    }

                    this.npcBirthdayGifts.Add(Path.GetFileNameWithoutExtension(File),giftInfo );
                    HappyBirthdayModCore.Instance.Monitor.Log("Loaded in gifts for npc for content pack: " + Path.GetFileNameWithoutExtension(File) + " : " + this.UniqueId);
                }
                catch (Exception err)
                {
                    HappyBirthdayModCore.Instance.Monitor.Log(err.ToString(), LogLevel.Error);
                }
            }
        }

        /// <summary>Used to load spouse birthday gifts from disk.</summary>
        public void loadSpouseBirthdayGifts()
        {
            string[] files = Directory.GetFiles(Path.Combine(this.baseContentPack.DirectoryPath, "ModAssets", "Data", "Gifts", "Spouses"));
            foreach (string File in files)
            {
                if (!Path.GetExtension(File).Contains("json")) continue;
                this.spouseBirthdayGifts.Add(Path.GetFileNameWithoutExtension(File), this.baseContentPack.ReadJsonFile<List<GiftInformation>>(Path.Combine("ModAssets", "Data", "Gifts", "Spouses", Path.GetFileNameWithoutExtension(File) + ".json")));
                HappyBirthdayModCore.Instance.Monitor.Log("Loaded in spouse gifts for npc for content pack: " + Path.GetFileNameWithoutExtension(File) + " : "+this.UniqueId);
            }
        }

        public virtual void loadBirthdayEvents()
        {
            string relativePath = Path.Combine("ModAssets", "Data", "Events");
            string abspath = Path.Combine(this.baseContentPack.DirectoryPath, relativePath);
            if (!Directory.Exists(abspath))
                Directory.CreateDirectory(abspath);

            string[] files = Directory.GetFiles(abspath);
            foreach (string file in files)
            {
                string pathToFile = Path.Combine(relativePath, Path.GetFileName(file));
                //Exclude non json files and directories that may exist here due to Vortex or some sort of mistakes.
                if (!pathToFile.EndsWith(".json")) continue;

                HappyBirthdayEventHelper eventHelper = this.baseContentPack.ReadJsonFile<HappyBirthdayEventHelper>(pathToFile);
                eventHelper.parseEventPreconditionsFromPreconditionStrings(BirthdayEventUtilities.BirthdayEventManager);



                if (eventHelper == null)
                    continue;

                if (BirthdayEventUtilities.BirthdayEventManager.events.ContainsKey(eventHelper.eventStringId))
                    continue;
                else
                    BirthdayEventUtilities.BirthdayEventManager.addEvent(eventHelper);

            }
        }
    }
}
