using StardewModdingAPI;

namespace Omegasis.HappyBirthday.Framework
{
    /// <summary>The mod configuration.</summary>
    public class ModConfig
    {
        /// <summary>The key which shows the menu.</summary>
        public SButton KeyBinding { get; set; } = SButton.O;

        /// <summary>The minimum amount of friendship needed to get a birthday gift.</summary>
        public int minNeutralFriendshipGiftLevel = 3;

        /// <summary>The max amount of friendship needed to get a neutral gift from an npc.</summary>
        public int maxNeutralFriendshipGiftLevel = 4;

        /// <summary>
        ///The minimum amount of friendship to get a like gift from an npc. 
        /// </summary>
        public int minLikeFriendshipLevel = 5;

        /// <summary>The max amount of friendship needed to get a liked gift from an npc.</summary>
        public int maxLikeFriendshipLevel = 6;

        /// <summary>The minimum amount of friendship needed to get a loved gift from an npc.</summary>
        public int minLoveFriendshipLevel = 7;

        /// <summary>The minimum amount of friendship needed to get a happy birthday greeting from an npc.</summary>
        public int minimumFriendshipLevelForBirthdayWish = 2;

        /// <summary>Handles different translations of files.</summary>
        public TranslationInfo translationInfo;

        /// <summary>Whether or not to load from the old BirthdayGifts.xnb located in StardewValley/Data or from the new BirthdayGifts.json located in the mod directory.</summary>
        public bool useLegacyBirthdayFiles;

        /// <summary>Construct an instance.</summary>
        public ModConfig()
        {
            this.translationInfo = new TranslationInfo();
            this.useLegacyBirthdayFiles = true;
        }
    }
}
