using System;
using System.Diagnostics;
using System.IO;

using System.Linq;
using Omegasis.SaveBackup.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace Omegasis.SaveBackup
{
    /// <summary>The mod entry point.</summary>
    public class SaveBackup : Mod
    {
        /*********
        ** Fields
        *********/
        /// <summary>The folder path containing the game's app data.</summary>
        private static readonly string AppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley");

        /// <summary>The folder path containing the game's saves.</summary>
        private static readonly string SavesPath = Path.Combine(SaveBackup.AppDataPath, "Saves");

        /// <summary>The folder path containing folders of the backups.</summary>
        private static readonly string BackupSavePath = Path.Combine(SaveBackup.AppDataPath, "Backed_Up_Saves");

        /// <summary>The folder path containing backups of the save before the player starts playing.</summary>
        private static readonly string PrePlayBackupsPath = Path.Combine(BackupSavePath, "Pre_Play_Saves");

        /// <summary>The folder path containing nightly backups of the save.</summary>
        private static readonly string NightlyBackupsPath = Path.Combine(BackupSavePath, "Nightly_InGame_Saves");


        /// <summary>The folder path containing the save data for Android.</summary>
        private static readonly string AndroidDataPath = Constants.DataPath;

        /// <summary>The folder path of the current save data for Android.</summary>
        private static string AndroidCurrentSavePath => Constants.CurrentSavePath;

        /// <summary>
        /// The base path for the backup save folder on Android.
        /// </summary>
        private static string AndroidBackupSavePath = Path.Combine(SaveBackup.AndroidDataPath, "Backed_Up_Saves");

        /// <summary>The folder path containing nightly backups of the save for Android.</summary>
        private static string AndroidNightlyBackupsPath => Path.Combine(AndroidBackupSavePath, Constants.SaveFolderName, "Nightly_InGame_Saves");

        /// <summary>The mod configuration.</summary>
        private ModConfig Config;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            this.Config = helper.ReadConfig<ModConfig>();

            if (string.IsNullOrEmpty(this.Config.AlternatePreplaySaveBackupPath) == false)
            {
                CreateCustomSavePathDirectories(this.Config.AlternatePreplaySaveBackupPath);
                this.BackupSaves(this.Config.AlternatePreplaySaveBackupPath);
            }
            else if(Constants.TargetPlatform != GamePlatform.Android)
            {
                CreateDefaultSaveDirectories();
                this.BackupSaves(SaveBackup.PrePlayBackupsPath);
            }

            helper.Events.GameLoop.Saving += this.OnSaving;
            helper.Events.GameLoop.Saved += this.OnSaved;
            helper.Events.GameLoop.SaveCreated += this.OnSaveCreated;
        }

        /// <summary>
        /// Creates the default folders for the save directories.
        /// </summary>
        private static void CreateDefaultSaveDirectories()
        {
            //Create the default directories if they do not exist.
            if (!Directory.Exists(BackupSavePath))
            {
                Directory.CreateDirectory(BackupSavePath);
            }
            if (!Directory.Exists(PrePlayBackupsPath))
            {
                Directory.CreateDirectory(PrePlayBackupsPath);
            }
            if (!Directory.Exists(NightlyBackupsPath))
            {
                Directory.CreateDirectory(NightlyBackupsPath);
            }
        }

        /// <summary>
        /// Creates the custom save path folders.
        /// </summary>
        /// <param name="CustomSavePath"></param>
        private static void CreateCustomSavePathDirectories(string CustomSavePath)
        {
            string prePlayPath = Path.Combine(CustomSavePath, "Pre_Play_Saves");
            string nightlyPath = Path.Combine(CustomSavePath, "Nightly_InGame_Saves");
            if (!Directory.Exists(prePlayPath))
            {
                Directory.CreateDirectory(prePlayPath);
            }
            if (!Directory.Exists(nightlyPath))
            {
                Directory.CreateDirectory(nightlyPath);
            }
        }

        /*********
        ** Private methods
        *********/
        /// <summary>Raised before the game begins writes data to the save file (except the initial save creation).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnSaving(object sender, SavingEventArgs e)
        {
            if (!this.Config.BackupAfterSave) this.DoBackup();
        }

        /// <summary>Raised after the game finishes writing data to the save file (except the initial save creation).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnSaved(object sender, SavedEventArgs e)
        {
            if (this.Config.BackupAfterSave) this.DoBackup();
        }

        /// <summary>Raised after the game finishes creating the save file.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnSaveCreated(object sender, SaveCreatedEventArgs e)
        {
            if (this.Config.BackupAfterSave) this.DoBackup();
        }

        private void DoBackup()
        {
            if (string.IsNullOrEmpty(this.Config.AlternateNightlySaveBackupPath) == false)
            {
                CreateCustomSavePathDirectories(this.Config.AlternateNightlySaveBackupPath);

                this.BackupSaves(this.Config.AlternateNightlySaveBackupPath);
            }
            else if (Constants.TargetPlatform != GamePlatform.Android)
            {
                CreateDefaultSaveDirectories();

                this.BackupSaves(SaveBackup.NightlyBackupsPath);
            }
            else
            {

                if (!Directory.Exists(SaveBackup.AndroidBackupSavePath))
                {
                    Directory.CreateDirectory(SaveBackup.AndroidBackupSavePath);
                }

                if (!Directory.Exists(SaveBackup.AndroidNightlyBackupsPath))
                {
                    Directory.CreateDirectory(SaveBackup.AndroidNightlyBackupsPath);
                }
                this.BackupSaves(SaveBackup.AndroidNightlyBackupsPath);
            }
        }

        /// <summary>Back up saves to the specified folder.</summary>
        /// <param name="folderPath">The folder path in which to generate saves.</param>
        private void BackupSaves(string folderPath)
        {
            /*
             Legacy code for SharpZipLib, but is probably not necessary anymore now that .Net is v5.0 across all platforms for SDV, but will keep this code here just in case I need to use it again at a later date.
            if (this.Config.UseZipCompression == false)
            {
                
                DirectoryCopy(Constants.TargetPlatform != GamePlatform.Android ? SaveBackup.SavesPath : SaveBackup.AndroidCurrentSavePath, Path.Combine(folderPath, $"backup-{DateTime.Now:yyyyMMdd'-'HHmmss}"), true);
                new DirectoryInfo(folderPath)
                .EnumerateDirectories()
                .OrderByDescending(f => f.CreationTime)
                .Skip(this.Config.SaveCount)
                .ToList()
                .ForEach(dir => dir.Delete(true));
                
            }
            else
            {
                
                FastZip fastZip = new FastZip();
                fastZip.UseZip64 = UseZip64.Off;
                bool recurse = true;  // Include all files by recursing through the directory structure
                string filter = null; // Dont filter any files at all
                fastZip.CreateZip(Path.Combine(folderPath, $"backup-{DateTime.Now:yyyyMMdd'-'HHmmss}.zip"), Constants.TargetPlatform != GamePlatform.Android ? SaveBackup.SavesPath : SaveBackup.AndroidCurrentSavePath, recurse, filter);
                new DirectoryInfo(folderPath)
                .EnumerateFiles()
                .OrderByDescending(f => f.CreationTime)
                .Skip(this.Config.SaveCount)
                .ToList()
                .ForEach(file => file.Delete());
                
            }
            */

            System.IO.Compression.ZipFile.CreateFromDirectory(Constants.TargetPlatform != GamePlatform.Android ? SaveBackup.SavesPath : SaveBackup.AndroidCurrentSavePath, Path.Combine(folderPath, $"backup-{DateTime.Now:yyyyMMdd'-'HHmmss}.zip"));

        }

        /*
        /// <summary>
        /// An uncompressed output method.
        /// </summary>
        /// <param name="sourceDirName"></param>
        /// <param name="destDirName"></param>
        /// <param name="copySubDirs"></param>
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
        */
    }
}
