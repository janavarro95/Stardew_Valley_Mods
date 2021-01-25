using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Omegasis.HappyBirthday.Framework;
using StardewValley;
using static System.String;
using SObject = StardewValley.Object;

namespace Omegasis.HappyBirthday
{
    public class GiftManager
    {
        public ModConfig Config => HappyBirthday.Config;

        private Dictionary<string, string> defaultBirthdayGifts = new Dictionary<string, string>()
        {
            ["Universal_Love_Gift"] = "74 1 446 1 204 1 446 5 773 1",
            ["Universal_Like_Gift"] = "-2 3 -7 1 -26 2 -75 5 -80 3 72 1 220 1 221 1 395 1 613 1 634 1 635 1 636 1 637 1 638 1 724 1 233 1 223 1 465 20 -79 5",
            ["Universal_Neutral_Gift"] = "194 1 262 5 -74 5 -75 3 334 5 335 1 390 20 388 20 -81 5 -79 3",
            ["Robin"] = " BestGifts/224 1 426 1 636 1/GoodGift/-6 5 -79 5 424 1 709 1/NeutralGift//",
            ["Demetrius"] = " Best Gifts/207 1 232 1 233 1 400 1/Good Gifts/-5 3 -79 5 422 1/NeutralGift/-4 3/",
            ["Maru"] = " BestGift/72 1 197 1 190 1 215 1 222 1 243 1 336 1 337 1 400 1 787 1/Good Gift/-260 1 62 1 64 1 66 1 68 1 70 1 334 1 335 1 725 1 726 1/NeutralGift/",
            ["Sebastian"] = " Best/84 1 227 1 236 1 575 1 305 1 /Good/267 1 276 1/Neutral/-4 3/",
            ["Linus"] = " Best/88 1 90 1 234 1 242 1 280 1/Good/-5 3 -6 5 -79 5 -81 10/Neutral/-4 3/",
            ["Pierre"] = " Best/202 1/Good/-5 3 -6 5 -7 1 18 1 22 1 402 1 418 1 259 1/Neutral//",
            ["Caroline"] = " Best/213 1 593 1/Good/-7 1 18 1 402 1 418 1/Neutral// ",
            ["Abigail"] = " Best/66 1 128 1 220 1 226 1 276 1 611 1/Good//Neutral// ",
            ["Alex"] = " Best/201 1 212 1 662 1 664 1/Good/-5 3/Neutral// ",
            ["George"] = " Best/20 1 205 1/Good/18 1 195 1 199 1 200 1 214 1 219 1 223 1 231 1 233 1/Neutral// ",
            ["Evelyn"] = " Best/72 1 220 1 239 1 284 1 591 1 595 1/Good/-6 5 18 1 402 1 418 1/Neutral// ",
            ["Lewis"] = " Best/200 1 208 1 235 1 260 1/Good/-80 5 24 1 88 1 90 1 192 1 258 1 264 1 272 1 274 1 278 1/Neutral// ",
            ["Clint"] = " Best/60 1 62 1 64 1 66 1 68 1 70 1 336 1 337 1 605 1 649 1 749 1 337 5/Good/334 20 335 10 336 5/Neutral// ",
            ["Penny"] = " Best/60 1 376 1 651 1 72 1 164 1 218 1 230 1 244 1 254 1/Good/-6 5 20 1 22 1/Neutral// ",
            ["Pam"] = " Best/24 1 90 1 199 1 208 1 303 1 346 1/Good/-6 5 -75 5 -79 5 18 1 227 1 228 1 231 1 232 1 233 1 234 1 235 1 236 1 238 1 402 1 418 1/Neutral/-4 3/ ",
            ["Emily"] = " Best/60 1 62 1 64 1 66 1 68 1 70 1 241 1 428 1 440 1 /Good/18 1 82 1 84 1 86 1 196 1 200 1 207 1 230 1 235 1 402 1 418 1/Neutral// ",
            ["Haley"] = " Best/221 1 421 1 610 1 88 1 /Good/18 1 60 1 62 1 64 1 70 1 88 1 222 1 223 1 232 1 233 1 234 1 402 1 418 1 /Neutral// ",
            ["Jas"] = " Best/221 1 595 1 604 1 /Good/18 1 60 1 64 1 70 1 88 1 232 1 233 1 234 1 222 1 223 1 340 1 344 1 402 1 418 1 /Neutral// ",
            ["Vincent"] = " Best/221 1 398 1 612 1 /Good/18 1 60 1 64 1 70 1 88 1 232 1 233 1 234 1 222 1 223 1 340 1 344 1 402 1 418 1 /Neutral// ",
            ["Jodi"] = " Best/72 1 200 1 211 1 214 1 220 1 222 1 225 1 231 1 /Good/-5 3 -6 5 -79 5 18 1 402 1 418 1 /Neutral// ",
            ["Kent"] = " Best/607 1 649 1 /Good/-5 3 -79 5 18 1 402 1 418 1 /Neutral// ",
            ["Sam"] = " Best/90 1 206 1 655 1 658 1 562 1 731 1/Good/167 1 210 1 213 1 220 1 223 1 224 1 228 1 232 1 233 1 239 1 -5 3/Neutral// ",
            ["Leah"] = " Best/196 1 200 1 348 1 606 1 651 1 650 1 426 1 430 1 /Good/-5 3 -6 5 -79 5 -81 10 18 1 402 1 406 1 408 1 418 1 86 1 /Neutral// ",
            ["Shane"] = " Best/206 1 215 1 260 1 346 1 /Good/-5 3 -79 5 303 1 /Neutral// ",
            ["Marnie"] = " Best/72 1 221 1 240 1 608 1 /Good/-5 3 -6 5 402 1 418 1 /Neutral// ",
            ["Elliott"] = " Best/715 1 732 1 218 1 444 1 /Good/727 1 728 1 -79 5 60 1 80 1 82 1 84 1 149 1 151 1 346 1 348 1 728 1 /Neutral/-4 3 / ",
            ["Gus"] = " Best/72 1 213 1 635 1 729 1 /Good/348 1 303 1 -7 1 18 1 /Neutral// ",
            ["Dwarf"] = " Best/60 1 62 1 64 1 66 1 68 1 70 1 749 1 /Good/82 1 84 1 86 1 96 1 97 1 98 1 99 1 121 1 122 1 /Neutral/-28 20 / ",
            ["Wizard"] = " Best/107 1 155 1 422 1 769 1 768 1 /Good/-12 3 72 1 82 1 84 1/Neutral// ",
            ["Harvey"] = " Best/348 1 237 1 432 1 395 1 342 1 /Good/-81 10 -79 5 -7 1 402 1 418 1 422 1 436 1 438 1 442 1 444 1 422 1 /Neutral// ",
            ["Sandy"] = " Best/18 1 402 1 418 1 /Good/-75 5 -79 5 88 1 428 1 436 1 438 1 440 1 /Neutral// ",
            ["Willy"] = " Best/72 1 143 1 149 1 154 1 276 1 337 1 698 1 /Good/66 1 336 1 340 1 699 1 707 1 /Neutral/-4 3 / ",
            ["Krobus"] = " Best/72 1 16 1 276 1 337 1 305 1 /Good/66 1 336 1 340 1 /Neutral// "
        };

