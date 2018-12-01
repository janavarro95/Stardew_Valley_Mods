using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Omegasis.HappyBirthday.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Monsters;
using SObject = StardewValley.Object;

namespace Omegasis.HappyBirthday
{
    /// <summary>The mod entry point.</summary>
    public class HappyBirthday : Mod, IAssetEditor, IAssetLoader
    {
        /*********
        ** Properties
        *********/
        /// <summary>The relative path for the current player's data file.</summary>
        private string DataFilePath;

        /// <summary>The absolute path for the current player's legacy data file.</summary>
        private string LegacyDataFilePath => Path.Combine(this.Helper.DirectoryPath, "Player_Birthdays", $"HappyBirthday_{Game1.player.Name}.txt");

        /// <summary>The mod configuration.</summary>
        private ModConfig Config;

        /// <summary>The data for the current player.</summary>
        private PlayerData PlayerData;

        /// <summary>Whether the player has chosen a birthday.</summary>
        private bool HasChosenBirthday => !string.IsNullOrEmpty(this.PlayerData.BirthdaySeason) && this.PlayerData.BirthdayDay != 0;

        /// <summary>The queue of villagers who haven't given a gift yet.</summary>
        private List<string> VillagerQueue;

        /// <summary>The gifts that villagers can give.</summary>
        private List<Item> PossibleBirthdayGifts;

        /// <summary>The next birthday gift the player will receive.</summary>
        private Item BirthdayGiftToReceive;

        /// <summary>Whether we've already checked for and (if applicable) set up the player's birthday today.</summary>
        private bool CheckedForBirthday;
        //private Dictionary<string, Dialogue> Dialogue;
        //private bool SeenEvent;
        public bool CanLoad<T>(IAssetInfo asset)
        {
            return asset.AssetNameEquals(@"Data\FarmerBirthdayDialogue");
        }

        /// <summary>Load a matched asset.</summary>
        /// <param name="asset">Basic metadata about the asset being loaded.</param>
        public T Load<T>(IAssetInfo asset)
        {
            return (T)(object)new Dictionary<string, string> // (T)(object) is a trick to cast anything to T if we know it's compatible
            {
                ["Robin"] = "Hey @, happy birthday! I'm glad you choose this town to move here to. ",
                ["Demetrius"] = "Happy birthday @! Make sure you take some time off today to enjoy yourself. $h",
                ["Maru"] = "Happy birthday @. I tried to make you an everlasting candle for you, but sadly that didn't work out. Maybe next year right? $h",
                ["Sebastian"] = "Happy birthday @. Here's to another year of chilling. ",
                ["Linus"] = "Happy birthday @. Thanks for visiting me even on your birthday. It makes me really happy. ",
                ["Pierre"] = "Hey @, happy birthday! Hopefully this next year for you will be a great one! ",
                ["Caroline"] = "Happy birthday @. Thank you for all that you've done for our community. I'm sure your parents must be proud of you.$h",
                ["Abigail"] = "Happy birthday @! Hopefully this year we can go on even more adventures together $h!",
                ["Alex"] = "Yo @, happy birthday! Maybe this will be your best year yet.$h",
                ["George"] = "When you get to my age birthdays come and go. Still happy birthday @.",
                ["Evelyn"] = "Happy birthday @. You have grown up to be such a fine individual and I'm sure you'll continue to grow. ",
                ["Lewis"] = "Happy birthday @! I'm thankful for what you have done for the town and I'm sure your grandfather would be proud of you.",
                ["Clint"] = "Hey happy birthday @. I'm sure this year is going to be great for you.",
                ["Penny"] = "Happy birthday @. May you enjoy all of life's blessings this year. ",
                ["Pam"] = "Happy birthday kid. We should have a drink to celebrate another year of life for you! $h",
                ["Emily"] = "I'm sensing a strong positive life energy about you, so it must be your birthday. Happy birthday @!$h",
                ["Haley"] = "Happy birthday @. Hopefully this year you'll get some good presents!$h",
                ["Jas"] = "Happy birthday @. I hope you have a good birthday.",
                ["Vincent"] = "Hey @ have you come to pl...oh it's your birthday? Happy birthday! ",
                ["Jodi"] = "Hello there @. Rumor has it that today is your birthday. In that case, happy birthday!$h",
                ["Kent"] = "Jodi told me that it was your birthday today @. Happy birthday and make sure to cherish every single day.",
                ["Sam"] = "Yo @ happy birthday! We'll have to have a birthday jam session for you some time!$h ",
                ["Leah"] = "Hey @ happy birthday! We should go to the saloon tonight and celebrate!$h ",
                ["Shane"] = "Happy birthday @. Keep working hard and I'm sure this next year for you will be a great one.",
                ["Marnie"] = "Hello there @. Everyone is talking about your birthday today and I wanted to make sure that I wished you a happy birthday as well, so happy birthday! $h ",
                ["Elliott"] = "What a wonderful day isn't it @? Especially since today is your birthday. I tried to make you a poem but I feel like the best way of putting it is simply, happy birthday. $h ",
                ["Gus"] = "Hey @ happy birthday! Hopefully you enjoy the rest of the day and make sure you aren't a stranger at the saloon!",
                ["Dwarf"] = "Happy birthday @. I hope that what I got you is acceptable for humans as well. ",
                ["Wizard"] = "The spirits told me that today is your birthday. In that case happy birthday @. ",
                ["Harvey"] = "Hey @, happy birthday! Make sure to come in for a checkup some time to make sure you live many more years! ",
                ["Sandy"] = "Hello there @. I heard that today was your birthday and I didn't want you feeling left out, so happy birthday!",
                ["Willy"] = "Aye @ happy birthday. Looking at you reminds me of ye days when I was just a guppy swimming out to sea. Continue to enjoy them youngin.$h",
                ["Krobus"] = "I have heard that it is tradition to give a gift to others on their birthday. In that case, happy birthday @."
            };
        }

