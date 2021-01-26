using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using StardustCore.UIUtilities.SpriteFonts.CharacterSheets;
using StardustCore.UIUtilities.SpriteFonts.Components;

namespace StardustCore.UIUtilities.SpriteFonts.Fonts
{
    public class VanillaFont : GenericFont
    {
        /// <summary>Construct an instance.</summary>
        public VanillaFont()
        {
            this.path = Path.Combine(SpriteFonts.FontDirectory, "Vanilla");
            if (!Directory.Exists(this.path))
                Directory.CreateDirectory(this.path);
            this.characterSheet = new VanillaCharacterSheet(this.path);
        }

        /// <summary> Takes a string and returns a textured string in it's place.</summary>
        public TexturedString ParseString(string str)
        {
            List<TexturedCharacter> characters = new List<TexturedCharacter>();
            foreach (char chr in str)
                characters.Add(this.characterSheet.getTexturedCharacter(chr));
            return new TexturedString(str, Vector2.Zero, characters);
        }

        /// <summary>Takes a string and returns a textured string in it's place. Also sets the new position.</summary>
        public TexturedString ParseString(string str, Vector2 position)
        {
            List<TexturedCharacter> characters = new List<TexturedCharacter>();
            foreach (char chr in str)
                characters.Add(this.characterSheet.getTexturedCharacter(chr));
            return new TexturedString(str, position, characters);
        }

        /// <summary>Takes a string and returns a textured string in it's place. Also sets the new position and string color.</summary>
        public TexturedString ParseString(string str, Vector2 position, Color stringColor, bool useRightPadding = true)
        {
            List<TexturedCharacter> characters = new List<TexturedCharacter>();
            foreach (char chr in str)
            {
                var c = this.characterSheet.getTexturedCharacter(chr);
                c.drawColor = stringColor;
                characters.Add(c);
            }
            return new TexturedString(str, position, characters, useRightPadding);
        }

        /// <summary>Takes a string and returns a textured string in it's place. Also sets the new position, label and string color.</summary>
        /// <param name="label">The label for the string.</param>
        /// <param name="str">The string that wil be parsed into textured characters.</param>
        /// <param name="position">The position to draw the textured string.</param>
        /// <param name="stringColor">The color of the textured string.</param>
        public TexturedString ParseString(string label, string str, Vector2 position, Color stringColor, bool useRightPadding = true)
        {
            List<TexturedCharacter> characters = new List<TexturedCharacter>();
            foreach (char chr in str)
            {
                var c = this.characterSheet.getTexturedCharacter(chr);
                c.drawColor = stringColor;
                characters.Add(c);
            }
            return new TexturedString(label, position, characters, false);
        }

        /// <summary>Takes a string and returns a textured string in it's place. Also sets the new position and string color.</summary>
        /// <param name="str">The string that wil be parsed into textured characters.</param>
        /// <param name="position">The position to draw the textured string.</param>
        /// <param name="stringColor">The color for the individual characters.</param>
        public TexturedString ParseString(string str, Vector2 position, List<Color> stringColor)
        {
            List<TexturedCharacter> characters = new List<TexturedCharacter>();
            int index = 0;
            foreach (char chr in str)
            {
                var c = this.characterSheet.getTexturedCharacter(chr);
                c.drawColor = stringColor.ElementAt(index);
                characters.Add(c);
                index++;
            }
            return new TexturedString(str, position, characters);
        }
    }
}
