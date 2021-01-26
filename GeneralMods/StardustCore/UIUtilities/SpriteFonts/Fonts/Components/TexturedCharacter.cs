using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardustCore.UIUtilities.SpriteFonts.Components
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
            this.texture = ModCore.ModHelper.Content.Load<Texture2D>(PathToTexture + ".png");
            this.spacing = new CharacterSpacing();
            this.drawColor = color;
            this.position = new Vector2();
        }

        public TexturedCharacter(char Character, string PathToTexture, Color color, int left, int right, int top, int bottom)
        {
            this.character = Character;
            this.pathToTexture = PathToTexture;
            string text = this.pathToTexture.Remove(0, 1);
            this.texture = ModCore.ModHelper.Content.Load<Texture2D>(text + ".png");
            this.spacing = new CharacterSpacing(left, right, top, bottom);
            this.drawColor = color;
            this.position = new Vector2();
        }

        public static TexturedCharacter Copy(TexturedCharacter original)
        {
            return new TexturedCharacter(original.character, original.pathToTexture, original.drawColor)
            {
                spacing = new CharacterSpacing(original.spacing.LeftPadding, original.spacing.RightPadding, original.spacing.TopPadding, original.spacing.BottomPadding)
            };
        }


        public void draw(SpriteBatch b)
        {
            b.Draw(this.texture, this.position, this.drawColor);
        }
    }
}
