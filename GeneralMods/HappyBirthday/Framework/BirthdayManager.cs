using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Omegasis.HappyBirthday.Framework.Constants;
using Omegasis.HappyBirthday.Framework.Menus;
using Omegasis.HappyBirthday.Framework.Utilities;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Monsters;

namespace Omegasis.HappyBirthday.Framework
{
    public class BirthdayManager
    {

        /// <summary>
        /// The birthday data for the player.
        /// </summary>
        public PlayerData playerBirthdayData;

        /// <summary>
        /// The birthdays for other players.
        /// </summary>
        public Dictionary<long, PlayerData> othersBirthdays;

        /// <summary>Whether we've already checked for and (if applicable) set up the player's birthday today.</summary>
        private bool checkedForBirthday;

        /// <summary>The queue of villagers who haven't given a gift yet.</summary>
        public Dictionary<string, VillagerInfo> villagerQueue;

        public BirthdayManager()
        {
            this.othersBirthdays = new Dictionary<long, PlayerData>();
            this.villagerQueue = new Dictionary<string, VillagerInfo>();
        }

        public void onDayStarted(object sender, StardewModdingAPI.Events.DayStartedEventArgs args)
        {

            this.sendOutBelatedBirthdayMail();
            this.resetVillagerQueue();
            this.setCheckedForBirthday(false);
        }

        /// <summary>Set the player's birthday/</summary>
        /// <param name="season">The birthday season.</param>
        /// <param name="day">The birthday day.</param>
        public void setBirthday(string season, int day)
        {
            if (this.playerBirthdayData == null)
            {
                this.playerBirthdayData = new PlayerData();
            }
            this.playerBirthdayData.BirthdaySeason = season;
            this.playerBirthdayData.BirthdayDay = day;
            this.playerBirthdayData.PlayersName = Game1.player.Name;
            this.playerBirthdayData.PlayerUniqueMultiplayerId = Game1.player.UniqueMultiplayerID;
        }

        /// <summary>Get whether today is the player's birthday.</summary>
        public bool isBirthday()
        {
            return this.isBirthday(Game1.currentSeason, Game1.dayOfMonth);
        }

        /// <summary>
        /// Checks to see if the player's birthday is on a given day of a given season.
        /// </summary>
        /// <param name="Season"></param>
        /// <param name="Day"></param>
        /// <returns></returns>
        public bool isBirthday(string Season, int Day)
        {
            if (this.playerBirthdayData == null) return false;

            return
                this.playerBirthdayData.BirthdayDay == Day
                && this.playerBirthdayData.BirthdaySeason.ToLower().Equals(Season);
        }

        /// <summary>
        /// Checks to see if a player's birthday was yesterday.
        /// </summary>
        /// <returns></returns>
        public bool wasBirthdayYesterday()
        {
            if (Game1.dayOfMonth == 1)
            {
                int day = 28;
                string season = "";
                if (Game1.IsSpring)
                {
                    season = "winter";
                }
                else if (Game1.IsSummer)
                {
                    season = "spring";
                }
                else if (Game1.IsFall)
                {
                    season = "summer";
                }
                else
                {
                    season = "fall";
                }
                return this.isBirthday(season, day);

            }
            return this.isBirthday(Game1.currentSeason, Game1.dayOfMonth - 1);
        }

        /// <summary>
        /// Checks to see if the player has choosen their birthday.
        /// </summary>
        /// <returns></returns>
        public bool hasChosenBirthday()
        {
            if (this.playerBirthdayData == null) return false;
            return !string.IsNullOrEmpty(this.playerBirthdayData.BirthdaySeason) && this.playerBirthdayData.BirthdayDay != 0;
        }

