using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.HappyBirthday.Framework.Events.Compatibility;
using Omegasis.HappyBirthday.Framework.Utilities;
using StardewValley;

namespace Omegasis.HappyBirthday.Framework.Events.EventPreconditions
{
    public static class HappyBirthdayPreconditionParsingMethods
    {
        public static FarmerBirthdayPrecondition ParseFarmerBirthdayPrecondition_Legacy(string[] preconditionData)
        {
            return new FarmerBirthdayPrecondition();
        }

        public static SpouseBirthdayPrecondition ParseSpouseBirthdayPrecondition_Legacy(string[] preconditionData)
        {
            return new SpouseBirthdayPrecondition();
        }

        public static HasChosenBirthdayPrecondition ParseHasChosenBirthdayPrecondition_Legacy(string[] preconditionData)
        {
            return new HasChosenBirthdayPrecondition(Convert.ToBoolean(preconditionData[1]));
        }

        public static HasChosenFavoriteGiftPrecondition ParseHasChosenFavoriteGiftPrecondition_Legacy(string[] preconditionData)
        {
            return new HasChosenFavoriteGiftPrecondition(Convert.ToBoolean(preconditionData[1]));
        }

        public static IsMarriedToPrecondition ParseIsMarriedToPrecondition_Legacy(string[] preconditionData)
        {
            return new IsMarriedToPrecondition(preconditionData[1]);
        }

        public static IsMarriedPrecondition ParseIsMarriedPrecondition_Legacy(string[] preconditionData)
        {
            return new IsMarriedPrecondition();
        }

        public static GameLocationIsHomePrecondition ParseGameLocationIsHomePrecondition_Legacy(string[] preconditionData)
        {
            return new GameLocationIsHomePrecondition();
        }

        public static FarmHouseLevelPrecondition ParseFarmHouseLevelPrecondition_Legacy(string[] preconditionData)
        {
            //Since some legacy data does not use the full length, I need to check for the condition here that there is a second variable or not for the comparison of the farmhouse levels.
            if (preconditionData.Length == 3)
            {
                return new FarmHouseLevelPrecondition(Convert.ToInt32(preconditionData[1]), Enum.Parse<Enums.ComparisonType>(preconditionData[2]));
            }
            return new FarmHouseLevelPrecondition(Convert.ToInt32(preconditionData[1]));
        }

        public static YearPrecondition ParseYearGreaterThanOrEqualToPrecondition_Legacy(string[] preconditionData)
        {
            return new YearPrecondition(Convert.ToInt32(preconditionData[1]), Enum.Parse<Enums.ComparisonType>(preconditionData[2]));
        }

        public static VillagersHaveEnoughFriendshipBirthdayPrecondition ParseVillagersHaveEnoughFriendshipBirthdayPrecondition_Legacy(string[] preconditionData)
        {
            return new VillagersHaveEnoughFriendshipBirthdayPrecondition();
        }

        public static IsStardewValleyExpandedInstalledPrecondition ParseIsStardewValleyExpandedInstalledPrecondition_Legacy(string[] preconditionData)
        {
            return new IsStardewValleyExpandedInstalledPrecondition(Convert.ToBoolean(preconditionData[1]));
        }



        public static bool ParseFarmerBirthdayPrecondition(GameLocation location, string eventId, string[] precondition)
        {
            return ParseFarmerBirthdayPrecondition_Legacy(precondition).meetsCondition();
        }

        public static bool ParseSpouseBirthdayPrecondition(GameLocation location, string eventId, string[] precondition)
        {
            return ParseSpouseBirthdayPrecondition_Legacy(precondition).meetsCondition();
        }

        public static bool ParseHasChosenBirthdayPrecondition(GameLocation location, string eventId, string[] precondition)
        {
            return ParseHasChosenBirthdayPrecondition_Legacy(precondition).meetsCondition();
        }

        public static bool ParseHasChosenFavoriteGiftPrecondition(GameLocation location, string eventId, string[] precondition)
        {
            return ParseHasChosenFavoriteGiftPrecondition_Legacy(precondition).meetsCondition();
        }

        public static bool ParseIsMarriedToPrecondition(GameLocation location, string eventId, string[] precondition)
        {
            return ParseIsMarriedToPrecondition_Legacy(precondition).meetsCondition();
        }

        public static bool ParseIsMarriedPrecondition(GameLocation location, string eventId, string[] precondition)
        {
            return ParseIsMarriedPrecondition_Legacy(precondition).meetsCondition();
        }

        public static bool ParseGameLocationIsHomePrecondition(GameLocation location, string eventId, string[] precondition)
        {
            return ParseGameLocationIsHomePrecondition_Legacy(precondition).meetsCondition();
        }

        public static bool ParseFarmHouseLevelPrecondition(GameLocation location, string eventId, string[] precondition)
        {
            return ParseFarmHouseLevelPrecondition_Legacy(precondition).meetsCondition();
        }

        public static bool ParseYearGreaterThanOrEqualToPrecondition(GameLocation location, string eventId, string[] precondition)
        {
            return ParseYearGreaterThanOrEqualToPrecondition_Legacy(precondition).meetsCondition();
        }

        public static bool ParseVillagersHaveEnoughFriendshipBirthdayPrecondition(GameLocation location, string eventId, string[] precondition)
        {
            return ParseVillagersHaveEnoughFriendshipBirthdayPrecondition_Legacy(precondition).meetsCondition();
        }

        public static bool ParseIsStardewValleyExpandedInstalledPrecondition(GameLocation location, string eventId, string[] precondition)
        {
            return ParseIsStardewValleyExpandedInstalledPrecondition_Legacy(precondition).meetsCondition();
        }
    }
}
