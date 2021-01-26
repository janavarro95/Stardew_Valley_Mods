using System.Collections.Generic;
using System.IO;
using StardewModdingAPI;
using StardustCore.UIUtilities;
using StardustCore.UIUtilities.SpriteFonts;

namespace StardustCore
{
    public class ModCore : Mod
    {
        public static IModHelper ModHelper;
        public static IMonitor ModMonitor;
        public static IManifest Manifest;
        public static TextureManager TextureManager;
        public static Dictionary<string, TextureManager> TextureManagers;

        public ModConfig config;

        public static string ContentDirectory;

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            ModHelper = this.Helper;
            ModMonitor = this.Monitor;
            Manifest = this.ModManifest;

            IlluminateFramework.Colors.InitializeColors();
            ContentDirectory = "Content";
            if (!Directory.Exists(ContentDirectory)) Directory.CreateDirectory(Path.Combine(ModHelper.DirectoryPath, "Content"));
            SpriteFonts.initialize();

            TextureManagers = new Dictionary<string, TextureManager>();
            TextureManager = new TextureManager();
            TextureManager.addTexture("Test1", new Texture2DExtended(ModCore.ModHelper, Path.Combine("Content", "Graphics", "MultiTest", "Test1.png")));
            TextureManager.addTexture("Test2", new Texture2DExtended(ModCore.ModHelper, Path.Combine("Content", "Graphics", "MultiTest", "Test2.png")));
            TextureManager.addTexture("Test3", new Texture2DExtended(ModCore.ModHelper, Path.Combine("Content", "Graphics", "MultiTest", "Test3.png")));
            TextureManagers.Add(this.ModManifest.UniqueID, TextureManager);

            this.config = ModHelper.ReadConfig<ModConfig>();
        }
    }
}
