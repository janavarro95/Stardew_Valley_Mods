using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Omegasis.HappyBirthday.Framework.Menus;
using Omegasis.StardustCore.Events;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Menus;

namespace Omegasis.HappyBirthday.Framework.Events
{
    public class BirthdayEventCommands
    {
        public static void showTranslatedMessage(Event Event, string[] EventData, StardewValley.EventContext eventContext=null)
        {
            string[] splits = EventData;

            StringBuilder stringBuilder= new StringBuilder();
            foreach(string s in splits)
            {
                stringBuilder.AppendLine(s);
            }

           // HappyBirthdayModCore.Instance.Monitor.Log("Data: " + stringBuilder.ToString());

            string translationKey = splits[1];
            List<string> eventCommands = Game1.CurrentEvent.eventCommands.ToList();
            EventHelper helper = new EventHelper();

           // HappyBirthdayModCore.Instance.Monitor.Log("Translation Key: " + translationKey);

            string translatedString = BirthdayEventUtilities.GetEventString(translationKey);
            // HappyBirthdayModCore.Instance.Monitor.Log("Event Message: " + translatedString);

            //helper.showMessage(translatedString);
            //eventCommands.Insert(Game1.CurrentEvent.CurrentCommand + 1, "message "+translatedString);

            if (Game1.activeClickableMenu == null)
            {
                Game1.drawObjectDialogue(translatedString);
            }
            //Event.eventCommands = eventCommands.ToArray();
            //Event.CurrentCommand++;
        }

        public static void speakWithBirthdayIncluded(Event Event, string[] EventData, StardewValley.EventContext eventContext = null)
        {
            string speakerName = EventData[1];
            //final message should be changed to string.format(GetEventString(),BirthdaySeason,BirthdayDay);
            string finalMessage = string.Format(BirthdayEventUtilities.GetEventString(EventData[2]), HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData.BirthdaySeason,
                    HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData.BirthdayDay);
            List<string> eventCommands = Game1.CurrentEvent.eventCommands.ToList();
            EventHelper helper = new EventHelper();
            helper.speak(speakerName, finalMessage);
            eventCommands.Insert(Game1.CurrentEvent.CurrentCommand + 1, helper.EventData);
            Event.eventCommands = eventCommands.ToArray();
            Event.CurrentCommand++;

        }


        public static void speakWithTranslatedMessage(Event Event, string[] EventData, StardewValley.EventContext eventContext = null)
        {
            string speakerName = EventData[1];
            string finalMessage = BirthdayEventUtilities.GetEventString(EventData[2]);
            List<string> eventCommands = Game1.CurrentEvent.eventCommands.ToList();
            EventHelper helper = new EventHelper();
            helper.speak(speakerName, finalMessage);
            eventCommands.Insert(Game1.CurrentEvent.CurrentCommand + 1, helper.EventData);
            Event.eventCommands = eventCommands.ToArray();
            Event.CurrentCommand++;

        }

        /// <summary>
        /// Speaks with a specific dialogue line loaded from a specific dialogue file.
        /// </summary>
        /// <param name="Event"></param>
        /// <param name="EventData"></param>
        public static void speakWithTranslatedMessageFromFile(Event Event, string[] EventData, StardewValley.EventContext eventContext = null)
        {
            string fileName = EventData[1];
            string speakerName = EventData[2];
            string dialogueKey = EventData[3];

            string finalMessage = BirthdayEventUtilities.GetEventString(dialogueKey);
            List<string> eventCommands = Game1.CurrentEvent.eventCommands.ToList();
            EventHelper helper = new EventHelper();
            helper.speak(speakerName, finalMessage);
            eventCommands.Insert(Game1.CurrentEvent.CurrentCommand + 1, helper.EventData);
            Event.eventCommands = eventCommands.ToArray();
            Event.CurrentCommand++;

        }

        public static void setShouldShowChooseBirthdayMenu(Event gameEvent = null, string[] EventData = null, EventContext eventContext = null)
        {
            BirthdayEventUtilities.ShouldAskPlayerForBirthday = true;

            OpenBirthdaySelectionMenu();
        }

        public static void OpenBirthdaySelectionMenu(Event gameEvent = null, string[] EventData = null, EventContext eventContext = null)
        {

            if (!HappyBirthdayModCore.Instance.birthdayManager.hasChosenBirthday() && Game1.activeClickableMenu == null && BirthdayEventUtilities.ShouldAskPlayerForBirthday)
                if (HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData != null)
                {
                    Game1.activeClickableMenu = new BirthdayMenu(HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData.BirthdaySeason, HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData.BirthdayDay, HappyBirthdayModCore.Instance.birthdayManager.setBirthday);
                    HappyBirthdayModCore.Instance.birthdayManager.setCheckedForBirthday(false);
                }
                else
                {
                    HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData = new PlayerData();
                    Game1.activeClickableMenu = new BirthdayMenu("", 0, HappyBirthdayModCore.Instance.birthdayManager.setBirthday);
                    HappyBirthdayModCore.Instance.birthdayManager.setCheckedForBirthday(false);
                }
        }

