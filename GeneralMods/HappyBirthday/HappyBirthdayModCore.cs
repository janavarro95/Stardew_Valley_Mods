using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Omegasis.HappyBirthday.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Menus;
using StardewValley.Monsters;
using Omegasis.HappyBirthday.Framework.ContentPack;
using Omegasis.HappyBirthday.Framework.Utilities;
using Omegasis.HappyBirthday.Framework.Configs;
using Omegasis.HappyBirthday.Framework.Menus;
using Omegasis.HappyBirthday.Framework.Events;
using Omegasis.HappyBirthday.Framework.Gifts;
using Omegasis.StardustCore.Events;
using Omegasis.HappyBirthday.Framework.Compatibility;
using StardewValley.Locations;
using ContentPatcher;
using StardewValley.GameData.Objects;

namespace Omegasis.HappyBirthday
{
    /// <summary>The mod entry point.</summary>
    public class HappyBirthdayModCore : Mod
    {
        /*********
        ** Fields
        *********/

        /// <summary>
        /// Manages all of the configs for Happy Birthday.
        /// </summary>
        public static ConfigManager Configs;

        /// <summary>Class to handle all birthday messages for this mod.</summary>
        public BirthdayMessages birthdayMessages;

        /// <summary>Class to handle all birthday gifts for this mod.</summary>
        public GiftManager giftManager;

        public static HappyBirthdayModCore Instance;

        public HappyBirthdayContentPackManager happyBirthdayContentPackManager;

        /// <summary>Handles different translations of files.</summary>
        public TranslationInfo translationInfo;

        /// <summary>
        /// Utilities for checking if it's a player's birthday, seeing if npcs have given birthday wishes already, etc.
        /// </summary>
        public BirthdayManager birthdayManager;

        public bool contentPacksInitalized;

        public IStardewAccessApi screenreader;

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {

            Instance = this;
            Configs = new ConfigManager();
            Configs.initializeConfigs();

            this.Helper.Events.GameLoop.GameLaunched += this.GameLoop_GameLaunched;

            this.Helper.Events.GameLoop.DayStarted += this.OnDayStarted;
            this.Helper.Events.GameLoop.DayEnding += this.OnDayEnded;

            this.Helper.Events.GameLoop.SaveCreated += this.GameLoop_SaveCreated;

            this.Helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;

            this.Helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
            this.Helper.Events.GameLoop.Saving += this.OnSaving;

            this.Helper.Events.Input.ButtonPressed += this.OnButtonPressed;

            this.Helper.Events.Display.MenuChanged += MenuUtilities.OnMenuChanged;

            this.Helper.Events.Display.RenderedActiveMenu += RenderUtilities.OnRenderedActiveMenu;
            this.Helper.Events.Display.RenderedHud += RenderUtilities.OnRenderedHud;

            this.Helper.Events.Multiplayer.ModMessageReceived += MultiplayerUtilities.Multiplayer_ModMessageReceived;
            this.Helper.Events.Multiplayer.PeerDisconnected += MultiplayerUtilities.Multiplayer_PeerDisconnected;

            this.Helper.Events.Player.Warped += BirthdayEventUtilities.Player_Warped;

            this.Helper.Events.GameLoop.ReturnedToTitle += this.GameLoop_ReturnedToTitle;

            this.Helper.Events.Content.AssetRequested += this.Content_AssetRequested;

            this.birthdayManager = new BirthdayManager();

            this.happyBirthdayContentPackManager = new HappyBirthdayContentPackManager();

            this.translationInfo = new TranslationInfo();

            LocalizedContentManager.OnLanguageChange += this.LocalizedContentManager_OnLanguageChange;

            this.Helper.ConsoleCommands.Add("Omegasis.Happy_Birthday.reset_birthday", "Resets the player's birthday and allows for them to choose it again.", this.birthdayManager.resetPlayersBirthday);
            this.Helper.ConsoleCommands.Add("Omegasis.Happy_Birthday.list_all_locations", "Prints the names of all GameLocations to the console. Useful for making events.", this.PrintAllGameLocations);
        }

        private void Content_AssetRequested(object sender, AssetRequestedEventArgs e)
        {
            if (e.NameWithoutLocale.BaseName.Equals(@"Data/mail"))
            {
                e.Edit(MailUtilities.EditMailAsset);
            }
        }

        private void GameLoop_SaveCreated(object sender, SaveCreatedEventArgs e)
        {
            this.initalizeHappyBirthdayContent();
        }

        private void GameLoop_ReturnedToTitle(object sender, ReturnedToTitleEventArgs e)
        {
            this.birthdayManager.reset();
        }

