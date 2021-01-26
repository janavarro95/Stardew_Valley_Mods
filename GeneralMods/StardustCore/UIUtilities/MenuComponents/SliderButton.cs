using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;
using StardustCore.UIUtilities.MenuComponents.Delegates.Functionality;

namespace StardustCore.UIUtilities.MenuComponents
{
    /// <summary>An enum describing the types of slider bars that can exist.</summary>
    public enum SliderStyle
    {
        Horizontal,
        Vertical,
        Square
    }

    /// <summary>Deals with all of the information pertaining to the limitations and movement of the slider bar button.</summary>
    public class SliderInformation
    {
        /// <summary>The yPosition offset of the slider bar button.</summary>
        public int xPos;

        /// <summary>The xPosition offset of the slider bar button.</summary>
        public int yPos;

        /// <summary>Sensitivity of the slider bar. Probably should be called sluggishness since a higher number makes it move slower.</summary>
        public int sensitivity;

        /// <summary>Min possible x position.</summary>
        public int minX;

        /// <summary>Min possible y position.</summary>
        public int minY;

        /// <summary>Max possible x position.</summary>
        public int maxX;
        /// <summary>Max possible y positon;</summary>
        public int maxY;


        /// <summary>Enum dealing with what kind of slider this is.</summary>
        public SliderStyle sliderStyle;

        /// <summary>Construct an instance.</summary>
        public SliderInformation(SliderStyle style, int startingPosition, int sensitivity = 1)
        {
            this.sliderStyle = style;
            switch (style)
            {
                case SliderStyle.Horizontal:
                    this.xPos = startingPosition;
                    break;

                case SliderStyle.Vertical:
                    this.yPos = startingPosition;
                    break;

                case SliderStyle.Square:
                    this.xPos = startingPosition;
                    this.yPos = startingPosition;
                    break;
            }
            this.minX = 0;
            this.minY = 0;
            this.maxX = 100;
            this.maxY = 100;
            this.sensitivity = sensitivity;
        }

        public SliderInformation(SliderStyle style, int startingPositionX, int startingPositionY, int sensitivity = 1)
        {
            this.sliderStyle = style;
            switch (style)
            {
                case SliderStyle.Horizontal:
                    this.xPos = startingPositionX;
                    break;

                case SliderStyle.Vertical:
                    this.yPos = startingPositionY;
                    break;

                case SliderStyle.Square:
                    this.xPos = startingPositionX;
                    this.yPos = startingPositionY;
                    break;
            }
            this.minX = 0;
            this.minY = 0;
            this.maxX = 100;
            this.maxY = 100;
            this.sensitivity = sensitivity;
        }

        public SliderInformation(SliderStyle style, int startingPositionX, int startingPositionY, int minX, int maxX, int minY, int maxY, int sensitivity = 1)
        {
            this.sliderStyle = style;
            switch (style)
            {
                case SliderStyle.Horizontal:
                    this.xPos = startingPositionX;
                    break;

                case SliderStyle.Vertical:
                    this.yPos = startingPositionY;
                    break;

                case SliderStyle.Square:
                    this.xPos = startingPositionX;
                    this.yPos = startingPositionY;
                    break;
            }
            this.minX = minX;
            this.minY = minY;
            this.maxX = maxX;
            this.maxY = maxY;
            this.sensitivity = sensitivity;
        }

        /// <summary>Offset that occurs when the slider button should be clicked and dragged.</summary>
        public void movementOffset(int x, int y)
        {
            if (this.sliderStyle == SliderStyle.Horizontal || this.sliderStyle == SliderStyle.Square)
            {
                if (x - this.xPos >= this.sensitivity)
                    this.xPos += (x - this.xPos) / this.sensitivity;
            }
            else if (this.sliderStyle == SliderStyle.Vertical || this.sliderStyle == SliderStyle.Square)
            {
                if (y - this.yPos >= this.sensitivity)
                    this.yPos += (y - this.yPos) / this.sensitivity;
            }
        }

        /// <summary>Offset that occurs when a button is pushed to modify the slider button position.</summary>
        public void offset(int x, int y)
        {
            this.xPos += x;
            this.yPos += y;
        }

