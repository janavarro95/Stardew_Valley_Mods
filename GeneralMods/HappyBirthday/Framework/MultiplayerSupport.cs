using System.Collections.Generic;
using StardewValley;

namespace Omegasis.HappyBirthday.Framework
{
    public class MultiplayerSupport
    {
        public static string FSTRING_SendBirthdayMessageToOthers = "Omegasis.HappyBirthday.Framework.Messages.SendBirthdayMessageToOtherPlayers";
        public static string FSTRING_SendBirthdayInfoToOthers = "Omegasis.HappyBirthday.Framework.Messages.SendBirthdayInfoToOtherPlayers";

        public static void SendBirthdayMessageToOtherPlayers()
        {
            string str = BirthdayMessages.GetTranslatedString("Happy Birthday: Farmhand Birthday Message");
            str.Replace("@", Game1.player.name);
            HUDMessage message = new HUDMessage(str, 1);

            foreach (KeyValuePair<long, Farmer> f in Game1.otherFarmers)
            {
                HappyBirthday.ModHelper.Multiplayer.SendMessage<string>(message.message, FSTRING_SendBirthdayMessageToOthers, new string[] { HappyBirthday.ModHelper.Multiplayer.ModID }, new long[] { f.Key });
            }
        }

        public static void SendBirthdayInfoToOtherPlayers()
        {
            foreach (KeyValuePair<long, Farmer> f in Game1.otherFarmers)
            {
                HappyBirthday.ModHelper.Multiplayer.SendMessage<KeyValuePair<long, PlayerData>>(new KeyValuePair<long, PlayerData>(Game1.player.uniqueMultiplayerID, HappyBirthday.PlayerBirthdayData), FSTRING_SendBirthdayInfoToOthers, new string[] { HappyBirthday.ModHelper.Multiplayer.ModID }, new long[] { f.Key });
            }
        }

        public static void SendBirthdayInfoToConnectingPlayer(long id)
        {
            HappyBirthday.ModHelper.Multiplayer.SendMessage<KeyValuePair<long, PlayerData>>(new KeyValuePair<long, PlayerData>(Game1.player.uniqueMultiplayerID, HappyBirthday.PlayerBirthdayData), FSTRING_SendBirthdayInfoToOthers, new string[] { HappyBirthday.ModHelper.Multiplayer.ModID }, new long[] { id });
        }
    }
}
