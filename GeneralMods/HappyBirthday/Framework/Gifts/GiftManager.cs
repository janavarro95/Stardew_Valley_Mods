using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using ContentPatcher;
using Omegasis.HappyBirthday.Framework;
using Omegasis.HappyBirthday.Framework.Configs;
using Omegasis.HappyBirthday.Framework.ContentPack;
using Omegasis.HappyBirthday.Framework.Gifts;
using Omegasis.HappyBirthday.Framework.Utilities;
using StardewModdingAPI;
using StardewValley;
using static System.String;
using static Omegasis.HappyBirthday.Framework.Gifts.GiftIDS;
using SObject = StardewValley.Object;

namespace Omegasis.HappyBirthday
{
    public class GiftManager
    {

        public Dictionary<string, string> legacyGiftIdResolverMap;
        //TODO: Make this the source of truth for what gifts can be selected from the favorite gift menu.
        public List<string> listOfFavoriteBirthdayGiftsToSelectFrom;

        /// <summary>The next birthday gift the player will receive.</summary>
        public Item BirthdayGiftToReceive;


        public Dictionary<string, List<GiftInformation>> npcBirthdayGifts;
        public Dictionary<string, List<GiftInformation>> spouseBirthdayGifts;
        public List<GiftInformation> defaultBirthdayGifts;

        public event EventHandler PostAllBirthdayGiftsRegistered;

        /// <summary>Construct an instance.</summary>
        public GiftManager()
        {
            //this.BirthdayGifts = new List<Item>();


            this.npcBirthdayGifts = new Dictionary<string, List<GiftInformation>>();
            this.spouseBirthdayGifts = new Dictionary<string, List<GiftInformation>>();
            this.defaultBirthdayGifts = new List<GiftInformation>();
            this.legacyGiftIdResolverMap = new Dictionary<string, string>();
            this.listOfFavoriteBirthdayGiftsToSelectFrom = new List<string>();
            this.rebuildLegacyGiftIdCache();
        }

        public virtual void rebuildLegacyGiftIdCache()
        {
            this.legacyGiftIdResolverMap.Clear();
            foreach (var v in GiftIDS.getLegacyGiftIds())
            {
                string legacyId = "StardewValley.Object." + Enum.GetName(typeof(GiftIDS.LegacyGiftIds), (int)v);
                string remappedId = "(O)"+(int)v;
                HappyBirthdayModCore.Instance.Monitor.Log("Building legacy gift id: " + legacyId + " "+ remappedId);
                this.legacyGiftIdResolverMap.Add(legacyId, remappedId);
            }
        }

        /// <summary>
        /// Reloads all of the birthday gifts from content packs.
        /// </summary>
        public virtual void reloadBirthdayGifts()
        {

            this.npcBirthdayGifts.Clear();
            this.spouseBirthdayGifts.Clear();
            this.defaultBirthdayGifts.Clear();
            this.listOfFavoriteBirthdayGiftsToSelectFrom.Clear();

            this.addInPotentialGiftsFromNPCsFromContentPacks();
            this.rebuildLegacyGiftIdCache();
        }

        public virtual void populatePotentialFavoriteGiftForPlayer()
        {
            //TODO: Fill this out for spouse gift selection menu.
        }


