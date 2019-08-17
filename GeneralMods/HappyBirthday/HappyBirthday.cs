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

namespace Omegasis.HappyBirthday
{
    /// <summary>The mod entry point.</summary>
    public class HappyBirthday : Mod, IAssetEditor
    {
        /*********
        ** Fields
        *********/
        /// <summary>The relative path for the current player's data file.</summary>
        private string DataFilePath;

        /// <summary>The absolute path for the current player's legacy data file.</summary>
        private string LegacyDataFilePath => Path.Combine(this.Helper.DirectoryPath, "Player_Birthdays", $"HappyBirthday_{Game1.player.Name}.txt");

        /// <summary>The mod configuration.</summary>
        public static ModConfig Config;

        /// <summary>The data for the current player.</summary>
        public static PlayerData PlayerBirthdayData;

        /// <summary>Wrapper for static field PlayerBirthdayData;</summary>
        public PlayerData PlayerData
        {
            get => PlayerBirthdayData;
            set => PlayerBirthdayData = value;
        }

        /// <summary>Whether the player has chosen a birthday.</summary>
        private bool HasChosenBirthday => !string.IsNullOrEmpty(this.PlayerData.BirthdaySeason) && this.PlayerData.BirthdayDay != 0;

        /// <summary>The queue of villagers who haven't given a gift yet.</summary>
        private List<string> VillagerQueue;

        /// <summary>Whether we've already checked for and (if applicable) set up the player's birthday today.</summary>
        private bool CheckedForBirthday;
        //private Dictionary<string, Dialogue> Dialogue;
        //private bool SeenEvent;

        public static IModHelper ModHelper;

        public static IMonitor ModMonitor;

        /// <summary>Class to handle all birthday messages for this mod.</summary>
        public BirthdayMessages messages;

        /// <summary>Class to handle all birthday gifts for this mod.</summary>
        public GiftManager giftManager;

        /// <summary>Checks if the current billboard is the daily quest screen or not.</summary>
        bool isDailyQuestBoard;

        Dictionary<long, PlayerData> othersBirthdays;

        public static HappyBirthday Instance;

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {

            Instance = this;

            //helper.Content.AssetLoaders.Add(new PossibleGifts());
            Config = helper.ReadConfig<ModConfig>();

            helper.Events.GameLoop.DayStarted += this.OnDayStarted;
            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
            helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
            helper.Events.GameLoop.Saving += this.OnSaving;
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            helper.Events.Display.MenuChanged += this.OnMenuChanged;

            helper.Events.Display.RenderedActiveMenu += this.OnRenderedActiveMenu;
            helper.Events.Display.RenderedHud += this.OnRenderedHud;
            //MultiplayerSupport.initializeMultiplayerSupport();
            ModHelper = this.Helper;
            ModMonitor = this.Monitor;

            this.messages = new BirthdayMessages();
            this.giftManager = new GiftManager();
            this.isDailyQuestBoard = false;

            ModHelper.Events.Multiplayer.ModMessageReceived += this.Multiplayer_ModMessageReceived;

            ModHelper.Events.Multiplayer.PeerDisconnected += this.Multiplayer_PeerDisconnected;

            this.othersBirthdays = new Dictionary<long, PlayerData>();

            
        }

        /// <summary>Get whether this instance can edit the given asset.</summary>
        /// <param name="asset">Basic metadata about the asset being loaded.</param>
        public bool CanEdit<T>(IAssetInfo asset)
        {
            return asset.AssetNameEquals(@"Data\mail");
        }

