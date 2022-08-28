using System;
using System.IO;
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
        /// The texture for the calendar button.
        /// </summary>
        private Texture2D calendarTexture;
        /// <summary>
        /// The texture for the quest button.
        /// </summary>
        private Texture2D questTexture;

        /// <summary>
        /// The button for the calendar menu.
        /// </summary>
        public ClickableTextureComponent billboardButton;
        /// <summary>
        /// The button for the quest menu.
        /// </summary>
        public ClickableTextureComponent questButton;

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            this.Config = helper.ReadConfig<ModConfig>();

            helper.ConsoleCommands.Add("Omegasis.BillboardAnywhere.ReloadConfig", "Reloads the config file for BillboardAnywhere to reposition the button for the inventory menu page.", this.reloadConfig);
            helper.ConsoleCommands.Add("Omegasis.BillboardAnywhere.SetcalendarButtonX", "<int>Sets the x position for the calendar button in the game menu.", this.setcalendarButtonX);
            helper.ConsoleCommands.Add("Omegasis.BillboardAnywhere.SetcalendarButtonY", "<int> Sets the y position for the calendar button in the game menu.", this.setcalendarButtonY);
            helper.ConsoleCommands.Add("Omegasis.BillboardAnywhere.SetcalendarButtonPosition", "<int,int> Sets the position for the calendar button in the game menu.", this.setcalendarButtonPosition);
            helper.ConsoleCommands.Add("Omegasis.BillboardAnywhere.SetQuestButtonX", "<int>Sets the x position for the quest button in the game menu.", this.setQuestButtonX);
            helper.ConsoleCommands.Add("Omegasis.BillboardAnywhere.SetQuestButtonY", "<int> Sets the y position for the quest button in the game menu.", this.setQuestButtonX);
            helper.ConsoleCommands.Add("Omegasis.BillboardAnywhere.SetQuestButtonPosition", "<int,int> Sets the position for the quest button in the game menu.", this.setQuestButtonPosition);
            helper.ConsoleCommands.Add("Omegasis.BillboardAnywhere.SetcalendarButtonVisibility", "<bool> Sets the visibility for the billboard button in the game menu.", this.setcalendarButtonVisibility);
            helper.ConsoleCommands.Add("Omegasis.BillboardAnywhere.SetQuestButtonVisibility", "<bool> Sets the visibility for the quest button in the game menu.", this.setQuestButtonVisibility);
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            helper.Events.Display.RenderedActiveMenu += this.RenderBillboardMenuButton;

            this.calendarTexture = helper.GameContent.Load<Texture2D>(Path.Combine("Assets", "Billboard.png"));
            this.questTexture = helper.GameContent.Load<Texture2D>(Path.Combine("Assets", "Quest.png"));
            this.billboardButton = new ClickableTextureComponent(new Rectangle((int)this.Config.CalendarOffsetFromMenu.X, (int)this.Config.CalendarOffsetFromMenu.Y, this.calendarTexture.Width, this.calendarTexture.Height), this.calendarTexture, new Rectangle(0, 0, this.calendarTexture.Width, this.calendarTexture.Height), 1f, false);
            this.questButton = new ClickableTextureComponent(new Rectangle((int)this.Config.QuestOffsetFromMenu.X, (int)this.Config.QuestOffsetFromMenu.Y, this.questTexture.Width, this.questTexture.Height), this.questTexture, new Rectangle(0, 0, this.questTexture.Width, this.questTexture.Height), 1f, false);
        }

        /*********
        ** Private methods
        *********/
        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
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

            // check if billboard icon was clicked
            else if (e.Button == SButton.MouseLeft && this.isInventoryPage())
            {
                Point mouse = Game1.getMousePosition(ui_scale: true);

                if (this.Config.EnableInventoryCalendarButton && this.billboardButton.containsPoint(mouse.X, mouse.Y))
                    Game1.activeClickableMenu = new Billboard(false);

                else if (this.Config.EnableInventoryQuestButton && this.questButton.containsPoint(mouse.X, mouse.Y))
                    Game1.activeClickableMenu = new Billboard(true);
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
                this.billboardButton.bounds = new Rectangle(Game1.activeClickableMenu.xPositionOnScreen + (int)this.Config.CalendarOffsetFromMenu.X, Game1.activeClickableMenu.yPositionOnScreen + (int)this.Config.CalendarOffsetFromMenu.Y, this.calendarTexture.Width, this.calendarTexture.Height);
                this.questButton.bounds = new Rectangle(Game1.activeClickableMenu.xPositionOnScreen + (int)this.Config.QuestOffsetFromMenu.X, Game1.activeClickableMenu.yPositionOnScreen + (int)this.Config.QuestOffsetFromMenu.Y, this.calendarTexture.Width, this.calendarTexture.Height);
                if (this.Config.EnableInventoryQuestButton) this.questButton.draw(Game1.spriteBatch);
                if (this.Config.EnableInventoryCalendarButton) this.billboardButton.draw(Game1.spriteBatch);
                GameMenu activeMenu = (Game1.activeClickableMenu as GameMenu);
                activeMenu.drawMouse(Game1.spriteBatch);

                if (this.billboardButton.containsPoint(Game1.getMousePosition().X, Game1.getMousePosition().Y))
                {
                    //My deepest appologies for not being able to personally translate more text.
                    if (Game1.content.GetCurrentLanguage() == LocalizedContentManager.LanguageCode.en)
                    {
                        if (this.Config.EnableInventoryCalendarButton == false) return;
                        IClickableMenu.drawHoverText(Game1.spriteBatch, "Open Billboard Menu", Game1.smallFont);
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
            }
        }

        /// <summary>
        /// Checks to see if the current active menu is the game menu and the current page is the inventory page.
        /// </summary>
        private bool isInventoryPage()
        {
            return
                Game1.activeClickableMenu is GameMenu gameMenu
                && gameMenu.GetCurrentPage() is InventoryPage;
        }


        /// <summary>
        /// Reloads the mod's config and repositions the menu button as necessary.
        /// </summary>
        private void reloadConfig(string Name, string[] Params)
        {
            this.Config = this.Helper.ReadConfig<ModConfig>();
            this.billboardButton = new ClickableTextureComponent(new Rectangle((int)this.Config.CalendarOffsetFromMenu.X, (int)this.Config.CalendarOffsetFromMenu.Y, this.calendarTexture.Width, this.calendarTexture.Height), this.calendarTexture, new Rectangle(0, 0, this.calendarTexture.Width, this.calendarTexture.Height), 1f, false);
            this.questButton = new ClickableTextureComponent(new Rectangle((int)this.Config.QuestOffsetFromMenu.X, (int)this.Config.QuestOffsetFromMenu.Y, this.questTexture.Width, this.questTexture.Height), this.questTexture, new Rectangle(0, 0, this.questTexture.Width, this.questTexture.Height), 1f, false);
        }

        /// <summary>
        /// Reloads the mod's config and repositions the menu button as necessary.
        /// </summary>
        private void reloadConfig()
        {
            this.Config = this.Helper.ReadConfig<ModConfig>();
            this.billboardButton = new ClickableTextureComponent(new Rectangle((int)this.Config.CalendarOffsetFromMenu.X, (int)this.Config.CalendarOffsetFromMenu.Y, this.calendarTexture.Width, this.calendarTexture.Height), this.calendarTexture, new Rectangle(0, 0, this.calendarTexture.Width, this.calendarTexture.Height), 1f, false);
            this.questButton = new ClickableTextureComponent(new Rectangle((int)this.Config.QuestOffsetFromMenu.X, (int)this.Config.QuestOffsetFromMenu.Y, this.questTexture.Width, this.questTexture.Height), this.questTexture, new Rectangle(0, 0, this.questTexture.Width, this.questTexture.Height), 1f, false);
        }

        /// <summary>
        /// Sets the x position of the menu button.
        /// </summary>
        /// <param name="Name">The name of the command.</param>
        /// <param name="Params">The parameters passed into the command.</param>
        private void setcalendarButtonX(string Name, string[] Params)
        {
            this.Config.CalendarOffsetFromMenu = new Vector2(Convert.ToInt32(Params[0]), this.Config.CalendarOffsetFromMenu.Y);
            this.Helper.WriteConfig<ModConfig>(this.Config);
            this.reloadConfig();
        }

        /// <summary>
        /// Sets the y position of the menu button.
        /// </summary>
        /// <param name="Name">The name of the command.</param>
        /// <param name="Params">The parameters passed into the command.</param>
        private void setcalendarButtonY(string Name, string[] Params)
        {
            this.Config.CalendarOffsetFromMenu = new Vector2(this.Config.CalendarOffsetFromMenu.X, Convert.ToInt32(Params[0]));
            this.Helper.WriteConfig<ModConfig>(this.Config);
            this.reloadConfig();
        }

        /// <summary>
        /// Sets the position of the menu button.
        /// </summary>
        /// <param name="Name">The name of the command.</param>
        /// <param name="Params">The parameters passed into the command.</param>
        private void setcalendarButtonPosition(string Name, string[] Params)
        {
            this.Config.CalendarOffsetFromMenu = new Vector2(Convert.ToInt32(Params[0]), Convert.ToInt32(Params[1]));
            this.Helper.WriteConfig<ModConfig>(this.Config);
            this.reloadConfig();
        }


        /// <summary>
        /// Sets the x position of the quest menu button.
        /// </summary>
        /// <param name="Name">The name of the command.</param>
        /// <param name="Params">The parameters passed into the command.</param>
        private void setQuestButtonX(string Name, string[] Params)
        {
            this.Config.QuestOffsetFromMenu = new Vector2(Convert.ToInt32(Params[0]), this.Config.QuestOffsetFromMenu.Y);
            this.Helper.WriteConfig<ModConfig>(this.Config);
            this.reloadConfig();
        }

        /// <summary>
        /// Sets the y position of the quest menu button.
        /// </summary>
        /// <param name="Name">The name of the command.</param>
        /// <param name="Params">The parameters passed into the command.</param>
        private void setQuestButtonY(string Name, string[] Params)
        {
            this.Config.QuestOffsetFromMenu = new Vector2(this.Config.QuestOffsetFromMenu.X, Convert.ToInt32(Params[0]));
            this.Helper.WriteConfig<ModConfig>(this.Config);
            this.reloadConfig();
        }

        /// <summary>
        /// Sets the position of the quest menu button.
        /// </summary>
        /// <param name="Name">The name of the command.</param>
        /// <param name="Params">The parameters passed into the command.</param>
        private void setQuestButtonPosition(string Name, string[] Params)
        {
            this.Config.QuestOffsetFromMenu = new Vector2(Convert.ToInt32(Params[0]), Convert.ToInt32(Params[1]));
            this.Helper.WriteConfig<ModConfig>(this.Config);
            this.reloadConfig();
        }

        /// <summary>
        /// Sets the visibility and functionality of the billboard menu button.
        /// </summary>
        /// <param name="Name">The name of the command.</param>
        /// <param name="Params">The parameters passed into the command.</param>
        private void setcalendarButtonVisibility(string Name, string[] Params)
        {
            this.Config.EnableInventoryCalendarButton = Convert.ToBoolean(Params[0]);
            this.Helper.WriteConfig<ModConfig>(this.Config);
            this.reloadConfig();
        }

        /// <summary>
        /// Sets the visibility and functionality of the quest menu button.
        /// </summary>
        /// <param name="Name">The name of the command.</param>
        /// <param name="Params">The parameters passed into the command.</param>
        private void setQuestButtonVisibility(string Name, string[] Params)
        {
            this.Config.EnableInventoryQuestButton = Convert.ToBoolean(Params[0]);
            this.Helper.WriteConfig<ModConfig>(this.Config);
            this.reloadConfig();
        }
    }
}