        public Dictionary<string, string> defaultSpouseBirthdayGifts = new Dictionary<string, string>()
        {

            ["Universal_Gifts"] = "74 1 446 1 204 1 446 5 773 1",
            ["Alex"] = "",
            ["Elliott"] = "",
            ["Harvey"] = "",
            ["Sam"] = "",
            ["Sebastian"] = "",
            ["Shane"] = "",
            ["Abigail"] = "",
            ["Emily"] = "",
            ["Haley"] = "",
            ["Leah"] = "",
            ["Maru"] = "",
            ["Penny"] = "",
        };

        public List<Item> BirthdayGifts;

        /// <summary>The next birthday gift the player will receive.</summary>
        public Item BirthdayGiftToReceive;

        /// <summary>Construct an instance.</summary>
        public GiftManager()
        {
            this.BirthdayGifts = new List<Item>();
            this.loadVillagerBirthdayGifts();
            this.loadSpouseBirthdayGifts();
        }

        /// <summary>Load birthday gift information from disk. Preferably from BirthdayGift.json in the mod's directory.</summary>
        public void loadVillagerBirthdayGifts()
        {
            string villagerGifts = Path.Combine("Content", "Gifts", "BirthdayGifts.json");
            if (!HappyBirthday.Config.useLegacyBirthdayFiles)
            {

                if (File.Exists(Path.Combine(HappyBirthday.ModHelper.DirectoryPath, villagerGifts)))
                    this.defaultBirthdayGifts = HappyBirthday.ModHelper.Data.ReadJsonFile<Dictionary<string, string>>(villagerGifts);
                else
                    HappyBirthday.ModHelper.Data.WriteJsonFile<Dictionary<string, string>>(villagerGifts, this.defaultBirthdayGifts);
            }
            else
            {
                if (File.Exists(Path.Combine(Game1.content.RootDirectory, "Data", "PossibleBirthdayGifts.xnb")))
                {
                    HappyBirthday.ModMonitor.Log("Legacy loading detected. Attempting to load from StardewValley/Content/Data/PossibleBirthdayGifts.xnb");
                    this.defaultBirthdayGifts = Game1.content.Load<Dictionary<string, string>>(Path.Combine("Data", "PossibleBirthdayGifts"));

                    HappyBirthday.ModHelper.Data.WriteJsonFile<Dictionary<string, string>>(villagerGifts, this.defaultBirthdayGifts);
                }
                else
                {
                    HappyBirthday.ModMonitor.Log("No birthday gift information found. Loading from internal birthday list and generating villagerGifts.json");
                    HappyBirthday.ModHelper.Data.WriteJsonFile<Dictionary<string, string>>(villagerGifts, this.defaultBirthdayGifts);
                }
            }
        }

