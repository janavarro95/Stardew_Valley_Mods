using System;
using System.Collections.Generic;
using System.IO;
using Omegasis.StardustCore.UIUtilities;
using Omegasis.StardustCore.UIUtilities.SpriteFonts;
using StardewModdingAPI;
using StardewValley;

namespace Omegasis.StardustCore
{
    public class StardustCoreModCore : Mod
    {

        public static IModHelper ModHelper;
        public static IMonitor ModMonitor;
        public static IManifest Manifest;
        public static StardustCoreModCore Instance;
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

            Instance = this;

            IlluminateFramework.Colors.initializeColors();
            ContentDirectory = "ModAssets";
            //if (!Directory.Exists(ContentDirectory)) Directory.CreateDirectory(Path.Combine(ModHelper.DirectoryPath, "Assets"));
            SpriteFonts.initialize();

            TextureManagers = new Dictionary<string, TextureManager>();
            TextureManager = new TextureManager(this.Helper.DirectoryPath,"StardustCore",Manifest);
            //TextureManager.addTexture("Test1", new Texture2DExtended(ModCore.ModHelper,Manifest,Path.Combine("Assets", "Graphics", "MultiTest", "Test1.png")));
            //TextureManager.addTexture("Test2", new Texture2DExtended(ModCore.ModHelper,Manifest, Path.Combine("Assets", "Graphics", "MultiTest", "Test2.png")));
            //TextureManager.addTexture("Test3", new Texture2DExtended(ModCore.ModHelper, Manifest,Path.Combine("Assets", "Graphics", "MultiTest", "Test3.png")));
            TextureManagers.Add(this.ModManifest.UniqueID, TextureManager);

            this.Helper.Events.GameLoop.GameLaunched += this.GameLoop_GameLaunched;

            this.Helper.ConsoleCommands.Add("Omegasis.StardustCore.ModdingUtilities.AddFriendship", "Adds a certain amount of friendship to the given npc. <name , amount>", AddNPCFriendship);

            this.config = ModHelper.ReadConfig<ModConfig>();

        }

        private void GameLoop_GameLaunched(object sender, StardewModdingAPI.Events.GameLaunchedEventArgs e)
        {
            //string soundbankPath=Path.Combine(Game1.content.RootDirectory, "XACT", "Sound Bank.xsb");
            //Directory.CreateDirectory(Path.Combine(this.Helper.DirectoryPath, "ProcessedGameFiles"));
            //this.Monitor.Log(Utilities.HexDumper.HexDumpString(soundbankPath), LogLevel.Info);
            //Utilities.HexDumper.StripSoundCuesToFile(Path.Combine(this.Helper.DirectoryPath, "ProcessedGameFiles", "SoundCues.json"),Utilities.HexDumper.StripSoundCuesFromHex(Utilities.HexDumper.HexDumpString(soundbankPath)));
            //Utilities.HexDumper.HexDumpFile(soundbankPath, Path.Combine(this.Helper.DirectoryPath, "ProcessedGameFiles", "SoundCuesRaw.json"));
        }

        public static void log(string message)
        {
            ModMonitor.Log(message);
        }


        public static void AddNPCFriendship(string ActionName, string[] Params)
        {
            string npcName = Params[0];
            int amount = Convert.ToInt32(Params[1]);
            if (Game1.player.friendshipData.ContainsKey(npcName)){
                Game1.player.friendshipData[npcName].Points += amount;
            }
            else
            {
                Game1.player.friendshipData.Add(npcName, new Friendship(amount));
            }

        }
    }
}
