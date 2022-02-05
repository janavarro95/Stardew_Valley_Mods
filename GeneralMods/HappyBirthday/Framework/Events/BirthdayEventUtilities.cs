using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Omegasis.HappyBirthday.Framework.Constants;
using Omegasis.HappyBirthday.Framework.Events.EventPreconditions;
using Omegasis.HappyBirthday.Framework.Menus;
using StardewModdingAPI.Events;
using StardewValley;
using StardustCore.Events;

namespace Omegasis.HappyBirthday.Framework.Events
{
    public static class BirthdayEventUtilities
    {
        public static EventManager BirthdayEventManager;
        public static bool ShouldAskPlayerForBirthday;
        public static bool ShouldAskPlayerForFavoriteGift;

        public static void Player_Warped(object sender, WarpedEventArgs e)
        {
            if (e.NewLocation == Game1.getLocationFromName("CommunityCenter"))
                BirthdayEventManager.startEventAtLocationIfPossible(EventIds.JunimoCommunityCenterBirthday);
            if (e.NewLocation == Game1.getLocationFromName("Trailer"))
                BirthdayEventManager.startEventAtLocationIfPossible(EventIds.BirthdayDatingPennyTrailer);
            if (e.NewLocation == Game1.getLocationFromName("Trailer_Big"))
                BirthdayEventManager.startEventAtLocationIfPossible(EventIds.BirthdayDatingPennyHouse);

            if (e.NewLocation == Game1.getLocationFromName("ScienceHouse"))
            {
                BirthdayEventManager.startEventAtLocationIfPossible(EventIds.BirthdayDatingMaru);
                BirthdayEventManager.startEventAtLocationIfPossible(EventIds.BirthdayDatingSebastian);
            }
            if (e.NewLocation == Game1.getLocationFromName("LeahHouse"))
                BirthdayEventManager.startEventAtLocationIfPossible(EventIds.BirthdayDatingLeah);
            if (e.NewLocation == Game1.getLocationFromName("SeedShop"))
                BirthdayEventManager.startEventAtLocationIfPossible(EventIds.BirthdayDatingAbigailSeedShop);
            if (e.NewLocation == Game1.getLocationFromName("Mine"))
                BirthdayEventManager.startEventAtLocationIfPossible(EventIds.BirthdayDatingAbigailMines);
            if (e.NewLocation == Game1.getLocationFromName("HaleyHouse"))
            {
                BirthdayEventManager.startEventAtLocationIfPossible(EventIds.BirthdayDatingEmily);
                BirthdayEventManager.startEventAtLocationIfPossible(EventIds.BirthdayDatingHaley);
            }
            if (e.NewLocation == Game1.getLocationFromName("HarveyRoom"))
                BirthdayEventManager.startEventAtLocationIfPossible(EventIds.BirthdayDatingHarvey);
            if (e.NewLocation == Game1.getLocationFromName("ElliottHouse"))
                BirthdayEventManager.startEventAtLocationIfPossible(EventIds.BirthdayDatingElliott);
            if (e.NewLocation == Game1.getLocationFromName("SamHouse"))
                BirthdayEventManager.startEventAtLocationIfPossible(EventIds.BirthdayDatingSam);
            if (e.NewLocation == Game1.getLocationFromName("JoshHouse"))
                BirthdayEventManager.startEventAtLocationIfPossible(EventIds.BirthdayDatingAlex);
            if (e.NewLocation == Game1.getLocationFromName("AnimalShop"))
                BirthdayEventManager.startEventAtLocationIfPossible(EventIds.BirthdayDatingShane);

            if (e.NewLocation == Game1.getLocationFromName("Farm"))
                BirthdayEventManager.startEventAtLocationIfPossible(EventIds.AskPlayerForBirthday);

            if (e.NewLocation == Game1.getLocationFromName("JojaMart"))
            {
                BirthdayEventManager.startEventAtLocationIfPossible(EventIds.JojaMartBirthday);
            }

        }

        public static void ClearEventsFromFarmer()
        {
            foreach (KeyValuePair<string, EventHelper> v in BirthdayEventManager.events)
                BirthdayEventManager.clearEventFromFarmer(v.Key);
        }

