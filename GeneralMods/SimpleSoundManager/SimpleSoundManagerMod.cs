using StardewModdingAPI;

namespace SimpleSoundManager
{
    public class SimpleSoundManagerMod : Mod
    {
        internal static IModHelper ModHelper;
        internal static IMonitor ModMonitor;

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            ModHelper = helper;
            ModMonitor = this.Monitor;
        }
    }
}
