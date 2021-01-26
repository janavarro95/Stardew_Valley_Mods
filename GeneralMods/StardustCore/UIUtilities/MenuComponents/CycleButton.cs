using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;
using StardustCore.UIUtilities.MenuComponents.Delegates.Functionality;

namespace StardustCore.UIUtilities.MenuComponents
{
    public class CycleButton : Button
    {
        public List<Button> buttons;
        public int buttonIndex;

        public CycleButton(Rectangle bounds, List<Button> buttons, Rectangle sourceRect, float scale)
            : base(bounds, buttons.ElementAt(0).animationManager.getExtendedTexture(), sourceRect, scale)
        {
            this.buttons = buttons;
            this.buttonIndex = 0;
        }

        public CycleButton(string name, string displayText, Rectangle bounds, List<Button> buttons, Rectangle sourceRect, float scale, Animations.Animation defaultAnimation, Color drawColor, Color textColor, ButtonFunctionality buttonFunctionality, bool animationEnabled, List<KeyValuePair<ClickableTextureComponent, ExtraTextureDrawOrder>> extraTexture)
            : base(name, bounds, buttons.ElementAt(0).animationManager.getExtendedTexture(), displayText, sourceRect, scale, defaultAnimation, drawColor, textColor, buttonFunctionality, animationEnabled, extraTexture)
        {
            this.buttons = buttons;
            this.buttonIndex = 0;
        }

        public CycleButton(string name, string displayText, Rectangle bounds, List<Button> buttons, Rectangle sourceRect, float scale, Animations.Animation defaultAnimation, Color drawColor, Color textColor, ButtonFunctionality buttonFunctionality, bool animationEnabled, Dictionary<string, List<Animations.Animation>> animationsToPlay, string startingKey, int startingAnimationFrame, List<KeyValuePair<ClickableTextureComponent, ExtraTextureDrawOrder>> extraTexture)
            : base(name, bounds, buttons.ElementAt(0).animationManager.getExtendedTexture(), displayText, sourceRect, scale, defaultAnimation, animationsToPlay, startingKey, drawColor, textColor, buttonFunctionality, startingAnimationFrame, animationEnabled, extraTexture)
        {
            this.buttons = buttons;
            this.buttonIndex = 0;
        }

        public override void onLeftClick()
        {
            //cycle button to next button and loop around if necessary.
            this.buttonIndex++;
            if (this.buttonIndex >= this.buttons.Count)
                this.buttonIndex = 0;

            base.onLeftClick();
        }

        public override void onLeftClick(int x, int y)
        {
            if (this.containsPoint(x, y))
            {
                ModCore.ModMonitor.Log("CLICK THE CYCLE BUTTON!");
                //cycle button to next button and loop around if necessary.
                this.buttonIndex++;
                ModCore.ModMonitor.Log("Index is! " + this.buttonIndex.ToString());
                if (this.buttonIndex >= this.buttons.Count)
                {
                    ModCore.ModMonitor.Log("NANIIII????");
                    this.buttonIndex = 0;
                }

                base.onLeftClick();
            }
        }

        public Button getCurrentButton()
        {
            return this.buttons.ElementAt(this.buttonIndex);
        }

        public string getCurrentButtonLabel()
        {
            return this.buttons.ElementAt(this.buttonIndex).label;
        }

        public string getCurrentButtonName()
        {
            return this.buttons.ElementAt(this.buttonIndex).name;
        }

        public override void draw(SpriteBatch b)
        {
            this.draw(b, Color.White);
        }

        //CHANGE ALL DRAW FUNCTIONS TO DRAW THE CURRENT BUTTON TEXTURE.
        //Also add in the code to also draw the label of the current button!!!
        public override void draw(SpriteBatch b, Color color)
        {
            this.draw(b, color, Vector2.Zero);
        }

        public override void draw(SpriteBatch b, Color color, Vector2 offset)
        {
            this.draw(b, color, offset, 0.5f);
        }

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

            b.Draw(this.getCurrentButton().animationManager.getTexture(), new Vector2(this.bounds.X + (int)offset.X, this.bounds.Y + (int)offset.Y), this.getCurrentButton().sourceRect, color, 0f, Vector2.Zero, this.getCurrentButton().scale, SpriteEffects.None, layerDepth);

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
            b.DrawString(Game1.smallFont, "Voice Mode: " + this.getCurrentButtonLabel(), new Vector2((float)(this.bounds.X + this.bounds.Width), (float)this.bounds.Y + ((float)(this.bounds.Height / 2) - Game1.smallFont.MeasureString(this.label).Y / 2f)), this.textColor);
        }
    }
}
