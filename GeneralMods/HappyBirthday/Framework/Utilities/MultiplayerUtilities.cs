using System.Collections.Generic;
using System.IO;
using System.Linq;
using StardewModdingAPI.Events;
using StardewValley;

namespace Omegasis.HappyBirthday.Framework.Utilities
{
    public class MultiplayerUtilities
    {
        public static string FSTRING_SendBirthdayMessageToOthers = "Omegasis.HappyBirthday.Framework.Messages.SendBirthdayMessageToOtherPlayers";
        public static string FSTRING_SendBirthdayInfoToOthers = "Omegasis.HappyBirthday.Framework.Messages.SendBirthdayInfoToOtherPlayers";
        public static string FSTRING_SendFarmhandBirthdayInfoToPlayer = "Omegasis.HappyBirthday.Framework.Messages.SendFarmhandBirthdayInfoToPlayer";
        public static string FSTRING_RequestBirthdayInfoFromServer = "Omegasis.HappyBirthday.Framework.Messages.RequestBirthdayInfoFromServer";

        public static void SendBirthdayMessageToOtherPlayers()
        {
            string str = Messages.GetMessage("Happy Birthday: Farmhand Birthday Message");
            str=str.Replace("@", Game1.player.Name);
            HUDMessage message = new HUDMessage(str, 1);

            foreach (KeyValuePair<long, Farmer> f in Game1.otherFarmers)
                HappyBirthdayModCore.Instance.Helper.Multiplayer.SendMessage(message.message, FSTRING_SendBirthdayMessageToOthers, new string[] { HappyBirthdayModCore.Instance.Helper.Multiplayer.ModID }, new long[] { f.Key });
        }

        public static void SendBirthdayInfoToOtherPlayers()
        {
            foreach (KeyValuePair<long, Farmer> f in Game1.otherFarmers)
                HappyBirthdayModCore.Instance.Helper.Multiplayer.SendMessage(new KeyValuePair<long, PlayerData>(Game1.player.UniqueMultiplayerID, HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData), FSTRING_SendBirthdayInfoToOthers, new string[] { HappyBirthdayModCore.Instance.Helper.Multiplayer.ModID }, new long[] { f.Key });
        }

        public static void SendBirthdayInfoToConnectingPlayer(long id)
        {
            HappyBirthdayModCore.Instance.Helper.Multiplayer.SendMessage(new KeyValuePair<long, PlayerData>(Game1.player.UniqueMultiplayerID, HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData), FSTRING_SendBirthdayInfoToOthers, new string[] { HappyBirthdayModCore.Instance.Helper.Multiplayer.ModID }, new long[] { id });
        }

        public static void SendFarmandBirthdayInfoToPlayer(long id, PlayerData FarmhandBirthday)
        {
            HappyBirthdayModCore.Instance.Helper.Multiplayer.SendMessage(new KeyValuePair<long, PlayerData>(id, FarmhandBirthday), FSTRING_SendFarmhandBirthdayInfoToPlayer, new string[] { HappyBirthdayModCore.Instance.Helper.Multiplayer.ModID }, new long[] { id });
        }
        /// <summary>
        /// Requests the info from the server.
        /// </summary>
        /// <param name="id">The id of the connecting farmhand.</param>
        public static void RequestFarmandBirthdayInfoFromServer()
        {
            HappyBirthdayModCore.Instance.Helper.Multiplayer.SendMessage(new KeyValuePair<long, string>(Game1.player.UniqueMultiplayerID, ""), FSTRING_RequestBirthdayInfoFromServer, new string[] { HappyBirthdayModCore.Instance.Helper.Multiplayer.ModID }, new long[] { });
        }

        /// <summary>Used to check for player disconnections.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        public static void Multiplayer_PeerDisconnected(object sender, PeerDisconnectedEventArgs e)
        {
            HappyBirthdayModCore.Instance.birthdayManager.removeOtherPlayerBirthdayData(e.Peer.PlayerID);
        }

        public static void Multiplayer_ModMessageReceived(object sender, ModMessageReceivedEventArgs e)
        {
            if (e.FromModID == HappyBirthdayModCore.Instance.Helper.Multiplayer.ModID && e.Type == FSTRING_SendBirthdayMessageToOthers)
            {
                string message = e.ReadAs<string>();
                Game1.hudMessages.Add(new HUDMessage(message, 1));
            }

            if (e.FromModID == HappyBirthdayModCore.Instance.Helper.Multiplayer.ModID && e.Type == FSTRING_SendBirthdayInfoToOthers)
            {
                KeyValuePair<long, PlayerData> message = e.ReadAs<KeyValuePair<long, PlayerData>>();


                if (message.Key.Equals(Game1.player.UniqueMultiplayerID))
                    HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData = message.Value;
                else if (!HappyBirthdayModCore.Instance.birthdayManager.othersBirthdays.ContainsKey(message.Key))
                {
                    HappyBirthdayModCore.Instance.birthdayManager.addOtherPlayerBirthdayData(message);
                    SendBirthdayInfoToConnectingPlayer(e.FromPlayerID);
                    HappyBirthdayModCore.Instance.Monitor.Log("Got other player's birthday data from: " + Game1.getFarmer(e.FromPlayerID).Name);
                }
                else
                {
                    //Brute force update birthday info if it has already been recevived but dont send birthday info again.
                    HappyBirthdayModCore.Instance.birthdayManager.updateOtherPlayerBirthdayData(message);
                    HappyBirthdayModCore.Instance.Monitor.Log("Got other player's birthday data from: " + Game1.getFarmer(e.FromPlayerID).Name);
                }
            }
            if (e.FromModID == HappyBirthdayModCore.Instance.Helper.Multiplayer.ModID && e.Type.Equals(FSTRING_SendFarmhandBirthdayInfoToPlayer))
            {
                KeyValuePair<long, PlayerData> message = e.ReadAs<KeyValuePair<long, PlayerData>>();
                if (Game1.player.UniqueMultiplayerID == message.Key)
                {
                    HappyBirthdayModCore.Instance.Monitor.Log("Got requested farmhand birthday info");
                    HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData = message.Value;
                }
                else
                    HappyBirthdayModCore.Instance.Monitor.Log("Picked up message for farmhand birthday but it was sent to the wrong player...");

            }
            if (e.FromModID == HappyBirthdayModCore.Instance.Helper.Multiplayer.ModID && e.Type == FSTRING_RequestBirthdayInfoFromServer)
                if (Game1.player.IsMainPlayer)
                {
                    KeyValuePair<long, string> message = e.ReadAs<KeyValuePair<long, string>>();
                    HappyBirthdayModCore.Instance.Monitor.Log("Got request from farmhand for birthday info" + Game1.getAllFarmhands().ToList().Find(i => i.UniqueMultiplayerID == message.Key).Name);
                    if (HappyBirthdayModCore.Instance.birthdayManager.othersBirthdays.ContainsKey(message.Key))
                    {
                        HappyBirthdayModCore.Instance.Monitor.Log("Sending requested farmhand info");
                        SendFarmandBirthdayInfoToPlayer(message.Key, HappyBirthdayModCore.Instance.birthdayManager.othersBirthdays[message.Key]);
                    }
                    else
                        HappyBirthdayModCore.Instance.Monitor.Log("For some reason requested birthday info was not found...");
                }
        }
    }
}
