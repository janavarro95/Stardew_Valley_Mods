using System;
using System.IO;

using GenericModConfigMenu;

using Leclair.Stardew.BetterGameMenu;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Omegasis.BillboardAnywhere.Framework;

using StardewModdingAPI;
using StardewModdingAPI.Events;

using StardewValley;
using StardewValley.Menus;

namespace Omegasis.BillboardAnywhere
{
    /// <summary>The mod entry point.</summary>
    public class BillboardAnywhere : Mod
    {
        /*********
        ** Fields
        *********/
        /// <summary>The mod configuration.</summary>
        private ModConfig Config;

        /// <summary>
        /// The API for Better Game Menu.
        /// </summary>
        private IBetterGameMenuApi BetterGameMenuApi;

        /// <summary>
        /// The texture for the calendar button.
        /// </summary>
        private Texture2D calendarTexture;
        /// <summary>
        /// The texture for the quest button.
        /// </summary>
        private Texture2D questTexture;
        /// <summary>
        /// The texture for the special order button.
        /// </summary>
        private Texture2D specialOrderTexture;
        /// <summary>
        /// The texture for the qi board button.
        /// </summary>
        private Texture2D qiBoardTexture;

        /// <summary>
        /// The button for the calendar menu.
        /// </summary>
        public ClickableTextureComponent calendarButton;
        /// <summary>
        /// The button for the quest menu.
        /// </summary>
        public ClickableTextureComponent questButton;
        /// <summary>
        /// The button for the special order menu.
        /// </summary>
        public ClickableTextureComponent specialOrderButton;
        /// <summary>
        /// The button for the qi board menu.
        /// </summary>
        public ClickableTextureComponent qiBoardButton;

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            this.Config = helper.ReadConfig<ModConfig>();

            helper.ConsoleCommands.Add("Omegasis.BillboardAnywhere.ReloadConfig", "Reloads the config file for BillboardAnywhere to reposition the button for the inventory menu page.", this.reloadConfig);

            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            helper.Events.Display.RenderedActiveMenu += this.RenderBillboardMenuButton;
            helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;

            this.calendarTexture = helper.ModContent.Load<Texture2D>(Path.Combine("Assets", "Billboard.png"));
            this.questTexture = helper.ModContent.Load<Texture2D>(Path.Combine("Assets", "Quest.png"));
            this.specialOrderTexture = helper.ModContent.Load<Texture2D>(Path.Combine("Assets", "SpecialOrdersBoard.png"));
            this.qiBoardTexture = helper.ModContent.Load<Texture2D>(Path.Combine("Assets", "SpecialOrderBoardQi.png"));

            this.reloadConfig();
        }

        /*********
        ** Private methods
        *********/

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            // get Better Game Menu's API (if it's installed) and store it for later use
            this.BetterGameMenuApi = this.Helper.ModRegistry.GetApi<IBetterGameMenuApi>("leclair.bettergamemenu");