        public bool CanEdit<T>(IAssetInfo asset)
        {
            return asset.AssetNameEquals(@"Data\mail");
        }

        public void Edit<T>(IAssetData asset)
        {
            asset
            .AsDictionary<string, string>()
            .Set("birthdayMom", "Dear @,^  Happy birthday sweetheart. It's been amazing watching you grow into the kind, hard working person that I've always dreamed that you would become. I hope you continue to make many more fond memories with the ones you love. ^  Love, Mom ^ P.S. Here's a little something that I made for you. %item object 221 1 %%");

            asset
            .AsDictionary<string, string>()
            .Set("birthdayDad", "Dear @,^  Happy birthday kiddo. It's been a little quiet around here on your birthday since you aren't around, but your mother and I know that you are making both your grandpa and us proud.  We both know that living on your own can be tough but we believe in you one hundred percent, just keep following your dreams.^  Love, Dad ^ P.S. Here's some spending money to help you out on the farm. Good luck! %item money 5000 5001 %%");
        }


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            helper.Content.AssetLoaders.Add(new PossibleGifts());
            this.Config = helper.ReadConfig<ModConfig>();

            TimeEvents.AfterDayStarted += this.TimeEvents_AfterDayStarted;
            GameEvents.UpdateTick += this.GameEvents_UpdateTick;
            SaveEvents.AfterLoad += this.SaveEvents_AfterLoad;
            SaveEvents.BeforeSave += this.SaveEvents_BeforeSave;
            ControlEvents.KeyPressed += this.ControlEvents_KeyPressed;

            //MultiplayerSupport.initializeMultiplayerSupport();
        }


        /*********
        ** Private methods
        *********/
        /// <summary>The method invoked after a new day starts.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void TimeEvents_AfterDayStarted(object sender, EventArgs e)
        {
            this.CheckedForBirthday = false;
        }

        /// <summary>The method invoked when the presses a keyboard button.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void ControlEvents_KeyPressed(object sender, EventArgsKeyPressed e)
        {
            // show birthday selection menu
            if (Context.IsPlayerFree && !this.HasChosenBirthday && e.KeyPressed.ToString() == this.Config.KeyBinding)
                Game1.activeClickableMenu = new BirthdayMenu(this.PlayerData.BirthdaySeason, this.PlayerData.BirthdayDay, this.SetBirthday);
        }

