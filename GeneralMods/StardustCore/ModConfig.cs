using StardewValley;

namespace Omegasis.StardustCore
{
    public class ModConfig
    {
        /// <summary>
        /// A custom max quality level that Stardust Core can recognize in case the default <see cref="QualityUtilities.GetMaxQualityLevel"/> is lower than expected.
        /// </summary>
        public int CustomMaxQualityLevel = StardewValley.Object.bestQuality;
        public ModConfig() { }
    }
}