        /// <summary>
        /// Called after all content packs have been loaded. It then combines the gifts from all of the content packs into this singular gift pool.
        /// </summary>
        public virtual void addInPotentialGiftsFromNPCsFromContentPacks()
        {



            //Loads in all gifts across all content packs across all translations.
            foreach (HappyBirthdayContentPack contentPack in HappyBirthdayModCore.Instance.happyBirthdayContentPackManager.contentPacks.Values.SelectMany(contentPackList => contentPackList))
            {
                HappyBirthdayModCore.Instance.Monitor.Log("Adding default gifts for content pack: " + contentPack.baseContentPack.Manifest.UniqueID);
                foreach (GiftInformation giftInfo in contentPack.getDefaultBirthdayGifts())
                {
                    this.defaultBirthdayGifts.Add(giftInfo);
                }
                foreach (KeyValuePair<string, List<GiftInformation>> giftInfo in contentPack.npcBirthdayGifts)
                {
                    if (this.npcBirthdayGifts.ContainsKey(giftInfo.Key))
                    {
                        HappyBirthdayModCore.Instance.Monitor.Log(string.Format("Adding npc {1} gifts for content pack: {0}", contentPack.baseContentPack.Manifest.UniqueID, giftInfo.Key));
                        this.npcBirthdayGifts[giftInfo.Key].AddRange(giftInfo.Value);
                    }
                    else
                    {
                        HappyBirthdayModCore.Instance.Monitor.Log(string.Format("Adding npc {1} gifts for content pack: {0}", contentPack.baseContentPack.Manifest.UniqueID, giftInfo.Key));
                        this.npcBirthdayGifts.Add(giftInfo.Key, giftInfo.Value);
                    }
                }
                foreach (KeyValuePair<string, List<GiftInformation>> giftInfo in contentPack.spouseBirthdayGifts)
                {
                    if (this.spouseBirthdayGifts.ContainsKey(giftInfo.Key))
                    {
                        HappyBirthdayModCore.Instance.Monitor.Log(string.Format("Adding spouse {1} gifts for content pack: {0}", contentPack.baseContentPack.Manifest.UniqueID, giftInfo.Key));
                        this.spouseBirthdayGifts[giftInfo.Key].AddRange(giftInfo.Value);
                    }
                    else
                    {
                        HappyBirthdayModCore.Instance.Monitor.Log(string.Format("Adding spouse {1} gifts for content pack: {0}", contentPack.baseContentPack.Manifest.UniqueID, giftInfo.Key));
                        this.spouseBirthdayGifts.Add(giftInfo.Key, giftInfo.Value);
                    }
                }
            }

            if (PostAllBirthdayGiftsRegistered != null)
            {
                PostAllBirthdayGiftsRegistered.Invoke(this, new EventArgs());
            }
        }

        public virtual bool registerDefaultBirthdayGift(string UniqueGiftId, int MinHeartsRequiredForGift, int MaxHeartsRequiredForGift, int MinStackAmount, int MaxStackAmount)
        {

            return this.registerDefaultBirthdayGift(new GiftInformation(UniqueGiftId, MinHeartsRequiredForGift, MaxHeartsRequiredForGift, MinStackAmount, MaxStackAmount));

        }

        public virtual bool registerDefaultBirthdayGift(GiftInformation giftInformation)
        {
            this.defaultBirthdayGifts.Add(giftInformation);
            return true;
        }

        public virtual bool unregisterDefaultBirthdayGift(string UniqueGiftId, int MinHeartsRequiredForGift, int MaxHeartsRequiredForGift, int MinStackAmount, int MaxStackAmount)
        {
            return this.unregisterDefaultBirthdayGift(new GiftInformation(UniqueGiftId, MinHeartsRequiredForGift, MaxHeartsRequiredForGift, MinStackAmount, MaxStackAmount));
        }

        public virtual bool unregisterDefaultBirthdayGift(GiftInformation giftInformation)
        {
            List<GiftInformation> removalList = new();
            for (int i = 0; i < this.defaultBirthdayGifts.Count; i++)
            {
                if (giftInformation.Equals(this.defaultBirthdayGifts[i]))
                {
                    removalList.Add(this.defaultBirthdayGifts[i]);
                }
            }
            foreach (GiftInformation info in removalList)
            {
                this.defaultBirthdayGifts.Remove(info);
            }
            return removalList.Count > 0;
        }

        public virtual bool registerNpcBirthdayGift(string NPC, string UniqueGiftId, int MinHeartsRequiredForGift, int MaxHeartsRequiredForGift, int MinStackAmount, int MaxStackAmount)
        {
            return this.registerNpcBirthdayGift(NPC, new GiftInformation(UniqueGiftId, MinHeartsRequiredForGift, MaxHeartsRequiredForGift, MinStackAmount, MaxStackAmount));
        }

        public virtual bool registerNpcBirthdayGift(string NPC, GiftInformation giftInformation)
        {
            if (this.npcBirthdayGifts.ContainsKey(giftInformation.objectID))
            {
                this.npcBirthdayGifts[NPC].Add(giftInformation);
            }
            else
            {
                this.npcBirthdayGifts.Add(NPC, new List<GiftInformation>() { giftInformation });
            }
            return true;
        }

        public virtual bool unregisterNPCBirthdayGift(string NPC, string UniqueGiftId, int MinHeartsRequiredForGift, int MaxHeartsRequiredForGift, int MinStackAmount, int MaxStackAmount)
        {
            return this.unregisterNPCBirthdayGift(NPC, new GiftInformation(UniqueGiftId, MinHeartsRequiredForGift, MaxHeartsRequiredForGift, MinStackAmount, MaxStackAmount));
        }

