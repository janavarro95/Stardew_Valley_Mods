using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;
using StardustCore.UIUtilities.MenuComponents.Delegates.Functionality;

namespace StardustCore.UIUtilities.MenuComponents
{
    public enum ExtraTextureDrawOrder
    {
        before,
        after
    }

    public class Button : ClickableTextureComponent
    {
        public Animations.AnimationManager animationManager;
        public Color textureColor;
        public Color textColor;

        public ButtonFunctionality buttonFunctionality;

        /// <summary>A list of textures to be drawn on top of the button.</summary>
        public List<KeyValuePair<ClickableTextureComponent, ExtraTextureDrawOrder>> extraTextures;

        /// <summary>Empty Constructor.</summary>
        public Button(Rectangle bounds, Texture2DExtended texture, Rectangle sourceRect, float scale)
            : base(bounds, texture.getTexture(), sourceRect, scale)
        {
            this.animationManager = new Animations.AnimationManager(texture, new Animations.Animation(sourceRect), false);
        }

        public Button(string name, Rectangle bounds, Texture2DExtended texture, Rectangle sourceRect, float scale)
            : base(bounds, texture.getTexture(), sourceRect, scale)
        {
            this.name = name;
            this.animationManager = new Animations.AnimationManager(texture, new Animations.Animation(sourceRect), false);
        }

        public Button(string name, string displayText, Rectangle bounds, Texture2DExtended texture, Rectangle sourceRect, float scale)
            : base(bounds, texture.getTexture(), sourceRect, scale)
        {
            this.name = name;
            this.label = displayText;
            this.animationManager = new Animations.AnimationManager(texture, new Animations.Animation(sourceRect), false);
        }

        /// <summary>Basic Button constructor.</summary>
        /// <param name="texture">The texture sheet to be drawn to the screen. Used with the animation manager this allows you to reference different parts of the sheet at any given time.</param>
        public Button(string name, Rectangle bounds, Texture2DExtended texture, string displayText, Rectangle sourceRect, float scale, Animations.Animation defaultAnimation, Color drawColor, Color textColor, ButtonFunctionality functionality, bool animationEnabled = true, List<KeyValuePair<ClickableTextureComponent, ExtraTextureDrawOrder>> extraTexture = null)
            : base(bounds, texture.getTexture(), sourceRect, scale)
        {
            this.animationManager = new Animations.AnimationManager(texture, defaultAnimation, animationEnabled);
            this.label = displayText;
            this.name = name;
            this.textureColor = drawColor; // ?? IlluminateFramework.Colors.getColorFromList("White");
            this.textColor = textColor; // ?? IlluminateFramework.Colors.getColorFromList("White");
            this.buttonFunctionality = functionality;
            this.extraTextures = extraTexture ?? new List<KeyValuePair<ClickableTextureComponent, ExtraTextureDrawOrder>>();
            this.scale = scale;
        }

        /// <summary>A more advanced Button constructor that deals with an animation manager.</summary>
        public Button(string name, Rectangle bounds, Texture2DExtended texture, string displayText, Rectangle sourceRect, float scale, Animations.Animation defaultAnimation, Dictionary<string, List<Animations.Animation>> animationsToPlay, string startingAnimationKey, Color drawColor, Color textColor, ButtonFunctionality functionality, int startingAnimationFrame = 0, bool animationEnabled = true, List<KeyValuePair<ClickableTextureComponent, ExtraTextureDrawOrder>> extraTexture = null)
            : base(bounds, texture.getTexture(), sourceRect, scale)
        {
            this.animationManager = new Animations.AnimationManager(texture, defaultAnimation, animationsToPlay, startingAnimationKey, startingAnimationFrame, animationEnabled);
            this.label = displayText;
            this.name = name;
            this.textureColor = drawColor; // ?? IlluminateFramework.Colors.getColorFromList("White");
            this.textColor = drawColor; // ?? IlluminateFramework.Colors.getColorFromList("White");
            this.buttonFunctionality = functionality;
            this.extraTextures = extraTexture ?? new List<KeyValuePair<ClickableTextureComponent, ExtraTextureDrawOrder>>();
            this.scale = scale;
        }

