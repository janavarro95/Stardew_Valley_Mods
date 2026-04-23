using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;

namespace Omegasis.HappyBirthday.Framework.Menus
{
    /// <summary>The menu which lets the player choose their birthday.</summary>
    public class BirthdayMenu : IClickableMenu
    {


        public const int springButtonId = 100;
        public const int summerButtonId = 101;
        public const int fallButtonId = 102;
        public const int winterButtonId = 103;

        /*********
        ** Fields
        *********/
        /// <summary>The labels to draw.</summary>
        private readonly List<ClickableComponent> Labels = new List<ClickableComponent>();

        /// <summary>The season buttons to draw.</summary>
        private readonly List<ClickableTextureComponent> seasonButtons = new List<ClickableTextureComponent>();

        /// <summary>The day buttons to draw.</summary>
        private readonly List<ClickableTextureComponent> dayButtons = new List<ClickableTextureComponent>();

        /// <summary>The OK button to draw.</summary>
        private ClickableTextureComponent okButton;

        /// <summary>The player's current birthday season.</summary>
        private string BirthdaySeason;
        private string seasonName;

        /// <summary>The player's current birthday day.</summary>
        private int birthdayDay;

        /// <summary>The callback to invoke when the birthday value changes.</summary>
        private Action<string, int> OnChanged;

        public bool alllFinished;

        public BirthdayHudMessage errorMessage;