            // get Generic Mod Config Menu's API (if it's installed)
            var configMenu = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu is null)
                return;

            // register mod
            configMenu.Register(
                mod: this.ModManifest,
                reset: () => this.Config = new ModConfig(),
                save: () => this.Helper.WriteConfig(this.Config)
            );

            // Quest Board
            configMenu.AddSectionTitle(
                mod: this.ModManifest,
                text: () => "Quest Board");

            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => "Enable Quest Board Button",
                tooltip: () => "Controls visiblity of the button",
                getValue: () => this.Config.EnableInventoryQuestButton,
                setValue: value => this.Config.EnableInventoryQuestButton = value
            );

            configMenu.AddNumberOption(
                mod: this.ModManifest,
                getValue: () => Convert.ToInt32(this.Config.QuestOffsetFromMenu.X),
                setValue: value => this.Config.QuestOffsetFromMenu = new Vector2(value, this.Config.QuestOffsetFromMenu.Y),
                name: () => "X Coordinate",
                tooltip: () => "X coordinate for the Quest Board button");

            configMenu.AddNumberOption(
                mod: this.ModManifest,
                getValue: () => Convert.ToInt32(this.Config.QuestOffsetFromMenu.Y),
                setValue: value => this.Config.QuestOffsetFromMenu = new Vector2(this.Config.QuestOffsetFromMenu.X, value),
                name: () => "Y Coordinate",
                tooltip: () => "Y coordinate for the Quest Board button");

            // Calendar
            configMenu.AddSectionTitle(
                mod: this.ModManifest,
                text: () => "Calendar");

            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => "Enable Calendar Button",
                tooltip: () => "Controls visiblity of the button",
                getValue: () => this.Config.EnableInventoryCalendarButton,
                setValue: value => this.Config.EnableInventoryCalendarButton = value
            );

            configMenu.AddNumberOption(
                mod: this.ModManifest,
                getValue: () => Convert.ToInt32(this.Config.CalendarOffsetFromMenu.X),
                setValue: value => this.Config.CalendarOffsetFromMenu = new Vector2(value, this.Config.CalendarOffsetFromMenu.Y),
                name: () => "Calendar X",
                tooltip: () => "X coordinate for the calendar button");

            configMenu.AddNumberOption(
                mod: this.ModManifest,
                getValue: () => Convert.ToInt32(this.Config.CalendarOffsetFromMenu.Y),
                setValue: value => this.Config.CalendarOffsetFromMenu = new Vector2(this.Config.CalendarOffsetFromMenu.X, value),
                name: () => "Calendar Y",
                tooltip: () => "Y coordinate for the calendar button");


            //Special Orders
            configMenu.AddSectionTitle(
                mod: this.ModManifest,
                text: () => "Special Orders Board");

            configMenu.AddParagraph(
                mod: this.ModManifest,
                text: () => "Note: Special Orders Board Button will not be visible until it has been unlocked");

            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => "Enable Special Orders Button",
                tooltip: () => "Controls visiblity of the button",
                getValue: () => this.Config.EnableSpecialOrdersButton,
                setValue: value => this.Config.EnableSpecialOrdersButton = value
            );

            configMenu.AddNumberOption(
                mod: this.ModManifest,
                getValue: () => Convert.ToInt32(this.Config.SpecialOrderOffsetFromMenu.X),
                setValue: value => this.Config.SpecialOrderOffsetFromMenu = new Vector2(value, this.Config.SpecialOrderOffsetFromMenu.Y),
                name: () => "X Coordinate",
                tooltip: () => "X coordinate for the Special Orders button");

            configMenu.AddNumberOption(
                mod: this.ModManifest,
                getValue: () => Convert.ToInt32(this.Config.SpecialOrderOffsetFromMenu.Y),
                setValue: value => this.Config.SpecialOrderOffsetFromMenu = new Vector2(this.Config.SpecialOrderOffsetFromMenu.X, value),
                name: () => "Y Coordinate",
                tooltip: () => "Y coordinate for the Special Orders button");

            //Qi Board
            configMenu.AddSectionTitle(
                mod: this.ModManifest,
                text: () => "Qi Board Board");

            configMenu.AddParagraph(
                mod: this.ModManifest,
                text: () => "Note: Qi Board Button will not be visible until it has been unlocked");

            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => "Enable Qi Board Button",
                tooltip: () => "Controls visiblity of the button",
                getValue: () => this.Config.EnableQiBoardButton,
                setValue: value => this.Config.EnableQiBoardButton = value
            );

            configMenu.AddNumberOption(
                mod: this.ModManifest,
                getValue: () => Convert.ToInt32(this.Config.QiOffsetFromMenu.X),
                setValue: value => this.Config.QiOffsetFromMenu = new Vector2(value, this.Config.QiOffsetFromMenu.Y),
                name: () => "X Coordinate",
                tooltip: () => "X coordinate for the Qi Board button");

            configMenu.AddNumberOption(
                mod: this.ModManifest,
                getValue: () => Convert.ToInt32(this.Config.QiOffsetFromMenu.Y),
                setValue: value => this.Config.QiOffsetFromMenu = new Vector2(this.Config.QiOffsetFromMenu.X, value),
                name: () => "Y Coordinate",
                tooltip: () => "Y coordinate for the Qi Board button");
        }

        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        /// 
        public void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            // load menu if key pressed
            if (Context.IsPlayerFree && e.Button == this.Config.CalendarKeyBinding)
                Game1.activeClickableMenu = new Billboard();
            else if (Context.IsPlayerFree && e.Button == this.Config.QuestBoardKeyBinding)
            {
                Game1.RefreshQuestOfTheDay();
                Game1.activeClickableMenu = new Billboard(true);
            }
            else if (Context.IsPlayerFree && e.Button == this.Config.SpecialOrderKeyBinding && Game1.player.eventsSeen.Contains("15389722"))
            {
                Game1.activeClickableMenu = new SpecialOrdersBoard();
            }
            else if (Context.IsPlayerFree && e.Button == this.Config.QiBoardKeyBinding && Game1.player.eventsSeen.Contains("10040609"))
            {
                Game1.activeClickableMenu = new SpecialOrdersBoard("Qi");
            }


            // check if billboard icon was clicked
            else if (e.Button == SButton.MouseLeft && this.isInventoryPage())
            {
                Point mouse = Game1.getMousePosition(ui_scale: true);

                if (this.Config.EnableInventoryCalendarButton && this.calendarButton.containsPoint(mouse.X, mouse.Y))
                    Game1.activeClickableMenu = new Billboard(false);

                else if (this.Config.EnableInventoryQuestButton && this.questButton.containsPoint(mouse.X, mouse.Y))
                    Game1.activeClickableMenu = new Billboard(true);

                else if (this.Config.EnableSpecialOrdersButton && this.specialOrderButton.containsPoint(mouse.X, mouse.Y) && Game1.player.eventsSeen.Contains("15389722"))
                    Game1.activeClickableMenu = new SpecialOrdersBoard();

                else if (this.Config.EnableQiBoardButton && this.qiBoardButton.containsPoint(mouse.X, mouse.Y) && Game1.player.eventsSeen.Contains("10040609"))
                    Game1.activeClickableMenu = new SpecialOrdersBoard("Qi");
            }

        }

        /// <summary>
        /// Renders the billboard button to the menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RenderBillboardMenuButton(object sender, RenderedActiveMenuEventArgs e)
        {
            if (this.isInventoryPage())
            {
                this.calendarButton.bounds = new Rectangle(Game1.activeClickableMenu.xPositionOnScreen + (int)this.Config.CalendarOffsetFromMenu.X, Game1.activeClickableMenu.yPositionOnScreen + (int)this.Config.CalendarOffsetFromMenu.Y, this.calendarTexture.Width, this.calendarTexture.Height);
                this.questButton.bounds = new Rectangle(Game1.activeClickableMenu.xPositionOnScreen + (int)this.Config.QuestOffsetFromMenu.X, Game1.activeClickableMenu.yPositionOnScreen + (int)this.Config.QuestOffsetFromMenu.Y, this.calendarTexture.Width, this.calendarTexture.Height);
                this.specialOrderButton.bounds = new Rectangle(Game1.activeClickableMenu.xPositionOnScreen + (int)this.Config.SpecialOrderOffsetFromMenu.X, Game1.activeClickableMenu.yPositionOnScreen + (int)this.Config.SpecialOrderOffsetFromMenu.Y, this.specialOrderTexture.Width, this.specialOrderTexture.Height);
                this.qiBoardButton.bounds = new Rectangle(Game1.activeClickableMenu.xPositionOnScreen + (int)this.Config.QiOffsetFromMenu.X, Game1.activeClickableMenu.yPositionOnScreen + (int)this.Config.QiOffsetFromMenu.Y, this.qiBoardTexture.Width, this.qiBoardTexture.Height);

                if (this.Config.EnableInventoryQuestButton) this.questButton.draw(Game1.spriteBatch);
                if (this.Config.EnableInventoryCalendarButton) this.calendarButton.draw(Game1.spriteBatch);
                if (this.Config.EnableSpecialOrdersButton && Game1.player.eventsSeen.Contains("15389722")) this.specialOrderButton.draw(Game1.spriteBatch);
                if (this.Config.EnableQiBoardButton && Game1.player.eventsSeen.Contains("10040609")) this.qiBoardButton.draw(Game1.spriteBatch);

                Game1.activeClickableMenu.drawMouse(e.SpriteBatch);

                if (this.calendarButton.containsPoint(Game1.getMousePosition().X, Game1.getMousePosition().Y))
                {
                    //My deepest appologies for not being able to personally translate more text.
                    if (Game1.content.GetCurrentLanguage() == LocalizedContentManager.LanguageCode.en)
                    {
                        if (this.Config.EnableInventoryCalendarButton == false) return;
                        IClickableMenu.drawHoverText(Game1.spriteBatch, "Open Calendar Menu", Game1.smallFont);
                    }
                }

                if (this.questButton.containsPoint(Game1.getMousePosition().X, Game1.getMousePosition().Y))
                {
                    //My deepest appologies once again for not being able to personally translate more text.
                    if (Game1.content.GetCurrentLanguage() == LocalizedContentManager.LanguageCode.en)
                    {
                        if (this.Config.EnableInventoryQuestButton == false) return;
                        IClickableMenu.drawHoverText(Game1.spriteBatch, "Open Quest Menu", Game1.smallFont);
                    }
                }

                if (this.specialOrderButton.containsPoint(Game1.getMousePosition().X, Game1.getMousePosition().Y))
                {
                    //Original author's deepest appologies once once again for not being able to personally translate more text.
                    if (Game1.content.GetCurrentLanguage() == LocalizedContentManager.LanguageCode.en)
                    {
                        if (this.Config.EnableSpecialOrdersButton == false) return;
                        if (!StardewValley.SpecialOrders.SpecialOrder.IsSpecialOrdersBoardUnlocked()) return;
                        IClickableMenu.drawHoverText(Game1.spriteBatch, "Open Special Orders Menu", Game1.smallFont);
                    }
                }

                if (this.qiBoardButton.containsPoint(Game1.getMousePosition().X, Game1.getMousePosition().Y))
                {
                    //Original author's deepest appologies once once again for not being able to personally translate more text. AGAIN
                    if (Game1.content.GetCurrentLanguage() == LocalizedContentManager.LanguageCode.en)
                    {
                        if (this.Config.EnableQiBoardButton == false) return;
                        if (!Game1.player.eventsSeen.Contains("10040609")) return;
                        IClickableMenu.drawHoverText(Game1.spriteBatch, "Open Qi Board Menu", Game1.smallFont);
                    }
                }
            }
        }

        /// <summary>
        /// Checks to see if the current active menu is the game menu and the current page is the inventory page.
        /// </summary>
        private bool isInventoryPage()
        {
            if (Game1.activeClickableMenu is GameMenu gameMenu)
                return gameMenu.GetCurrentPage() is InventoryPage;
            return this.BetterGameMenuApi?.ActivePage is InventoryPage;
        }


        /// <summary>
        /// Reloads the mod's config and repositions the menu button as necessary.
        /// </summary>
        private void reloadConfig(string Name, string[] Params)
        {
            this.Config = this.Helper.ReadConfig<ModConfig>();
            this.reloadConfig();
        }

        /// <summary>
        /// Reloads the mod's config and repositions the menu button as necessary.
        /// </summary>
        private void reloadConfig()
        {
            this.Config = this.Helper.ReadConfig<ModConfig>();
            this.calendarButton = new ClickableTextureComponent(new Rectangle((int)this.Config.CalendarOffsetFromMenu.X, (int)this.Config.CalendarOffsetFromMenu.Y, this.calendarTexture.Width, this.calendarTexture.Height), this.calendarTexture, new Rectangle(0, 0, this.calendarTexture.Width, this.calendarTexture.Height), 1f, false);
            this.questButton = new ClickableTextureComponent(new Rectangle((int)this.Config.QuestOffsetFromMenu.X, (int)this.Config.QuestOffsetFromMenu.Y, this.questTexture.Width, this.questTexture.Height), this.questTexture, new Rectangle(0, 0, this.questTexture.Width, this.questTexture.Height), 1f, false);
            this.specialOrderButton = new ClickableTextureComponent(new Rectangle((int)this.Config.SpecialOrderOffsetFromMenu.X, (int)this.Config.SpecialOrderOffsetFromMenu.Y, this.specialOrderTexture.Width, this.specialOrderTexture.Height), this.specialOrderTexture, new Rectangle(0, 0, this.specialOrderTexture.Width, this.specialOrderTexture.Height), 1f, false);
            this.qiBoardButton = new ClickableTextureComponent(new Rectangle((int)this.Config.QiOffsetFromMenu.X, (int)this.Config.QiOffsetFromMenu.Y, this.qiBoardTexture.Width, this.qiBoardTexture.Height), this.qiBoardTexture, new Rectangle(0, 0, this.qiBoardTexture.Width, this.qiBoardTexture.Height), 1f, false);
        }
    }
}
