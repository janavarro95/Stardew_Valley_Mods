using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.StardustCore.UIUtilities.MenuComponents.ComponentsV2
{
    public class BlinkingText
    {
        public string displayText;
        public int currentFrame;
        public int maxFrames;
        public bool drawThis;

        public BlinkingText(string text,int existsForXMilliseconds)
        {
            this.displayText = text;
            this.maxFrames = existsForXMilliseconds;
            this.currentFrame = existsForXMilliseconds;
        }

        public void update(Microsoft.Xna.Framework.GameTime time)
        {
            this.currentFrame -= time.ElapsedGameTime.Milliseconds;

            if (this.currentFrame <= 0 && this.currentFrame>=-this.maxFrames)
            {
                this.drawThis = false;
            }
            else if(this.currentFrame>0)
            {
                if (this.currentFrame > 0)
                {
                    this.drawThis = true;
                }
            }
            else if(this.currentFrame<-this.maxFrames)
            {
                this.currentFrame = this.maxFrames;
            }
        }
        public void draw(Microsoft.Xna.Framework.Graphics.SpriteBatch b, Microsoft.Xna.Framework.Graphics.SpriteFont font, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Color color)
        {
            if (this.drawThis)
            {
                b.DrawString(font, this.displayText, position, color);
            }
        }
    }
}
