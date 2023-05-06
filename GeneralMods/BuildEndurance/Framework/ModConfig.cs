namespace Omegasis.BuildEndurance.Framework
{
    /// <summary>The mod settings and player data.</summary>
    internal class ModConfig
    {
        /// <summary>The XP points needed to reach the next endurance level.</summary>
        public double ExpToNextLevel { get; set; } = 20;

        /// <summary>The initial stamina bonus to apply regardless of the player's endurance level.</summary>
        public int BaseStaminaBonus { get; set; }

        /// <summary>The multiplier for the experience points to need to reach an endurance level relative to the previous one.</summary>
        public double ExpCurve { get; set; } = 1.15;

        /// <summary>The maximum endurance level the player can reach.</summary>
        public int MaxLevel { get; set; } = 100;

        /// <summary>The amount of stamina the player should gain for each endurance level.</summary>
        public int StaminaIncreasePerLevel { get; set; } = 1;

        /// <summary>The experience points to gain for using a tool.</summary>
        public int ExpForToolUse { get; set; } = 1;

        /// <summary>The experience points to gain for eating or drinking.</summary>
        public int ExpForEating { get; set; } = 2;

        /// <summary>The experience points to gain for sleeping.</summary>
        public int ExpForSleeping { get; set; } = 10;

        /// <summary>The experience points to gain for reaching a state of exhaustion for the day.</summary>
        public int ExpForExhaustion { get; set; } = 25;

        /// <summary>The experience points to gain for collapsing for the day.</summary>
        public int ExpForCollapsing { get; set; } = 50;
    }
}