        public virtual bool unregisterNPCBirthdayGift(string NPC, GiftInformation giftInformation)
        {
            if (!this.npcBirthdayGifts.ContainsKey(NPC)) return false;
            List<GiftInformation> removalList = new();
            for (int i = 0; i < this.npcBirthdayGifts[NPC].Count; i++)
            {
                if (giftInformation.Equals(this.npcBirthdayGifts[NPC][i]))
                {
                    removalList.Add(this.npcBirthdayGifts[NPC][i]);
                }
            }
            foreach (GiftInformation info in removalList)
            {
                this.npcBirthdayGifts[NPC].Remove(info);
            }
            return removalList.Count > 0;
        }

        public virtual bool registerSpouseBirthdayGift(string SpouseName, string UniqueGiftId, int MinHeartsRequiredForGift, int MaxHeartsRequiredForGift, int MinStackAmount, int MaxStackAmount)
        {
            return this.registerSpouseBirthdayGift(SpouseName, new GiftInformation(UniqueGiftId, MinHeartsRequiredForGift, MaxHeartsRequiredForGift, MinStackAmount, MaxStackAmount));
        }

        public virtual bool registerSpouseBirthdayGift(string SpouseName, GiftInformation giftInformation)
        {
            if (this.spouseBirthdayGifts.ContainsKey(giftInformation.objectID))
            {
                this.spouseBirthdayGifts[SpouseName].Add(giftInformation);
            }
            else
            {
                this.spouseBirthdayGifts.Add(SpouseName, new List<GiftInformation>() { giftInformation });
            }
            return true;
        }


        public virtual bool unregisterSpouseBirthdayGift(string Spouse, string UniqueGiftId, int MinHeartsRequiredForGift, int MaxHeartsRequiredForGift, int MinStackAmount, int MaxStackAmount)
        {
            return this.unregisterSpouseBirthdayGift(Spouse, new GiftInformation(UniqueGiftId, MinHeartsRequiredForGift, MaxHeartsRequiredForGift, MinStackAmount, MaxStackAmount));
        }

        public virtual bool unregisterSpouseBirthdayGift(string Spouse, GiftInformation giftInformation)
        {
            if (!this.spouseBirthdayGifts.ContainsKey(Spouse)) return false;
            List<GiftInformation> removalList = new();
            for (int i = 0; i < this.spouseBirthdayGifts[Spouse].Count; i++)
            {
                if (giftInformation.Equals(this.spouseBirthdayGifts[Spouse][i]))
                {
                    removalList.Add(this.spouseBirthdayGifts[Spouse][i]);
                }
            }
            foreach (GiftInformation info in removalList)
            {
                this.spouseBirthdayGifts[Spouse].Remove(info);
            }
            return removalList.Count > 0;
        }



        /// <summary>
        /// Gets the next birthday gift that would be received by the given npc.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Item getNextBirthdayGift(string name)
        {
            if (Game1.player.friendshipData.ContainsKey(name))
            {
                if (Game1.player.getSpouse() != null)
                {
                    if (Game1.player.getSpouse().Name.Equals(name))
                    {
                        //Get spouse gift here
                        Item gift = this.getSpouseBirthdayGift(name);
                        return gift;
                    }
                    else
                    {
                        Item gift = this.getNonSpouseBirthdayGift(name);
                        return gift;
                    }
                }
                else
                {
                    if (this.npcBirthdayGifts.ContainsKey(name))
                    {
                        Item gift = this.getNonSpouseBirthdayGift(name);
                        return gift;
                    }
                    else
                    {
                        Item gift = this.getDefaultBirthdayGift(name);
                        return gift;

                    }
                }
            }
            else
            {
                if (this.npcBirthdayGifts.ContainsKey(name))
                {

                    Item gift = this.getNonSpouseBirthdayGift(name);
                    return gift;
                }
                else
                {
                    Item gift = this.getDefaultBirthdayGift(name);
                    return gift;
                }
            }
        }

        /// <summary>
        /// Tries to get a default spouse birthday gift.
        /// </summary>
        /// <param name="name"></param>
        public Item getNonSpouseBirthdayGift(string name)
        {
            int heartLevel = Game1.player.getFriendshipHeartLevelForNPC(name);

            List<GiftInformation> possibleItems = new List<GiftInformation>();
            if (this.npcBirthdayGifts.ContainsKey(name))
            {
                List<GiftInformation> npcPossibleGifts = this.npcBirthdayGifts[name];

                foreach (GiftInformation info in npcPossibleGifts)
                {

                    if (info == null)
                    {
                        continue;
                    }

                    if (info.minRequiredHearts <= heartLevel && heartLevel <= info.maxRequiredHearts)
                    {
                        possibleItems.Add(info);
                    }
                }

                Item gift;
                int index = StardewValley.Game1.random.Next(possibleItems.Count);
                gift = possibleItems[index].getOne();
                return gift;

            }
            else
            {
                Item gift = this.getDefaultBirthdayGift(name);
                return gift;
            }

        }