        public static void OpenGiftSelectionMenu()
        {
            if (Game1.activeClickableMenu == null && HappyBirthdayModCore.Instance.birthdayManager.hasChoosenFavoriteGift() == false && BirthdayEventUtilities.ShouldAskPlayerForFavoriteGift)
            {
                Game1.activeClickableMenu = new FavoriteGiftMenu();
                HappyBirthdayModCore.Instance.birthdayManager.setCheckedForBirthday(false);
                return;
            }
        }


        public static void setShouldShowChooseFavoriteGiftMenu(Event gameEvent = null, string[] EventData = null, EventContext eventContext = null)
        {
            BirthdayEventUtilities.ShouldAskPlayerForFavoriteGift = true;
            OpenGiftSelectionMenu();
        }

        /// <summary>
        /// Speak a given dialogue if it is the player's birthday or not.
        /// </summary>
        /// <param name="CurrentEvent"></param>
        public static void speakIfTodayIsPlayersBirthday(Event CurrentEvent, string[] args, StardewValley.EventContext eventContext=null)
        {
            if (HappyBirthdayModCore.Instance.birthdayManager.isBirthday())
                CurrentEvent.CurrentCommand++;
            else
                CurrentEvent.CurrentCommand += 3;
        }

        /// <summary>
        /// Progresses the game's current event command by 1
        /// </summary>
        /// <param name="Event"></param>
        public static void nextCommand(Event Event)
        {
            Event.CurrentCommand++;
        }

        /// <summary>
        /// Skips the next event command for the game. Necessary for branching.
        /// </summary>
        /// <param name="Event"></param>
        public static void skipNextCommand(Event CurrentEvent, string[] args=null, StardewValley.EventContext eventContext = null)
        {
            CurrentEvent.CurrentCommand += 2;
        }

        public static void givePlayerFavoriteGift(Event gameEvent = null, string[] EventData = null, EventContext eventContext = null)
        {
            Item gift = HappyBirthdayModCore.Instance.giftManager.getSpouseBirthdayGift(Game1.player.spouse);
            Game1.player.addItemByMenuIfNecessary(gift);
            gameEvent.CurrentCommand++;
        }

        public static void makeObjectsTemporarilyInvisible(Event Event, string[] data, EventContext eventContext = null)
        {

            List<Vector2> tilePositions = new List<Vector2>();
            for(int i = 1; i < data.Length; i += 2)
            {
                if (string.IsNullOrEmpty(data[i]))
                {
                    continue;
                }
                int x = Convert.ToInt32(data[i]);
                int y = Convert.ToInt32(data[i + 1]);

                Vector2 tilePos = new Vector2(x, y);
                tilePositions.Add(tilePos);
            }

            foreach(StardewValley.Object obj in Game1.currentLocation.Objects.Values)
            {
                if (tilePositions.Contains(obj.TileLocation))
                {
                    obj.isTemporarilyInvisible = true;
                }

            }
            foreach (StardewValley.Objects.Furniture obj in Game1.currentLocation.furniture)
            {
                if (tilePositions.Contains(obj.TileLocation))
                {
                    obj.isTemporarilyInvisible = true;
                }
            }

            Event.CurrentCommand++;

        }

        public static void setUpSpouseAndFarmerToCorrectFarmhouseStandingLocation(Event Event, string[] data, EventContext eventContext = null)
        {
            if (eventContext.Location is FarmHouse)
            {
                Point spouseStandingSpot = (eventContext.Location as FarmHouse).getKitchenStandingSpot();
                foreach (NPC npc in Event.actors)
                {
                    if (npc.Name == Game1.player.getSpouse().Name)
                    {
                        npc.setTilePosition(spouseStandingSpot);
                        Game1.viewport.X = spouseStandingSpot.X * 64 + 32 - Game1.viewport.Width / 2;
                        Game1.viewport.Y = spouseStandingSpot.Y * 64 + 32 - Game1.viewport.Height / 2;
                    }
                }
                Event.farmer.setTileLocation(new Vector2(spouseStandingSpot.X + 3, spouseStandingSpot.Y));
                Event.CurrentCommand++;
            }
        }

        public static void setSpouseFacingDirection(Event Event, string[] data, EventContext eventContext = null)
        {
            int facingDirection = Convert.ToInt32(data[1]);
            foreach (NPC npc in Event.actors)
            {
                if (npc.Name == Game1.player.getSpouse().Name)
                {
                    npc.faceDirection(facingDirection);
                }
            }
            Event.currentCommand++;
        }

    }
}