        /// <summary>Edit a matched asset.</summary>
        /// <param name="asset">A helper which encapsulates metadata about an asset and enables changes to it.</param>
        public void Edit<T>(IAssetData asset)
        {
            IDictionary<string, string> data = asset.AsDictionary<string, string>().Data;

            string momMail = BirthdayMessages.GetTranslatedString("Mail:birthdayMom");
            string dadMail = BirthdayMessages.GetTranslatedString("Mail:birthdayDad");

            data["birthdayMom"] = momMail;
            data["birthdayDad"] = dadMail;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Used to check for player disconnections.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void Multiplayer_PeerDisconnected(object sender, PeerDisconnectedEventArgs e)
        {
            this.othersBirthdays.Remove(e.Peer.PlayerID);
        }

        private void Multiplayer_ModMessageReceived(object sender, ModMessageReceivedEventArgs e)
        {
            if (e.FromModID == ModHelper.Multiplayer.ModID && e.Type == MultiplayerSupport.FSTRING_SendBirthdayMessageToOthers)
            {
                string message = e.ReadAs<string>();
                Game1.hudMessages.Add(new HUDMessage(message, 1));
            }

            if (e.FromModID == ModHelper.Multiplayer.ModID && e.Type == MultiplayerSupport.FSTRING_SendBirthdayInfoToOthers)
            {
                KeyValuePair<long, PlayerData> message = e.ReadAs<KeyValuePair<long, PlayerData>>();
                if (!this.othersBirthdays.ContainsKey(message.Key))
                {
                    this.othersBirthdays.Add(message.Key, message.Value);
                    MultiplayerSupport.SendBirthdayInfoToConnectingPlayer(e.FromPlayerID);
                    this.Monitor.Log("Got other player's birthday data from: " + Game1.getFarmer(e.FromPlayerID).name);
                }
                else
                {
                    //Brute force update birthday info if it has already been recevived but dont send birthday info again.
                    this.othersBirthdays.Remove(message.Key);
                    this.othersBirthdays.Add(message.Key, message.Value);
                    this.Monitor.Log("Got other player's birthday data from: " + Game1.getFarmer(e.FromPlayerID).name);
                }
            }
        }

        /// <summary>Raised after drawing the HUD (item toolbar, clock, etc) to the sprite batch, but before it's rendered to the screen.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnRenderedHud(object sender, RenderedHudEventArgs e)
        {
            if (Game1.activeClickableMenu == null || this.PlayerData?.BirthdaySeason?.ToLower() != Game1.currentSeason.ToLower())
                return;

            if (Game1.activeClickableMenu is Billboard billboard)
            {
                if (this.isDailyQuestBoard || billboard.calendarDays == null)
                    return;

                //Game1.player.FarmerRenderer.drawMiniPortrat(Game1.spriteBatch, new Vector2(Game1.activeClickableMenu.xPositionOnScreen + 152 + (index - 1) % 7 * 32 * 4, Game1.activeClickableMenu.yPositionOnScreen + 230 + (index - 1) / 7 * 32 * 4), 1f, 4f, 2, Game1.player);

                string hoverText = "";
                List<string> texts = new List<string>();

                foreach (var clicky in billboard.calendarDays)
                {
                    if (clicky.containsPoint(Game1.getMouseX(), Game1.getMouseY()))
                    {
                        if (!string.IsNullOrEmpty(clicky.hoverText))
                            texts.Add(clicky.hoverText); //catches npc birhday names.
                        else if (!string.IsNullOrEmpty(clicky.name))
                            texts.Add(clicky.name); //catches festival dates.
                    }
                }

                for (int i = 0; i < texts.Count; i++)
                {
                    hoverText += texts[i]; //Append text.
                    if (i != texts.Count - 1)
                        hoverText += Environment.NewLine; //Append new line.
                }

                if (!string.IsNullOrEmpty(hoverText))
                {
                    var oldText = this.Helper.Reflection.GetField<string>(Game1.activeClickableMenu, "hoverText");
                    oldText.SetValue(hoverText);
                }
            }
        }

        /// <summary>When a menu is open (<see cref="Game1.activeClickableMenu"/> isn't null), raised after that menu is drawn to the sprite batch but before it's rendered to the screen.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnRenderedActiveMenu(object sender, RenderedActiveMenuEventArgs e)
        {
            if (Game1.activeClickableMenu == null || this.isDailyQuestBoard)
                return;

            //Don't do anything if birthday has not been chosen yet.
            if (this.PlayerData == null)
                return;

            if (Game1.activeClickableMenu is Billboard)
            {
                if (!string.IsNullOrEmpty(this.PlayerData.BirthdaySeason))
                {
                    if (this.PlayerData.BirthdaySeason.ToLower() == Game1.currentSeason.ToLower())
                    {
                        int index = this.PlayerData.BirthdayDay;
                        Game1.player.FarmerRenderer.drawMiniPortrat(Game1.spriteBatch, new Vector2(Game1.activeClickableMenu.xPositionOnScreen + 152 + (index - 1) % 7 * 32 * 4, Game1.activeClickableMenu.yPositionOnScreen + 230 + (index - 1) / 7 * 32 * 4), 0.5f, 4f, 2, Game1.player);
                    }
                }

                foreach (var pair in this.othersBirthdays)
                {
                    int index = pair.Value.BirthdayDay;
                    if (pair.Value.BirthdaySeason != Game1.currentSeason.ToLower()) continue; //Hide out of season birthdays.
                    index = pair.Value.BirthdayDay;
                    Game1.player.FarmerRenderer.drawMiniPortrat(Game1.spriteBatch, new Vector2(Game1.activeClickableMenu.xPositionOnScreen + 152 + (index - 1) % 7 * 32 * 4, Game1.activeClickableMenu.yPositionOnScreen + 230 + (index - 1) / 7 * 32 * 4), 0.5f, 4f, 2, Game1.getFarmer(pair.Key));
                }

                Game1.activeClickableMenu.drawMouse(e.SpriteBatch);
            }
        }

        /// <summary>Raised after a game menu is opened, closed, or replaced.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnMenuChanged(object sender, MenuChangedEventArgs e)
        {
            switch (e.NewMenu)
            {
                case null:
                    this.isDailyQuestBoard = false;
                    return;

                case Billboard billboard:
                    {
                        this.isDailyQuestBoard = ModHelper.Reflection.GetField<bool>((Game1.activeClickableMenu as Billboard), "dailyQuestBoard", true).GetValue();
                        if (this.isDailyQuestBoard)
                            return;

                        Texture2D text = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);
                        Color[] col = new Color[1];
                        col[0] = new Color(0, 0, 0, 1);
                        text.SetData<Color>(col);
                        //players birthday position rect=new ....

                        string bdayDisplay = Game1.content.LoadString("Strings\\UI:Billboard_Birthday");

                        if (!string.IsNullOrEmpty(this.PlayerData.BirthdaySeason))
                        {
                            if (this.PlayerData.BirthdaySeason.ToLower() == Game1.currentSeason.ToLower())
                            {
                                int index = this.PlayerData.BirthdayDay;
                                Rectangle birthdayRect = new Rectangle(Game1.activeClickableMenu.xPositionOnScreen + 152 + (index - 1) % 7 * 32 * 4, Game1.activeClickableMenu.yPositionOnScreen + 200 + (index - 1) / 7 * 32 * 4, 124, 124);
                                // billboard.calendarDays.Add(new ClickableTextureComponent("", birthdayRect, "", $"{Game1.player.Name}'s Birthday", text, new Rectangle(0, 0, 124, 124), 1f, false));
                                billboard.calendarDays.Add(new ClickableTextureComponent("", birthdayRect, "", string.Format(bdayDisplay, Game1.player.Name), text, new Rectangle(0, 0, 124, 124), 1f, false));

                            }
                        }

                        foreach (var pair in this.othersBirthdays)
                        {
                            if (pair.Value.BirthdaySeason != Game1.currentSeason.ToLower()) continue;
                            int index = pair.Value.BirthdayDay;
                            Rectangle otherBirthdayRect = new Rectangle(Game1.activeClickableMenu.xPositionOnScreen + 152 + (index - 1) % 7 * 32 * 4, Game1.activeClickableMenu.yPositionOnScreen + 200 + (index - 1) / 7 * 32 * 4, 124, 124);
                            // billboard.calendarDays.Add(new ClickableTextureComponent("", otherBirthdayRect, "", $"{Game1.getFarmer(pair.Key).Name}'s Birthday", text, new Rectangle(0, 0, 124, 124), 1f, false));
                            billboard.calendarDays.Add(new ClickableTextureComponent("", otherBirthdayRect, "", string.Format(bdayDisplay, Game1.getFarmer(pair.Key).Name), text, new Rectangle(0, 0, 124, 124), 1f, false));
                        }

                        break;
                    }
            }
        }