        /// <summary>
        /// Tries to get a spouse birthday gift.
        /// </summary>
        /// <param name="name"></param>
        public Item getSpouseBirthdayGift(string name)
        {
            if (HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData.potentialFavoriteGifts.Count > 0)
            {
                int giftValue = Game1.random.Next(HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData.potentialFavoriteGifts.Count);
                string selectedBirthdayGift = HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData.potentialFavoriteGifts.ElementAt(giftValue);

                return this.getItemFromId(selectedBirthdayGift);
            }

            int heartLevel = Game1.player.getFriendshipHeartLevelForNPC(name);


            List<GiftInformation> possibleItems = new List<GiftInformation>();
            if (this.spouseBirthdayGifts.ContainsKey(name))
            {
                List<GiftInformation> npcPossibleGifts = this.spouseBirthdayGifts[name];
                foreach (GiftInformation info in npcPossibleGifts)
                {
                    if (info.minRequiredHearts <= heartLevel && heartLevel <= info.maxRequiredHearts)
                    {
                        possibleItems.Add(info);
                    }
                }

                Item gift;
                int index = StardewValley.Game1.random.Next(possibleItems.Count);
                gift = possibleItems[index].getOne();
                return gift;
            }
            else
            {
                return this.getNonSpouseBirthdayGift(name);
            }

        }

        /// <summary>
        /// Tries to get a default birthday gift.
        /// </summary>
        /// <param name="name"></param>
        public Item getDefaultBirthdayGift(string name)
        {
            int heartLevel = Game1.player.getFriendshipHeartLevelForNPC(name);

            List<GiftInformation> possibleItems = new List<GiftInformation>();

            List<GiftInformation> npcPossibleGifts = this.defaultBirthdayGifts;

            //Just give the player a random item if there are no default registered gifts.
            if (this.defaultBirthdayGifts.Count == 0)
            {
                int randomItemIndex = Game1.random.Next(this.legacyGiftIdResolverMap.Count);
                Item randomItem = this.getItemFromId(this.legacyGiftIdResolverMap.ElementAt(randomItemIndex).Value);
                return randomItem;
            }

            //If for some reason this given npc doesn't have a gift just keep picking random npcs until something happens.
            if (npcPossibleGifts.Count == 0)
            {
                List<NPC> npcs = NPCUtilities.GetAllNonSpecialHumanNpcs();
                int npcRandomDefault = Game1.random.Next(npcs.Count);
                NPC npc = npcs[npcRandomDefault];
                return this.getDefaultBirthdayGift(npc.Name);
            }

            foreach (GiftInformation info in npcPossibleGifts)
            {
                if (info.minRequiredHearts <= heartLevel && heartLevel <= info.maxRequiredHearts)
                {
                    possibleItems.Add(info);
                }
            }



            Item gift;
            try
            {
                int index = StardewValley.Game1.random.Next(possibleItems.Count);
                gift = possibleItems[index].getOne();
                return gift;
            }
            catch (Exception err)
            {
                throw new ArgumentOutOfRangeException(string.Format("There were no possible items in the list of possible items for this npc's birthday gift list. The Npcs name is {0} and the original error message is this {1}", name, err.ToString()));
            }


        }

        /// <summary>
        /// Actually sets the next birthday gift to receieve or drops it on the ground for the player to pick up afterwards.
        /// </summary>
        /// <param name="gift"></param>
        public virtual void setNextBirthdayGift(Item gift)
        {
            if (Game1.player.isInventoryFull())
                Game1.createItemDebris(gift, Game1.player.getStandingPosition(), Game1.player.getDirection());
            else
                this.BirthdayGiftToReceive = gift;
        }

        /// <summary>Set the next birthday gift the player will receive.</summary>
        /// <param name="name">The villager's name who's giving the gift.</param>
        /// <remarks>This returns gifts based on the speaker's heart level towards the player: neutral for 3-4, good for 5-6, and best for 7-10.</remarks>
        public void setNextBirthdayGift(string name)
        {
            Item gift = this.getNextBirthdayGift(name);
            this.setNextBirthdayGift(gift);
        }

