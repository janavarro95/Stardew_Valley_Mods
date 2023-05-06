using System.IO;
using Omegasis.BuildHealth.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace Omegasis.BuildHealth
{
    /// <summary>The mod entry point.</summary>
    public class BuildHealth : Mod
    {
        /*********
        ** Fields
        *********/
        /// <summary>The relative path for the current player's data file.</summary>
        private string RelativeDataPath => Path.Combine("data", $"{Constants.SaveFolderName}.json");

        /// <summary>The mod settings and player data.</summary>
        private ModConfig Config;

        /// <summary>The data for the current player.</summary>
        private PlayerData PlayerData;

        /// <summary>Whether the player recently gained XP for tool use.</summary>
        private bool HasRecentToolExp;

        /// <summary>Whether the player was eating last time we checked.</summary>
        private bool WasEating;

        /// <summary>The player's health last time we checked it.</summary>
        private int LastHealth;

        /// <summary>Whether the player has collapsed today.</summary>
        private bool WasCollapsed;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
            helper.Events.GameLoop.DayStarted += this.OnDayStarted;
            helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
            helper.Events.GameLoop.Saving += this.OnSaving;

            this.Config = helper.ReadConfig<ModConfig>();
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Raised after the game state is updated (≈60 times per second).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            // nerf how quickly tool xp is gained (I hope)
            if (e.IsOneSecond && this.HasRecentToolExp)
                this.HasRecentToolExp = false;

            // give XP when player finishes eating
            if (Game1.player.isEating)
                this.WasEating = true;
            else if (this.WasEating)
            {
                this.PlayerData.CurrentExp += this.Config.ExpForEating;
                this.WasEating = false;
            }

            // give XP when player uses tool
            if (!this.HasRecentToolExp && Game1.player.UsingTool)
            {
                this.PlayerData.CurrentExp += this.Config.ExpForToolUse;
                this.HasRecentToolExp = true;
            }

            // give XP for taking damage
            var player = Game1.player;
            if (this.LastHealth > player.health)
            {
                this.PlayerData.CurrentExp += this.LastHealth - player.health;
                this.LastHealth = player.health;
            }
            else if (this.LastHealth < player.health)
                this.LastHealth = player.health;

            // give XP when player stays up too late or collapses
            if (!this.WasCollapsed && this.shouldFarmerPassout())
            {
                this.PlayerData.CurrentExp += this.Config.ExpForCollapsing;
                this.WasCollapsed = true;
            }
        }

        /// <summary>Raised after the player loads a save slot and the world is initialised.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            // reset state
            this.HasRecentToolExp = false;
            this.WasEating = false;
            this.LastHealth = Game1.player.health;
            this.WasCollapsed = false;

            // load player data
            this.PlayerData = this.Helper.Data.ReadJsonFile<PlayerData>(this.RelativeDataPath)
                ?? new PlayerData {
                    ExpToNextLevel = this.Config.ExpToNextLevel
                };

            if (this.PlayerData.OriginalMaxHealth == 0)
                this.PlayerData.OriginalMaxHealth = Game1.player.maxHealth;

            // reset if needed
            if (this.PlayerData.ClearModEffects)
            {
                Game1.player.maxHealth = this.PlayerData.OriginalMaxHealth;
                this.PlayerData.ExpToNextLevel = this.Config.ExpToNextLevel;
                this.PlayerData.CurrentExp = 0;
                this.PlayerData.CurrentLevelHealthBonus = 0;
                this.PlayerData.OriginalMaxHealth = Game1.player.maxHealth;
                this.PlayerData.CurrentLevel = 0;
                this.PlayerData.ClearModEffects = false;
            }

            // else apply health bonus
            else {
                Game1.player.maxHealth = this.Config.BaseHealthBonus + this.PlayerData.CurrentLevelHealthBonus + this.PlayerData.OriginalMaxHealth;
                if (Game1.player.health > Game1.player.maxHealth)
                    Game1.player.health = Game1.player.maxHealth;
            }
        }

        /// <summary>Raised after the game begins a new day (including when the player loads a save).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            // reset data
            this.LastHealth = Game1.player.maxHealth;
            this.WasCollapsed = false;

            // update settings
            var player = Game1.player;
            this.PlayerData.CurrentExp += this.Config.ExpForSleeping;
            if (this.PlayerData.OriginalMaxHealth == 0)
                this.PlayerData.OriginalMaxHealth = player.maxHealth; //grab the initial Health value

            if (this.PlayerData.CurrentLevel < this.Config.MaxLevel)
            {
                while (this.PlayerData.CurrentExp >= this.PlayerData.ExpToNextLevel && this.PlayerData.CurrentLevel<this.Config.MaxLevel)
                {
                    this.PlayerData.CurrentLevel += 1;
                    this.PlayerData.CurrentExp = this.PlayerData.CurrentExp - this.PlayerData.ExpToNextLevel;
                    this.PlayerData.ExpToNextLevel =
                        (this.Config.ExpCurve * this.PlayerData.ExpToNextLevel);
                    player.maxHealth += this.Config.HealthIncreasePerLevel;
                    this.PlayerData.CurrentLevelHealthBonus += this.Config.HealthIncreasePerLevel;
                }
            }
        }

        /// <summary>Raised before the game begins writes data to the save file (except the initial save creation).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnSaving(object sender, SavingEventArgs e)
        {
            // save data
            this.Helper.Data.WriteJsonFile(this.RelativeDataPath, this.PlayerData);
        }

        public bool shouldFarmerPassout()
        {
            if (Game1.player.stamina <= 0 || Game1.player.health <= 0 || Game1.timeOfDay >= 2600) return true;
            else return false;
        }
    }
}