        /// <summary>Raised after the game begins a new day (including when the player loads a save).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            this.CheckedForBirthday = false;
        }

        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            // show birthday selection menu
            if (Game1.activeClickableMenu != null) return;
            if (Context.IsPlayerFree && !this.HasChosenBirthday && e.Button == Config.KeyBinding)
                Game1.activeClickableMenu = new BirthdayMenu(this.PlayerData.BirthdaySeason, this.PlayerData.BirthdayDay, this.SetBirthday);
        }

        /// <summary>Raised after the player loads a save slot and the world is initialised.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            this.DataFilePath = Path.Combine("data", $"{Game1.player.Name}_{Game1.player.UniqueMultiplayerID}.json");

            // reset state
            this.VillagerQueue = new List<string>();
            this.CheckedForBirthday = false;

            // load settings
            this.MigrateLegacyData();
            this.PlayerData = this.Helper.Data.ReadJsonFile<PlayerData>(this.DataFilePath) ?? new PlayerData();

            ;

            if (PlayerBirthdayData != null)
            {
                ModMonitor.Log("Send all birthday information from " + Game1.player.name);
                MultiplayerSupport.SendBirthdayInfoToOtherPlayers();
            }
            //this.SeenEvent = false;
            //this.Dialogue = new Dictionary<string, Dialogue>();
        }

        /// <summary>Raised before the game begins writes data to the save file (except the initial save creation).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnSaving(object sender, SavingEventArgs e)
        {
            if (this.HasChosenBirthday)
                this.Helper.Data.WriteJsonFile(this.DataFilePath, this.PlayerData);
        }

        /// <summary>Raised after the game state is updated (≈60 times per second).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady || Game1.eventUp || Game1.isFestival())
                return;
            if (!this.HasChosenBirthday && Game1.activeClickableMenu == null && Game1.player.Name.ToLower() != "unnamed farmhand")
            {
                Game1.activeClickableMenu = new BirthdayMenu(this.PlayerData.BirthdaySeason, this.PlayerData.BirthdayDay, this.SetBirthday);
                this.CheckedForBirthday = false;
            }

            if (!this.CheckedForBirthday && Game1.activeClickableMenu == null)
            {
                this.CheckedForBirthday = true;

                // set up birthday
                if (this.IsBirthday())
                {
                    string starMessage = BirthdayMessages.GetTranslatedString("Happy Birthday: Star Message");

                    Messages.ShowStarMessage(starMessage);
                    MultiplayerSupport.SendBirthdayMessageToOtherPlayers();




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

                            //Add in birthday dialogues for npc.
                            try
                            {
                                if (Game1.player.getFriendshipHeartLevelForNPC(npc.Name) >= Config.minimumFriendshipLevelForBirthdayWish)
                                {
                                    bool spouseMessage = false; //Used to determine if there is a valid spouse message for the player. If false load in the generic birthday wish.
                                    //Check if npc name is spouse's name. If no spouse then add in generic dialogue.
                                    if (this.messages.spouseBirthdayWishes.ContainsKey(npc.Name) && Game1.player.isMarried())
                                    {
                                        //this.Monitor.Log("Spouse Checks out");
                                        //Check to see if spouse message exists.
                                        if (!string.IsNullOrEmpty(this.messages.spouseBirthdayWishes[npc.Name]))
                                        {
                                            spouseMessage = true;
                                            Dialogue d = new Dialogue(this.messages.spouseBirthdayWishes[npc.Name], npc);
                                            npc.CurrentDialogue.Push(d);
                                            if (npc.CurrentDialogue.ElementAt(0) != d) npc.setNewDialogue(this.messages.spouseBirthdayWishes[npc.Name]);
                                        }
                                        else
                                            this.Monitor.Log("No spouse message???", LogLevel.Warn);
                                    }
                                    if (!spouseMessage)
                                    {
                                        //Load in 
                                        Dialogue d = new Dialogue(this.messages.birthdayWishes[npc.Name], npc);
                                        npc.CurrentDialogue.Push(d);
                                        if (npc.CurrentDialogue.ElementAt(0) != d) npc.setNewDialogue(this.messages.birthdayWishes[npc.Name]);
                                    }
                                }
                            }
                            catch
                            {
                                if (Game1.player.getFriendshipHeartLevelForNPC(npc.Name) >= Config.minimumFriendshipLevelForBirthdayWish)
                                {
                                    Dialogue d = new Dialogue("Happy Birthday @!", npc);
                                    npc.CurrentDialogue.Push(d);
                                    if (npc.CurrentDialogue.ElementAt(0) != d)
                                        npc.setNewDialogue("Happy Birthday @!");
                                }
                            }
                        }
                    }
                }

                //Don't constantly set the birthday menu.
                if (Game1.activeClickableMenu?.GetType() == typeof(BirthdayMenu))
                    return;

                // ask for birthday date
                if (!this.HasChosenBirthday && Game1.activeClickableMenu == null)
                {
                    Game1.activeClickableMenu = new BirthdayMenu(this.PlayerData.BirthdaySeason, this.PlayerData.BirthdayDay, this.SetBirthday);
                    this.CheckedForBirthday = false;
                }
            }

            // Set birthday gift for the player to recieve from the npc they are currently talking with.
            if (Game1.currentSpeaker != null)
            {
                string name = Game1.currentSpeaker.Name;
                if (Game1.player.getFriendshipHeartLevelForNPC(name) <= Config.minNeutralFriendshipGiftLevel) return;
                if (this.IsBirthday() && this.VillagerQueue.Contains(name))
                {
                    try
                    {
                        this.giftManager.SetNextBirthdayGift(Game1.currentSpeaker.Name);
                        this.VillagerQueue.Remove(Game1.currentSpeaker.Name);
                    }
                    catch (Exception ex)
                    {
                        this.Monitor.Log(ex.ToString(), LogLevel.Error);
                    }
                }

                //Validate the gift and give it to the player.
                if (this.giftManager.BirthdayGiftToReceive != null)
                {
                    while (this.giftManager.BirthdayGiftToReceive.Name == "Error Item" || this.giftManager.BirthdayGiftToReceive.Name == "Rock" || this.giftManager.BirthdayGiftToReceive.Name == "???")
                        this.giftManager.SetNextBirthdayGift(Game1.currentSpeaker.Name);
                    Game1.player.addItemByMenuIfNecessaryElseHoldUp(this.giftManager.BirthdayGiftToReceive);
                    this.giftManager.BirthdayGiftToReceive = null;
                }
            }
        }

        /// <summary>Set the player's birthday/</summary>
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
                {
                    if (this.PlayerData == null)
                        this.PlayerData = new PlayerData();
                }
            }
            catch
            {
                // migrate to new file
                try
                {
                    string[] text = File.ReadAllLines(this.LegacyDataFilePath);
                    this.Helper.Data.WriteJsonFile(this.DataFilePath, new PlayerData
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