        /// <summary>The method invoked after the player loads a save.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void SaveEvents_AfterLoad(object sender, EventArgs e)
        {
            this.DataFilePath = Path.Combine("data", Game1.player.Name + "_" + Game1.player.UniqueMultiplayerID+".json");

            // reset state
            this.VillagerQueue = new List<string>();
            this.PossibleBirthdayGifts = new List<Item>();
            this.BirthdayGiftToReceive = null;
            this.CheckedForBirthday = false;

            // load settings
            this.MigrateLegacyData();
            this.PlayerData = Helper.Data.ReadJsonFile<PlayerData>(this.DataFilePath) ?? new PlayerData();
            
            //this.SeenEvent = false;
            //this.Dialogue = new Dictionary<string, Dialogue>();
        }

        /// <summary>The method invoked just before the game updates the saves.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void SaveEvents_BeforeSave(object sender, EventArgs e)
        {
            if (this.HasChosenBirthday)
                Helper.Data.WriteJsonFile(this.DataFilePath, this.PlayerData);
        }

        /// <summary>The method invoked when the game updates (roughly 60 times per second).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void GameEvents_UpdateTick(object sender, EventArgs e)
        {
            if (!Context.IsWorldReady || Game1.eventUp || Game1.isFestival())
                return;

            if (!this.CheckedForBirthday)
            {
                this.CheckedForBirthday = true;

                // set up birthday
                if (this.IsBirthday())
                {
                    Messages.ShowStarMessage("It's your birthday today! Happy birthday!");
                    //MultiplayerSupport.SendBirthdayMessageToOtherPlayers();


                    Game1.player.mailbox.Add("birthdayMom");
                    Game1.player.mailbox.Add("birthdayDad");

                    try
                    {
                        this.ResetVillagerQueue();
                    }
                    catch (Exception ex)
                    {
                        this.Monitor.Log(ex.ToString(), LogLevel.Error);
                    }
                    foreach (GameLocation location in Game1.locations)
                    {
                        foreach (NPC npc in location.characters)
                        {
                            if (npc is Child || npc is Horse || npc is Junimo || npc is Monster || npc is Pet)
                                continue;

                            try
                            {
                                Dialogue d = new Dialogue(Game1.content.Load<Dictionary<string, string>>("Data\\FarmerBirthdayDialogue")[npc.Name], npc);
                                npc.CurrentDialogue.Push(d);
                                if (npc.CurrentDialogue.ElementAt(0) != d) npc.setNewDialogue(Game1.content.Load<Dictionary<string, string>>("Data\\FarmerBirthdayDialogue")[npc.Name]);
                            }
                            catch
                            {
                                Dialogue d = new Dialogue("Happy Birthday @!", npc);
                                npc.CurrentDialogue.Push(d);
                                if (npc.CurrentDialogue.ElementAt(0) != d)
                                    npc.setNewDialogue("Happy Birthday @!");
                            }
                        }
                    }
                }

                if (Game1.activeClickableMenu != null)
                {
                    if (Game1.activeClickableMenu.GetType() == typeof(BirthdayMenu)) return;
                }
                // ask for birthday date
                if (!this.HasChosenBirthday)
                {
                    Game1.activeClickableMenu = new BirthdayMenu(this.PlayerData.BirthdaySeason, this.PlayerData.BirthdayDay, this.SetBirthday);
                    this.CheckedForBirthday = false;
                }
            }

            // unreachable since we exit early if Game1.eventUp
            //if (Game1.eventUp)
            //{
            //    foreach (string npcName in this.VillagerQueue)
            //    {
            //        NPC npc = Game1.getCharacterFromName(npcName);

            //        try
            //        {
            //            this.Dialogue.Add(npcName, npc.CurrentDialogue.Pop());
            //        }
            //        catch (Exception ex)
            //        {
            //            this.Monitor.Log(ex.ToString(), LogLevel.Error);
            //            this.Dialogue.Add(npcName, npc.CurrentDialogue.ElementAt(0));
            //            npc.loadSeasonalDialogue();
            //        }

            //        this.SeenEvent = true;
            //    }
            //}

            //if (!Game1.eventUp && this.SeenEvent)
            //{
            //    foreach (KeyValuePair<string, Dialogue> v in this.Dialogue)
            //    {
            //        NPC npc = Game1.getCharacterFromName(v.Key);
            //        npc.CurrentDialogue.Push(v.Value);
            //    }
            //    this.Dialogue.Clear();
            //    this.SeenEvent = false;
            //}

            // set birthday gift
            if (Game1.currentSpeaker != null)
            {
                string name = Game1.currentSpeaker.Name;
                if (this.IsBirthday() && this.VillagerQueue.Contains(name))
                {
                    try
                    {
                        this.SetNextBirthdayGift(Game1.currentSpeaker.Name);
                        this.VillagerQueue.Remove(Game1.currentSpeaker.Name);
                    }
                    catch (Exception ex)
                    {
                        this.Monitor.Log(ex.ToString(), LogLevel.Error);
                    }
                }
            }
            if (this.BirthdayGiftToReceive != null && Game1.currentSpeaker != null)
            {
                while (this.BirthdayGiftToReceive.Name == "Error Item" || this.BirthdayGiftToReceive.Name == "Rock" || this.BirthdayGiftToReceive.Name == "???")
                    this.SetNextBirthdayGift(Game1.currentSpeaker.Name);
                Game1.player.addItemByMenuIfNecessaryElseHoldUp(this.BirthdayGiftToReceive);
                this.BirthdayGiftToReceive = null;
            }
        }

