
using StardewValley;

namespace Omegasis.HappyBirthday.Framework
{
    /// <summary>Provides utility methods for displaying messages to the user.</summary>
    internal class Messages
    {
        /*********
        ** Public methods
        *********/
        /// <summary>Show a message to the user with a star icon.</summary>
        /// <param name="message">The message to display.</param>
        public static void ShowStarMessage(string message)
        {
            //IEnumerable<Farmer> players= Game1.getAllFarmers();

            Game1.addHUDMessage(new HUDMessage(message, 1));

            if (!message.Contains("Inventory"))
            {
                Game1.playSound("cancel");
                return;
            }
            if (!Game1.player.mailReceived.Contains("BackpackTip"))
            {
                Game1.player.mailReceived.Add("BackpackTip");
                Game1.addMailForTomorrow("pierreBackpack", false, false);
            }
        }

    }
}