        public static void InitializeBirthdayEvents()
        {
            //Dialogue commands.
            BirthdayEventManager.addCustomEventLogic("Omegasis.HappyBirthday.Events.ShowTranslatedMessage", BirthdayEventCommands.showTranslatedMessage);
            StardustCore.Compatibility.SpaceCore.SpaceCoreAPIUtil.RegisterCustomEventCommand("Omegasis.HappyBirthday.Events.ShowTranslatedMessage", BirthdayEventCommands.showTranslatedMessage);
            BirthdayEventManager.addCustomEventLogic("Omegasis.HappyBirthday.Events.SpeakWithBirthdayMessageIncluded", BirthdayEventCommands.speakWithBirthdayIncluded);
            StardustCore.Compatibility.SpaceCore.SpaceCoreAPIUtil.RegisterCustomEventCommand("Omegasis.HappyBirthday.Events.SpeakWithBirthdayMessageIncluded", BirthdayEventCommands.speakWithBirthdayIncluded);
            BirthdayEventManager.addCustomEventLogic("Omegasis.HappyBirthday.Events.SpeakWithTranslatedMessage", BirthdayEventCommands.speakWithTranslatedMessage);
            StardustCore.Compatibility.SpaceCore.SpaceCoreAPIUtil.RegisterCustomEventCommand("Omegasis.HappyBirthday.Events.SpeakWithTranslatedMessage", BirthdayEventCommands.speakWithTranslatedMessage);
            BirthdayEventManager.addCustomEventLogic("Omegasis.HappyBirthday.Events.SpeakIfTodayIsPlayersBirthday", BirthdayEventCommands.speakIfTodayIsPlayersBirthday);
            StardustCore.Compatibility.SpaceCore.SpaceCoreAPIUtil.RegisterCustomEventCommand("Omegasis.HappyBirthday.Events.SpeakIfTodayIsPlayersBirthday", BirthdayEventCommands.speakIfTodayIsPlayersBirthday);

            //Menu commands.
            BirthdayEventManager.addCustomEventLogic("Omegasis.HappyBirthday.Events.ShowBirthdaySelectionMenu", BirthdayEventCommands.setShouldShowChooseBirthdayMenu);
            StardustCore.Compatibility.SpaceCore.SpaceCoreAPIUtil.RegisterCustomEventCommand("Omegasis.HappyBirthday.Events.ShowBirthdaySelectionMenu", BirthdayEventCommands.setShouldShowChooseBirthdayMenu);
            BirthdayEventManager.addCustomEventLogic("Omegasis.HappyBirthday.Events.ShowFavoriteGiftSelectionMenu", BirthdayEventCommands.setShouldShowChooseFavoriteGiftMenu);
            StardustCore.Compatibility.SpaceCore.SpaceCoreAPIUtil.RegisterCustomEventCommand("Omegasis.HappyBirthday.Events.ShowFavoriteGiftSelectionMenu", BirthdayEventCommands.setShouldShowChooseFavoriteGiftMenu);

            //Utility Commands
            BirthdayEventManager.addCustomEventLogic("Omegasis.HappyBirthday.Events.SkipNextCommand", BirthdayEventCommands.skipNextCommand);
            StardustCore.Compatibility.SpaceCore.SpaceCoreAPIUtil.RegisterCustomEventCommand("Omegasis.HappyBirthday.Events.SkipNextCommand", BirthdayEventCommands.skipNextCommand);


            //Additional Preconditions
            BirthdayEventManager.eventPreconditionParsingMethods.Add(FarmerBirthdayPrecondition.EventPreconditionId, HappyBirthdayPreconditionParsingMethods.ParseFarmerBirthdayPrecondition);
            BirthdayEventManager.eventPreconditionParsingMethods.Add(SpouseBirthdayPrecondition.EventPreconditionId, HappyBirthdayPreconditionParsingMethods.ParseSpouseBirthdayPrecondition);
            BirthdayEventManager.eventPreconditionParsingMethods.Add(HasChosenBirthdayPrecondition.EventPreconditionId, HappyBirthdayPreconditionParsingMethods.ParseHasChosenBirthdayPrecondition);

            List<EventHelper> defaultBirthdayEvents = new List<EventHelper>()
            {
                //Villager/Npc birthday events.
                BirthdayEvents.CommunityCenterJunimoBirthday(),

                //Female dating candidate birthday events.
                BirthdayEvents.DatingBirthday_Penny(),
                BirthdayEvents.DatingBirthday_Penny_BigHome(),
                BirthdayEvents.DatingBirthday_Maru(),
                BirthdayEvents.DatingBirthday_Leah(),
                BirthdayEvents.DatingBirthday_Abigail_Seedshop(),
                BirthdayEvents.DatingBirthday_Abigail_Mine(),
                BirthdayEvents.DatingBirthday_Emily(),
                BirthdayEvents.DatingBirthday_Haley(),

                //Male dating candidate birthday events.
                BirthdayEvents.DatingBirthday_Harvey(),
                BirthdayEvents.DatingBirthday_Elliott(),
                BirthdayEvents.DatingBirthday_Sam(),
                BirthdayEvents.DatingBirthday_Alex(),
                BirthdayEvents.DatingBirthday_Shane(),
                BirthdayEvents.DatingBirthday_Sebastian(),

                //Set up birthday events
                BirthdayEvents.LewisAsksPlayerForBirthday(),

                BirthdayEvents.JojaBirthday()
        };
            string relativePath = Path.Combine("ModAssets", "Data", "Events");
            string abspath = Path.Combine(HappyBirthdayModCore.Instance.Helper.DirectoryPath, relativePath);
            if (!Directory.Exists(abspath))
                Directory.CreateDirectory(abspath);

            string[] files = Directory.GetFiles(abspath);
            foreach (string file in files)
            {
                EventHelper eventHelper = HappyBirthdayModCore.Instance.Helper.Data.ReadJsonFile<EventHelper>(Path.Combine(relativePath, Path.GetFileName(file)));
                eventHelper.parseEventPreconditionsFromPreconditionStrings(BirthdayEventManager);



                if (eventHelper == null)
                    continue;

                if (BirthdayEventManager.events.ContainsKey(eventHelper.eventStringId))
                    continue;
                else
                    BirthdayEventManager.addEvent(eventHelper);

            }

            foreach (EventHelper eventHelper in defaultBirthdayEvents)
                if (BirthdayEventManager.events.ContainsKey(eventHelper.eventStringId))
                    continue;
                else
                {
                    BirthdayEventManager.addEvent(eventHelper);
                    HappyBirthdayModCore.Instance.Helper.Data.WriteJsonFile(Path.Combine(relativePath, eventHelper.eventStringId + ".json"), eventHelper);

                }
        }

        public static void UpdateEventManager()
        {
            if (BirthdayEventManager != null)
                BirthdayEventManager.update();
        }




        /// <summary>
        /// Gets a string to be displayed during the event.
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static string GetEventString(string Key)
        {



            return HappyBirthdayModCore.Instance.translationInfo.getEventString(Key);
        }

    }
}
