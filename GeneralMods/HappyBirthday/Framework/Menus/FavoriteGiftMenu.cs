using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Omegasis.HappyBirthday.Framework.Gifts;
using Omegasis.HappyBirthday.Framework.Utilities;
using Omegasis.StardustCore.UIUtilities.MenuComponents.ComponentsV1;
using StardewValley;
using StardewValley.GameData.Objects;
using StardewValley.Menus;

namespace Omegasis.HappyBirthday.Framework.Menus
{
    public class FavoriteGiftMenu : IClickableMenu
    {


        /*********
       ** Fields
       *********/
        /// <summary>The labels to draw.</summary>
        private readonly List<ClickableComponent> Labels = new List<ClickableComponent>();
        private List<ClickableTextureComponent> itemButtons = new List<ClickableTextureComponent>();

        /// <summary>The OK button to draw.</summary>
        private ClickableTextureComponent OkButton;

        /// <summary>
        /// The item that is currently being hover overed.
        /// </summary>
        public ClickableTextureComponent currentHoverTextureItem;

        public bool allFinished;

        private int currentPageNumber;

        private HashSet<string> potentialFavoriteGiftIds = new HashSet<string>();
        private List<ClickableTextureComponent> potentialFavoriteGifts = new List<ClickableTextureComponent>();

        private ClickableTextureComponent _leftButton;
        private ClickableTextureComponent _rightButton;

        private int _maxRowsToDisplay = 5;
        private int _maxColumnsToDisplay = 9;

        /// <summary>
        /// The search box used for looking for specific items.
        /// </summary>
        public GiftSearchTextBox searchBox;

        public static int menuWidth = 632 + borderWidth * 2;
        public static int menuHeight = 600 + borderWidth * 2 + Game1.tileSize;

        public HashSet<string> validItems = new HashSet<string>();

        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="season">The initial birthday season.</param>
        /// <param name="day">The initial birthday day.</param>
        /// <param name="onChanged">The callback to invoke when the birthday value changes.</param>
        public FavoriteGiftMenu()
            : base((int)getAppropriateMenuPosition().X, (int)getAppropriateMenuPosition().Y, menuWidth, menuHeight)
        {
            this.searchBox = new GiftSearchTextBox(null, null, Game1.dialogueFont, Game1.textColor);
            Game1.keyboardDispatcher.Subscriber = this.searchBox;
            this.searchBox.Selected = false;
            this.searchBox.onTextReceived += this.SearchBox_onTextReceived;
            this.searchBox.OnBackspacePressed += this.SearchBox_OnBackspacePressed;

            this.validItems = this.getListOfValidItemIds();

            this.setUpPositions();
        }

        public static Vector2 getAppropriateMenuPosition()
        {
            Vector2 defaultPosition = new Vector2(Game1.uiViewport.Width / 2 - menuWidth / 2, (Game1.uiViewport.Height / 2 - menuHeight / 2));

            //Force the viewport into a position that it should fit into on the screen???
            if (defaultPosition.X + menuWidth > Game1.uiViewport.Width)
            {
                defaultPosition.X = 0;
            }

            if (defaultPosition.Y + menuHeight > Game1.uiViewport.Height)
            {
                defaultPosition.Y = 0;
            }
            return defaultPosition;

        }

        private void SearchBox_OnBackspacePressed(TextBox sender)
        {
            Game1.playSound("Cowboy_gunshot");
            this.searchBox.backSpacePressed();
            this.validItems.Clear();
            this.validItems = this.getListOfValidItemIds();
            this.populateGiftsToDisplay();
            this.currentPageNumber = 0;
        }

        private void SearchBox_onTextReceived(object sender, string e)
        {
            Game1.playSound("Cowboy_gunshot");
            this.validItems.Clear();
            this.validItems = this.getListOfValidItemIds();
            this.populateGiftsToDisplay();
            this.currentPageNumber = 0;
        }

