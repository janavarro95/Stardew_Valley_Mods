using System.IO;
using Omegasis.StardustCore.UIUtilities.SpriteFonts.Fonts;

namespace Omegasis.StardustCore.UIUtilities.SpriteFonts
{
    /// <summary>Manages Fonts for Stardust core. While all fonts variables can be accessed from their classes, they can also hold a reference here.</summary>
    public class SpriteFonts
    {
        public static string FontDirectory;

        public static VanillaFont vanillaFont;

        public static void initialize()
        {
            FontDirectory = Path.Combine(StardustCoreModCore.ContentDirectory, "Fonts");
            vanillaFont = new VanillaFont();
        }
    }
}