        /// <summary>Draws the button and all of it's components.</summary>
        public new void draw(SpriteBatch b, Color color, float layerDepth)
        {
            if (this.extraTextures != null)
            {
                foreach (var v in this.extraTextures)
                {
                    if (v.Value == ExtraTextureDrawOrder.before)
                        v.Key.draw(b);
                }
            }

            this.animationManager.tickAnimation();
            if (!this.visible)
                return;
            if (this.drawShadow)
                Utility.drawWithShadow(b, this.texture, new Vector2((float)this.bounds.X + (float)(this.sourceRect.Width / 2) * this.baseScale, (float)this.bounds.Y + (float)(this.sourceRect.Height / 2) * this.baseScale), this.sourceRect, this.textureColor, 0.0f, new Vector2((float)(this.sourceRect.Width / 2), (float)(this.sourceRect.Height / 2)), this.scale, false, layerDepth, -1, -1, 0.35f);
            else
                b.Draw(this.texture, new Vector2((float)this.bounds.X + (float)(this.sourceRect.Width / 2) * this.baseScale, (float)this.bounds.Y + (float)(this.sourceRect.Height / 2) * this.baseScale), new Rectangle?(this.sourceRect), this.textureColor, 0.0f, new Vector2((float)(this.sourceRect.Width / 2), (float)(this.sourceRect.Height / 2)), this.scale, SpriteEffects.None, layerDepth);
            if (!string.IsNullOrEmpty(this.label))
            {
                b.DrawString(Game1.smallFont, this.label, new Vector2((float)(this.bounds.X + this.bounds.Width), (float)this.bounds.Y + ((float)(this.bounds.Height / 2) - Game1.smallFont.MeasureString(this.label).Y / 2f)), this.textColor);
            }

            if (this.hoverText != "")
            {
                //Game1.drawDialogueBox(Game1.getMousePosition().X, Game1.getMousePosition().Y, false, false, this.hoverText);
                //StardustCore.ModCore.ModMonitor.Log("HOVER???");
                b.DrawString(Game1.smallFont, this.hoverText, new Vector2((float)(this.bounds.X + this.bounds.Width), (float)this.bounds.Y + ((float)(this.bounds.Height) - Game1.smallFont.MeasureString(this.label).Y / 2f)), this.textColor, 0f, Vector2.Zero, this.scale, SpriteEffects.None, layerDepth - 0.5f);
            }

            if (this.extraTextures != null)
            {
                foreach (var v in this.extraTextures)
                {
                    if (v.Value == ExtraTextureDrawOrder.after)
                        v.Key.draw(b);
                }
            }
        }

        /// <summary>Draw the button.</summary>
        public virtual void draw(SpriteBatch b)
        {
            if (!this.visible)
                return;
            this.draw(b, Color.White, Vector2.Zero);
        }

        /// <summary>Draw the button.</summary>
        public virtual void draw(SpriteBatch b, Color color)
        {
            if (!this.visible)
                return;
            this.draw(b, color, Vector2.Zero);
        }

        public virtual void draw(SpriteBatch b, Color color, Vector2 offset)
        {
            if (this.extraTextures != null)
            {
                foreach (var v in this.extraTextures)
                {
                    if (v.Value == ExtraTextureDrawOrder.before)
                        v.Key.draw(b, color, 0.4f);
                }
            }

            float depth = 0.4f;
            b.Draw(this.animationManager.getTexture(), new Vector2(this.bounds.X + (int)offset.X, this.bounds.Y + (int)offset.Y), this.sourceRect, color, 0f, Vector2.Zero, this.scale, SpriteEffects.None, depth);

            if (this.extraTextures != null)
            {
                foreach (var v in this.extraTextures)
                {
                    if (v.Value == ExtraTextureDrawOrder.after)
                        v.Key.draw(b, color, 0.4f);
                }
            }
            if (string.IsNullOrEmpty(this.label))
                return;
            b.DrawString(Game1.smallFont, this.label, new Vector2((float)(this.bounds.X + this.bounds.Width), (float)this.bounds.Y + ((float)(this.bounds.Height / 2) - Game1.smallFont.MeasureString(this.label).Y / 2f)), this.textColor);

        }

