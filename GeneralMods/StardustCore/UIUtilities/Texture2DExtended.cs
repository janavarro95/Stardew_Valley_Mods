using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;

namespace StardustCore.UIUtilities
{
    public class Texture2DExtended
    {
        /*********
        ** Accessors
        *********/
        /// <summary>The underlying texture.</summary>
        public Texture2D Texture { get; }

        /// <summary>The texture width.</summary>
        public int Width => this.Texture.Width;

        /// <summary>The texture height.</summary>
        public int Height => this.Texture.Height;


        /*********
        ** Public methods
        *********/
        /// <summary>Empty/null constructor.</summary>
        public Texture2DExtended()
        {
            this.Texture = null;
        }

        /// <summary>Construct an instance.</summary>
        public Texture2DExtended(Texture2D texture)
        {
            this.Texture = texture;
        }

        public Texture2DExtended(IModHelper helper, string path, ContentSource contentSource = ContentSource.ModFolder)
        {
            this.Texture = helper.Content.Load<Texture2D>(path, contentSource);
        }

        public Texture2DExtended Copy()
        {
            Texture2D clone = new Texture2D(this.Texture.GraphicsDevice, this.Texture.Width, this.Texture.Height);
            Color[] data = new Color[clone.Width * clone.Height];
            this.Texture.GetData(data);
            clone.SetData(data);

            return new Texture2DExtended(clone);
        }

        /// <summary>Returns the actual 2D texture held by this wrapper class.</summary>
        public Texture2D getTexture()
        {
            return this.Texture;
        }
    }
}