        /// <summary>Gets the slider information for a label.</summary>
        public string getLabelInformation(bool getExtraInformation = true)
        {
            if (!getExtraInformation) return "";
            switch (this.sliderStyle)
            {
                case SliderStyle.Horizontal:
                    return this.xPos.ToString();

                case SliderStyle.Vertical:
                    return this.yPos.ToString();

                case SliderStyle.Square:
                    return this.xPos + "," + this.yPos;

                default:
                    return "";
            }
        }
    }

    /// <summary>A menu component that is a sider bar so that you can adjust for different settings.</summary>
    public class SliderButton : Button
    {
        /// <summary>Holds all of the information for a slider.</summary>
        public SliderInformation sliderInformation;

        /// <summary>Determines whether or not to get extra information for the slider bar.</summary>
        public bool getLabelXYPos;

        /// <summary>The texture for the actual slider bar.</summary>
        public Button sliderBar;

        /// <summary>Construct an instance.</summary>
        public SliderButton(Rectangle bounds, Texture2DExtended buttonTexture, Button barTexture, Rectangle sourceRect, float scale, SliderInformation sliderInformation, bool getLabelXYPos = true)
            : base(bounds, buttonTexture, sourceRect, scale)
        {
            this.sliderInformation = sliderInformation;
            this.getLabelXYPos = getLabelXYPos;
            this.sliderBar = barTexture;
            this.initializeBounds();
        }

        /// <summary>Construct an instance.</summary>
        public SliderButton(string name, string displayText, Rectangle bounds, Texture2DExtended buttonTexture, Button barTexture, Rectangle sourceRect, float scale, SliderInformation sliderInformation, Animations.Animation defaultAnimation, Color drawColor, Color textColor, ButtonFunctionality buttonFunctionality, bool animationEnabled, List<KeyValuePair<ClickableTextureComponent, ExtraTextureDrawOrder>> extraTexture, bool getLabelXYPos = true)
            : base(name, bounds, buttonTexture, displayText, sourceRect, scale, defaultAnimation, drawColor, textColor, buttonFunctionality, animationEnabled, extraTexture)
        {
            this.sliderInformation = sliderInformation;
            this.getLabelXYPos = getLabelXYPos;
            this.sliderBar = barTexture;
            this.initializeBounds();
        }

        /// <summary>Construct an instance.</summary>
        public SliderButton(string name, string displayText, Rectangle bounds, Texture2DExtended buttonTexture, Button barTexture, Rectangle sourceRect, float scale, SliderInformation sliderInformation, Animations.Animation defaultAnimation, Color drawColor, Color textColor, ButtonFunctionality buttonFunctionality, bool animationEnabled, Dictionary<string, List<Animations.Animation>> animationsToPlay, string startingKey, int startingAnimationFrame, List<KeyValuePair<ClickableTextureComponent, ExtraTextureDrawOrder>> extraTexture, bool getLabelXYPos = true)
            : base(name, bounds, buttonTexture, displayText, sourceRect, scale, defaultAnimation, animationsToPlay, startingKey, drawColor, textColor, buttonFunctionality, startingAnimationFrame, animationEnabled, extraTexture)
        {
            this.sliderInformation = sliderInformation;
            this.getLabelXYPos = getLabelXYPos;
            this.sliderBar = barTexture;
            this.initializeBounds();
        }