        public virtual void draw(SpriteBatch b, Color color, Vector2 offset, float layerDepth)
        {
            if (this.extraTextures != null)
            {
                foreach (var v in this.extraTextures)
                {
                    if (v.Value == ExtraTextureDrawOrder.before)
                        v.Key.draw(b, color, layerDepth);
                }
            }

            b.Draw(this.animationManager.getTexture(), new Vector2(this.bounds.X + (int)offset.X, this.bounds.Y + (int)offset.Y), this.sourceRect, color, 0f, Vector2.Zero, this.scale, SpriteEffects.None, layerDepth);

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
            b.DrawString(Game1.smallFont, this.label, new Vector2((float)(this.bounds.X + this.bounds.Width), (float)this.bounds.Y + ((float)(this.bounds.Height / 2) - Game1.smallFont.MeasureString(this.label).Y / 2f)), this.textColor);
        }

        /// <summary>Swaps if the button is visible or not. Also toggles the animation manager appropriately.</summary>
        public void swapVisibility()
        {
            if (this.visible)
            {
                this.visible = false;
                this.animationManager.disableAnimation();
            }
        }

        /// <summary>The functionality that occurs when the button is clicked with the right mouse button.</summary>
        public void onRightClick()
        {
            this.buttonFunctionality?.rightClick?.run();
        }

        /// <summary>The functionality that occurs when the button is clicked with the left mouse button.</summary>
        public virtual void onLeftClick()
        {
            this.buttonFunctionality?.leftClick?.run();
        }

        public virtual void onLeftClick(int x, int y)
        {
            //IDK do something???
            if (this.containsPoint(x, y))
                this.buttonFunctionality?.leftClick?.run();
        }

        /// <summary>Triggers when the button is consistently held.</summary>
        public virtual void onLeftClickHeld(int x, int y) { }

        /// <summary>The functionality that occcurs when the button is hover overed.</summary>
        public virtual void onHover()
        {
            this.buttonFunctionality?.hover?.run();
        }

        /// <summary>Returns a new object based off of the data of this object.</summary>
        /// <returns>A Button object that is identical to the one passed in.</returns>
        public virtual Button clone()
        {
            var b = new Button(this.name, this.bounds, this.animationManager.getExtendedTexture(), this.label, this.sourceRect, this.scale, this.animationManager.defaultDrawFrame, this.textureColor, this.textColor, this.buttonFunctionality, true);
            if (b.buttonFunctionality.hover == null)
                ModCore.ModMonitor.Log("I'm null!");
            return b;
        }

        /// <summary>Makes a clone of the passed in button but at a new position.</summary>
        /// <param name="newPosition"></param>
        public virtual Button clone(Vector2 newPosition)
        {
            return new Button(this.name, new Rectangle((int)newPosition.X, (int)newPosition.Y, this.bounds.Width, this.bounds.Height), this.animationManager.getExtendedTexture(), this.label, this.sourceRect, this.scale, this.animationManager.defaultDrawFrame, this.textureColor, this.textColor, this.buttonFunctionality, true);
            //if (b.buttonFunctionality.hover == null)
            //    StardustCore.ModCore.ModMonitor.Log("I'm null!");
        }

        /// <summary>Returns a new object based off of the data of this object.</summary>
        /// <returns>A Button object that is identical to the one passed in.</returns>
        public virtual Button copy()
        {
            return this.clone();
        }

        /// <summary>Generates a new "null" Button.</summary>
        public static Button EmptyButton()
        {
            return new Button(new Rectangle(0, 0, 16, 16), new Texture2DExtended(), new Rectangle(0, 0, 16, 16), 1f)
            {
                label = "Null"
            };
        }

        /// <summary>Generates a new "null" Button.</summary>
        public static Button Empty()
        {
            return EmptyButton();
        }

        public string getDisplayText()
        {
            return this.label;
        }
    }
}