        public static int menuWidth = 632 + borderWidth * 2;
        public static int menuHeight = 600 + borderWidth * 2 + Game1.tileSize;

        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="season">The initial birthday season.</param>
        /// <param name="day">The initial birthday day.</param>
        /// <param name="onChanged">The callback to invoke when the birthday value changes.</param>
        public BirthdayMenu(string season, int day, Action<string, int> onChanged)
            : base((int)getAppropriateMenuPosition().X, (int)getAppropriateMenuPosition().Y, menuWidth , menuHeight)
        {
            this.updateMenu(season, day, onChanged);
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

        public virtual void updateMenu(string Season, int Day, Action<string,int > OnChanged)
        {
            string toSpeak = "";
            this.BirthdaySeason = HappyBirthdayModCore.Instance.translationInfo.getTranslatedBaseGameString(Season);
            this.seasonName = Season;
            this.birthdayDay = Day;
            this.OnChanged = OnChanged;
            this.setUpPositions();
            toSpeak = $"{this.BirthdaySeason}";
            if (Day != 0)
                toSpeak = $"{toSpeak} {Day}";
            if (toSpeak != "")
                HappyBirthdayModCore.Instance.SayWithMenuChecker($"{toSpeak} {Messages.GetMessage("Selected")}", true);
        }

        /// <summary>The method called when the game window changes size.</summary>
        /// <param name="oldBounds">The former viewport.</param>
        /// <param name="newBounds">The new viewport.</param>
        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            base.gameWindowSizeChanged(oldBounds, newBounds);
            //this.xPositionOnScreen = (int)getAppropriateMenuPosition().X;
            //this.yPositionOnScreen = (int)getAppropriateMenuPosition().Y;
            this.setUpPositions();
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Regenerate the UI.</summary>
        private void setUpPositions()
        {

            HappyBirthdayModCore.Instance.Monitor.Log("Current translation code is: " + Game1.content.GetCurrentLanguage());

            this.Labels.Clear();
            this.dayButtons.Clear();
            this.seasonButtons.Clear();
            this.okButton = new ClickableTextureComponent("OK", new Rectangle(this.xPositionOnScreen + this.width - borderWidth - spaceToClearSideBorder - Game1.tileSize, this.yPositionOnScreen + this.height - borderWidth - spaceToClearTopBorder + Game1.tileSize / 4, Game1.tileSize, Game1.tileSize), "", null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46), 1f)
            {
                myID = 200,
                leftNeighborID = -7777,
                upNeighborID = -7777
            };

            string bdaySeason = HappyBirthdayModCore.Instance.translationInfo.getTranslatedBaseGameString("Birthday") + " " + Messages.GetMessage("Season");
            string bdayDay = HappyBirthdayModCore.Instance.translationInfo.getTranslatedBaseGameString("Birthday") + " " + Messages.GetMessage("Date");
            this.Labels.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + spaceToClearSideBorder + borderWidth + Game1.tileSize * 3 + 8, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder - Game1.tileSize / 8, 1, 1), bdaySeason + ": " + this.BirthdaySeason));
            this.Labels.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + spaceToClearSideBorder + borderWidth + Game1.tileSize * 3 + 8, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize, Game1.tileSize * 2, Game1.tileSize), bdayDay + ": " + this.birthdayDay));
            this.seasonButtons.Add(new ClickableTextureComponent("Spring", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + Game1.tileSize * 1 - Game1.tileSize / 4, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + (int)(Game1.tileSize * 3.10) - Game1.tileSize / 4, Game1.tileSize * 2, Game1.tileSize), "", "", Game1.mouseCursors, this.getSpringButton(), Game1.pixelZoom) {
                myID=springButtonId,
                downNeighborID = -7777,
                rightNeighborID=summerButtonId
            });
            this.seasonButtons.Add(new ClickableTextureComponent("Summer", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + Game1.tileSize * 3 - Game1.tileSize / 4, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + (int)(Game1.tileSize * 3.10) - Game1.tileSize / 4, Game1.tileSize * 2, Game1.tileSize), "", "", Game1.mouseCursors, this.getSummerButton(), Game1.pixelZoom)
            {
                myID = summerButtonId,
                downNeighborID = -7777,
                leftNeighborID=springButtonId,
                rightNeighborID=fallButtonId

            });
            this.seasonButtons.Add(new ClickableTextureComponent("Fall", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + Game1.tileSize * 5 - Game1.tileSize / 4, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + (int)(Game1.tileSize * 3.1) - Game1.tileSize / 4, Game1.tileSize * 2, Game1.tileSize), "", "", Game1.mouseCursors, this.getFallButton(), Game1.pixelZoom)
            {
                myID=fallButtonId,
                downNeighborID = -7777,
                leftNeighborID=summerButtonId,
                rightNeighborID=winterButtonId
            });
            this.seasonButtons.Add(new ClickableTextureComponent("Winter", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + Game1.tileSize * 7 - Game1.tileSize / 4, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + (int)(Game1.tileSize * 3.1) - Game1.tileSize / 4, Game1.tileSize * 2, Game1.tileSize), "", "", Game1.mouseCursors, this.getWinterButton(), Game1.pixelZoom)
            {
                myID=winterButtonId,
                downNeighborID = -7777,
                leftNeighborID=fallButtonId,
                rightNeighborID = -7777
            });

            this.dayButtons.Add(new ClickableTextureComponent("1", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + Game1.tileSize * 1 - Game1.tileSize / 4, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 4 - Game1.tileSize / 4, Game1.tileSize * 1, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(8, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 1,
                downNeighborID = 8,
                rightNeighborID = 2,
                upNeighborID = -7777
            });
            this.dayButtons.Add(new ClickableTextureComponent("2", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + Game1.tileSize * 2 - Game1.tileSize / 4, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 4 - Game1.tileSize / 4, Game1.tileSize * 1, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(16, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 2,
                downNeighborID = 9,
                leftNeighborID = 1,
                rightNeighborID = 3,
                upNeighborID = -7777
            });
            this.dayButtons.Add(new ClickableTextureComponent("3", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + Game1.tileSize * 3 - Game1.tileSize / 4, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 4 - Game1.tileSize / 4, Game1.tileSize * 1, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(24, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 3,
                downNeighborID = 10,
                leftNeighborID = 2,
                rightNeighborID = 4,
                upNeighborID = -7777
            });
            this.dayButtons.Add(new ClickableTextureComponent("4", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + Game1.tileSize * 4 - Game1.tileSize / 4, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 4 - Game1.tileSize / 4, Game1.tileSize * 1, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(32, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 4,
                downNeighborID = 11,
                leftNeighborID = 3,
                rightNeighborID = 5,
                upNeighborID = -7777
            });
            this.dayButtons.Add(new ClickableTextureComponent("5", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + Game1.tileSize * 5 - Game1.tileSize / 4, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 4 - Game1.tileSize / 4, Game1.tileSize * 1, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(40, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 5,
                downNeighborID = 12,
                leftNeighborID = 4,
                rightNeighborID = 6,
                upNeighborID = -7777
            });
            this.dayButtons.Add(new ClickableTextureComponent("6", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + Game1.tileSize * 6 - Game1.tileSize / 4, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 4 - Game1.tileSize / 4, Game1.tileSize * 1, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(48, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 6,
                downNeighborID = 13,
                leftNeighborID = 5,
                rightNeighborID = 7,
                upNeighborID = -7777
            });
            this.dayButtons.Add(new ClickableTextureComponent("7", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + Game1.tileSize * 7 - Game1.tileSize / 4, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 4 - Game1.tileSize / 4, Game1.tileSize * 1, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(56, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 7,
                downNeighborID = 14,
                leftNeighborID = 6,
                rightNeighborID = 8,
                upNeighborID = -7777
            });
            this.dayButtons.Add(new ClickableTextureComponent("8", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + Game1.tileSize * 1 - Game1.tileSize / 4, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 5 - Game1.tileSize / 4, Game1.tileSize * 1, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(64, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 8,
                downNeighborID = 15,
                leftNeighborID = 7,
                rightNeighborID = 9,
                upNeighborID = 1
            });
            this.dayButtons.Add(new ClickableTextureComponent("9", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + Game1.tileSize * 2 - Game1.tileSize / 4, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 5 - Game1.tileSize / 4, Game1.tileSize * 1, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(72, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 9,
                downNeighborID = 16,
                leftNeighborID = 8,
                rightNeighborID = 10,
                upNeighborID = 2
            });
            this.dayButtons.Add(new ClickableTextureComponent("10", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 2.75) - Game1.tileSize / 4, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 5 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(8, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 10,
                downNeighborID = 17,
                leftNeighborID = 9,
                rightNeighborID = 11,
                upNeighborID = 3
            });
            this.dayButtons.Add(new ClickableTextureComponent("10", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 3.25) - Game1.tileSize / 3, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 5 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(0, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 10,
                downNeighborID = 17,
                leftNeighborID = 9,
                rightNeighborID = 11,
                upNeighborID = 3
            });
            this.dayButtons.Add(new ClickableTextureComponent("11", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 3.75) - Game1.tileSize / 4, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 5 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(8, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 11,
                downNeighborID = 18,
                leftNeighborID = 10,
                rightNeighborID = 12,
                upNeighborID = 4
            });
            this.dayButtons.Add(new ClickableTextureComponent("11", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 4.25) - Game1.tileSize / 3, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 5 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(8, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 11,
                downNeighborID = 18,
                leftNeighborID = 10,
                rightNeighborID = 12,
                upNeighborID = 4
            });
            this.dayButtons.Add(new ClickableTextureComponent("12", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 4.75) - Game1.tileSize / 4, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 5 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(8, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 12,
                downNeighborID = 19,
                leftNeighborID = 11,
                rightNeighborID = 13,
                upNeighborID = 5
            });
            this.dayButtons.Add(new ClickableTextureComponent("12", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 5.25) - Game1.tileSize / 3, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 5 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(16, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 12,
                downNeighborID = 19,
                leftNeighborID = 11,
                rightNeighborID = 13,
                upNeighborID = 5
            });
            this.dayButtons.Add(new ClickableTextureComponent("13", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 5.75) - Game1.tileSize / 4, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 5 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(8, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 13,
                downNeighborID = 20,
                leftNeighborID = 12,
                rightNeighborID = 14,
                upNeighborID = 6
            });
            this.dayButtons.Add(new ClickableTextureComponent("13", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 6.25) - Game1.tileSize / 3, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 5 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(24, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 13,
                downNeighborID = 20,
                leftNeighborID = 12,
                rightNeighborID = 14,
                upNeighborID = 6
            });
            this.dayButtons.Add(new ClickableTextureComponent("14", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 6.75) - Game1.tileSize / 4, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 5 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(8, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 14,
                downNeighborID = 21,
                leftNeighborID = 13,
                rightNeighborID = 15,
                upNeighborID = 7
            });
            this.dayButtons.Add(new ClickableTextureComponent("14", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 7.25) - Game1.tileSize / 3, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 5 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(32, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 14,
                downNeighborID = 21,
                leftNeighborID = 13,
                rightNeighborID = 15,
                upNeighborID = 7
            });
            this.dayButtons.Add(new ClickableTextureComponent("15", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 0.75) - Game1.tileSize / 4, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 6 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(8, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 15,
                downNeighborID = 22,
                leftNeighborID = 14,
                rightNeighborID = 16,
                upNeighborID = 8
            });
            this.dayButtons.Add(new ClickableTextureComponent("15", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 1.25) - Game1.tileSize / 3, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 6 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(40, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 15,
                downNeighborID = 22,
                leftNeighborID = 14,
                rightNeighborID = 16,
                upNeighborID = 8
            });
            this.dayButtons.Add(new ClickableTextureComponent("16", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 1.75) - Game1.tileSize / 4, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 6 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(8, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 16,
                downNeighborID = 23,
                leftNeighborID = 15,
                rightNeighborID = 17,
                upNeighborID = 9
            });
            this.dayButtons.Add(new ClickableTextureComponent("16", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 2.25) - Game1.tileSize / 3, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 6 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(48, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 16,
                downNeighborID = 23,
                leftNeighborID = 15,
                rightNeighborID = 17,
                upNeighborID = 9
            });
            this.dayButtons.Add(new ClickableTextureComponent("17", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 2.75) - Game1.tileSize / 4, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 6 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(8, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 17,
                downNeighborID = 24,
                leftNeighborID = 16,
                rightNeighborID = 18,
                upNeighborID = 10
            });
            this.dayButtons.Add(new ClickableTextureComponent("17", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 3.25) - Game1.tileSize / 3, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 6 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(56, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 17,
                downNeighborID = 24,
                leftNeighborID = 16,
                rightNeighborID = 18,
                upNeighborID = 10
            });
            this.dayButtons.Add(new ClickableTextureComponent("18", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 3.75) - Game1.tileSize / 4, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 6 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(8, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 18,
                downNeighborID = 25,
                leftNeighborID = 17,
                rightNeighborID = 19,
                upNeighborID = 11
            });
            this.dayButtons.Add(new ClickableTextureComponent("18", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 4.25) - Game1.tileSize / 3, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 6 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(64, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 18,
                downNeighborID = 25,
                leftNeighborID = 17,
                rightNeighborID = 19,
                upNeighborID = 11
            });
            this.dayButtons.Add(new ClickableTextureComponent("19", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 4.75) - Game1.tileSize / 4, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 6 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(8, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 19,
                downNeighborID = 26,
                leftNeighborID = 18,
                rightNeighborID = 20,
                upNeighborID = 12
            });
            this.dayButtons.Add(new ClickableTextureComponent("19", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 5.25) - Game1.tileSize / 3, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 6 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(72, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 19,
                downNeighborID = 26,
                leftNeighborID = 18,
                rightNeighborID = 20,
                upNeighborID = 12
            });
            this.dayButtons.Add(new ClickableTextureComponent("20", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 5.75) - Game1.tileSize / 4, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 6 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(16, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 20,
                downNeighborID = 27,
                leftNeighborID = 19,
                rightNeighborID = 21,
                upNeighborID = 13
            });
            this.dayButtons.Add(new ClickableTextureComponent("20", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 6.25) - Game1.tileSize / 3, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 6 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(0, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 20,
                downNeighborID = 27,
                leftNeighborID = 19,
                rightNeighborID = 21,
                upNeighborID = 13
            });
            this.dayButtons.Add(new ClickableTextureComponent("21", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 6.75) - Game1.tileSize / 4, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 6 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(16, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 21,
                downNeighborID = 28,
                leftNeighborID = 20,
                rightNeighborID = 22,
                upNeighborID = 14
            });
            this.dayButtons.Add(new ClickableTextureComponent("21", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 7.25) - Game1.tileSize / 3, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 6 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(8, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 21,
                downNeighborID = 28,
                leftNeighborID = 20,
                rightNeighborID = 22,
                upNeighborID = 14
            });
            this.dayButtons.Add(new ClickableTextureComponent("22", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 0.75) - Game1.tileSize / 4, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 7 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(16, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 22,
                downNeighborID = -7777,
                leftNeighborID = 21,
                rightNeighborID = 23,
                upNeighborID = 15
            });
            this.dayButtons.Add(new ClickableTextureComponent("22", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 1.25) - Game1.tileSize / 3, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 7 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(16, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 22,
                downNeighborID = -7777,
                leftNeighborID = 21,
                rightNeighborID = 23,
                upNeighborID = 15
            });
            this.dayButtons.Add(new ClickableTextureComponent("23", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 1.75) - Game1.tileSize / 4, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 7 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(16, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 23,
                downNeighborID = -7777,
                leftNeighborID = 22,
                rightNeighborID = 24,
                upNeighborID = 16
             });
            this.dayButtons.Add(new ClickableTextureComponent("23", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 2.25) - Game1.tileSize / 3, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 7 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(24, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 23,
                downNeighborID = -7777,
                leftNeighborID = 22,
                rightNeighborID = 24,
                upNeighborID = 16
             });
            this.dayButtons.Add(new ClickableTextureComponent("24", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 2.75) - Game1.tileSize / 4, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 7 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(16, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 24,
                downNeighborID = -7777,
                leftNeighborID = 23,
                rightNeighborID = 25,
                upNeighborID = 17
             });
            this.dayButtons.Add(new ClickableTextureComponent("24", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 3.25) - Game1.tileSize / 3, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 7 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(32, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 24,
                downNeighborID = -7777,
                leftNeighborID = 23,
                rightNeighborID = 25,
                upNeighborID = 17
             });
            this.dayButtons.Add(new ClickableTextureComponent("25", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 3.75) - Game1.tileSize / 4, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 7 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(16, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 25,
                downNeighborID = -7777,
                leftNeighborID = 24,
                rightNeighborID = 26,
                upNeighborID = 18
             });
            this.dayButtons.Add(new ClickableTextureComponent("25", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 4.25) - Game1.tileSize / 3, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 7 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(40, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 25,
                downNeighborID = -7777,
                leftNeighborID = 24,
                rightNeighborID = 26,
                upNeighborID = 18
             });
            this.dayButtons.Add(new ClickableTextureComponent("26", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 4.75) - Game1.tileSize / 4, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 7 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(16, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 26,
                downNeighborID = -7777,
                leftNeighborID = 25,
                rightNeighborID = 27,
                upNeighborID = 19
             });
            this.dayButtons.Add(new ClickableTextureComponent("26", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 5.25) - Game1.tileSize / 3, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 7 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(48, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 26,
                downNeighborID = -7777,
                leftNeighborID = 25,
                rightNeighborID = 27,
                upNeighborID = 19
             });
            this.dayButtons.Add(new ClickableTextureComponent("27", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 5.75) - Game1.tileSize / 4, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 7 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(16, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 27,
                downNeighborID = -7777,
                leftNeighborID = 26,
                rightNeighborID = 28,
                upNeighborID = 20
             });
            this.dayButtons.Add(new ClickableTextureComponent("27", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 6.25) - Game1.tileSize / 3, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 7 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(56, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 27,
                downNeighborID = -7777,
                leftNeighborID = 26,
                rightNeighborID = 28,
                upNeighborID = 20
             });
            this.dayButtons.Add(new ClickableTextureComponent("28", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 6.75) - Game1.tileSize / 4, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 7 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(16, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 28,
                downNeighborID = -7777,
                leftNeighborID = 27,
                rightNeighborID = -7777,
                upNeighborID = 21
             });
            this.dayButtons.Add(new ClickableTextureComponent("28", new Rectangle(this.xPositionOnScreen + spaceToClearSideBorder + borderWidth + (int)(Game1.tileSize * 7.25) - Game1.tileSize / 3, this.yPositionOnScreen + borderWidth + spaceToClearTopBorder + Game1.tileSize * 7 - Game1.tileSize / 4, Game1.tileSize / 2, Game1.tileSize), "", "", Game1.content.Load<Texture2D>("LooseSprites\\font_bold"), new Rectangle(64, 16, 8, 12), Game1.pixelZoom)
            {
                myID = 28,
                downNeighborID = -7777,
                leftNeighborID = 27,
                rightNeighborID = -7777,
                upNeighborID = 21
             });
            ;

        }

        protected override void customSnapBehavior(int direction, int oldRegion, int oldID)
        {
            switch (direction)
            {
            case 0: // up
                // all custom up movement is from OkButton or top row of days to seasons
                // snap to whichever is currently selected
                switch (this.seasonName)
                {
                    default: // if days were selected this shouldn't be blank; but let's be safe and select Spring.
                    case "Spring":
                        this.setCurrentlySnappedComponentTo(springButtonId);
                        break;
                    case "Summer":
                        this.setCurrentlySnappedComponentTo(summerButtonId);
                        break;
                    case "Fall":
                        this.setCurrentlySnappedComponentTo(fallButtonId);
                        break;
                    case "Winter":
                        this.setCurrentlySnappedComponentTo(winterButtonId);
                        break;
                }
                break;
            case 1: // right
                // all custom right movements go to OKButton if available
                if (this.birthdayDay != 0 && string.IsNullOrEmpty(this.seasonName) == false && this.isFestivalDay() == false)
                    this.setCurrentlySnappedComponentTo(200);
                break;
            case 2: // down
                // custom down movement can either be from seasons to days
                // or bottom row of days to OkButton if available
                if (oldID >= springButtonId && oldID <= winterButtonId && (string.IsNullOrEmpty(this.seasonName) == false))
                {
                    if (this.birthdayDay >= 1 && this.birthdayDay <= 28)
                    {
                        this.setCurrentlySnappedComponentTo(this.birthdayDay);
                    } else {
                        this.setCurrentlySnappedComponentTo(1);
                    }
                } else if (oldID >= 21 && oldID <= 28) {
                    if (this.birthdayDay != 0 && string.IsNullOrEmpty(this.seasonName) == false && this.isFestivalDay() == false)
                        this.setCurrentlySnappedComponentTo(200);
                } else {
                    // Nothing should reach here,but to be safe snap to default.
                    this.snapToDefaultClickableComponent();
                }
                break;
            case 3: // left
                // Only from OkButton to days
                if (oldID == 200 && (string.IsNullOrEmpty(this.seasonName) == false))
                {
                    // move to selected day if already selected; otherwise just go to 28
                    if (this.birthdayDay >=1 || this.birthdayDay <= 28)
                    {
                        this.setCurrentlySnappedComponentTo(this.birthdayDay);
                    } else {
                        this.setCurrentlySnappedComponentTo(28);
                    }
                } else {
                    this.snapToDefaultClickableComponent();
                }
                break;
            }
        }

        /// <summary>Handle a button click.</summary>
        /// <param name="name">The button name that was clicked.</param>
        private void handleButtonClick(string name)
        {
            if (name == null)
                return;

            switch (name)
            {
                // season button
                case "Spring":
                case "Summer":
                case "Fall":
                case "Winter":
                    this.BirthdaySeason = HappyBirthdayModCore.Instance.translationInfo.getTranslatedBaseGameString(name);
                    this.seasonName = name;
                    this.OnChanged(this.seasonName, this.birthdayDay);
                    this.updateMenu(this.seasonName, this.birthdayDay, this.OnChanged);
                    break;

                // OK button
                case "OK":
                    /*
                    if (this.BirthdayDay >= 1 || this.BirthdayDay <= 28)
                    {
                        MultiplayerSupport.SendBirthdayInfoToOtherPlayers(); //Send updated info to others.
                    }
                    */
                    this.alllFinished = true;

                    if (Game1.CurrentEvent != null)
                    {
                        Game1.CurrentEvent.CurrentCommand++;
                    }

                    Game1.exitActiveMenu();
                    break;

                default:
                    this.birthdayDay = Convert.ToInt32(name);
                    this.OnChanged(this.seasonName, this.birthdayDay);
                    this.updateMenu(this.seasonName, this.birthdayDay, this.OnChanged);
                    break;
            }
            Game1.playSound("coin");
        }

        /// <summary>The method invoked when the player left-clicks on the menu.</summary>
        /// <param name="x">The X-position of the cursor.</param>
        /// <param name="y">The Y-position of the cursor.</param>
        /// <param name="playSound">Whether to enable sound.</param>
        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            //If the season is not selected then the day buttons can't be clicked. Thanks to @Potato#5266 on the SDV discord for this tip.
            if (string.IsNullOrEmpty(this.seasonName) == false)
                foreach (ClickableTextureComponent button in this.dayButtons.ToList())
                    if (button.containsPoint(x, y))
                    {
                        this.handleButtonClick(button.name);
                        button.scale -= 0.5f;
                        button.scale = Math.Max(3.5f, button.scale);
                    }

            foreach (ClickableTextureComponent button in this.seasonButtons.ToList())
                if (button.containsPoint(x, y))
                {
                    this.handleButtonClick(button.name);
                    button.scale -= 0.5f;
                    button.scale = Math.Max(3.5f, button.scale);
                }

            if (this.okButton.containsPoint(x, y))
            {
                if (this.isFestivalDay())
                {
                    string message = Messages.GetMessage("BirthdayError_FestivalDay", "You can't have your birthday on this day. Sorry!");
                    if (!string.IsNullOrEmpty(message))
                    {
                        Game1.addHUDMessage(new BirthdayHudMessage(message));
                        return;
                    }
                    return;
                }
                if (this.seasonName == "" || this.birthdayDay == 0) return;
                this.handleButtonClick(this.okButton.name);
                this.okButton.scale -= 0.25f;
                this.okButton.scale = Math.Max(0.75f, this.okButton.scale);
            }
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
            foreach (ClickableTextureComponent button in this.dayButtons)
                button.scale = button.containsPoint(x, y)
                    ? Math.Min(button.scale + 0.02f, button.baseScale + 0.1f)
                    : Math.Max(button.scale - 0.02f, button.baseScale);

            foreach (ClickableTextureComponent button in this.seasonButtons)
                button.scale = button.containsPoint(x, y)
                    ? Math.Min(button.scale + 0.02f, button.baseScale + 0.1f)
                    : Math.Max(button.scale - 0.02f, button.baseScale);

            this.okButton.scale = this.okButton.containsPoint(x, y)
                ? Math.Min(this.okButton.scale + 0.02f, this.okButton.baseScale + 0.1f)
                : Math.Max(this.okButton.scale - 0.02f, this.okButton.baseScale);

        }

        public new void populateClickableComponentList()
        {
            ///
            if (this.seasonButtons != null && this.dayButtons != null && this.okButton != null)
            {
                this.allClickableComponents = new List<ClickableComponent>(this.seasonButtons.Count + this.dayButtons.Count + 1);
                this.allClickableComponents.AddRange(this.seasonButtons);
                this.allClickableComponents.AddRange(this.dayButtons);
                this.allClickableComponents.Add(this.okButton);
            }
        }

        /// <summary>The method invoked when the player presses a key..</summary>
        /// <param name="key">Keys enum.</param>
        public override void receiveKeyPress(Keys key)
        {
            if (HappyBirthdayModCore.Instance.screenreader != null)
            {
                if (this.allClickableComponents == null || this.allClickableComponents.Count == 0)
                    this.populateClickableComponentList();
                if (this.currentlySnappedComponent == null)
                    this.snapToDefaultClickableComponent();
                if (key != 0)
                    this.applyMovementKey(key);
            }
        }

        /// <summary>Draw the menu to the screen.</summary>
        /// <param name="b">The sprite batch.</param>
        public override void draw(SpriteBatch b)
        {
            // draw menu box
            Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true);
            //b.Draw(Game1.daybg, new Vector2((this.xPositionOnScreen + Game1.tileSize + Game1.tileSize * 2 / 3 - 2), (this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 4)), Color.White);
            //Game1.player.FarmerSprite.draw(b, new Vector2((this.xPositionOnScreen + Game1.tileSize + Game1.tileSize * 2 / 3 - 2), (this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 4)),1f);

            // draw day buttons
            if (string.IsNullOrEmpty(this.seasonName) == false)
                foreach (ClickableTextureComponent button in this.dayButtons)
                    button.draw(b);

            // draw season buttons
            foreach (ClickableTextureComponent button in this.seasonButtons)
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
            if (this.birthdayDay != 0 && string.IsNullOrEmpty(this.seasonName) == false && this.isFestivalDay() == false)
                this.okButton.draw(b);
            else
            {
                this.okButton.draw(b);
                this.okButton.draw(b, Color.Black * 0.5f, 0.97f);
            }

            if (Game1.hudMessages.Count > 0)
            {
                int position = 0;
                foreach(HUDMessage message in Game1.hudMessages)
                {
                    if(message is BirthdayHudMessage)
                    {
                        int height = 1;
                        message.draw(b, position, ref height);
                    }
                    position++;
                }
            }

            // draw cursor
            this.drawMouse(b);
        }

        public bool isFestivalDay()
        {
            if (this.birthdayDay == 0 || string.IsNullOrEmpty(this.BirthdaySeason)) return false;
            if (this.BirthdaySeason.ToLowerInvariant() == "spring")
            {
                if (this.birthdayDay == 13) return true;
                if (this.birthdayDay == 24) return true;
            }
            if (this.BirthdaySeason.ToLowerInvariant() == "summer")
                if (this.birthdayDay == 11) return true; //The lua
            if (this.BirthdaySeason.ToLowerInvariant() == "fall")
                if (this.birthdayDay == 16) return true;
            if (this.BirthdaySeason.ToLowerInvariant() == "winter")
            {
                if (this.birthdayDay == 8) return true;
                if (this.birthdayDay == 25) return true;
            }
            return false;
        }

        public Rectangle getSpringButton()
        {
            //For some reason turkish and italian don't use translated words for the seasons???
            if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.zh)
                return new Rectangle(188, 437, 32, 9);
            else
                return new Rectangle(188, 438, 32, 9);
        }

        public Rectangle getSummerButton()
        {
            return new Rectangle(220, 438, 32, 8);
        }
        public Rectangle getFallButton()
        {
            return new Rectangle(188, 447, 32, 10);
        }
        public Rectangle getWinterButton()
        {
            return new Rectangle(220, 448, 32, 8);
        }

        public override bool readyToClose()
        {
            return this.alllFinished;
        }

        public override void update(GameTime time)
        {
        }


        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        //              Gamepad controlls here           //
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//


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

        public override void applyMovementKey(int direction)
        {
            if (this.allClickableComponents == null || this.allClickableComponents.Count == 0)
                this.populateClickableComponentList();
            //HappyBirthdayModCore.Instance.SayWithMenuChecker($"Direction: {direction}", true);
            ClickableComponent old = this.currentlySnappedComponent;
            base.applyMovementKey(direction);
            if (this.currentlySnappedComponent != null && this.currentlySnappedComponent != old)
            {
                this.currentlySnappedComponent.snapMouseCursorToCenter();
                this.speakCurrentlySnappedComponent();
            }
        }

        public void speakCurrentlySnappedComponent()
        {
            if ( HappyBirthdayModCore.Instance.screenreader != null && this.currentlySnappedComponent != null)
            {
                string toSpeak = "";
                if (string.IsNullOrEmpty(this.currentlySnappedComponent.name))
                {
                    toSpeak = $"{this.currentlySnappedComponent.myID}";
                } else {
                    toSpeak = this.currentlySnappedComponent.name;
                }
                if ((this.currentlySnappedComponent.name == this.seasonName) || (this.currentlySnappedComponent.myID == this.birthdayDay))
                {
                    string selected = Messages.GetMessage("Selected");
                    toSpeak = $"{selected} {toSpeak}";
                } else if (toSpeak == "OK") {
                    toSpeak = $"{toSpeak} button";
                }
                if (toSpeak != "")
                    HappyBirthdayModCore.Instance.SayWithMenuChecker(toSpeak, true);
            }
        }

        public override void setCurrentlySnappedComponentTo(int id)
        {
            base.setCurrentlySnappedComponentTo(id);
            if (this.currentlySnappedComponent != null)
            {
                this.currentlySnappedComponent.snapMouseCursorToCenter();
                this.speakCurrentlySnappedComponent();
            }
        }

        public override void snapToDefaultClickableComponent()
        {
            this.setCurrentlySnappedComponentTo(springButtonId);
        }
    }
}
