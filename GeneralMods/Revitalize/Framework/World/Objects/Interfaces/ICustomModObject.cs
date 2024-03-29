using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Omegasis.StardustCore.Animations;
using StardewValley;

namespace Omegasis.Revitalize.Framework.World.Objects.Interfaces
{
   public interface ICustomModObject: IBasicItemInfoProvider
    {
        [XmlIgnore]
        public AnimationManager AnimationManager
        {
            get
            {
                if (this.basicItemInformation == null) return null;
                if (this.basicItemInformation.animationManager == null) return null;
                return this.basicItemInformation.animationManager;
            }
        }

        [XmlIgnore]
        public Texture2D CurrentTextureToDisplay
        {

            get
            {
                if (this.AnimationManager == null) return null;
                return this.AnimationManager.getTexture();
            }
        }

        Item getOne();

        void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f, float transparency, float Scale);
    }
}
