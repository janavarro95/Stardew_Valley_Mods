using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.HappyBirthday.Framework.Constants;
using Omegasis.HappyBirthday.Framework.ContentPack;
using StardewValley;

namespace Omegasis.HappyBirthday.Framework.Utilities
{
    public static class MailUtilities
    {


        public static void EditMailAsset(StardewModdingAPI.IAssetData asset)
        {


            //if (HappyBirthdayModCore.Instance.contentPacksInitalized == false) return;

            IDictionary<string, string> data = asset.AsDictionary<string, string>().Data;
            data[MailKeys.MomBirthdayMessageKey] = GetMomsMailMessage();
            data[MailKeys.DadBirthdayMessageKey] = GetDadsMailMessage();
            data[MailKeys.DadMarriedBirthdayMessageKey] = GetDadsMailMessage();

            foreach (string MailKey in MailKeys.GetAllNonBelatedMailKeysExcludingParents())
            {
                UpdateMailMessage(ref data, MailKey);
            }

            foreach (KeyValuePair<string, string> npcNameToMailKey in MailKeys.GetAllBelatedBirthdayMailKeys())
            {
                string npcName = npcNameToMailKey.Key;
                string mailKey = npcNameToMailKey.Value;

                Item gift = HappyBirthdayModCore.Instance.giftManager.getNextBirthdayGift(npcName);
                string formattedMailItemString = GetItemMailStringFormat(gift.QualifiedItemId, gift.Stack, npcName);


                //Add some special handling here to allow for belated birthday wishes from modded npcs that don't have specific dialogue.
                string mailMessage = GetMailMessage(mailKey);
                mailMessage = string.Format(mailMessage, formattedMailItemString);

                HappyBirthdayModCore.Instance.Monitor.Log("NPC mail key is " + mailKey);
                HappyBirthdayModCore.Instance.Monitor.Log("NPC mail message is " + mailMessage);
                HappyBirthdayModCore.Instance.Monitor.Log("NPC formattedMailItemString message is " + formattedMailItemString);

                if (string.IsNullOrEmpty(mailMessage))
                {
                    mailMessage = GetMailMessage("Omegasis.HappyBirthday_BelatedBirthdayWish_Generic_Fallback_Npc_Message");

                    NPC npc = Game1.getCharacterFromName(npcName);
                    if (npc != null)
                    {
                        npcName = npc.displayName;
                    }

                    data[mailKey] = string.Format(mailMessage, formattedMailItemString, npcName);
                    continue;
                }
                else
                {
                    data[mailKey] = mailMessage;
                }
            }
        }

        /// <summary>
        /// Creates the mail message from dad.
        /// </summary>
        /// <returns></returns>
        public static string GetDadsMailMessage()
        {
            string dadCPGiftString = HappyBirthdayModCore.Instance.giftManager.getRandomPossibleGiftMailStringFromDad();
            if (!string.IsNullOrEmpty(dadCPGiftString))
            {
                return dadCPGiftString;
            }


            int moneyToGet = Game1.year == 1 ? HappyBirthdayModCore.Configs.mailConfig.dadBirthdayYear1MoneyGivenAmount : HappyBirthdayModCore.Configs.mailConfig.dadBirthdayMoneyGivenAmount;

            string formattedString = string.Format("%item money {0} %%", moneyToGet);

            if (Game1.player.isMarriedOrRoommates() && Game1.player.isRoommate("Krobus") == false)
            {
                string birthdayMessage = GetMailMessage(MailKeys.DadMarriedBirthdayMessageKey);
                if (string.IsNullOrEmpty(birthdayMessage) == false)
                {
                    return string.Format(GetMailMessage(MailKeys.DadMarriedBirthdayMessageKey), formattedString);
                }
            }

            return string.Format(GetMailMessage(MailKeys.DadBirthdayMessageKey), formattedString);
        }

        /// <summary>
        /// Gets the proper mail string for getting items in the mail.
        /// </summary>
        /// <param name="ParentSheetIndex"></param>
        /// <param name="StackSize"></param>
        /// <returns></returns>
        public static string GetItemMailStringFormat(int ParentSheetIndex, int StackSize, string NPCName)
        {
            if (!string.IsNullOrEmpty(NPCName))
            {
                NPC npc = Game1.getCharacterFromName(NPCName);
                if (npc == null) return "";

                if (Game1.player.getFriendshipHeartLevelForNPC(NPCName) < HappyBirthdayModCore.Configs.modConfig.minimumFriendshipLevelForBirthdayWish)
                {
                    return "";
                }
            }

            return string.Format("%item object {0} {1} %%", ParentSheetIndex, StackSize);
        }

        public static string GetItemMailStringFormat(string ItemId, int StackSize, string NPCName)
        {
            if (!string.IsNullOrEmpty(NPCName))
            {
                NPC npc = Game1.getCharacterFromName(NPCName);
                if (npc == null) return "";

                if (Game1.player.getFriendshipHeartLevelForNPC(NPCName) < HappyBirthdayModCore.Configs.modConfig.minimumFriendshipLevelForBirthdayWish)
                {
                    return "";
                }
            }

            return string.Format("%item id {0} {1} %%", ItemId, StackSize);
        }

