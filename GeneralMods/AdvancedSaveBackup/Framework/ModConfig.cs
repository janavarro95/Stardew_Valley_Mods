namespace Omegasis.SaveBackup.Framework
{
    /// <summary>The mod configuration.</summary>
    internal class ModConfig
    {
        /// <summary>The number of save backups to keep for each type.</summary>
        public int SaveCount { get; set; } = 30;

        public bool UseZipCompression { get; set; } = true;

        /// <summary>
        /// Change this to change where your saves back up.
        /// </summary>
        public string AlternateNightlySaveBackupPath { get; set; } = "";

        public string AlternatePreplaySaveBackupPath { get; set; } = "";

        /// <summary>Back up the save after saving instead of before saving.</summary>
        public bool BackupAfterSave { get; set; } = false;
    }
}