        /// <summary>Set the player's birtday/</summary>
        /// <param name="season">The birthday season.</param>
        /// <param name="day">The birthday day.</param>
        private void SetBirthday(string season, int day)
        {
            this.PlayerData.BirthdaySeason = season;
            this.PlayerData.BirthdayDay = day;
        }

        /// <summary>Reset the queue of villager names.</summary>
        private void ResetVillagerQueue()
        {
            this.VillagerQueue.Clear();

            foreach (GameLocation location in Game1.locations)
            {
                foreach (NPC npc in location.characters)
                {
                    if (npc is Child || npc is Horse || npc is Junimo || npc is Monster || npc is Pet)
                        continue;
                    if (this.VillagerQueue.Contains(npc.Name))
                        continue;
                    this.VillagerQueue.Add(npc.Name);
                }
            }
        }

        /// <summary>Set the next birthday gift the player will receive.</summary>
        /// <param name="name">The villager's name who's giving the gift.</param>
        /// <remarks>This returns gifts based on the speaker's heart level towards the player: neutral for 0-3, good for 4-6, and best for 7-10.</remarks>
        private void SetNextBirthdayGift(string name)
        {
            Item gift;
            if (this.PossibleBirthdayGifts.Count > 0)
            {
                Random random = new Random();
                int index = random.Next(this.PossibleBirthdayGifts.Count);
                gift = this.PossibleBirthdayGifts[index];
                if (Game1.player.isInventoryFull())
                    Game1.createItemDebris(gift, Game1.player.getStandingPosition(), Game1.player.getDirection());
                else
                    this.BirthdayGiftToReceive = gift;
                return;
            }

            this.PossibleBirthdayGifts.AddRange(this.GetDefaultBirthdayGifts(name));

            Random rnd2 = new Random();
            int r2 = rnd2.Next(this.PossibleBirthdayGifts.Count);
            gift = this.PossibleBirthdayGifts.ElementAt(r2);
            if (Game1.player.isInventoryFull())
                Game1.createItemDebris(gift, Game1.player.getStandingPosition(), Game1.player.getDirection());
            else
                this.BirthdayGiftToReceive = gift;

            this.PossibleBirthdayGifts.Clear();
        }

