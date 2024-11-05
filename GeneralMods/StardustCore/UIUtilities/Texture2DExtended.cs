using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using Omegasis.StardustCore.Networking;
using StardewModdingAPI;
using StardewValley;

namespace Omegasis.StardustCore.UIUtilities
{
    public class Texture2DExtended : INetObject<NetFields>
    {

        public readonly NetString name = new NetString();

        [XmlIgnore]
        public string Name
        {
            get
            {
                return this.name.Value;
            }
            set
            {
                this.name.Value = value;
            }
        }

        [XmlIgnore]
        public Texture2D texture;

        public readonly NetString path = new NetString();

        public string Path
        {
            get
            {
                return this.path.Value;
            }
            set
            {
                this.path.Value = value;
            }
        }

        public readonly NetString modID = new NetString();
        public readonly NetString textureManagerId = new NetString();

        [XmlIgnore]
        public int Width
        {
            get
            {
                return this.texture.Width;
            }
        }
        [XmlIgnore]
        public int Height
        {
            get
            {
                return this.texture.Height;
            }
        }

        [XmlIgnore]
        public NetFields NetFields { get; } = new NetFields("Omegasis.StardustCore.UIUtilities.Texture2DExtended");

        /// <summary>Empty/null constructor.</summary>
        public Texture2DExtended()
        {
            this.name.Value = "";
            this.texture = null;
            this.path.Value = "";
            this.modID.Value = "";
            this.textureManagerId.Value = "";
            this.NetFields.AddFields(this.getNetFields().ToArray());
        }


        /// <summary>Construct an instance.</summary>
        /// <param name="path">The relative path to file on disk. See StardustCore.Utilities.getRelativePath(modname,path);
        public Texture2DExtended(IManifest manifest, string path, string TextureManagerId)
        {
            this.name.Value = System.IO.Path.GetFileNameWithoutExtension(path);
            this.path.Value = path;
            this.modID.Value = manifest.UniqueID;
            this.textureManagerId.Value = TextureManagerId;
            this.loadTexture();
        }

        public Texture2DExtended(string modID, string path, string TextureManagerId)
        {
            this.name.Value = System.IO.Path.GetFileNameWithoutExtension(path);
            this.path.Value = path;
            this.modID.Value = modID;
            this.textureManagerId.Value = TextureManagerId;
            this.loadTexture();
            this.NetFields.AddFields(this.getNetFields().ToArray());
        }

        public Texture2DExtended(IContentPack content, string path, string TextureManagerId)
        {
            this.name.Value = System.IO.Path.GetFileNameWithoutExtension(path);
            this.path.Value = path;
            this.modID.Value = content.Manifest.UniqueID;
            this.textureManagerId.Value = TextureManagerId;
            this.loadTexture();
            this.NetFields.AddFields(this.getNetFields().ToArray());


        }

        public virtual void setFields(Texture2DExtended other)
        {
            this.name.Value = other.name.Value;
            this.path.Value = other.path.Value;
            this.modID.Value = other.modID.Value;
            this.textureManagerId.Value = other.textureManagerId.Value;
            this.loadTexture();
        }

        protected virtual List<INetSerializable> getNetFields()
        {
            return new List<INetSerializable>()
            {
                this.name,
                this.path,
                this.modID,
                this.textureManagerId
            };
        }

        public Texture2DExtended Copy()
        {
            return new Texture2DExtended(this.modID.Value, this.path.Value, this.textureManagerId.Value);
        }

        /// <summary>Returns the actual 2D texture held by this wrapper class.</summary>
        public virtual Texture2D getTexture()
        {
            if (this.texture != null)
            {
                return this.texture;
            }
            else
            {
                this.loadTexture();
                return this.texture;
            }
        }

        /// <summary>
        /// Sets the texture 2d for this extended texture.
        /// </summary>
        /// <param name="text"></param>
        public virtual void setTexture(Texture2D text)
        {
            this.texture = text;
        }

        /// <summary>
        /// Loads the texture if this texture is null;
        /// </summary>
        public virtual void loadTexture()
        {
            if (string.IsNullOrEmpty(this.path.Value))
            {
                StardustCoreModCore.log("Texture path is null: " + this.path.Value);
                return;

            }
            if (string.IsNullOrEmpty(this.modID.Value))
            {
                StardustCoreModCore.log("Texture modId is null?");
                return;
            }
            if (string.IsNullOrEmpty(this.textureManagerId.Value))
            {
                StardustCoreModCore.log("Texture manager id is null?");
                return;
            }

            if (this.texture == null)
            {
                this.texture = StardustCore.UIUtilities.TextureManager.GetTextureManager(this.modID.Value, this.textureManagerId.Value).loadTexture(this.Path, this.Name, this);

                if (this.texture == null)
                {
                    this.texture = StardustCore.UIUtilities.TextureManager.GetTextureManager(this.modID.Value, this.textureManagerId.Value).getTexture(this.Name);
                }
            }
        }
    }
}