        /// <summary>
        /// Creates the mail message from mom.
        /// </summary>
        /// <returns></returns>
        public static string GetMomsMailMessage()
        {
            int itemToGet = HappyBirthdayModCore.Configs.mailConfig.momBirthdayItemGive;
            int stackSizeToGet = HappyBirthdayModCore.Configs.mailConfig.momBirthdayItemGiveStackSize;
            string formattedString = GetItemMailStringFormat(itemToGet, stackSizeToGet, "");

            return string.Format(GetMailMessage(MailKeys.MomBirthdayMessageKey), formattedString);
        }

        /// <summary>
        /// Gets a mail message from the list of loaded strings that are currently selected from the current content pack.
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static string GetMailMessage(string Key)
        {
            //Code for Content Patcher Content Packs
            string message = "";
            string key = string.Format("Data/Mail:{0}", Key);

            try
            {
                Dictionary<string, string> data = HappyBirthdayModCore.Instance.Helper.GameContent.Load<Dictionary<string, string>>("Data/Mail");
                message = data[Key];

                HappyBirthdayModCore.Instance.Monitor.Log("THE MESSAGE IS: " + message);

            }
            catch (Exception e)
            {
                HappyBirthdayModCore.Instance.Monitor.Log(e.ToString());
                message = "";
            }

            if (!string.IsNullOrEmpty(message) && !message.Equals(key))
            {
                return message;
            }

            //Code for legacy Happy Birthday Content Packs
            return HappyBirthdayModCore.Instance.translationInfo.getMailString(Key);
        }

        /// <summary>
        /// Removes all birthday mail that the player could have seen that was added by this mod.
        /// </summary>
        public static void RemoveAllBirthdayMail()
        {
            foreach (string MailKey in MailKeys.GetAllMailKeys())
            {
                RemoveBirthdayMailIfReceived(MailKey);
            }
        }

        /// <summary>
        /// Removes a piece of mail from the Player's list of seen mail with the given mail key.
        /// </summary>
        /// <param name="MailKey"></param>
        public static void RemoveBirthdayMailIfReceived(string MailKey)
        {
            if (Game1.player.mailReceived.Contains(MailKey))
            {
                Game1.player.mailReceived.Remove(MailKey);
            }
        }

        /// <summary>
        /// Updates a mail message with a given mail key.
        /// </summary>
        /// <param name="MailData"></param>
        /// <param name="MailKey"></param>
        /// <param name="FormattingArgs">The string args to be used in replacing the mail keys.</param>
        public static void UpdateMailMessage(ref IDictionary<string, string> MailData, string MailKey, params string[] FormattingArgs)
        {
            MailData[MailKey] = string.Format(GetMailMessage(MailKey), FormattingArgs);
        }

        /// <summary>
        /// Adds all of the birthday mail to the player's mailbox.
        /// </summary>
        public static void AddBirthdayMailToMailbox()
        {

            Game1.player.mailbox.Add(MailKeys.MomBirthdayMessageKey);
            Game1.player.mailbox.Add(MailKeys.DadBirthdayMessageKey);

            foreach (NPC npc in NPCUtilities.GetAllNonSpecialHumanNpcs())
            {
                string npcName = npc.Name;
                if (Game1.player.friendshipData.ContainsKey(npcName))
                {
                    if (Game1.player.friendshipData[npcName].IsDating())
                    {
                        string mailKey = "";
                        if (npcName.Equals("Abigail"))
                        {
                            if (Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth).ToLowerInvariant().Equals("wed") || Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth).ToLowerInvariant().Equals("wed."))
                            {
                                mailKey = MailKeys.CreateDatingPartyInvitationKey(npcName, "_Wednesday");
                            }
                            else
                            {
                                mailKey = MailKeys.CreateDatingPartyInvitationKey(npcName);
                            }
                        }
                        else
                        {
                            mailKey = MailKeys.CreateDatingPartyInvitationKey(npcName);
                        }
                        if (!string.IsNullOrEmpty(mailKey))
                        {
                            Game1.player.mailbox.Add(mailKey);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adds the belated birthday mail to the player's mailbox.
        /// </summary>
        /// <param name="NpcsToReceieveMailFrom"></param>
        public static void AddBelatedBirthdayMailToMailbox(List<string> NpcsToReceieveMailFrom)
        {
            HappyBirthdayModCore.Instance.Helper.GameContent.InvalidateCache("Data/Mail");


            foreach (string npcName in NpcsToReceieveMailFrom)
            {
                if (NPCUtilities.ShouldWishPlayerHappyBirthday(npcName))
                {
                    string mailKey = MailKeys.CreateBelatedBirthdayWishMailKey(npcName);
                    Game1.player.mailbox.Add(mailKey);
                }
            }
        }

    }
}