        /// <summary>Get the default gift items.</summary>
        /// <param name="name">The villager's name.</param>
        private IEnumerable<SObject> GetDefaultBirthdayGifts(string name)
        {
            List<SObject> gifts = new List<SObject>();
            try
            {
                // read from birthday gifts file
                IDictionary<string, string> data = Game1.content.Load<Dictionary<string, string>>("Data\\PossibleBirthdayGifts");
                data.TryGetValue(name, out string text);
                if (text != null)
                {
                    string[] fields = text.Split('/');

                    // love
                    if (Game1.player.getFriendshipHeartLevelForNPC(name) >= 7)
                    {
                        string[] loveFields = fields[1].Split(' ');
                        for (int i = 0; i < loveFields.Length; i += 2)
                        {
                            try
                            {
                                gifts.AddRange(this.GetItems(Convert.ToInt32(loveFields[i]), Convert.ToInt32(loveFields[i + 1])));
                            }
                            catch { }
                        }
                    }

                    // like
                    if (Game1.player.getFriendshipHeartLevelForNPC(name) >= 4 && Game1.player.getFriendshipHeartLevelForNPC(name) <= 6)
                    {
                        string[] likeFields = fields[3].Split(' ');
                        for (int i = 0; i < likeFields.Length; i += 2)
                        {
                            try
                            {
                                gifts.AddRange(this.GetItems(Convert.ToInt32(likeFields[i]), Convert.ToInt32(likeFields[i + 1])));
                            }
                            catch { }
                        }
                    }

                    // neutral
                    if (Game1.player.getFriendshipHeartLevelForNPC(name) >= 0 && Game1.player.getFriendshipHeartLevelForNPC(name) <= 3)
                    {
                        string[] neutralFields = fields[5].Split(' ');

                        for (int i = 0; i < neutralFields.Length; i += 2)
                        {
                            try
                            {
                                gifts.AddRange(this.GetItems(Convert.ToInt32(neutralFields[i]), Convert.ToInt32(neutralFields[i + 1])));
                            }
                            catch { }
                        }
                    }
                }

                // get NPC's preferred gifts
                if (Game1.player.getFriendshipHeartLevelForNPC(name) >= 7)
                    gifts.AddRange(this.GetUniversalItems("Love", true));
                if (Game1.player.getFriendshipHeartLevelForNPC(name) >= 4 && Game1.player.getFriendshipHeartLevelForNPC(name) <= 6)
                    this.PossibleBirthdayGifts.AddRange(this.GetUniversalItems("Like", true));
                if (Game1.player.getFriendshipHeartLevelForNPC(name) >= 0 && Game1.player.getFriendshipHeartLevelForNPC(name) <= 3)
                    this.PossibleBirthdayGifts.AddRange(this.GetUniversalItems("Neutral", true));
            }
            catch
            {
                // get NPC's preferred gifts
                if (Game1.player.getFriendshipHeartLevelForNPC(name) >= 7)
                {
                    this.PossibleBirthdayGifts.AddRange(this.GetUniversalItems("Love", false));
                    this.PossibleBirthdayGifts.AddRange(this.GetLovedItems(name));
                }
                if (Game1.player.getFriendshipHeartLevelForNPC(name) >= 4 && Game1.player.getFriendshipHeartLevelForNPC(name) <= 6)
                {
                    this.PossibleBirthdayGifts.AddRange(this.GetLikedItems(name));
                    this.PossibleBirthdayGifts.AddRange(this.GetUniversalItems("Like", false));
                }
                if (Game1.player.getFriendshipHeartLevelForNPC(name) >= 0 && Game1.player.getFriendshipHeartLevelForNPC(name) <= 3)
                    this.PossibleBirthdayGifts.AddRange(this.GetUniversalItems("Neutral", false));
            }
            //TODO: Make different tiers of gifts depending on the friendship, and if it is the spouse.
            /*
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(198, 1));
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(204, 1));
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(220, 1));
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(221, 1));
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(223, 1));
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(233, 1));
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(234, 1));
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(286, 5));
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(368, 5));
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(608, 1));
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(612, 1));
                this.possible_birthday_gifts.Add((Item)new SytardewValley.Object(773, 1));
                */

            return gifts;
        }

