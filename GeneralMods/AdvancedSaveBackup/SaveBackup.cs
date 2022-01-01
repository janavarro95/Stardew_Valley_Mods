using System;
using System.Diagnostics;
using System.IO;

using System.Linq;
using ICSharpCode.SharpZipLib.Zip;
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

        /// <summary>The folder path containing backups of the save before the player starts playing.</summary>
        private static readonly string PrePlayBackupsPath = Path.Combine(SaveBackup.AppDataPath, "Backed_Up_Saves", "Pre_Play_Saves");

        /// <summary>The folder path containing nightly backups of the save.</summary>
        private static readonly string NightlyBackupsPath = Path.Combine(SaveBackup.AppDataPath, "Backed_Up_Saves", "Nightly_InGame_Saves");


        /// <summary>The folder path containing the save data for Android.</summary>
        private static readonly string AndroidDataPath = Constants.DataPath;

        /// <summary>The folder path of the current save data for Android.</summary>
        private static string AndroidCurrentSavePath => Constants.CurrentSavePath;

        /// <summary>The folder path containing nightly backups of the save for Android.</summary>
        private static string AndroidNightlyBackupsPath => Path.Combine(SaveBackup.AndroidDataPath, "Backed_Up_Saves", Constants.SaveFolderName, "Nightly_InGame_Saves");

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
                this.BackupSaves(this.Config.AlternatePreplaySaveBackupPath);
            }
            else if(Constants.TargetPlatform != GamePlatform.Android)
            {
                this.BackupSaves(SaveBackup.PrePlayBackupsPath);
            }

            helper.Events.GameLoop.Saving += this.OnSaving;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Raised before the game begins writes data to the save file (except the initial save creation).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnSaving(object sender, SavingEventArgs e)
        {
            if (string.IsNullOrEmpty(this.Config.AlternateNightlySaveBackupPath) == false)
            {
                this.BackupSaves(this.Config.AlternateNightlySaveBackupPath);
            }
            else if (Constants.TargetPlatform != GamePlatform.Android)
            {
                this.BackupSaves(SaveBackup.NightlyBackupsPath);
            }
            else
            {
                this.BackupSaves(SaveBackup.AndroidNightlyBackupsPath);
            }
        }

        /// <summary>Back up saves to the specified folder.</summary>
        /// <param name="folderPath">The folder path in which to generate saves.</param>
        private void BackupSaves(string folderPath)
        {
            //Ensure the backup directory is created and exists.
            //CreateDirectory will do nothing if it already exists.
            Directory.CreateDirectory(folderPath);
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


            /*
            // back up saves This used compression but it always causes a library loading issue for OS
            Directory.CreateDirectory(folderPath);
            ZipFile.CreateFromDirectory(SaveBackup.SavesPath, Path.Combine(folderPath, $"backup-{DateTime.Now:yyyyMMdd'-'HHmmss}.zip"));

            // delete old backups

            */
        }

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



        ///<summary>
        ///<see cref="https://github.com/Pathoschild/SMAPI/blob/develop/src/SMAPI.Mods.SaveBackup/ModEntry.cs"/>>
        ///This code originated from Pathoschilds SMAPI SaveBackup Mod. All rights for this code goes to them. If this code is not allowed to be here *please* contact Omegasis to discus a proper work around.
        /// Create a zip using a process command on MacOS.
        /// <param name="sourcePath">The file or directory path to zip.</param>
        /// <param name="destination">The destination file to create.</param>
        /// </summary>
        private void CompressUsingMacProcess(string sourcePath, FileInfo destination)
        {
            DirectoryInfo saveFolder = new DirectoryInfo(sourcePath);
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "zip",
                Arguments = $"-rq \"{destination.FullName}\" \"{saveFolder.Name}\" -x \"*.DS_Store\" -x \"__MACOSX\"",
                WorkingDirectory = $"{saveFolder.FullName}/../",
                CreateNoWindow = true
            };
            new Process { StartInfo = startInfo }.Start();
        }
    }
}
