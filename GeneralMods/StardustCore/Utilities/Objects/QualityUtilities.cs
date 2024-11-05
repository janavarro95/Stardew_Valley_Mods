using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;

namespace Omegasis.StardustCore.Utilities.Objects
{
    /// <summary>
    /// Utilities to deal with quality for items.
    /// </summary>
    public static class QualityUtilities
    {
        /// <summary>
        /// Gets the maximum quality level that the mod recognizes for outputs.
        /// </summary>
        /// <returns></returns>
        public static int GetMaxQualityLevel()
        {
            return GetQualityProgressionList().Max();
        }

        /// <summary>
        /// Gets the progression list for going from one quality tier to another quality tier.
        /// </summary>
        /// <param name="includeNegativeQuality">Should negative quality be included.</param>
        /// <returns></returns>
        public static List<int> GetQualityProgressionList(bool includeNegativeQuality = true)
        {

            List<int> qualityProgression = new List<int>();

            if (StardustCoreModCore.Instance.Helper.ModRegistry.IsLoaded("spacechase0.AQualityMod") && includeNegativeQuality)
            {
                qualityProgression.Add(-2);
            }
            qualityProgression.Add(StardewValley.Object.lowQuality);
            qualityProgression.Add(StardewValley.Object.medQuality);
            qualityProgression.Add(StardewValley.Object.highQuality);
            qualityProgression.Add(StardewValley.Object.bestQuality);
            if (StardustCoreModCore.Instance.Helper.ModRegistry.IsLoaded("spacechase0.AQualityMod"))
            {
                qualityProgression.Add(6);
            }

            if(StardustCoreModCore.Instance.config.CustomMaxQualityLevel > qualityProgression.Last())
            {
                qualityProgression.Add(StardustCoreModCore.Instance.config.CustomMaxQualityLevel);
            }

            return qualityProgression;

        }
    }
}