        /// <summary>Get the items loved by all villagers.</summary>
        /// <param name="group">The group to get (one of <c>Like</c>, <c>Love</c>, <c>Neutral</c>).</param>
        /// <param name="isBirthdayGiftList">Whether to get data from <c>Data\PossibleBirthdayGifts.xnb</c> instead of the game data.</param>
        private IEnumerable<SObject> GetUniversalItems(string group, bool isBirthdayGiftList)
        {
            if (!isBirthdayGiftList)
            {
                // get raw data
                Game1.NPCGiftTastes.TryGetValue($"Universal_{group}", out string text);
                if (text == null)
                    yield break;

                // parse
                string[] neutralIDs = text.Split(' ');
                foreach (string neutralID in neutralIDs)
                {
                    foreach (SObject obj in this.GetItems(Convert.ToInt32(neutralID)))
                        yield return obj;
                }
            }
            else
            {
                // get raw data
                Dictionary<string, string> data = Game1.content.Load<Dictionary<string, string>>("Data\\PossibleBirthdayGifts");
                data.TryGetValue($"Universal_{group}_Gift", out string text);
                if (text == null)
                    yield break;

                // parse
                string[] array = text.Split(' ');
                for (int i = 0; i < array.Length; i += 2)
                {
                    foreach (SObject obj in this.GetItems(Convert.ToInt32(array[i]), Convert.ToInt32(array[i + 1])))
                        yield return obj;
                }
            }
        }

        /// <summary>Get a villager's loved items.</summary>
        /// <param name="name">The villager's name.</param>
        private IEnumerable<SObject> GetLikedItems(string name)
        {
            // get raw data
            Game1.NPCGiftTastes.TryGetValue(name, out string text);
            if (text == null)
                yield break;

            // parse
            string[] data = text.Split('/');
            string[] likedIDs = data[3].Split(' ');
            foreach (string likedID in likedIDs)
            {
                foreach (SObject obj in this.GetItems(Convert.ToInt32(likedID)))
                    yield return obj;
            }
        }

        /// <summary>Get a villager's loved items.</summary>
        /// <param name="name">The villager's name.</param>
        private IEnumerable<SObject> GetLovedItems(string name)
        {
            // get raw data
            Game1.NPCGiftTastes.TryGetValue(name, out string text);
            if (text == null)
                yield break;

            // parse
            string[] data = text.Split('/');
            string[] lovedIDs = data[1].Split(' ');
            foreach (string lovedID in lovedIDs)
            {
                foreach (SObject obj in this.GetItems(Convert.ToInt32(lovedID)))
                    yield return obj;
            }
        }

        /// <summary>Get the items matching the given ID.</summary>
        /// <param name="id">The category or item ID.</param>
        private IEnumerable<SObject> GetItems(int id)
        {
            return id < 0
                ? ObjectUtility.GetObjectsInCategory(id)
                : new[] { new SObject(id, 1) };
        }

        /// <summary>Get the items matching the given ID.</summary>
        /// <param name="id">The category or item ID.</param>
        /// <param name="stack">The stack size.</param>
        private IEnumerable<SObject> GetItems(int id, int stack)
        {
            foreach (SObject obj in this.GetItems(id))
                yield return new SObject(obj.ParentSheetIndex, stack);
        }

        /// <summary>Get whether today is the player's birthday.</summary>
        private bool IsBirthday()
        {
            return
                this.PlayerData.BirthdayDay == Game1.dayOfMonth
                && this.PlayerData.BirthdaySeason == Game1.currentSeason;
        }

        /// <summary>Migrate the legacy settings for the current player.</summary>
        private void MigrateLegacyData()
        {
            // skip if no legacy data or new data already exists
            try
            {
                if (!File.Exists(this.LegacyDataFilePath) || File.Exists(this.DataFilePath))
                    if (this.PlayerData == null) this.PlayerData = new PlayerData();
                         return;
            }
            catch(Exception err)
            {
                err.ToString();
                // migrate to new file
                try
                {
                    string[] text = File.ReadAllLines(this.LegacyDataFilePath);
                    Helper.Data.WriteJsonFile(this.DataFilePath, new PlayerData
                    {
                        BirthdaySeason = text[3],
                        BirthdayDay = Convert.ToInt32(text[5])
                    });

                    FileInfo file = new FileInfo(this.LegacyDataFilePath);
                    file.Delete();
                    if (!file.Directory.EnumerateFiles().Any())
                        file.Directory.Delete();
                }
                catch (Exception ex)
                {
                    this.Monitor.Log($"Error migrating data from the legacy 'Player_Birthdays' folder for the current player. Technical details:\n {ex}", LogLevel.Error);
                }
            }
            
        }

    }
}