        /// <summary>Interfaces the offset function for sliderInformation.</summary>
        public void offset(int x, int y)
        {
            if (x < this.sliderBar.bounds.X)
                x = this.sliderBar.bounds.X;
            if (y < this.sliderBar.bounds.Y)
                y = this.sliderBar.bounds.Y;
            if (x > (this.sliderBar.bounds.X) + (this.sliderBar.bounds.Width))
                x = (this.sliderBar.bounds.X) + (this.sliderBar.bounds.Width);
            if (y > (this.sliderBar.bounds.Y) + (this.sliderBar.bounds.Height))
                y = (this.sliderBar.bounds.Y) + (this.sliderBar.bounds.Height);

            //Get offset from button.
            int xOffset = (this.bounds.X - x); //267-300
            int yOffset = (this.bounds.Y - y);

            if (this.sliderInformation.sliderStyle == SliderStyle.Horizontal || this.sliderInformation.sliderStyle == SliderStyle.Square)
            {
                this.sliderInformation.xPos = (x - this.sliderBar.bounds.X) / (int)this.scale;
                this.bounds.X = this.sliderBar.bounds.X + this.sliderInformation.xPos * (int)this.scale;
                if (this.bounds.X > this.sliderBar.bounds.X + this.sliderBar.bounds.Width)
                {
                    this.bounds.X = this.sliderBar.bounds.X + this.sliderBar.bounds.Width;
                }
            }

            if (this.sliderInformation.sliderStyle == SliderStyle.Vertical || this.sliderInformation.sliderStyle == SliderStyle.Square)
            {
                this.sliderInformation.yPos = (y - this.sliderBar.bounds.Y) / (int)this.scale;
                this.bounds.Y = this.sliderBar.bounds.Y + this.sliderInformation.yPos * (int)this.scale;

                if (this.bounds.Y > this.sliderBar.bounds.Y + this.sliderBar.bounds.Height)
                {
                    this.bounds.Y = this.sliderBar.bounds.Y + this.sliderBar.bounds.Height;
                }
            }

            //this.sliderInformation.offset(xOffset, yOffset);
        }

        /// <summary>Sets the initial position for the button at sliderPosition.x=0;</summary>
        public void initializeBounds()
        {
            this.bounds.X += this.sliderInformation.xPos * (int)this.scale;
            this.bounds.Y += this.sliderInformation.yPos * (int)this.scale;
        }

        /// <summary>Interfaces the movementOffset function for sliderInformation.</summary>
        public void movementOffset(int x, int y)
        {
            this.offset(x, y);
        }

        /// <summary>Draws the slider.</summary>
        public override void draw(SpriteBatch b, Color color)
        {
            this.draw(b, color, Vector2.Zero, 0.0f);
        }

        /// <summary>Checks if the button or the slider bar has been clicked.</summary>
        public override bool containsPoint(int x, int y)
        {
            //Got to check to see if I am also dealing with my sliderBar
            return this.sliderBar.containsPoint(x, y) || base.containsPoint(x, y);
        }

        /// <summary>Triggers when the slider itself is clicked.</summary>
        public override void onLeftClick(int x, int y)
        {
            if (this.sliderBar.containsPoint(x, y))
            {
                int xPos = x;
                int yPos = y;
                this.offset(xPos, yPos);
            }
        }

        /// <summary>Triggers when the button component of the slider bar is held.</summary>
        public override void onLeftClickHeld(int x, int y)
        {
            if (this.containsPoint(x, y))
                this.movementOffset(x, y);
        }

        /// <summary>Draws the slider.</summary>
        public override void draw(SpriteBatch b, Color color, Vector2 offset, float layerDepth)
        {
            if (this.extraTextures != null)
            {
                foreach (var v in this.extraTextures)
                {
                    if (v.Value == ExtraTextureDrawOrder.before)
                        v.Key.draw(b, color, layerDepth);
                }
            }

            //Draw the slider bar.
            this.sliderBar.draw(b, color, offset, layerDepth);
            //b.Draw(this..getTexture(), new Vector2(this.bounds.X + (int)offset.X, this.bounds.Y + (int)offset.Y), this.sourceRect, color, 0f, Vector2.Zero, this.scale, SpriteEffects.None, layerDepth);
            b.Draw(this.animationManager.getTexture(), new Vector2(this.bounds.X + (int)offset.X, this.bounds.Y), this.sourceRect, color, 0f, Vector2.Zero, this.scale, SpriteEffects.None, layerDepth + 0.01f);

            if (this.extraTextures != null)
            {
                foreach (var v in this.extraTextures)
                {
                    if (v.Value == ExtraTextureDrawOrder.after)
                        v.Key.draw(b, color, layerDepth);
                }
            }

            if (string.IsNullOrEmpty(this.label))
                return;
            b.DrawString(Game1.smallFont, this.label + this.sliderInformation.getLabelInformation(this.getLabelXYPos), new Vector2((float)((this.sliderBar.bounds.X + this.sliderBar.bounds.Width + offset.X + this.bounds.Width)), (float)this.sliderBar.bounds.Y + ((float)(this.sliderBar.bounds.Height / 2) - Game1.smallFont.MeasureString(this.label).Y / 2f) + offset.X), this.textColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth + 0.02f);
        }
    }
}
