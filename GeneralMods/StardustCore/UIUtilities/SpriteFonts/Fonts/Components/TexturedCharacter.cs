using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace Omegasis.StardustCore.UIUtilities.SpriteFonts.Components
{
    public class TexturedCharacter
    {
        public char character;
        public string pathToTexture;
        public Texture2D texture;
        public CharacterSpacing spacing;
        public Color drawColor;
        public Vector2 position;

        public TexturedCharacter() { }

        public TexturedCharacter(char Character, string PathToTexture, Color color)
        {
            this.character = Character;
            this.pathToTexture = PathToTexture;
            this.texture = StardustCoreModCore.ModHelper.ModContent.Load<Texture2D>(PathToTexture + ".png");
            this.spacing = new CharacterSpacing();
            this.drawColor = color;
            this.position = new Vector2();
        }

        public TexturedCharacter(char Character, Texture2D Texture, Color color)
        {
            this.character = Character;
            this.texture = Texture;
            this.spacing = new CharacterSpacing();
            this.drawColor = color;
            this.position = new Vector2();
        }

        public TexturedCharacter(char Character, string PathToTexture, Color color, int left, int right, int top, int bottom)
        {
            this.character = Character;
            this.pathToTexture = PathToTexture;
            string text = this.pathToTexture.Remove(0, 1);
            this.texture = StardustCoreModCore.ModHelper.ModContent.Load<Texture2D>(text + ".png");
            this.spacing = new CharacterSpacing(left, right, top, bottom);
            this.drawColor = color;
            this.position = new Vector2();
        }

        public static TexturedCharacter Copy(TexturedCharacter original)
        {
            if (string.IsNullOrEmpty(original.pathToTexture))
            {
                Texture2D text = new Texture2D(Game1.graphics.GraphicsDevice, original.texture.Width, original.texture.Height);
                Color[] colors = new Color[text.Width * text.Height];
                original.texture.GetData(colors);
                text.SetData(colors);
                return new TexturedCharacter(original.character, text, original.drawColor)
                {
                    spacing = new CharacterSpacing(original.spacing.LeftPadding, original.spacing.RightPadding, original.spacing.TopPadding, original.spacing.BottomPadding)
                };
            }

            return new TexturedCharacter(original.character, original.pathToTexture, original.drawColor)
            {
                spacing = new CharacterSpacing(original.spacing.LeftPadding, original.spacing.RightPadding, original.spacing.TopPadding, original.spacing.BottomPadding)
            };
        }


        public void draw(SpriteBatch b)
        {
            b.Draw(this.texture, this.position, this.drawColor);
        }

        public void draw(SpriteBatch b,Rectangle sourceRectangle,float Scale, float Depth)
        {
            b.Draw(this.texture, this.position, sourceRectangle, this.drawColor, 0f, Vector2.Zero, Scale, SpriteEffects.None, Depth);
        }
    }
}
