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

namespace Omegasis.HappyBirthday
{
    /// <summary>The mod entry point.</summary>
    public class HappyBirthdayModCore : Mod
    {
        //TODO: Migrate all original content pack dialogue to ContentPatcher dialogue.
        //TODO: Migrate all Spouse Dialogue. (Just have a when condition for when you are married)
        //TODO: Migrate all Mail
        //TODO: Migrate all events.
        //TODO: Migrate all translated strings (Can I just have a "fake") file and have Content Patcher take care of this?





        //TODO: Include changelog documentation for new command, ability to select multiple birthday gifts, and support for modded items.

        //TODO: Make an official StardewValleyExpanded ContentPack.
        //TODO: Add support for content packs to override event ids that are registered. Aka allow StardewValleyExpanded Content Pack events to override their vanilla counterparts.


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

        private int delay = 5;

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

            /*
            if (this.Helper.ContentPacks.GetOwned().Count() == 0)
            {
                this.drawDialogueBoxWithError("There are ZERO Happy birthday content packs found for the mod. Without at least one installed there is no guaranteed that this mod will work due to missing dialogue errors. Please install at least one HappyBirthdayContent pack before continuing. One can be found at https://www.nexusmods.com/stardewvalley/mods/11148 for English dialogue. Thank you!");
                return;
            }
            */
            
            foreach (IContentPack contentPack in this.Helper.ContentPacks.GetOwned())
            {
                this.happyBirthdayContentPackManager.registerNewContentPack(contentPack);
            }
            /*
            if (this.happyBirthdayContentPackManager.getHappyBirthdayContentPacksForCurrentLanguageCode().Count == 0)
            {
                if (HappyBirthdayModCore.Configs.modConfig.fallbackToEnglishTranslationWhenPossible)
                {
                    if (this.happyBirthdayContentPackManager.getHappyBirthdayContentPacksForEnglishLanguageCode().Count == 0)
                    {
                        this.drawDialogueBoxWithError("There were zero content packs for Happy Birthday for the English language code en-US. This is a fatal error as the modded cutscenes WILL NOT work without at least one proper content pack installed. Did you mean to install one? One can be found at https://www.nexusmods.com/stardewvalley/mods/11148 for English dialogue. If one is installed, is the language code in TranslationInfo.json correct?");
                        return;
                    }
                }

                this.drawDialogueBoxWithError(string.Format("There were zero content packs for Happy Birthday for the given language code {0}. This is a fatal error as the modded cutscenes WILL NOT work without at least one proper content pack installed. Did you mean to install one?", LocalizedContentManager.CurrentLanguageString));
                return;
            }
            */

            this.giftManager.addInPotentialGiftsFromNPCsFromContentPacks();
            MailUtilities.RemoveAllBirthdayMail();

            BirthdayEventUtilities.InitializeBirthdayEvents();
            this.contentPacksInitalized = true;
        }

        private void drawDialogueBoxWithError(string error)
        {
            Game1.activeClickableMenu = new DialogueBox(error);
            this.Monitor.Log(error, LogLevel.Error);
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
                if (this.birthdayManager.playerBirthdayData == null)
                {

                    this.birthdayManager.playerBirthdayData = new PlayerData();
                    this.birthdayManager.playerBirthdayData.BirthdayDay = 1;
                    this.birthdayManager.playerBirthdayData.BirthdaySeason = "spring";
                    this.birthdayManager.playerBirthdayData.PlayersName = Game1.player.Name;
                    this.birthdayManager.playerBirthdayData.PlayerUniqueMultiplayerId = Game1.player.UniqueMultiplayerID;
                }

                this.birthdayManager.setCheckedForBirthday(true);

                this.birthdayManager.setUpPlayersBirthday();
            }

        }

        public void SayWithMenuChecker(string text, bool interrupt)
        {
            if (this.screenreader != null)
                this.screenreader.SayWithMenuChecker(text, interrupt);
        }


    }
}
