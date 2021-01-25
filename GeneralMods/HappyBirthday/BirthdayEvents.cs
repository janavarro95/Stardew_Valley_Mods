using StardewValley;

namespace Omegasis.HappyBirthday
{
    // TODO: Make all the events
    // Resources:https://stardewvalleywiki.com/Modding:Event_data
    public class BirthdayEvents
    {
        public Event communityCenterJunimoEvent;
        public Event marriedNoKidsEvent;
        public Event surpriseBirthdayPartyEvent;
        public Event marriedWithOneKidEvent;
        public Event marriedWithTwoKidsEvent;

        public BirthdayEvents()
        {
            this.initializeEvents();
        }

        public void initializeEvents()
        {
            Event e = new Event("", -1, Game1.player);
            Game1.player.currentLocation.currentEvent = new Event();
        }
    }
}
