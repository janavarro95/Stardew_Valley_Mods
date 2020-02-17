using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
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
        private static string AppDataPath => Constants.TargetPlatform != GamePlatform.Android ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley") : Constants.DataPath;

        /// <summary>The folder path containing the game's saves.</summary>
        private static string SavesPath => Constants.TargetPlatform != GamePlatform.Android ? Path.Combine(SaveBackup.AppDataPath, "Saves") : Constants.CurrentSavePath;

        /// <summary>The folder path containing backups of the save before the player starts playing.</summary>
        private static readonly string PrePlayBackupsPath = Path.Combine(SaveBackup.AppDataPath, "Backed_Up_Saves", "Pre_Play_Saves");

        /// <summary>The folder path containing nightly backups of the save.</summary>
        private static string NightlyBackupsPath => Constants.TargetPlatform != GamePlatform.Android ? Path.Combine(SaveBackup.AppDataPath, "Backed_Up_Saves", "Nightly_InGame_Saves") : Path.Combine(SaveBackup.AppDataPath, "Backed_Up_Saves", Constants.SaveFolderName, "Nightly_InGame_Saves");

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

            if(Constants.TargetPlatform != GamePlatform.Android)
                this.BackupSaves(SaveBackup.PrePlayBackupsPath);

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
            this.BackupSaves(SaveBackup.NightlyBackupsPath);
        }

        /// <summary>Recursively copy a directory or file.</summary>
        /// <param name="source">The file or folder to copy.</param>
        /// <param name="targetFolder">The folder to copy into.</param>
        /// <param name="copyRoot">Whether to copy the root folder itself, or <c>false</c> to only copy its contents.</param>
        /// <param name="filter">A filter which matches the files or directories to copy, or <c>null</c> to copy everything.</param>
        /// <remarks>Derived from the SMAPI installer code.</remarks>
        /// <returns>Returns whether any files were copied.</returns>
        private bool RecursiveCopy(FileSystemInfo source, DirectoryInfo targetFolder, bool copyRoot = true)
        {
            if (!source.Exists)
                return false;

            bool anyCopied = false;

            switch (source)
            {
                case FileInfo sourceFile:
                    targetFolder.Create();
                    sourceFile.CopyTo(Path.Combine(targetFolder.FullName, sourceFile.Name));
                    anyCopied = true;
                    break;

                case DirectoryInfo sourceDir:
                    DirectoryInfo targetSubfolder = copyRoot ? new DirectoryInfo(Path.Combine(targetFolder.FullName, sourceDir.Name)) : targetFolder;
                    foreach (var entry in sourceDir.EnumerateFileSystemInfos())
                        anyCopied = this.RecursiveCopy(entry, targetSubfolder) || anyCopied;
                    break;

                default:
                    throw new NotSupportedException($"Unknown filesystem info type '{source.GetType().FullName}'.");
            }

            return anyCopied;
        }

        /// <summary>Create a zip using the .NET compression library.</summary>
        /// <param name="sourcePath">The file or directory path to zip.</param>
        /// <param name="destination">The destination file to create.</param>
        /// <exception cref="NotSupportedException">The compression libraries aren't available on this system.</exception>
        private void CompressUsingNetFramework(string sourcePath, FileInfo destination)
        {
            // get compress method
            MethodInfo createFromDirectory;
            try
            {
                // create compressed backup
                Assembly coreAssembly = Assembly.Load("System.IO.Compression, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089") ?? throw new InvalidOperationException("Can't load System.IO.Compression assembly.");
                Assembly fsAssembly = Assembly.Load("System.IO.Compression.FileSystem, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089") ?? throw new InvalidOperationException("Can't load System.IO.Compression assembly.");
                Type compressionLevelType = coreAssembly.GetType("System.IO.Compression.CompressionLevel") ?? throw new InvalidOperationException("Can't load CompressionLevel type.");
                Type zipFileType = fsAssembly.GetType("System.IO.Compression.ZipFile") ?? throw new InvalidOperationException("Can't load ZipFile type.");
                createFromDirectory = zipFileType.GetMethod("CreateFromDirectory", new[] { typeof(string), typeof(string), compressionLevelType, typeof(bool) }) ?? throw new InvalidOperationException("Can't load ZipFile.CreateFromDirectory method.");
            }
            catch (Exception ex)
            {
                throw new NotSupportedException("Couldn't load the .NET compression libraries on this system.", ex);
            }

            // compress file
            createFromDirectory.Invoke(null, new object[] { sourcePath, destination.FullName, CompressionLevel.Fastest, false });
        }

        private void OriginCompressLogic(string folderPath)
        {
            ZipFile.CreateFromDirectory(SaveBackup.SavesPath, Path.Combine(folderPath, $"backup-{DateTime.Now:yyyyMMdd'-'HHmmss}.zip"));
        }

        /// <summary>Back up saves to the specified folder.</summary>
        /// <param name="folderPath">The folder path in which to generate saves.</param>
        private void BackupSaves(string folderPath)
        {
            // back up saves
            Directory.CreateDirectory(folderPath);
            if(Constants.TargetPlatform == GamePlatform.Android)
            {
                FileInfo targetFile = new FileInfo(Path.Combine(folderPath, $"backup-{DateTime.Now:yyyyMMdd'-'HHmmss}.zip"));
                try
                {
                    this.CompressUsingNetFramework(folderPath, targetFile);
                }
                catch (NotSupportedException)
                {
                    this.RecursiveCopy(new DirectoryInfo(SaveBackup.SavesPath), new DirectoryInfo(Path.Combine(folderPath, $"backup-{DateTime.Now:yyyyMMdd'-'HHmmss}")), false);
                }
            }
            else
            {
                this.OriginCompressLogic(folderPath);
            }

            // delete old backups
            new DirectoryInfo(folderPath)
                .GetFileSystemInfos()
                .OrderByDescending(f => f.CreationTime)
                .Skip(this.Config.SaveCount)
                .ToList()
                .ForEach(file =>
                {
                    if (file is DirectoryInfo folder)
                        folder.Delete(recursive: true);
                    else
                        file.Delete();
                });
        }
    }
}