        public Item getItemFromId(string itemId, int amount = 1, int quality = 0, bool allowNull = false)
        {
            string newId = this.resolveLegacyGiftId(itemId);
            HappyBirthdayModCore.Instance.Monitor.Log("Requesting gift with id:" + newId);
            return ItemRegistry.Create<Item>(newId, amount, quality, allowNull);
        }

        public string resolveLegacyGiftId(string id)
        {
            if (this.legacyGiftIdResolverMap.ContainsKey(id))
            {
                return this.legacyGiftIdResolverMap[id];
            }
            return id;
        }

        public string getRandomPossibleGiftMailStringFromMom()
        {

            string message = "";
            string key = "Mods/Omegasis.HappyBirthday/MailOnlyGifts:HappyBirthday_Mom";
            try
            {
                message = Game1.content.LoadString(key);
            }
            catch
            {
                message = "";
            }

            var api = HappyBirthdayModCore.Instance.Helper.ModRegistry.GetApi<IContentPatcherAPI>("Pathoschild.ContentPatcher");
            IManagedTokenString tokenString = api.ParseTokenString(HappyBirthdayModCore.Instance.ModManifest, message, new SemanticVersion("2.9.0"));
            tokenString.UpdateContext();
            if (tokenString.Value != null)
            {
                message = tokenString.Value;
            }

            if (!string.IsNullOrEmpty(message) && !message.Equals(key))
            {
                List<string> giftInformation = message.Split("%item").ToList();
                giftInformation.RemoveAll(string.IsNullOrEmpty);

                if (giftInformation.Count > 0)
                {

                    int index = Game1.random.Next(0, giftInformation.Count);
                    string chosenString = giftInformation[index];

                    if (chosenString.StartsWith(" "))
                    {
                        chosenString = chosenString.Trim();
                    }
                    if (!string.IsNullOrEmpty(chosenString))
                    {
                        if (chosenString.StartsWith("money") || chosenString.StartsWith("id"))
                        {
                            return "%item " + chosenString + " %%";
                        }
                    }

                }
            }



            if (HappyBirthdayModCore.Configs.mailConfig.momBirthdayGiftsToGive.Count == 0)
            {
                return MailUtilities.GetItemMailStringFormat(HappyBirthdayModCore.Configs.mailConfig.momBirthdayItemGive, 1, "");
            }

            string chosen = HappyBirthdayModCore.Configs.mailConfig.momBirthdayGiftsToGive[Game1.random.Next(0, HappyBirthdayModCore.Configs.mailConfig.momBirthdayGiftsToGive.Count)];
            string[] splits = chosen.Split(' ');

            if (splits.Length == 0)
            {
                return MailUtilities.GetItemMailStringFormat(HappyBirthdayModCore.Configs.mailConfig.momBirthdayItemGive, 1, "");
            }

            if (splits.Length == 1)
            {
                return MailUtilities.GetItemMailStringFormat(splits[0], 1, "");
            }

            return MailUtilities.GetItemMailStringFormat(splits[0], Convert.ToInt32(splits[1]),"");

        }

        public string getRandomPossibleGiftMailStringFromDad()
        {

            string message = "";
            string key = "Mods/Omegasis.HappyBirthday/MailOnlyGifts:HappyBirthday_Dad";
            try
            {
                message = Game1.content.LoadString(key);
            }
            catch(Exception e)
            {

                message = "";
            }

            var api = HappyBirthdayModCore.Instance.Helper.ModRegistry.GetApi<IContentPatcherAPI>("Pathoschild.ContentPatcher");
            IManagedTokenString tokenString = api.ParseTokenString(HappyBirthdayModCore.Instance.ModManifest, message, new SemanticVersion("2.9.0"));
            tokenString.UpdateContext();
            if (tokenString.Value != null)
            {
                message = tokenString.Value;
            }

            if (!string.IsNullOrEmpty(message) && !message.Equals(key))
            {
                List<string> giftInformation = message.Split("%item").ToList();
                giftInformation.RemoveAll(string.IsNullOrEmpty);


                if (giftInformation.Count > 0)
                {

                    int index = Game1.random.Next(0, giftInformation.Count);
                    string chosenString = giftInformation[index];
                    if (chosenString.StartsWith(" "))
                    {
                        chosenString = chosenString.Trim();
                    }
                    if (!string.IsNullOrEmpty(chosenString))
                    {
                        if (chosenString.StartsWith("money") || chosenString.StartsWith("id"))
                        {
                            return "%item " + chosenString + " %%";
                        }
                    }

                }
            }

            if (message.Equals(key))
            {
                return "";
            }

            return message;

        }
    }

}
