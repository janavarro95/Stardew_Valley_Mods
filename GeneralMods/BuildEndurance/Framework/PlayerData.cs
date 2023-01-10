namespace Omegasis.BuildEndurance.Framework
{
    /// <summary>The data for the current player.</summary>
    internal class PlayerData
    {
        /// <summary>The player's current endurance level.</summary>
        public int CurrentLevel { get; set; }

        /// <summary>The player's current endurance XP points.</summary>
        public double CurrentExp { get; set; }

        /// <summary>The XP points needed to reach the next endurance level.</summary>
        public double ExpToNextLevel { get; set; } = 20;

        /// <summary>The stamina points to add to the player's base stamina due to their current endurance level.</summary>
        public int CurrentLevelStaminaBonus { get; set; }

        /// <summary>Whether to reset all changes by the mod to the default values (i.e. start over).</summary>
        public bool ClearModEffects { get; set; }

        /// <summary>The player's original max stamina value, excluding mod effects.</summary>
        public int OriginalMaxStamina { get; set; }

        /// <summary>The player's stamina last time they slept.</summary>
        public int NightlyStamina { get; set; }
    }
}