        /// <summary>The method called when the game window changes size.</summary>
        /// <param name="oldBounds">The former viewport.</param>
        /// <param name="newBounds">The new viewport.</param>
        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            base.gameWindowSizeChanged(oldBounds, newBounds);
            this.setUpPositions();
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Regenerate the UI.</summary>
        private void setUpPositions()
        {
            this.Labels.Clear();
            this.OkButton = new ClickableTextureComponent("OK", new Rectangle(this.xPositionOnScreen + this.width - borderWidth - spaceToClearSideBorder - Game1.tileSize, this.yPositionOnScreen + this.height - borderWidth - spaceToClearTopBorder + Game1.tileSize / 4, Game1.tileSize, Game1.tileSize), "", null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46), 1f);
            this._leftButton = new ClickableTextureComponent("LeftButton", new Rectangle(this.xPositionOnScreen + this.width - borderWidth - spaceToClearSideBorder - Game1.tileSize, this.yPositionOnScreen + this.height - borderWidth - spaceToClearTopBorder + Game1.tileSize / 4 + 96, Game1.tileSize, Game1.tileSize), "", null, HappyBirthdayModCore.Instance.Helper.ModContent.Load<Texture2D>(Path.Combine("ModAssets", "Graphics", "lastPageButton.png")), new Rectangle(0, 0, 32, 32), 2f);
            this._rightButton = new ClickableTextureComponent("RightButton", new Rectangle(this.xPositionOnScreen + this.width - borderWidth - spaceToClearSideBorder - Game1.tileSize + 96, this.yPositionOnScreen + this.height - borderWidth - spaceToClearTopBorder + Game1.tileSize / 4 + 96, Game1.tileSize, Game1.tileSize), "", null, HappyBirthdayModCore.Instance.Helper.ModContent.Load<Texture2D>(Path.Combine("ModAssets", "Graphics", "nextPageButton.png")), new Rectangle(0, 0, 32, 32), 2f);

