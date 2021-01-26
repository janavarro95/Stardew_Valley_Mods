using System.Collections.Generic;
using StardustCore.UIUtilities.SpriteFonts.Components;

namespace StardustCore.UIUtilities.SpriteFonts.CharacterSheets
{
    public class GenericCharacterSheets
    {
        public Dictionary<char, TexturedCharacter> CharacterAtlus;

        public virtual TexturedCharacter getTexturedCharacter(char c)
        {
            return new TexturedCharacter();
        }
    }
}