        public override object GetApi()
        {
            return new HappyBirthday.Framework.API.HappyBirthdayAPI();
        }


        /*********
        ** Private methods
        *********/

        private void GameLoop_GameLaunched(object sender, GameLaunchedEventArgs e)
        {
            this.birthdayMessages = new BirthdayMessages();
            this.giftManager = new GiftManager();
            MenuUtilities.IsDailyQuestBoard = false;

            BirthdayEventUtilities.InitializeBirthdayEventCommands();

            this.screenreader = this.Helper.ModRegistry.GetApi<IStardewAccessApi>("shoaib.stardewaccess");


            var api = this.Helper.ModRegistry.GetApi<IContentPatcherAPI>("Pathoschild.ContentPatcher");
            api.RegisterToken(this.ModManifest, "IsPlayersBirthday", () =>
            {
                // save is loaded
                if (Context.IsWorldReady)
                    return [this.birthdayManager.isBirthday().ToString()];

                // or save is currently loading
                if (SaveGame.loaded?.player != null)
                    return [this.birthdayManager.isBirthday().ToString()];

                // no save loaded (e.g. on the title screen)
                return null;
            });

            api.RegisterToken(this.ModManifest, "MinimumHeartsForBirthdayWishes", () =>
            {
                if (HappyBirthdayModCore.Configs.modConfig == null)
                {
                    return ["2"];
                }
                //Instance.Monitor.Log("The token result is: " + HappyBirthdayModCore.Configs.modConfig.minimumFriendshipLevelForBirthdayWish.ToString()); 

                return [HappyBirthdayModCore.Configs.modConfig.minimumFriendshipLevelForBirthdayWish.ToString()];
            });
            api.RegisterToken(this.ModManifest, "AffectionateSpouseWord", () =>
            {
                return [this.birthdayMessages.getAffectionateSpouseWord()];
            });

            api.RegisterToken(this.ModManifest, "TimeOfDay", () =>
            {
                // save is loaded
                if (Context.IsWorldReady)
                    return [this.birthdayMessages.getTimeOfDayString()];

                // or save is currently loading
                if (SaveGame.loaded?.player != null)
                    return [this.birthdayMessages.getTimeOfDayString()];


                return null;
            });

            api.RegisterToken(this.ModManifest, "MomsBirthdayGift", () =>
            {
                // save is loaded
                if (Context.IsWorldReady)
                    return [this.giftManager.getRandomPossibleGiftMailStringFromMom()];

                // or save is currently loading
                if (SaveGame.loaded?.player != null)
                    return [this.giftManager.getRandomPossibleGiftMailStringFromMom()];

                // no save loaded (e.g. on the title screen)
                return null;
            });

            api.RegisterToken(this.ModManifest, "DadsBirthdayGift", () =>
            {
                // save is loaded
                if (Context.IsWorldReady)
                    return [this.giftManager.getRandomPossibleGiftMailStringFromDad()];

                // or save is currently loading
                if (SaveGame.loaded?.player != null)
                    return [this.giftManager.getRandomPossibleGiftMailStringFromDad()];

                // no save loaded (e.g. on the title screen)
                return null;
            });

            api.RegisterToken(this.ModManifest, "DadsMoneyAmount", () =>
            {
                if (Game1.year == 1)
                {
                    return [Convert.ToString(Configs.mailConfig.dadBirthdayYear1MoneyGivenAmount)];
                }
                else
                {
                    return [Convert.ToString(Configs.mailConfig.dadBirthdayMoneyGivenAmount)];
                }
            });

            api.RegisterToken(this.ModManifest, "RandomCookedDish", () =>
            {
                List<string> list = new List<string>();

                foreach (string id in Game1.objectData.Keys)
                {
                    ObjectData d = Game1.objectData[id];
                    if (d.Category == StardewValley.Object.CookingCategory)
                    {
                        list.Add(id);
                    }
                }
                int index = Game1.random.Next(list.Count);
                string chosenId = list[index];
                return [chosenId];
            });

            api.RegisterToken(this.ModManifest, "RandomFlower", () =>
            {
                List<string> list = new List<string>();

                foreach (string id in Game1.objectData.Keys)
                {
                    ObjectData d = Game1.objectData[id];
                    if (d.Category == StardewValley.Object.flowersCategory)
                    {
                        list.Add(id);
                    }
                }
                int index = Game1.random.Next(list.Count);
                string chosenId = list[index];
                return [chosenId];
            });

            api.RegisterToken(this.ModManifest, "RandomForage", () =>
            {
                List<string> list = new List<string>();

                foreach (string id in Game1.objectData.Keys)
                {
                    ObjectData d = Game1.objectData[id];
                    if (d.Category == -81)
                    {
                        list.Add(id);
                    }
                }
                int index = Game1.random.Next(list.Count);
                string chosenId = list[index];
                return [chosenId];
            });

            api.RegisterToken(this.ModManifest, "RandomFertilizer", () =>
            {
                List<string> list = new List<string>();

                foreach (string id in Game1.objectData.Keys)
                {
                    ObjectData d = Game1.objectData[id];
                    if (d.Category == StardewValley.Object.fertilizerCategory)
                    {
                        list.Add(id);
                    }
                }
                int index = Game1.random.Next(list.Count);
                string chosenId = list[index];
                return [chosenId];
            });

            api.RegisterToken(this.ModManifest, "RandomNonRareSeeds", () =>
            {
                List<string> list = new List<string>();

                foreach (string id in Game1.objectData.Keys)
                {
                    ObjectData d = Game1.objectData[id];
                    if (d.Category == StardewValley.Object.SeedsCategory)
                    {
                        if (id == "347")
                        {
                            //Sweet Gem Berry
                            continue;
                        }
                        if (id == "499")
                        {
                            //Ancient Seeds
                            continue;
                        }
                        if (d.Name.Contains("Sapling"))
                        {
                            //Don't include saplings.
                            continue;
                        }

                        list.Add(id);
                    }
                }
                int index = Game1.random.Next(list.Count);
                string chosenId = list[index];
                return [chosenId];
            });

        }