            string title = Messages.GetMessage("FavoriteGift");
            this.Labels.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + 128, this.yPositionOnScreen + 128, 1, 1), title));


            this.searchBox.X = this.xPositionOnScreen + 64;
            this.searchBox.Y = this.yPositionOnScreen;
            this.searchBox.Width = 256;
            this.searchBox.Height = 192;

            this.populateGiftsToDisplay();

        }

        /// <summary>Handle a button click.</summary>
        /// <param name="name">The button name that was clicked.</param>
        private void handleButtonClick(string name)
        {
            if (name == null)
                return;

            switch (name)
            {
                // OK button
                case "OK":
                    HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData.potentialFavoriteGifts = this.potentialFavoriteGiftIds;
                    MultiplayerUtilities.SendBirthdayInfoToOtherPlayers();
                    this.allFinished = true;
                    if (Game1.CurrentEvent != null)
                    {
                        Game1.CurrentEvent.CurrentCommand++;
                    }
                    Game1.exitActiveMenu();
                    break;

                case "LeftButton":
                    if (this.currentPageNumber == 0) break;
                    else
                    {
                        this.currentPageNumber--;
                        this.setUpPositions();
                    }
                    break;

                case "RightButton":
                    HashSet<string> ids = this.validItems;
                    int value = (this.currentPageNumber + 1) * this._maxRowsToDisplay * this._maxColumnsToDisplay;
                    if (value >= ids.Count) break;
                    else
                    {
                        this.currentPageNumber++;
                        this.setUpPositions();
                    }

                    break;

                default:
                    break;
            }
            Game1.playSound("coin");
        }

        /// <summary>
        /// Adds a favorite gidt to be selected for a potential birthday gift for the player.
        /// </summary>
        /// <param name="i"></param>
        public virtual void addFavoriteGift(Item i)
        {
            if (i == null)
            {
                return;
            }
            if (this.potentialFavoriteGiftIds.Contains(i.QualifiedItemId)) {
                return;
            }
            this.potentialFavoriteGiftIds.Add(i.QualifiedItemId);
            HappyBirthdayModCore.Instance.Monitor.Log(string.Format("Added {0} as a potential favorited gift.", i.QualifiedItemId));
            Rectangle textureBounds = GameLocation.getSourceRectForObject(i.ParentSheetIndex);
            float itemScale = 4f;
            Rectangle placementBounds = new Rectangle((int)(this.xPositionOnScreen + 64 + 16 * itemScale + Game1.tinyFont.MeasureString(Messages.GetMessage("FavoriteGift")).X) + (this.potentialFavoriteGifts.Count * 64), (int)(this.yPositionOnScreen + 64 + 16 * itemScale), 64, 64);
            ClickableTextureComponent favoriteGiftButton = new ClickableTextureComponent(i.DisplayName, placementBounds, "", i.getHoverBoxText(i), Game1.objectSpriteSheet, textureBounds, 4f, true);
            favoriteGiftButton.item = i;
            this.potentialFavoriteGifts.Add(favoriteGiftButton);
            this.potentialFavoriteGiftIds.Add(i.QualifiedItemId);
        }

        public virtual void removeFavoriteGift(ClickableTextureComponent favoriteGift)
        {
            if (favoriteGift == null)
            {
                return;
            }
            if (favoriteGift.item == null)
            {
                return;
            }

            this.potentialFavoriteGifts.Remove(favoriteGift);
            this.potentialFavoriteGiftIds.Remove(favoriteGift.item.QualifiedItemId);
            for (int i = 0; i < this.potentialFavoriteGifts.Count; i++)
            {

                ClickableTextureComponent favoriteGiftButton = this.potentialFavoriteGifts[i];
                float itemScale = 4f;
                Rectangle placementBounds = new Rectangle((int)(this.xPositionOnScreen + 64 + 16 * itemScale + Game1.tinyFont.MeasureString(Messages.GetMessage("FavoriteGift")).X) + (i * 64), (int)(this.yPositionOnScreen + 64 + 16 * itemScale), 64, 64);
                favoriteGiftButton.setPosition(placementBounds.X, placementBounds.Y);

            }
        }

        /// <summary>The method invoked when the player left-clicks on the menu.</summary>
        /// <param name="x">The X-position of the cursor.</param>
        /// <param name="y">The Y-position of the cursor.</param>
        /// <param name="playSound">Whether to enable sound.</param>
        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {

            foreach (ClickableTextureComponent button in this.itemButtons)
            {
                if (button.containsPoint(x, y))
                {
                    this.handleButtonClick(button.name);
                    button.scale -= 0.5f;
                    button.scale = Math.Max(3.5f, button.scale);
                    this.addFavoriteGift(button.item);
                    break;
                }
            }
            ClickableTextureComponent possiblyClickedFavoriteGiftButton = null;
            foreach (ClickableTextureComponent favoriteGiftButton in this.potentialFavoriteGifts)
            {
                if (favoriteGiftButton.containsPoint(x, y))
                {
                    possiblyClickedFavoriteGiftButton = favoriteGiftButton;
                    break;
                }

            }
            if (possiblyClickedFavoriteGiftButton != null)
            {
                this.removeFavoriteGift(possiblyClickedFavoriteGiftButton);
                this.handleButtonClick(possiblyClickedFavoriteGiftButton.name);
                return;
            }



            if (this.OkButton.containsPoint(x, y))
            {
                this.handleButtonClick(this.OkButton.name);
                this.OkButton.scale -= 0.25f;
                this.OkButton.scale = Math.Max(0.75f, this.OkButton.scale);
            }
            if (this._leftButton.containsPoint(x, y))
                this.handleButtonClick(this._leftButton.name);
            if (this._rightButton.containsPoint(x, y))
                this.handleButtonClick(this._rightButton.name);

            Rectangle r = new Rectangle(this.searchBox.X, this.searchBox.Y, this.searchBox.Width, this.searchBox.Height / 2);
            if (r.Contains(x, y))
            {
                this.searchBox.Update();
                this.searchBox.SelectMe();
            }
            else
                this.searchBox.Selected = false;
        }

        /// <summary>The method invoked when the player right-clicks on the lookup UI.</summary>
        /// <param name="x">The X-position of the cursor.</param>
        /// <param name="y">The Y-position of the cursor.</param>
        /// <param name="playSound">Whether to enable sound.</param>
        public override void receiveRightClick(int x, int y, bool playSound = true) { }

        /// <summary>The method invoked when the player hovers the cursor over the menu.</summary>
        /// <param name="x">The X-position of the cursor.</param>
        /// <param name="y">The Y-position of the cursor.</param>
        public override void performHoverAction(int x, int y)
        {
            bool hoverOverItemButtonThisFrame = false;
            foreach (ClickableTextureComponent button in this.itemButtons)
                if (button.containsPoint(x, y))
                {
                    button.scale = button.containsPoint(x, y)
                        ? Math.Min(button.scale + 0.02f, button.baseScale + 0.1f)
                        : Math.Max(button.scale - 0.02f, button.baseScale);
                    this.currentHoverTextureItem = button;
                    hoverOverItemButtonThisFrame = true;
                }

            foreach (ClickableTextureComponent favoriteGiftButton in this.potentialFavoriteGifts)
            {
                if (favoriteGiftButton != null && favoriteGiftButton.containsPoint(x, y))
                {
                    this.currentHoverTextureItem = favoriteGiftButton;
                    hoverOverItemButtonThisFrame = true;
                }
            }

            if (hoverOverItemButtonThisFrame == false)
                this.currentHoverTextureItem = null;

            this.OkButton.scale = this.OkButton.containsPoint(x, y)
                ? Math.Min(this.OkButton.scale + 0.02f, this.OkButton.baseScale + 0.1f)
                : Math.Max(this.OkButton.scale - 0.02f, this.OkButton.baseScale);



        }


        public override bool readyToClose()
        {
            return this.allFinished;
        }


        /// <summary>
        /// Populates all of the potential gifts to display for the mod.
        /// </summary>
        /// <returns></returns>
        private void populateGiftsToDisplay()
        {
            this.itemButtons.Clear();
            HashSet<string> validItems = this.getListOfValidItemIds();

            for (int row = 0; row < this._maxRowsToDisplay; row++)
                for (int column = 0; column < this._maxColumnsToDisplay; column++)
                {
                    int value = this.currentPageNumber * this._maxRowsToDisplay * this._maxColumnsToDisplay + row * this._maxColumnsToDisplay + column;
                    if (value >= validItems.Count) continue;

                    string id = validItems.ElementAt(value);

                    Item item = HappyBirthdayModCore.Instance.giftManager.getItemFromId(id);
                    Rectangle textureBounds = GameLocation.getSourceRectForObject(item.ParentSheetIndex);
                    float itemScale = 4f;
                    Rectangle placementBounds = new Rectangle((int)(this.xPositionOnScreen + 64 + column * 16 * itemScale), (int)(this.yPositionOnScreen + 256 + row * 16 * itemScale), 64, 64);
                    ClickableTextureComponent menuItemDisplayComponent = new ClickableTextureComponent(id, placementBounds, "", id, Game1.objectSpriteSheet, textureBounds, 4f, true);
                    menuItemDisplayComponent.item = item;
                    menuItemDisplayComponent.name = menuItemDisplayComponent.item.DisplayName;
                    this.itemButtons.Add(menuItemDisplayComponent);
                }
        }

        /// <summary>
        /// Gets a complete list of items that can be selected for birthday gifts, allowing for new ones from content packs and current legacy ones.
        /// </summary>
        /// <returns></returns>
        public virtual HashSet<string> getListOfValidItemIds()
        {
            HashSet<string> itemIds = new HashSet<string>();
            HashSet<string> validItems = new HashSet<string>();
            foreach (string id in HappyBirthdayModCore.Instance.giftManager.listOfFavoriteBirthdayGiftsToSelectFrom)
            {
                if (itemIds.Contains(id) == false)
                {
                    itemIds.Add(id);
                }
            }

            if (HappyBirthdayModCore.Configs.modConfig.useOnlyFavoriteGiftsListToSelectFrom == false)
            {

                foreach (string id in HappyBirthdayModCore.Instance.giftManager.legacyGiftIdResolverMap.Values)
                {
                    if (itemIds.Contains(id) == false)
                    {
                        //HappyBirthdayModCore.Instance.Monitor.Log("Adding favorite birthday gift: " + id);
                        itemIds.Add(id);
                    }
                }
            }


            if (string.IsNullOrEmpty(this.searchBox.Text) == false)
            {
                foreach (string id in itemIds)
                {
                    //TODO: I know this isn't efficient, so I need to figure out how to optimize not creating the item twice.
                    Item item = HappyBirthdayModCore.Instance.giftManager.getItemFromId(id);
                    if (item.DisplayName.ToLowerInvariant().Contains(this.searchBox.Text.ToLowerInvariant()))
                        validItems.Add(id);

                }
            }
            else
            {
                validItems = itemIds;
            }


            return validItems;
        }

        public override void receiveGamePadButton(Buttons b)
        {
            if (b.Equals(Buttons.A))
            {
                this.receiveLeftClick(Game1.getMouseX(), Game1.getMouseY(), true);
            }
        }

        public override bool areGamePadControlsImplemented()
        {
            return true;
        }

        /// <summary>
        /// Make this true if free cursor movement is desired.
        /// </summary>
        /// <returns></returns>
        public override bool overrideSnappyMenuCursorMovementBan()
        {
            return true;
        }


        /// <summary>Draw the menu to the screen.</summary>
        /// <param name="b">The sprite batch.</param>
        public override void draw(SpriteBatch b)
        {
            // draw menu box
            Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true);
            //b.Draw(Game1.daybg, new Vector2((this.xPositionOnScreen + Game1.tileSize + Game1.tileSize * 2 / 3 - 2), (this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 4)), Color.White);
            //Game1.player.FarmerSprite.draw(b, new Vector2((this.xPositionOnScreen + Game1.tileSize + Game1.tileSize * 2 / 3 - 2), (this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 4)),1f);

            this.searchBox.Draw(b, true);

            // draw season buttons
            foreach (ClickableTextureComponent button in this.itemButtons)
                button.draw(b);

            foreach (ClickableTextureComponent button in this.potentialFavoriteGifts)
                button.draw(b);

            // draw labels
            foreach (ClickableComponent label in this.Labels)
            {
                Color color = Color.Violet;
                Utility.drawTextWithShadow(b, label.name, Game1.smallFont, new Vector2(label.bounds.X, label.bounds.Y), color);
            }
            foreach (ClickableComponent label in this.Labels)
            {
                string text = "";
                Color color = Game1.textColor;
                Utility.drawTextWithShadow(b, label.name, Game1.smallFont, new Vector2(label.bounds.X, label.bounds.Y), color);
                if (text.Length > 0)
                    Utility.drawTextWithShadow(b, text, Game1.smallFont, new Vector2(label.bounds.X + Game1.tileSize / 3 - Game1.smallFont.MeasureString(text).X / 2f, label.bounds.Y + Game1.tileSize / 2), color);
            }

            // draw OK button
            if (this.potentialFavoriteGifts.Count > 0)
                this.OkButton.draw(b);
            else
            {
                this.OkButton.draw(b);
                this.OkButton.draw(b, Color.Black * 0.5f, 0.97f);
            }
            this._leftButton.draw(b);
            this._rightButton.draw(b);

            if (this.currentHoverTextureItem != null)
                //Draws the item tooltip in the menu.
                drawToolTip(b, this.currentHoverTextureItem.item.getDescription(), this.currentHoverTextureItem.item.DisplayName, this.currentHoverTextureItem.item);

            // draw cursor
            this.drawMouse(b);
        }

    }
}