        /// <summary>
        /// Checks to see if the player has chosen a favorite gift yet.
        /// </summary>
        /// <returns></returns>
        public bool hasChoosenFavoriteGift()
        {
            if (this.playerBirthdayData == null)
            {
                return false;
            }
            else
            {
                if (this.playerBirthdayData.potentialFavoriteGifts.Count==0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public virtual void removeOtherPlayerBirthdayData(long MultiplayerId)
        {
            this.othersBirthdays.Remove(MultiplayerId);
        }

        public virtual void addOtherPlayerBirthdayData(KeyValuePair<long, PlayerData> birthdayData)
        {
            this.othersBirthdays.Add(birthdayData.Key, birthdayData.Value);
        }

        public virtual void updateOtherPlayerBirthdayData(KeyValuePair<long, PlayerData> birthdayData)
        {
            this.removeOtherPlayerBirthdayData(birthdayData.Key);
            this.addOtherPlayerBirthdayData(birthdayData);
        }

        /// <summary>
        /// Sends out the belated birthday mail to the player for the gifts/messages that they did not receive the day before.
        /// </summary>
        protected virtual void sendOutBelatedBirthdayMail()
        {
            if (this.wasBirthdayYesterday())
            {
                if (this.villagerQueue.Count > 0)
                {
                    MailUtilities.AddBelatedBirthdayMailToMailbox(this.villagerQueue.Keys.ToList());
                }
            }
        }

        /// <summary>Reset the queue of villager names.</summary>
        protected virtual void resetVillagerQueue()
        {

            this.villagerQueue.Clear();

            foreach (NPC npc in NPCUtilities.GetAllHumanNpcs())
            {
                if (this.villagerQueue.ContainsKey(npc.Name))
                    continue;
                this.villagerQueue.Add(npc.Name, new VillagerInfo());
            }
        }

        /// <summary>
        /// Sets up the players birthday.
        /// </summary>
        public virtual void setUpPlayersBirthday()
        {
            if (this.isBirthday())
            {
                string starMessage = Messages.GetMessage("Happy Birthday: Star Message");
                Messages.ShowStarMessage(starMessage);
                MultiplayerUtilities.SendBirthdayMessageToOtherPlayers();

                MailUtilities.AddBirthdayMailToMailbox();

                foreach (NPC npc in NPCUtilities.GetAllHumanNpcs())
                {
                    string message = HappyBirthdayModCore.Instance.birthdayMessages.getBirthdayMessage(npc.Name);
                    Dialogue d = new Dialogue(npc, "", message);
                    npc.CurrentDialogue.Push(d);
                    if (npc.CurrentDialogue.ElementAt(0) != d) npc.setNewDialogue(message);

                    /*
                    if (Game1.player.getFriendshipHeartLevelForNPC(npc.Name) >= HappyBirthdayModCore.Configs.modConfig.minimumFriendshipLevelForBirthdayWish)
                    {
                        //TODO: DON'T DELETE, MIGRATE TO CONTENT PATCHER PACK INSTEAD!!!
                    }
                    */
                }
            }
        }

        /// <summary>
        /// Sees if the mod has checked for the players birthday already or not.
        /// </summary>
        /// <returns></returns>
        public virtual bool hasCheckedForBirthday()
        {
            return this.checkedForBirthday;
        }

        /// <summary>
        /// Checks to see if the mod has checked for if the player's birthday is set.
        /// </summary>
        /// <param name="Value"></param>
        public virtual void setCheckedForBirthday(bool Value)
        {
            this.checkedForBirthday = Value;
        }


        /// <summary>
        /// Checks to see if a character is in the villager queue.
        /// </summary>
        /// <param name="NpcName"></param>
        /// <returns></returns>
        public virtual bool isVillagerInQueue(string NpcName)
        {
            return this.villagerQueue.ContainsKey(NpcName);
        }

        /// <summary>
        /// Checks to see if a villager in the villager queue has given a birthday wish.
        /// </summary>
        /// <param name="NpcName">The name of the npc.</param>
        /// <returns></returns>
        public virtual bool hasGivenBirthdayWish(string NpcName)
        {
            if (this.isVillagerInQueue(NpcName) == false) return false;
            return this.villagerQueue[NpcName].hasGivenBirthdayWish;
        }

        /// <summary>
        /// Checks to see if a villager in the villager queue has given a birthday gift.
        /// </summary>
        /// <param name="NpcName">The name of the npc.</param>
        /// <returns></returns>
        public virtual bool hasGivenBirthdayGift(string NpcName)
        {
            if (this.isVillagerInQueue(NpcName) == false) return false;
            return this.villagerQueue[NpcName].hasGivenBirthdayGift;
        }

        /// <summary>
        /// Resets birthday information.
        /// </summary>
        public virtual void reset()
        {
            this.resetVillagerQueue();
            this.playerBirthdayData = null;
            this.othersBirthdays.Clear();
        }

        public void resetPlayersBirthday(string name, string[] args)
        {
            if (Game1.player == null)
            {
                HappyBirthdayModCore.Instance.Monitor.Log("Error: Can only reset the player's birthday while actively playing a character.");
                return;
            }

            if (Game1.activeClickableMenu != null)
            {
                HappyBirthdayModCore.Instance.Monitor.Log("Error: Can only reset the player's birthday while there are no menus open. Please close all menus and try again.");
                return;
            }

            if (Game1.player != null && Game1.activeClickableMenu==null)
            {
                if (this.playerBirthdayData != null)
                {
                    this.playerBirthdayData.BirthdayDay = 0;
                    this.playerBirthdayData.BirthdaySeason = "";
                    this.checkedForBirthday = false;
                }
                Game1.activeClickableMenu = new BirthdayMenu("spring", 1, this.setBirthday);
            }
        }
    }
}