        private void LocalizedContentManager_OnLanguageChange(LocalizedContentManager.LanguageCode code)
        {
            //Reload the birthday gifts since they are local to the content packs.
            this.giftManager.reloadBirthdayGifts();
        }

        /// <summary>Raised after the game begins a new day (including when the player loads a save).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            SaveManager.OnDayStarted(sender, e);
            this.birthdayManager.onDayStarted(sender, e);

            BirthdayEventUtilities.ClearEventsFromFarmer();
            BirthdayEventUtilities.OnDayStarted();
        }

        private void OnDayEnded(object sender, DayEndingEventArgs e)
        {
            SaveManager.OnDayEnded(sender, e);
        }

        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            // show birthday selection menu
            if (Game1.activeClickableMenu != null) return;
            if (Context.IsPlayerFree && !this.birthdayManager.hasChosenBirthday() && e.Button == Configs.modConfig.KeyBinding)
                Game1.activeClickableMenu = new BirthdayMenu(this.birthdayManager.playerBirthdayData.BirthdaySeason, this.birthdayManager.playerBirthdayData.BirthdayDay, this.birthdayManager.setBirthday);
        }

        /// <summary>Raised after the player loads a save slot and the world is initialised.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            this.initalizeHappyBirthdayContent();
        }

        protected virtual void initalizeHappyBirthdayContent()
        {
            if (this.contentPacksInitalized) return;

            foreach (IContentPack contentPack in this.Helper.ContentPacks.GetOwned())
            {
                this.happyBirthdayContentPackManager.registerNewContentPack(contentPack);
            }

            this.giftManager.addInPotentialGiftsFromNPCsFromContentPacks();
            MailUtilities.RemoveAllBirthdayMail();

            BirthdayEventUtilities.InitializeBirthdayEvents();
            this.contentPacksInitalized = true;
        }

        /// <summary>Raised before the game begins writes data to the save file (except the initial save creation).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnSaving(object sender, SavingEventArgs e)
        {
            //SaveManager.Save(Game1.player.uniqueMultiplayerID);
        }

        /// <summary>Raised after the game state is updated (≈60 times per second).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {

            if (Game1.CurrentEvent != null)
            {
                BirthdayEventUtilities.UpdateEventManager();
                return;
            }

            if (!Context.IsWorldReady || Game1.isFestival())
            {
            }


            //Below code sets up menus for selecting the new birthday for the player.


            if (!this.birthdayManager.hasCheckedForBirthday() && Game1.activeClickableMenu == null)
            {

                this.birthdayManager.setCheckedForBirthday(true);

                this.birthdayManager.setUpPlayersBirthday();
            }

        }

        public void SayWithMenuChecker(string text, bool interrupt)
        {
            if (this.screenreader != null)
                this.screenreader.SayWithMenuChecker(text, interrupt);
        }


        private void PrintAllGameLocations(string name, string[] args)
        {
            foreach (GameLocation gl in Game1.locations)
            {
                this.Monitor.Log(gl.Name);
            }
        }


    }
}
