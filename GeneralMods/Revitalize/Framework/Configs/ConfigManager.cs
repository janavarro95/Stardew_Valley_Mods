using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.Configs.ObjectsConfigs;
using Omegasis.Revitalize.Framework.Configs.ShopConfigs;
using Omegasis.Revitalize.Framework.Configs.WorldConfigs;
using StardewModdingAPI;

namespace Omegasis.Revitalize.Framework.Configs
{
    /// <summary>
    /// Handles holding all of the config information.
    /// </summary>
    public class ConfigManager
    {

        public ObjectConfigManager objectConfigManager;

        public ShopsConfigManager shopsConfigManager;

        public WorldConfigManager worldConfigManager;

        /// <summary>
        /// Configs for dealing with revitalize content packs.
        /// </summary>
        public ContentPackConfig contentPackConfig;

        public ConfigManager()
        {

            this.objectConfigManager = new ObjectConfigManager();
            this.shopsConfigManager = new ShopsConfigManager();
            this.worldConfigManager = new WorldConfigManager();

            this.contentPackConfig = InitializeConfig<ContentPackConfig>("ContentPackConfigs.cs");
        }

        /// <summary>
        /// Initializes a config for Revitalize.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="RelativePathToConfig"></param>
        /// <returns></returns>
        public static T InitializeConfig<T>(params string[] RelativePathToConfig) where T : class
        {
            string combinedPath = Path.Combine(RelativePathToConfig);
            if (string.IsNullOrEmpty(combinedPath))
            {
                throw new Exception("A relative path to a config file MUST be supplied otherwise a file access error will be thrown.");
            }

            return InitializeConfig<T>(Revitalize.RevitalizeModCore.ModHelper, RelativePathToConfig);
        }

        /// <summary>
        /// Initializes the config at the given relative path or creates it. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="helper">The mod helper to use to get the full path for file existence checking.</param>
        /// <param name="RelativePathToConfig"></param>
        /// <returns></returns>
        public static T InitializeConfig<T>(IModHelper helper, params string[] RelativePathToConfig) where T : class
        {
            string combinedPath = Path.Combine(RelativePathToConfig);
            if (string.IsNullOrEmpty(combinedPath))
            {
                throw new Exception("A relative path to a config file MUST be supplied otherwise a file access error will be thrown.");
            }

            string relativePath = Path.Combine(RelativePathToConfig);
            if (File.Exists(Path.Combine(helper.DirectoryPath, relativePath)))
                return helper.Data.ReadJsonFile<T>(relativePath);
            else
            {
                T Config = (T)Activator.CreateInstance(typeof(T));
                helper.Data.WriteJsonFile(relativePath, Config);
                return Config;
            }
        }
    }
}