        /// <summary>Used to load spouse birthday gifts from disk.</summary>
        public void loadSpouseBirthdayGifts()
        {
            string spouseGifts = Path.Combine("Content", "Gifts", "SpouseBirthdayGifts.json");
            if (File.Exists(Path.Combine(HappyBirthday.ModHelper.DirectoryPath, spouseGifts)))
            {
                HappyBirthday.ModMonitor.Log("Load from SpouseBirthdayGifts.json");
                this.defaultSpouseBirthdayGifts = HappyBirthday.ModHelper.Data.ReadJsonFile<Dictionary<string, string>>(spouseGifts);
            }
            else
            {
                HappyBirthday.ModMonitor.Log("SpouseBirthdayGifts.json created from default spouse birthday gift information and can be overridden.");
                HappyBirthday.ModHelper.Data.WriteJsonFile<Dictionary<string, string>>(spouseGifts, this.defaultSpouseBirthdayGifts);
            }
        }

        /// <summary>Get the default gift items.</summary>
        /// <param name="name">The villager's name.</param>
        private IEnumerable<SObject> GetDefaultBirthdayGifts(string name)
        {
            List<SObject> gifts = new List<SObject>();
            try
            {
                // read from birthday gifts file
                IDictionary<string, string> data = this.defaultBirthdayGifts;
                data.TryGetValue(name, out string text);
                if (text != null)
                {
                    string[] fields = text.Split('/');

                    // love
                    if (Game1.player.getFriendshipHeartLevelForNPC(name) >= this.Config.minLoveFriendshipLevel)
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
                    if (Game1.player.getFriendshipHeartLevelForNPC(name) >= this.Config.minLikeFriendshipLevel && Game1.player.getFriendshipHeartLevelForNPC(name) <= this.Config.maxLikeFriendshipLevel)
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
                    if (Game1.player.getFriendshipHeartLevelForNPC(name) >= this.Config.minNeutralFriendshipGiftLevel && Game1.player.getFriendshipHeartLevelForNPC(name) <= this.Config.maxNeutralFriendshipGiftLevel)
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
                if (Game1.player.getFriendshipHeartLevelForNPC(name) >= this.Config.minLoveFriendshipLevel)
                    gifts.AddRange(this.GetUniversalItems("Love", true));
                if (Game1.player.getFriendshipHeartLevelForNPC(name) >= this.Config.minLikeFriendshipLevel && Game1.player.getFriendshipHeartLevelForNPC(name) <= this.Config.maxLikeFriendshipLevel)
                    this.BirthdayGifts.AddRange(this.GetUniversalItems("Like", true));
                if (Game1.player.getFriendshipHeartLevelForNPC(name) >= this.Config.minNeutralFriendshipGiftLevel && Game1.player.getFriendshipHeartLevelForNPC(name) <= this.Config.maxNeutralFriendshipGiftLevel)
                    this.BirthdayGifts.AddRange(this.GetUniversalItems("Neutral", true));
            }
            catch
            {
                // get NPC's preferred gifts
                if (Game1.player.getFriendshipHeartLevelForNPC(name) >= this.Config.minLoveFriendshipLevel)
                {
                    this.BirthdayGifts.AddRange(this.GetUniversalItems("Love", false));
                    this.BirthdayGifts.AddRange(this.GetLovedItems(name));
                }
                if (Game1.player.getFriendshipHeartLevelForNPC(name) >= this.Config.minLikeFriendshipLevel && Game1.player.getFriendshipHeartLevelForNPC(name) <= this.Config.maxLikeFriendshipLevel)
                {
                    this.BirthdayGifts.AddRange(this.GetLikedItems(name));
                    this.BirthdayGifts.AddRange(this.GetUniversalItems("Like", false));
                }
                if (Game1.player.getFriendshipHeartLevelForNPC(name) >= this.Config.minNeutralFriendshipGiftLevel && Game1.player.getFriendshipHeartLevelForNPC(name) <= this.Config.maxNeutralFriendshipGiftLevel)
                    this.BirthdayGifts.AddRange(this.GetUniversalItems("Neutral", false));
            }

            //Add in spouse specific birthday gifts.
            if (Game1.player.isMarried())
            {
                if (name == Game1.player.spouse)
                {
                    this.BirthdayGifts.AddRange(this.getSpouseBirthdayGifts(name));
                    this.BirthdayGifts.AddRange(this.getSpouseBirthdayGifts("Universal_Gifts"));
                }
            }

            return gifts;
        }

        private IEnumerable<Item> getSpouseBirthdayGifts(string npcName)
        {
            Dictionary<string, string> data = this.defaultSpouseBirthdayGifts;
            data.TryGetValue(npcName, out string text);
            if (IsNullOrEmpty(text))
                yield break;

            // parse
            string[] array = text.Split(' ');
            for (int i = 0; i < array.Length; i += 2)
            {
                foreach (SObject obj in this.GetItems(Convert.ToInt32(array[i]), Convert.ToInt32(array[i + 1])))
                    yield return obj;
            }
        }

        /// <summary>Get the items loved by all villagers.</summary>
        /// <param name="group">The group to get (one of <c>Like</c>, <c>Love</c>, <c>Neutral</c>).</param>
        /// <param name="isBirthdayGiftList">Whether to get data from <c>Data\BirthdayGifts.xnb</c> instead of the game data.</param>
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
                Dictionary<string, string> data = Game1.content.Load<Dictionary<string, string>>("Data\\BirthdayGifts");
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

        /// <summary>Set the next birthday gift the player will receive.</summary>
        /// <param name="name">The villager's name who's giving the gift.</param>
        /// <remarks>This returns gifts based on the speaker's heart level towards the player: neutral for 3-4, good for 5-6, and best for 7-10.</remarks>
        public void SetNextBirthdayGift(string name)
        {
            Item gift;
            if (this.BirthdayGifts.Count > 0)
            {
                Random random = new Random();
                int index = random.Next(this.BirthdayGifts.Count);
                gift = this.BirthdayGifts[index];
                if (Game1.player.isInventoryFull())
                    Game1.createItemDebris(gift, Game1.player.getStandingPosition(), Game1.player.getDirection());
                else
                    this.BirthdayGiftToReceive = gift;
                return;
            }

            this.BirthdayGifts.AddRange(this.GetDefaultBirthdayGifts(name));

            Random rnd2 = new Random();
            int r2 = rnd2.Next(this.BirthdayGifts.Count);
            gift = this.BirthdayGifts.ElementAt(r2);
            //Attempt to balance sapplings from being too OP as a birthday gift.
            if (gift.Name.Contains("Sapling"))
            {
                gift.Stack = 1; //A good investment?
            }
            if (gift.Name.Contains("Rare Seed"))
            {
                gift.Stack = 2; //Still a little op but less so than 5.
            }

            if (Game1.player.isInventoryFull())
                Game1.createItemDebris(gift, Game1.player.getStandingPosition(), Game1.player.getDirection());
            else
                this.BirthdayGiftToReceive = gift;

            this.BirthdayGifts.Clear();
        }
    }
}
