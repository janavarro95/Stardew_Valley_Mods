using System.Collections.Generic;
using System.IO;
using Omegasis.HappyBirthday.Framework.Utilities;
using StardewValley;

namespace Omegasis.HappyBirthday.Framework
{
    /// <summary>
    /// Manages save data for happy birthday.
    /// </summary>
    public static class SaveManager
    {

        /// <summary>
        /// Forces a read when the day starts since for farmhands they don't seem to hook into the OnSave/OnLoad methods with SMAPI.
        /// </summary>
        public static void OnDayStarted(object Sender, StardewModdingAPI.Events.DayStartedEventArgs args)
        {
            if (HappyBirthdayModCore.Instance.birthdayManager.hasChosenBirthday() == false)
            {
                HappyBirthdayModCore.Instance.Monitor.Log("Loading player's birthday on new day started.");
                Load(Game1.player.UniqueMultiplayerID);
            }
        }

        /// <summary>
        /// Forces player's birthday to be saved at the end of the day.
        /// </summary>
        public static void OnDayEnded(object Sender, StardewModdingAPI.Events.DayEndingEventArgs args)
        {
            Save(Game1.player.UniqueMultiplayerID);
        }

        /// <summary>
        /// Called when the day has ended. Updates the mod's birthday info and villager queue if they have changed.
        /// </summary>
        /// <param name="UniqueMultiplayerId"></param>
        public static void Save(long UniqueMultiplayerId)
        {
            Farmer player = Game1.GetPlayer(UniqueMultiplayerId);
            //Use only the unique multiplayer id since the farmer's name can include special characters which cause the save file system to break.
            string uniqueSaveName = $"{player.UniqueMultiplayerID}";
            string dataDirectory = Path.Combine("data", uniqueSaveName);
            string dataFilePath = Path.Combine(dataDirectory, uniqueSaveName + ".json");
            string villagerQueuePath = Path.Combine(dataDirectory, uniqueSaveName + "_VillagerBirthdayGiftsQueue.json");

            if (HappyBirthdayModCore.Instance.birthdayManager.hasChosenBirthday())
            {
                //Write birthday file to disk.
                HappyBirthdayModCore.Instance.Helper.Data.WriteJsonFile(dataFilePath, HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData);
                HappyBirthdayModCore.Instance.Helper.Data.WriteJsonFile(villagerQueuePath, HappyBirthdayModCore.Instance.birthdayManager.villagerQueue);
            }
        }

        /// <summary>
        /// Loads the player's birthday information from disk.
        /// </summary>
        /// <param name="UniqueMultiplayerId"></param>
        public static void Load(long UniqueMultiplayerId)
        {
            Farmer player = Game1.GetPlayer(UniqueMultiplayerId);
            string legacyUniqueSaveName = $"{player.Name}_{player.UniqueMultiplayerID}";
            string uniqueSaveName = $"{player.UniqueMultiplayerID}";
            string dataDirectory = Path.Combine("data", legacyUniqueSaveName);
            string dataFilePath = Path.Combine(dataDirectory, legacyUniqueSaveName + ".json");
            string villagerQueuePath = Path.Combine(dataDirectory, legacyUniqueSaveName + "_VillagerBirthdayGiftsQueue.json");

            HappyBirthdayModCore.Instance.Monitor.Log("Loading player's birthday from: " + dataFilePath);
            // reset state
            HappyBirthdayModCore.Instance.birthdayManager.setCheckedForBirthday(false);

            //Loads the player's birthday from disk.

            //Attempt to load legacy birthday data using the old farmer's name convention.
            PlayerData playerData;
            try
            {
                playerData = HappyBirthdayModCore.Instance.Helper.Data.ReadJsonFile<PlayerData>(dataFilePath);

                //If loading the legacy way doesn't work, try to load the new way using just the unique multiplayer id.
                if (playerData == null)
                {
                    dataDirectory = Path.Combine("data", uniqueSaveName);
                    dataFilePath = Path.Combine(dataDirectory, uniqueSaveName + ".json");
                    villagerQueuePath = Path.Combine(dataDirectory, uniqueSaveName + "_VillagerBirthdayGiftsQueue.json");
                    playerData = HappyBirthdayModCore.Instance.Helper.Data.ReadJsonFile<PlayerData>(dataFilePath);
                }
                if (playerData == null)
                {
                    playerData = new PlayerData();
                }
            }
            catch
            {
                //If we can't load the data, use the new format.

                dataDirectory = Path.Combine("data", uniqueSaveName);
                dataFilePath = Path.Combine(dataDirectory, uniqueSaveName + ".json");
                villagerQueuePath = Path.Combine(dataDirectory, uniqueSaveName + "_VillagerBirthdayGiftsQueue.json");
                playerData = HappyBirthdayModCore.Instance.Helper.Data.ReadJsonFile<PlayerData>(dataFilePath);
                //If the player's data is still null, just create it.
                if (playerData == null)
                {
                    playerData = new PlayerData();
                }
            }

            HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData = playerData;
            HappyBirthdayModCore.Instance.birthdayManager.villagerQueue = HappyBirthdayModCore.Instance.Helper.Data.ReadJsonFile<Dictionary<string, VillagerInfo>>(villagerQueuePath) ?? new Dictionary<string, VillagerInfo>();

            if (HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData != null)
            {
                //This is still necessary to so that other players know when this player's birthday is.
                MultiplayerUtilities.SendBirthdayInfoToOtherPlayers();
            }
        }

    }
}
