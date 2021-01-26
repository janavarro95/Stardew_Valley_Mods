using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using StardustCore.UIUtilities;
using StardustCore.UIUtilities.MenuComponents;

namespace StardustCore.Menus
{
    // TODO: make buttons to cycle through page
    public class ModularGameMenu : IClickableMenuExtended
    {
        public static SortedDictionary<string, List<KeyValuePair<Button, IClickableMenuExtended>>> StaticMenuTabsAndPages = new SortedDictionary<string, List<KeyValuePair<Button, IClickableMenuExtended>>>();

        public string hoverText = "";
        public string descriptionText = "";
        public Dictionary<Button, IClickableMenuExtended> menuTabsAndPages;

        public Button currentButton;
        public IClickableMenuExtended currentMenu;

        public bool invisible;
        public static bool forcePreventClose;

        public const int tabsPerPage = 12;

        public int currentPageIndex;

        public ModularGameMenu()
            : base(Game1.viewport.Width / 2 - (800 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, 800 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2, true)
        {
            //ModCore.ModMonitor.Log("INITIALIZE MENU: ", LogLevel.Alert);
            if (Game1.activeClickableMenu == null)
                Game1.playSound("bigSelect");
            GameMenu.forcePreventClose = false;

            this.menuTabsAndPages = new Dictionary<Button, IClickableMenuExtended>();
            //ModCore.ModMonitor.Log("INITIALIZE MENU: ", LogLevel.Alert);
            foreach (var v in StaticMenuTabsAndPages)
            {
                //ModCore.ModMonitor.Log("LIST A GO GO", LogLevel.Alert);
                foreach (var pair in v.Value)
                {
                    this.AddMenuTab(pair.Key, pair.Value.clone());
                    //ModCore.ModMonitor.Log("ADD IN A PART", LogLevel.Alert);
                }
                //this.menuTabsAndPages.Add(v.Key,v.Value.clone());
            }
        }

        public static void AddTabsForMod(IManifest modManifest, List<KeyValuePair<Button, IClickableMenuExtended>> menuComponents)
        {
            StaticMenuTabsAndPages.Add(modManifest.UniqueID, menuComponents);
        }

        public void AddMenuTab(Button b, IClickableMenuExtended menu)
        {
            int count = this.menuTabsAndPages.Count % tabsPerPage;
            int xPos = this.xPositionOnScreen + count * 64;
            int yPos = this.yPositionOnScreen;

            b.bounds = new Rectangle(xPos, yPos, b.bounds.Width, b.bounds.Height);

            if (b.extraTextures == null)
            {
                this.menuTabsAndPages.Add(b, menu);
                return;
            }
            for (int i = 0; i < b.extraTextures.Count; i++)
            {
                ClickableTextureComponent text = b.extraTextures.ElementAt(i).Key;
                Rectangle bounds = new Rectangle(b.bounds.X + text.bounds.X, b.bounds.Y + text.bounds.Y, text.bounds.Width, text.bounds.Height);
                text.bounds = bounds;
                b.extraTextures[i] = new KeyValuePair<ClickableTextureComponent, ExtraTextureDrawOrder>(text, b.extraTextures.ElementAt(i).Value);
            }

            this.menuTabsAndPages.Add(b, menu);
        }

        /// <summary>Get how many pages there should be for the modular menu.</summary>
        public int getNumberOfPages()
        {
            int count = (this.menuTabsAndPages.Count / tabsPerPage);
            return count;
        }

        /// <summary>Takes in the static declared tabs and tries to set the menu tabs to that.</summary>
        public ModularGameMenu(int startingTab)
            : base(Game1.viewport.Width / 2 - (800 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, 800 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2, true)
        {
            ModCore.ModMonitor.Log("INITIALIZE MENU: ", LogLevel.Alert);
            if (Game1.activeClickableMenu == null)
                Game1.playSound("bigSelect");
            GameMenu.forcePreventClose = false;

            this.menuTabsAndPages = new Dictionary<Button, IClickableMenuExtended>();
            ModCore.ModMonitor.Log("INITIALIZE MENU: ", LogLevel.Alert);
            foreach (var v in StaticMenuTabsAndPages)
            {
                ModCore.ModMonitor.Log("LIST A GO GO", LogLevel.Alert);
                foreach (var pair in v.Value)
                {
                    this.AddMenuTab(pair.Key, pair.Value.clone());
                    ModCore.ModMonitor.Log("ADD IN A PART", LogLevel.Alert);
                }
                //this.menuTabsAndPages.Add(v.Key,v.Value.clone());
            }
            this.currentPageIndex = startingTab % tabsPerPage;
            this.changeTab(startingTab);
        }

        /// <summary>Takes in the static declared tabs and tries to set the menu tabs to that.</summary>
        public ModularGameMenu(string startingTab)
            : base(Game1.viewport.Width / 2 - (800 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, 800 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2, true)
        {
            ModCore.ModMonitor.Log("INITIALIZE MENU: ", LogLevel.Alert);
            if (Game1.activeClickableMenu == null)
                Game1.playSound("bigSelect");
            GameMenu.forcePreventClose = false;

            this.menuTabsAndPages = new Dictionary<Button, IClickableMenuExtended>();
            ModCore.ModMonitor.Log("INITIALIZE MENU: ", LogLevel.Alert);
            foreach (var v in StaticMenuTabsAndPages)
            {
                ModCore.ModMonitor.Log("LIST A GO GO", LogLevel.Alert);
                foreach (var pair in v.Value)
                {
                    this.AddMenuTab(pair.Key, pair.Value.clone());
                    ModCore.ModMonitor.Log("ADD IN A PART", LogLevel.Alert);
                }
                //this.menuTabsAndPages.Add(v.Key,v.Value.clone());
            }
            this.changeTab(this.getTabIndexFromName(startingTab));
        }

        /// <summary>More modular menu to only have a subset of buttons and pages.</summary>
        public ModularGameMenu(int startingTab, Dictionary<Button, IClickableMenuExtended> tabsAndPages) : base(Game1.viewport.Width / 2 - (800 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, 800 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2, true)
        {
            foreach (var v in tabsAndPages)
            {
                this.AddMenuTab(v.Key, v.Value.clone());
            }
            this.changeTab(startingTab);
        }

        /// <summary>More modular menu to only have a subset of buttons and pages.</summary>
        public ModularGameMenu(string startingTab, Dictionary<Button, IClickableMenuExtended> tabsAndPages)
            : base(Game1.viewport.Width / 2 - (800 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, 800 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2, true)
        {
            foreach (var v in tabsAndPages)
            {
                this.AddMenuTab(v.Key, v.Value.clone());
            }
            this.changeTab(this.getTabIndexFromName(startingTab));
        }


        public int getTabIndexFromName(string tabLabel)
        {
            for (int i = 0; i < this.menuTabsAndPages.Count; i++)
            {
                var ok = this.menuTabsAndPages.ElementAt(i);
                if (ok.Key.label == tabLabel)
                {
                    return i;
                }
            }
            return 0; //If I can't find it just do a default.
        }

        public override void snapToDefaultClickableComponent()
        {
            /*
            if (this.currentTab < this.pages.Count)
                this.pages[this.currentTab].snapToDefaultClickableComponent();
            if (this.junimoNoteIcon == null || this.currentTab >= this.pages.Count || this.pages[this.currentTab].allClickableComponents.Contains((ClickableComponent)this.junimoNoteIcon))
                return;
            this.pages[this.currentTab].allClickableComponents.Add((ClickableComponent)this.junimoNoteIcon);
            */
            this.currentMenu.snapToDefaultClickableComponent();
        }

        public override void receiveGamePadButton(Buttons b)
        {
            base.receiveGamePadButton(b);
            /*
            switch (b)
            {
                case Buttons.Back:
                    if (this.currentTab != 0)
                        break;
                    this.pages[this.currentTab].receiveGamePadButton(b);
                    break;
                case Buttons.RightTrigger:
                    if (this.currentTab == 3)
                    {
                        Game1.activeClickableMenu = (IClickableMenu)new GameMenu(4, -1);
                        break;
                    }
                    if (this.currentTab >= 7 || !this.pages[this.currentTab].readyToClose())
                        break;
                    this.changeTab(this.currentTab + 1);
                    break;
                case Buttons.LeftTrigger:
                    if (this.currentTab == 3)
                    {
                        Game1.activeClickableMenu = (IClickableMenu)new GameMenu(2, -1);
                        break;
                    }
                    if (this.currentTab <= 0 || !this.pages[this.currentTab].readyToClose())
                        break;
                    this.changeTab(this.currentTab - 1);
                    break;
            }
            */
            this.currentMenu.receiveGamePadButton(b);
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            base.receiveLeftClick(x, y, playSound);
            //click the menu and stuff here I guess.
            Dictionary<Button, IClickableMenuExtended> subTabs = new Dictionary<Button, IClickableMenuExtended>();
            for (int i = this.currentPageIndex * tabsPerPage; i < (this.currentPageIndex + 1) * tabsPerPage; i++)
            {
                if (i >= this.menuTabsAndPages.Count)
                    break;

                var pair = this.menuTabsAndPages.ElementAt(i);
                subTabs.Add(pair.Key, pair.Value);
            }

            foreach (var tab in subTabs)
            {
                if (tab.Key.containsPoint(x, y))
                {
                    ModCore.ModMonitor.Log(tab.Key.label);
                    //this.hoverText = tab.Key.label;
                    this.changeTab(tab.Key, tab.Value);

                    return;
                }
            }

            //Check for submenu leftclick
            this.currentMenu.receiveLeftClick(x, y, playSound);
        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
            //base.receiveLeftClick(x, y, playSound);
            //click the menu and stuff here I guess.

            //Check for submenu right
            this.currentMenu.receiveRightClick(x, y);
        }

        public override void receiveScrollWheelAction(int direction)
        {
            base.receiveScrollWheelAction(direction);
            //this.pages[this.currentTab].receiveScrollWheelAction(direction);
            //check for submenu scroll action.
            this.currentMenu.receiveScrollWheelAction(direction);
        }

        public override void performHoverAction(int x, int y)
        {
            base.performHoverAction(x, y);
            this.hoverText = "";
            this.currentMenu.performHoverAction(x, y);
            //this.pages[this.currentTab].performHoverAction(x, y);

            Dictionary<Button, IClickableMenuExtended> subTabs = new Dictionary<Button, IClickableMenuExtended>();
            for (int i = this.currentPageIndex * tabsPerPage; i < (this.currentPageIndex + 1) * tabsPerPage; i++)
            {
                if (i >= this.menuTabsAndPages.Count)
                    break;

                var pair = this.menuTabsAndPages.ElementAt(i);
                subTabs.Add(pair.Key, pair.Value);
            }

            foreach (var tab in subTabs)
            {
                if (tab.Key.containsPoint(x, y))
                {
                    this.hoverText = tab.Key.label;
                    return;
                }
            }
        }

        public override void releaseLeftClick(int x, int y)
        {
            base.releaseLeftClick(x, y);
            //this.pages[this.currentTab].releaseLeftClick(x, y);

            this.currentMenu.releaseLeftClick(x, y);
        }

        public void pageBack()
        {
            if (this.currentPageIndex != 0)
                this.currentPageIndex--;
        }

        public void pageForward()
        {
            if (this.currentPageIndex < this.getNumberOfPages())
                this.currentPageIndex++;
        }

        public override void leftClickHeld(int x, int y)
        {
            base.leftClickHeld(x, y);
            //this.pages[this.currentTab].leftClickHeld(x, y);
            this.currentMenu.leftClickHeld(x, y);
        }

        public override bool readyToClose()
        {
            if (!GameMenu.forcePreventClose)
                return this.currentMenu.readyToClose();
            return false;
        }

        public void changeTab(int which)
        {
            this.currentPageIndex = which % tabsPerPage;
            this.currentButton = this.menuTabsAndPages.ElementAt(which).Key;
            this.currentMenu = this.menuTabsAndPages.ElementAt(which).Value;
        }

        public void changeTab(Button b, IClickableMenuExtended menu)
        {
            this.currentButton = b;
            this.currentMenu = menu;
        }

        public void setTabNeighborsForCurrentPage() { }

        public override void draw(SpriteBatch b)
        {
            if (!this.invisible)
            {
                if (!Game1.options.showMenuBackground)
                    b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.4f);
                //Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, currentMenu.width, currentMenu.height, false, true, (string)null, false);
                //this.pages[this.currentTab].draw(b);
                this.drawDialogueBoxBackground();

                b.End();
                b.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointClamp, (DepthStencilState)null, (RasterizerState)null);
                Dictionary<Button, IClickableMenuExtended> subTabs = new Dictionary<Button, IClickableMenuExtended>();
                for (int i = this.currentPageIndex * tabsPerPage; i < (this.currentPageIndex + 1) * tabsPerPage; i++)
                {
                    if (i >= this.menuTabsAndPages.Count)
                        break;

                    var pair = this.menuTabsAndPages.ElementAt(i);
                    subTabs.Add(pair.Key, pair.Value);
                }

                foreach (var tab in subTabs)
                {
                    if (tab.Key.visible)
                    {
                        if (tab.Key == this.currentButton)
                        {
                            tab.Key.draw(b, Color.White, new Vector2(0, 0));
                            continue;
                        }
                        tab.Key.draw(b);
                    }
                }
                this.currentMenu.draw(b);

                if (Game1.options.hardwareCursor)
                    return;
                b.Draw(Game1.mouseCursors, new Vector2((float)Game1.getOldMouseX(), (float)Game1.getOldMouseY()), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, Game1.options.gamepadControls ? 44 : 0, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float)(4.0 + (double)Game1.dialogueButtonScale / 150.0), SpriteEffects.None, 1f);
            }
        }

        public override bool areGamePadControlsImplemented()
        {
            return false;
        }

        public override void receiveKeyPress(Keys key)
        {
            if (Game1.options.menuButton.Contains(new InputButton(key)) && this.readyToClose())
            {
                Game1.exitActiveMenu();
                Game1.playSound("bigDeSelect");
            }
            //this.pages[this.currentTab].receiveKeyPress(key);
            this.currentMenu.receiveKeyPress(key);
        }

        public override void emergencyShutDown()
        {
            base.emergencyShutDown();
            this.currentMenu.emergencyShutDown();
        }
    }
}
