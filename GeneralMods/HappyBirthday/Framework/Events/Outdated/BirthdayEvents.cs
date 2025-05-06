using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;
using Microsoft.Xna.Framework;
using Omegasis.HappyBirthday.Framework.Constants;
using Omegasis.StardustCore.Events.Preconditions.TimeSpecific;
using Omegasis.StardustCore.Events.Preconditions;
using Omegasis.StardustCore.Events.Preconditions.PlayerSpecific;
using Omegasis.StardustCore.Events.Preconditions.NPCSpecific;
using Omegasis.HappyBirthday.Framework.Events.EventPreconditions;
using Omegasis.StardustCore.Events;
using Omegasis.StardustCore.IlluminateFramework;
using Omegasis.StardustCore.Utilities;
using Omegasis.HappyBirthday.Framework.Events.Compatibility;
using Omegasis.HappyBirthday.Framework.Utilities;

namespace Omegasis.HappyBirthday.Framework.Events.Outdated
{
    public class BirthdayEvents
    {

        /// <summary>
        /// Creates the junimo birthday party event.
        /// </summary>
        /// <returns></returns>
        public static HappyBirthdayEventHelper CommunityCenterJunimoBirthday()
        {
            List<EventPrecondition> conditions = new List<EventPrecondition>();
            conditions.Add(new FarmerBirthdayPrecondition());
            conditions.Add(new GameLocationPrecondition(Game1.getLocationFromName("CommunityCenter")));
            conditions.Add(new TimeOfDayPrecondition(600, 2600));


            conditions.Add(new CanReadJunimoEventPrecondition());
            conditions.Add(new IsJojaMemberEventPrecondition(false));


            //conditions.Add(new HasUnlockedCommunityCenter()); //Infered by the fact that you must enter the community center to trigger this event anyways.
            HappyBirthdayEventHelper e = new HappyBirthdayEventHelper(EventIds.JunimoCommunityCenterBirthday, "Omegasis.HappyBirthday.JunimosCommunityCenterBirthdayParty", 2, conditions, new EventStartData("playful", 32, 12, new EventStartData.FarmerData(32, 22, EventHelper.FacingDirection.Up), new List<EventStartData.NPCData>()));

            e.AddInJunimoActor("Juni", new Vector2(32, 10), Colors.getRandomJunimoColor());
            e.AddInJunimoActor("Juni2", new Vector2(30, 11), Colors.getRandomJunimoColor());
            e.AddInJunimoActor("Juni3", new Vector2(34, 11), Colors.getRandomJunimoColor());
            e.AddInJunimoActor("Juni4", new Vector2(26, 11), Colors.getRandomJunimoColor());
            e.AddInJunimoActor("Juni5", new Vector2(28, 11), Colors.getRandomJunimoColor());
            e.AddInJunimoActor("Juni6Tank", new Vector2(38, 10), Colors.getRandomJunimoColor());
            e.AddInJunimoActor("Juni7", new Vector2(27, 16), Colors.getRandomJunimoColor());
            e.AddInJunimoActor("Juni8", new Vector2(40, 15), Colors.getRandomJunimoColor());
            e.AddJunimoAdvanceMoveTiles(new JunimoAdvanceMoveData("Juni6Tank", new List<Point>()
            {
                new Point(38,10),
                new Point(38,11),
                new Point(39,11),
                new Point(40,11),
                new Point(41,11),
                new Point(42,11),
                new Point(42,10),
                new Point(41,10),
                new Point(40,10),
                new Point(39,10),

            }, 60, 1, true)); ;

            e.FlipJunimoActor("Juni5", true);
            e.junimoFaceDirection("Juni4", EventHelper.FacingDirection.Right); //Make a junimo face right.
            e.junimoFaceDirection("Juni5", EventHelper.FacingDirection.Left);
            e.junimoFaceDirection("Juni7", EventHelper.FacingDirection.Down);
            e.animate("Juni", true, true, 250, new List<int>()
            {
                28,
                29,
                30,
                31
            });
            e.animate("Juni7", false, true, 250, new List<int>()
            {
                44,45,46,47
            });
            e.animate("Juni8", false, true, 250, new List<int>()
            {
                12,13,14,15
            });

            e.globalFadeIn();

            e.moveFarmerUp(10, EventHelper.FacingDirection.Up, true);

            e.junimoFaceDirection("Juni4", EventHelper.FacingDirection.Down);
            e.junimoFaceDirection("Juni5", EventHelper.FacingDirection.Down);
            e.RemoveJunimoAdvanceMove("Juni6Tank");
            e.junimoFaceDirection("Juni6Tank", EventHelper.FacingDirection.Down);
            e.junimoFaceDirection("Juni7", EventHelper.FacingDirection.Right);
            e.FlipJunimoActor("Juni8", true);
            e.junimoFaceDirection("Juni8", EventHelper.FacingDirection.Left);

            e.playSound("junimoMeep1");

            e.emoteFarmer_ExclamationMark();
            e.addTranslatedMessageToBeShown("JunimoBirthdayParty_0");
            e.emoteFarmer_Heart();
            e.globalFadeOut(0.010);
            e.setViewportPosition(-100, -100);
            e.addTranslatedMessageToBeShown("JunimoBirthdayParty_1");
            e.addTranslatedMessageToBeShown("PartyOver");
            e.addObjectToPlayersInventory(220, 1, false);

            e.end();

            return e;
        }


        /// <summary>
        /// Birthday event for when the player is dating Penny.
        /// Status: Completed.
        /// </summary>
        /// <returns></returns>
        public static HappyBirthdayEventHelper DatingBirthday_Penny()
        {

            NPC penny = Game1.getCharacterFromName("Penny");
            NPC pam = Game1.getCharacterFromName("Pam");

            List<EventPrecondition> conditions = new List<EventPrecondition>();
            conditions.Add(new FarmerBirthdayPrecondition());
            conditions.Add(new GameLocationPrecondition(Game1.getLocationFromName("Trailer")));
            conditions.Add(new TimeOfDayPrecondition(600, 2600));
            conditions.Add(new DatingNPCEventPrecondition(penny));
            conditions.Add(new IsStardewValleyExpandedInstalledPrecondition(false));

            //conditions.Add(new StardustCore.Events.Preconditions.NPCSpecific.DatingNPC(Game1.getCharacterFromName("Penny"));
            HappyBirthdayEventHelper e = new HappyBirthdayEventHelper(EventIds.BirthdayDatingPennyTrailer, "Omegasis.HappyBirthday.DatingPennyBirthdayParty", 2, conditions, new EventStartData("playful", 12, 8, new EventStartData.FarmerData(12, 9, EventHelper.FacingDirection.Up), new List<EventStartData.NPCData>() {
                new EventStartData.NPCData(penny,12,7, EventHelper.FacingDirection.Up),
                new EventStartData.NPCData(pam,15,4, EventHelper.FacingDirection.Down)
            }));

            e.globalFadeIn();

            e.moveFarmerUp(1, EventHelper.FacingDirection.Up, false);

            e.actorFaceDirection("Penny", EventHelper.FacingDirection.Down);

            //starting = starting.Replace("@", Game1.player.Name);
            e.speakWithTranslatedMessage(penny, "DatingPennyBirthday_Penny:0");
            e.speakWithTranslatedMessage(pam, "DatingPennyBirthday_Pam:0");
            e.speakWithTranslatedMessage(penny, "DatingPennyBirthday_Penny:1");
            e.speakWithTranslatedMessage(pam, "DatingPennyBirthday_Pam:1");
            e.emote_Angry("Penny");
            e.speakWithTranslatedMessage(penny, "DatingPennyBirthday_Penny:2"); //penny2
            e.speakWithTranslatedMessage(penny, "DatingPennyBirthday_Penny:3"); //penny3

            e.moveActorLeft("Penny", 3, EventHelper.FacingDirection.Up, true);
            e.moveFarmerRight(2, EventHelper.FacingDirection.Up, false);
            e.moveFarmerUp(3, EventHelper.FacingDirection.Down, false);
            e.moveActorRight("Penny", 5, EventHelper.FacingDirection.Up, true);
            e.moveActorUp("Penny", 1, EventHelper.FacingDirection.Up, true);
            e.speakWithTranslatedMessage(pam, "DatingPennyBirthday_Pam:2"); //pam2
            e.speakWithTranslatedMessage(penny, "DatingPennyBirthday_Penny:4");//penny4

            e.emoteFarmer_Heart();
            e.emote_Heart("Penny");
            e.globalFadeOut(0.010);
            e.setViewportPosition(-100, -100);
            e.addTranslatedMessageToBeShown("DatingPennyBirthday_Finish:0"); //penny party finish 0
            e.addTranslatedMessageToBeShown("DatingPennyBirthday_Finish:1");// penny party finish 1
            e.addObjectToPlayersInventory(220, 1, false);
            e.addObjectToPlayersInventory(346, 1, false);

            e.addTranslatedMessageToBeShown("PartyOver");

            e.end();

            return e;
        }

        public static HappyBirthdayEventHelper DatingBirthday_Penny_BigHome()
        {

            NPC penny = Game1.getCharacterFromName("Penny");
            NPC pam = Game1.getCharacterFromName("Pam");

            List<EventPrecondition> conditions = new List<EventPrecondition>();
            conditions.Add(new FarmerBirthdayPrecondition());
            conditions.Add(new GameLocationPrecondition(Game1.getLocationFromName("Trailer_Big")));
            conditions.Add(new TimeOfDayPrecondition(600, 2600));
            conditions.Add(new DatingNPCEventPrecondition(penny));

            //conditions.Add(new StardustCore.Events.Preconditions.NPCSpecific.DatingNPC(Game1.getCharacterFromName("Penny"));
            HappyBirthdayEventHelper e = new HappyBirthdayEventHelper(EventIds.BirthdayDatingPennyHouse, "Omegasis.HappyBirthday.DatingPennyBirthdayParty_BigHome", 2, conditions, new EventStartData("playful", 14, 8, new EventStartData.FarmerData(12, 11, EventHelper.FacingDirection.Up), new List<EventStartData.NPCData>() {
                new EventStartData.NPCData(penny,12,7, EventHelper.FacingDirection.Up),
                new EventStartData.NPCData(pam,15,4, EventHelper.FacingDirection.Down)
            }));

            e.globalFadeIn();

            e.moveFarmerUp(3, EventHelper.FacingDirection.Up, false);

            e.actorFaceDirection("Penny", EventHelper.FacingDirection.Down);
            //starting = starting.Replace("@", Game1.player.Name);
            e.speakWithTranslatedMessage(penny, "DatingPennyBirthday_Penny:0");
            e.speakWithTranslatedMessage(pam, "DatingPennyBirthday_Pam:0");
            e.speakWithTranslatedMessage(penny, "DatingPennyBirthday_Penny:1");
            e.speakWithTranslatedMessage(pam, "DatingPennyBirthday_Pam:1");
            e.emote_Angry("Penny");
            e.speakWithTranslatedMessage(penny, "DatingPennyBirthday_Penny:2"); //penny2
            e.speakWithTranslatedMessage(penny, "DatingPennyBirthday_Penny:3"); //penny3

            e.moveActorLeft("Penny", 3, EventHelper.FacingDirection.Up, true);
            e.moveFarmerRight(2, EventHelper.FacingDirection.Up, false);
            e.moveFarmerUp(3, EventHelper.FacingDirection.Down, false);
            e.moveActorRight("Penny", 5, EventHelper.FacingDirection.Up, true);
            e.moveActorUp("Penny", 1, EventHelper.FacingDirection.Up, true);
            e.speakWithTranslatedMessage(pam, "DatingPennyBirthday_Pam:2"); //pam2
            e.speakWithTranslatedMessage(penny, "DatingPennyBirthday_Penny:4");//penny4

            e.emoteFarmer_Heart();
            e.emote_Heart("Penny");
            e.globalFadeOut(0.010);
            e.setViewportPosition(-100, -100);
            e.addTranslatedMessageToBeShown("DatingPennyBirthday_Finish:0"); //penny party finish 0
            e.addTranslatedMessageToBeShown("DatingPennyBirthday_Finish:1");// penny party finish 1
            e.addObjectToPlayersInventory(220, 1, false);
            e.addObjectToPlayersInventory(346, 1, false);

            e.addTranslatedMessageToBeShown("PartyOver");

            e.end();

            return e;
        }

        /// <summary>
        /// Birthday event for when the player is dating Maru.
        /// Finished.
        /// </summary>
        /// <returns></returns>
        public static HappyBirthdayEventHelper DatingBirthday_Maru()
        {
            List<EventPrecondition> conditions = new List<EventPrecondition>();
            conditions.Add(new FarmerBirthdayPrecondition());
            conditions.Add(new GameLocationPrecondition(Game1.getLocationFromName("ScienceHouse")));
            conditions.Add(new TimeOfDayPrecondition(600, 2600));

            NPC maru = Game1.getCharacterFromName("Maru");
            NPC sebastian = Game1.getCharacterFromName("Sebastian");
            NPC robin = Game1.getCharacterFromName("Robin");
            NPC demetrius = Game1.getCharacterFromName("Demetrius");

            conditions.Add(new DatingNPCEventPrecondition(maru));

            HappyBirthdayEventHelper e = new HappyBirthdayEventHelper(EventIds.BirthdayDatingMaru, "Omegasis.HappyBirthday.DatingMaruBirthdayParty", 2, conditions, new EventStartData("playful", 28, 12, new EventStartData.FarmerData(23, 12, EventHelper.FacingDirection.Right), new List<EventStartData.NPCData>() {
                new EventStartData.NPCData(maru,27,11, EventHelper.FacingDirection.Down),
                new EventStartData.NPCData(sebastian,26,13, EventHelper.FacingDirection.Up),
                new EventStartData.NPCData(robin,28,9, EventHelper.FacingDirection.Up),
                new EventStartData.NPCData(demetrius,30,11, EventHelper.FacingDirection.Left)
            }));
            e.globalFadeIn();

            e.moveFarmerRight(3, EventHelper.FacingDirection.Right, true);
            e.npcFaceDirection(maru, EventHelper.FacingDirection.Left);
            e.npcFaceDirection(demetrius, EventHelper.FacingDirection.Left);
            //Seb is already facing up.
            e.npcFaceDirection(robin, EventHelper.FacingDirection.Down);

            //Dialogue goes here.
            //Seriously improve dialogue lines. Maru is probably the NPC I know the least about.
            e.speakWithTranslatedMessage(maru, "DatingMaruBirthday_Maru:0"); //maru 0
            e.speakWithTranslatedMessage(demetrius, "DatingMaruBirthday_Demetrius:0"); //demetrius 0
            e.speakWithTranslatedMessage(maru, "DatingMaruBirthday_Maru:1");//Maru 1 //Spoiler she doesn't.
            e.speakWithTranslatedMessage(sebastian, "DatingMaruBirthday_Sebastian:0"); //sebastian 0
            e.speakWithTranslatedMessage(robin, "DatingMaruBirthday_Robin:0"); //robin 0
            e.speakWithTranslatedMessage(demetrius, "DatingMaruBirthday_Demetrius:1"); //demetrius 1
            e.emote_ExclamationMark("Robin");
            e.npcFaceDirection(robin, EventHelper.FacingDirection.Up);
            e.speakWithTranslatedMessage(robin, "DatingMaruBirthday_Robin:1"); //robin 1
            e.npcFaceDirection(robin, EventHelper.FacingDirection.Down);
            e.moveActorDown("Robin", 1, EventHelper.FacingDirection.Down, false);
            e.addObject(27, 12, 220);

            e.speakWithTranslatedMessage(maru, "DatingMaruBirthday_Maru:2"); //maru 2
            e.emoteFarmer_Thinking();
            e.speakWithTranslatedMessage(sebastian, "DatingMaruBirthday_Sebastian:1"); //Sebastian 1
            e.speakWithTranslatedMessage(maru, "DatingMaruBirthday_Maru:3"); //maru 3

            //Event finish commands.
            e.emoteFarmer_Heart();
            e.emote_Heart("Maru");
            e.globalFadeOut(0.010);
            e.setViewportPosition(-100, -100);
            e.addTranslatedMessageToBeShown("DatingMaruBirthday_Finish:0"); //maru party finish 0
            e.addTranslatedMessageToBeShown("DatingMaruBirthday_Finish:1"); //maru party finish 0
            e.addObjectToPlayersInventory(220, 1, false);

            e.addTranslatedMessageToBeShown("PartyOver");
            e.end();
            return e;
        }

        /// <summary>
        /// Birthday event for when the player is dating Leah.
        /// Finished.
        /// </summary>
        /// <returns></returns>
        public static HappyBirthdayEventHelper DatingBirthday_Leah()
        {
            NPC leah = Game1.getCharacterFromName("Leah");

            List<EventPrecondition> conditions = new List<EventPrecondition>();
            conditions.Add(new FarmerBirthdayPrecondition());
            conditions.Add(new GameLocationPrecondition(Game1.getLocationFromName("LeahHouse")));
            conditions.Add(new TimeOfDayPrecondition(600, 2600));
            conditions.Add(new DatingNPCEventPrecondition(leah));
            conditions.Add(new IsStardewValleyExpandedInstalledPrecondition(false));

            HappyBirthdayEventHelper e = new HappyBirthdayEventHelper(EventIds.BirthdayDatingLeah, "Omegasis.HappyBirthday.DatingLeahBirthdayParty", 2, conditions, new EventStartData("playful", 12, 7, new EventStartData.FarmerData(7, 9, EventHelper.FacingDirection.Up), new List<EventStartData.NPCData>() {
                new EventStartData.NPCData(leah,14,11, EventHelper.FacingDirection.Left),
            }));
            e.addObject(11, 11, 220);
            e.globalFadeIn();
            e.moveFarmerUp(2, EventHelper.FacingDirection.Up, false);
            e.moveFarmerRight(5, EventHelper.FacingDirection.Down, false);
            e.npcFaceDirection(leah, EventHelper.FacingDirection.Up);
            e.speakWithTranslatedMessage(leah, "DatingLeahBirthday_Leah:0"); //0
            e.moveFarmerDown(2, EventHelper.FacingDirection.Down, false);
            e.moveFarmerRight(1, EventHelper.FacingDirection.Down, false);
            e.moveFarmerDown(1, EventHelper.FacingDirection.Down, false);
            e.speakWithTranslatedMessage(leah, "DatingLeahBirthday_Leah:1"); //1
            e.emoteFarmer_Happy();
            e.speakWithTranslatedMessage(leah, "DatingLeahBirthday_Leah:2");//2
            e.speakWithTranslatedMessage(leah, "DatingLeahBirthday_Leah:3");//3
            e.speakWithTranslatedMessage(leah, "DatingLeahBirthday_Leah:4");//4


            e.emoteFarmer_Heart();
            e.emote_Heart("Leah");
            e.globalFadeOut(0.010);
            e.setViewportPosition(-100, -100);
            e.addTranslatedMessageToBeShown("DatingLeahBirthday_Finish:0"); //maru party finish 0
            e.addTranslatedMessageToBeShown("DatingLeahBirthday_Finish:1"); //maru party finish 0
            e.addObjectToPlayersInventory(220, 1, false);
            e.addTranslatedMessageToBeShown("PartyOver");
            e.end();
            return e;
        }



        /// <summary>
        /// Birthday event for when the player is dating Abigail.
        /// Finished.
        /// </summary>
        /// <returns></returns>
        public static HappyBirthdayEventHelper DatingBirthday_Abigail_Seedshop()
        {
            List<EventPrecondition> conditions = new List<EventPrecondition>();
            conditions.Add(new FarmerBirthdayPrecondition());
            conditions.Add(new GameLocationPrecondition(Game1.getLocationFromName("SeedShop")));
            conditions.Add(new TimeOfDayPrecondition(600, 2600));

            if (Game1.player.hasCompletedCommunityCenter() == false)
                conditions.Add(new DayOfWeekPrecondition(true, true, true, false, true, true, true));

            NPC abigail = Game1.getCharacterFromName("Abigail");
            NPC pierre = Game1.getCharacterFromName("Pierre");
            NPC caroline = Game1.getCharacterFromName("Caroline");

            conditions.Add(new DatingNPCEventPrecondition(abigail));

            HappyBirthdayEventHelper e = new HappyBirthdayEventHelper(EventIds.BirthdayDatingAbigailSeedShop, "Omegasis.HappyBirthday.DatingAbigailBirthdayParty_SeedShop", 2, conditions, new EventStartData("playful", 35, 7, new EventStartData.FarmerData(31, 11, EventHelper.FacingDirection.Up), new List<EventStartData.NPCData>() {
                new EventStartData.NPCData(abigail,36,9, EventHelper.FacingDirection.Left),
                new EventStartData.NPCData(pierre,33,6, EventHelper.FacingDirection.Down),
                new EventStartData.NPCData(caroline,35,5, EventHelper.FacingDirection.Up),
            }));
            e.globalFadeIn();

            //Dialogue here.
            e.moveFarmerUp(2, EventHelper.FacingDirection.Right, false);
            e.moveFarmerRight(4, EventHelper.FacingDirection.Right, false);

            e.speakWithTranslatedMessage(abigail, "DatingAbigailBirthday_Abigail:0"); //abi 0

            e.npcFaceDirection(caroline, EventHelper.FacingDirection.Down);

            e.speakWithTranslatedMessage(pierre, "DatingAbigailBirthday_Pierre:0"); //pie 0
            e.speakWithTranslatedMessage(caroline, "DatingAbigailBirthday_Caroline:0"); //car 0
            e.addObject(35, 5, 220);
            e.speakWithTranslatedMessage(abigail, "DatingAbigailBirthday_Abigail:1"); //abi 1
            e.speakWithTranslatedMessage(pierre, "DatingAbigailBirthday_Pierre:1"); //pie 1
            e.speakWithTranslatedMessage(caroline, "DatingAbigailBirthday_Caroline:1"); //car 1
            e.speakWithTranslatedMessage(caroline, "DatingAbigailBirthday_Caroline:2"); //car 2
            e.speakWithTranslatedMessage(abigail, "DatingAbigailBirthday_Abigail:2"); //abi 2
            e.emoteFarmer_Thinking();
            e.speakWithTranslatedMessage(abigail, "DatingAbigailBirthday_Abigail:3");//abi 3
            e.speakWithTranslatedMessage(abigail, "DatingAbigailBirthday_Abigail:4");///abi 4

            e.emoteFarmer_Heart();
            e.emote_Heart("Abigail");
            e.globalFadeOut(0.010);
            e.setViewportPosition(-100, -100);
            e.addTranslatedMessageToBeShown("DatingAbigailBirthday_Finish:0"); //abi party finish 0
            e.addTranslatedMessageToBeShown("DatingAbigailBirthday_Finish:1"); //abi party finish 0
            e.addObjectToPlayersInventory(220, 1, false);
            e.addTranslatedMessageToBeShown("PartyOver");
            e.end();
            return e;

        }


        public static HappyBirthdayEventHelper DatingBirthday_Abigail_Mine()
        {
            List<EventPrecondition> conditions = new List<EventPrecondition>();
            conditions.Add(new FarmerBirthdayPrecondition());
            conditions.Add(new GameLocationPrecondition(Game1.getLocationFromName("Mine")));
            conditions.Add(new TimeOfDayPrecondition(600, 2600));

            var v = new IsJojaMemberEventPrecondition(true);
            if (v.meetsCondition())
                conditions.Add(new DayOfWeekPrecondition(false, false, false, true, false, false, false));
            else
                if (Game1.player.hasCompletedCommunityCenter() == false)
                conditions.Add(new DayOfWeekPrecondition(false, false, false, true, false, false, false));

            NPC abigail = Game1.getCharacterFromName("Abigail");

            conditions.Add(new DatingNPCEventPrecondition(abigail));

            HappyBirthdayEventHelper e = new HappyBirthdayEventHelper(EventIds.BirthdayDatingAbigailMines, "Omegasis.HappyBirthday.DatingAbigailBirthdayParty_Mines", 2, conditions, new EventStartData("playful", 18, 8, new EventStartData.FarmerData(18, 12, EventHelper.FacingDirection.Up), new List<EventStartData.NPCData>() {
                new EventStartData.NPCData(abigail,18,4, EventHelper.FacingDirection.Down),
            }));
            e.globalFadeIn();

            //Dialogue here.
            e.moveFarmerUp(7, EventHelper.FacingDirection.Up, false);

            e.speakWithTranslatedMessage(abigail, "DatingAbigailBirthday_Mine_Abigail:0"); //abi 0

            e.speakWithTranslatedMessage(abigail, "DatingAbigailBirthday_Mine_Abigail:1"); //abi 1
            e.emoteFarmer_QuestionMark();
            e.speakWithTranslatedMessage(abigail, "DatingAbigailBirthday_Mine_Abigail:2"); //abi 2
            e.speakWithTranslatedMessage(abigail, "DatingAbigailBirthday_Mine_Abigail:3");//abi 3
            e.emoteFarmer_Thinking();
            e.speakWithTranslatedMessage(abigail, "DatingAbigailBirthday_Mine_Abigail:4");///abi 4

            e.emoteFarmer_Heart();
            e.emote_Heart("Abigail");
            e.globalFadeOut(0.010);
            e.setViewportPosition(-100, -100);
            e.addTranslatedMessageToBeShown("DatingAbigailBirthday_Mine_Finish:0"); //abi party finish 0
            e.addTranslatedMessageToBeShown("DatingAbigailBirthday_Mine_Finish:1"); //abi party finish 0
            e.addObjectToPlayersInventory(220, 1, false);
            e.addTranslatedMessageToBeShown("PartyOver");
            e.end();
            return e;

        }

        public static HappyBirthdayEventHelper DatingBirthday_Emily()
        {
            List<EventPrecondition> conditions = new List<EventPrecondition>();
            conditions.Add(new FarmerBirthdayPrecondition());
            conditions.Add(new GameLocationPrecondition(Game1.getLocationFromName("HaleyHouse")));
            conditions.Add(new TimeOfDayPrecondition(600, 2600));

            NPC emily = Game1.getCharacterFromName("Emily");

            conditions.Add(new DatingNPCEventPrecondition(emily));

            HappyBirthdayEventHelper e = new HappyBirthdayEventHelper(EventIds.BirthdayDatingEmily, "Omegasis.HappyBirthday.DatingEmilyBirthdayParty", 2, conditions, new EventStartData("playful", 20, 18, new EventStartData.FarmerData(11, 20, EventHelper.FacingDirection.Right), new List<EventStartData.NPCData>() {
                new EventStartData.NPCData(emily,20,17, EventHelper.FacingDirection.Down),
            }));
            e.globalFadeIn();

            //Dialogue here.
            e.moveFarmerRight(9, EventHelper.FacingDirection.Up, false);

            e.speakWithTranslatedMessage(emily, "DatingEmilyBirthday_Emily:0"); //emi 0
            e.speakWithTranslatedMessage(emily, "DatingEmilyBirthday_Emily:1"); //emi 0
            e.emoteFarmer_Happy();
            e.speakWithTranslatedMessage(emily, "DatingEmilyBirthday_Emily:2"); //emi 0
            e.speakWithTranslatedMessage(emily, "DatingEmilyBirthday_Emily:3"); //emi 0
            e.speakWithTranslatedMessage(emily, "DatingEmilyBirthday_Emily:4"); //emi 0
            e.emoteFarmer_Thinking();
            e.speakWithTranslatedMessage(emily, "DatingEmilyBirthday_Emily:5"); //emi 0


            e.emoteFarmer_Heart();
            e.emote_Heart("Emily");
            e.globalFadeOut(0.010);
            e.setViewportPosition(-100, -100);
            e.addTranslatedMessageToBeShown("DatingEmilyBirthday_Finish:0"); //abi party finish 0
            e.addTranslatedMessageToBeShown("DatingEmilyBirthday_Finish:1"); //abi party finish 0
            e.addObjectToPlayersInventory(220, 1, false);
            e.addTranslatedMessageToBeShown("PartyOver");
            e.end();
            return e;
        }


        public static HappyBirthdayEventHelper DatingBirthday_Haley()
        {

            List<EventPrecondition> conditions = new List<EventPrecondition>();
            conditions.Add(new FarmerBirthdayPrecondition());
            conditions.Add(new GameLocationPrecondition(Game1.getLocationFromName("HaleyHouse")));
            conditions.Add(new TimeOfDayPrecondition(600, 2600));

            NPC haley = Game1.getCharacterFromName("Haley");

            conditions.Add(new DatingNPCEventPrecondition(haley));

            HappyBirthdayEventHelper e = new HappyBirthdayEventHelper(EventIds.BirthdayDatingHaley, "Omegasis.HappyBirthday.DatingHaleyBirthdayParty", 2, conditions, new EventStartData("playful", 20, 18, new EventStartData.FarmerData(11, 20, EventHelper.FacingDirection.Right), new List<EventStartData.NPCData>() {
                new EventStartData.NPCData(haley,20,17, EventHelper.FacingDirection.Down),
            }));
            e.globalFadeIn();

            //Dialogue here.
            e.moveFarmerRight(9, EventHelper.FacingDirection.Up, false);

            e.speakWithTranslatedMessage(haley, "DatingHaleyBirthday_Haley:0");
            e.speakWithTranslatedMessage(haley, "DatingHaleyBirthday_Haley:1");
            e.emoteFarmer_Happy();
            e.speakWithTranslatedMessage(haley, "DatingHaleyBirthday_Haley:2");
            e.speakWithTranslatedMessage(haley, "DatingHaleyBirthday_Haley:3");
            e.emoteFarmer_Thinking();
            e.speakWithTranslatedMessage(haley, "DatingHaleyBirthday_Haley:4");


            e.emoteFarmer_Heart();
            e.emote_Heart("Haley");
            e.globalFadeOut(0.010);
            e.setViewportPosition(-100, -100);
            e.addTranslatedMessageToBeShown("DatingHaleyBirthday_Finish:0"); //abi party finish 0
            e.addTranslatedMessageToBeShown("DatingHaleyBirthday_Finish:1"); //abi party finish 0
            e.addObjectToPlayersInventory(221, 1, false);
            e.addTranslatedMessageToBeShown("PartyOver");
            e.end();
            return e;

        }

        public static HappyBirthdayEventHelper DatingBirthday_Sam()
        {
            List<EventPrecondition> conditions = new List<EventPrecondition>();
            conditions.Add(new FarmerBirthdayPrecondition());
            conditions.Add(new GameLocationPrecondition(Game1.getLocationFromName("SamHouse")));
            conditions.Add(new TimeOfDayPrecondition(600, 2600));

            NPC sam = Game1.getCharacterFromName("Sam");

            conditions.Add(new DatingNPCEventPrecondition(sam));

            HappyBirthdayEventHelper e = new HappyBirthdayEventHelper(EventIds.BirthdayDatingSam, "Omegasis.HappyBirthday.DatingSamBirthdayParty", 2, conditions, new EventStartData("playful", 3, 6, new EventStartData.FarmerData(7, 9, EventHelper.FacingDirection.Up), new List<EventStartData.NPCData>() {
                new EventStartData.NPCData(sam,3,5, EventHelper.FacingDirection.Down),
            }));
            e.globalFadeIn();

            //Dialogue here.
            e.moveFarmerUp(4, EventHelper.FacingDirection.Up, false);
            e.moveFarmerLeft(3, EventHelper.FacingDirection.Left, false);
            e.npcFaceDirection(sam, EventHelper.FacingDirection.Right);

            e.speakWithTranslatedMessage(sam, "DatingSamBirthday_Sam:0");
            e.speakWithTranslatedMessage(sam, "DatingSamBirthday_Sam:1");
            e.speakWithTranslatedMessage(sam, "DatingSamBirthday_Sam:2");
            e.speakWithTranslatedMessage(sam, "DatingSamBirthday_Sam:3");
            e.emoteFarmer_Heart();
            e.emote_Heart("Sam");
            e.globalFadeOut(0.010);
            e.setViewportPosition(-100, -100);
            e.addTranslatedMessageToBeShown("DatingSamBirthday_Finish:0"); //sam party finish 0
            e.addTranslatedMessageToBeShown("DatingSamBirthday_Finish:1"); //sam party finish 0
            e.addObjectToPlayersInventory(206, 1, false);
            e.addObjectToPlayersInventory(167, 1, false);
            e.addTranslatedMessageToBeShown("PartyOver");
            e.end();
            return e;
        }

        /// <summary>
        /// Event that occurs when the player is dating Sebastian.
        /// Status: Finished.
        /// </summary>
        /// <returns></returns>
        public static HappyBirthdayEventHelper DatingBirthday_Sebastian()
        {
            List<EventPrecondition> conditions = new List<EventPrecondition>();
            conditions.Add(new FarmerBirthdayPrecondition());
            conditions.Add(new GameLocationPrecondition(Game1.getLocationFromName("ScienceHouse")));
            conditions.Add(new TimeOfDayPrecondition(600, 2600));

            NPC maru = Game1.getCharacterFromName("Maru");
            NPC sebastian = Game1.getCharacterFromName("Sebastian");
            NPC robin = Game1.getCharacterFromName("Robin");
            NPC demetrius = Game1.getCharacterFromName("Demetrius");

            conditions.Add(new DatingNPCEventPrecondition(sebastian));

            HappyBirthdayEventHelper e = new HappyBirthdayEventHelper(EventIds.BirthdayDatingSebastian, "Omegasis.HappyBirthday.DatingSebastianBirthdayParty", 2, conditions, new EventStartData("playful", 28, 12, new EventStartData.FarmerData(23, 12, EventHelper.FacingDirection.Right), new List<EventStartData.NPCData>() {
                new EventStartData.NPCData(maru,27,11, EventHelper.FacingDirection.Down),
                new EventStartData.NPCData(sebastian,26,13, EventHelper.FacingDirection.Up),
                new EventStartData.NPCData(robin,28,9, EventHelper.FacingDirection.Up),
                new EventStartData.NPCData(demetrius,30,11, EventHelper.FacingDirection.Left)
            }));
            e.globalFadeIn();

            e.moveFarmerRight(3, EventHelper.FacingDirection.Right, true);
            e.npcFaceDirection(maru, EventHelper.FacingDirection.Left);
            e.npcFaceDirection(demetrius, EventHelper.FacingDirection.Left);
            //Seb is already facing up.
            e.npcFaceDirection(robin, EventHelper.FacingDirection.Down);

            //Dialogue goes here.
            //Seriously improve dialogue lines. Maru is probably the NPC I know the least about.
            e.speakWithTranslatedMessage(sebastian, "DatingSebastianBirthday_Sebastian:0"); //sebastian 0
            e.speakWithTranslatedMessage(robin, "DatingSebastianBirthday_Robin:0"); //maru 0
            e.speakWithTranslatedMessage(maru, "DatingSebastianBirthday_Maru:0");//Maru 0
            e.speakWithTranslatedMessage(robin, "DatingSebastianBirthday_Robin:1"); //robin 0
            e.speakWithTranslatedMessage(demetrius, "DatingSebastianBirthday_Demetrius:0"); //demetrius 0
            e.speakWithTranslatedMessage(sebastian, "DatingSebastianBirthday_Sebastian:1"); //Sebastian 1
            e.emote_ExclamationMark("Robin");
            e.npcFaceDirection(robin, EventHelper.FacingDirection.Up);
            e.speakWithTranslatedMessage(robin, "DatingSebastianBirthday_Robin:2"); //robin 1
            e.npcFaceDirection(robin, EventHelper.FacingDirection.Down);
            e.moveActorDown("Robin", 1, EventHelper.FacingDirection.Down, false);
            e.addObject(27, 12, 220);
            e.speakWithTranslatedMessage(demetrius, "DatingSebastianBirthday_Demetrius:1"); //maru 2
            e.emoteFarmer_Thinking();
            e.speakWithTranslatedMessage(maru, "DatingSebastianBirthday_Maru:1"); //maru 3
            e.speakWithTranslatedMessage(sebastian, "DatingSebastianBirthday_Sebastian:2"); //Sebastian 1

            //Event finish commands.
            e.emoteFarmer_Heart();
            e.emote_Heart("Sebastian");
            e.globalFadeOut(0.010);
            e.setViewportPosition(-100, -100);
            e.addTranslatedMessageToBeShown("DatingSebastianBirthday_Finish:0"); //maru party finish 0
            e.addTranslatedMessageToBeShown("DatingSebastianBirthday_Finish:1"); //maru party finish 0
            e.addObjectToPlayersInventory(220, 1, false);

            e.addTranslatedMessageToBeShown("PartyOver");
            e.end();
            return e;
        }



        public static HappyBirthdayEventHelper DatingBirthday_Elliott()
        {
            List<EventPrecondition> conditions = new List<EventPrecondition>();
            conditions.Add(new FarmerBirthdayPrecondition());
            conditions.Add(new GameLocationPrecondition(Game1.getLocationFromName("ElliottHouse")));
            conditions.Add(new TimeOfDayPrecondition(600, 2600));

            NPC elliott = Game1.getCharacterFromName("Elliott");

            conditions.Add(new DatingNPCEventPrecondition(elliott));

            HappyBirthdayEventHelper e = new HappyBirthdayEventHelper(EventIds.BirthdayDatingElliott, "Omegasis.HappyBirthday.DatingElliottBirthdayParty", 2, conditions, new EventStartData("playful", 3, 5, new EventStartData.FarmerData(3, 8, EventHelper.FacingDirection.Up), new List<EventStartData.NPCData>() {
                new EventStartData.NPCData(elliott,3,5, EventHelper.FacingDirection.Down),
            }));
            e.globalFadeIn();

            //Dialogue here.
            e.moveFarmerUp(2, EventHelper.FacingDirection.Up, false);
            e.speakWithTranslatedMessage(elliott, "DatingElliottBirthday_Elliott:0");
            e.speakWithTranslatedMessage(elliott, "DatingElliottBirthday_Elliott:1");
            e.speakWithTranslatedMessage(elliott, "DatingElliottBirthday_Elliott:2");
            e.speakWithTranslatedMessage(elliott, "DatingElliottBirthday_Elliott:3");
            e.speakWithTranslatedMessage(elliott, "DatingElliottBirthday_Elliott:4");
            e.emoteFarmer_Thinking();
            e.speakWithTranslatedMessage(elliott, "DatingElliottBirthday_Elliott:5");
            e.emoteFarmer_Heart();
            e.emote_Heart("Elliott");
            e.globalFadeOut(0.010);
            e.setViewportPosition(-100, -100);
            e.addTranslatedMessageToBeShown("DatingElliottBirthday_Finish:0"); //abi party finish 0
            e.addTranslatedMessageToBeShown("DatingElliottBirthday_Finish:1"); //abi party finish 0
            e.addObjectToPlayersInventory(220, 1, false);
            e.addTranslatedMessageToBeShown("PartyOver");
            e.end();
            return e;
        }


        public static HappyBirthdayEventHelper DatingBirthday_Shane()
        {

            List<EventPrecondition> conditions = new List<EventPrecondition>();
            conditions.Add(new FarmerBirthdayPrecondition());
            conditions.Add(new GameLocationPrecondition(Game1.getLocationFromName("AnimalShop")));
            conditions.Add(new TimeOfDayPrecondition(600, 2600));

            NPC shane = Game1.getCharacterFromName("Shane");

            conditions.Add(new DatingNPCEventPrecondition(shane));

            HappyBirthdayEventHelper e = new HappyBirthdayEventHelper(EventIds.BirthdayDatingShane, "Omegasis.HappyBirthday.DatingShaneBirthdayParty", 2, conditions, new EventStartData("playful", 26, 15, new EventStartData.FarmerData(19, 18, EventHelper.FacingDirection.Left), new List<EventStartData.NPCData>() {
                new EventStartData.NPCData(shane,25,16, EventHelper.FacingDirection.Down),
            }));
            e.globalFadeIn();

            //Dialogue here.
            e.moveFarmerRight(3, EventHelper.FacingDirection.Right, false);
            e.moveFarmerUp(2, EventHelper.FacingDirection.Up, false);
            e.moveFarmerRight(2, EventHelper.FacingDirection.Right, false);
            e.npcFaceDirection(shane, EventHelper.FacingDirection.Left);

            e.speakWithTranslatedMessage(shane, "DatingShaneBirthday_Shane:0");
            e.speakWithTranslatedMessage(shane, "DatingShaneBirthday_Shane:1");
            e.speakWithTranslatedMessage(shane, "DatingShaneBirthday_Shane:2");
            e.speakWithTranslatedMessage(shane, "DatingShaneBirthday_Shane:3");
            e.emoteFarmer_Heart();
            e.emote_Heart("Shane");
            e.globalFadeOut(0.010);
            e.setViewportPosition(-100, -100);
            e.addTranslatedMessageToBeShown("DatingShaneBirthday_Finish:0"); //sam party finish 0
            e.addTranslatedMessageToBeShown("DatingShaneBirthday_Finish:1"); //sam party finish 0
            e.addObjectToPlayersInventory(206, 1, false);
            e.addObjectToPlayersInventory(167, 1, false);
            e.addTranslatedMessageToBeShown("PartyOver");
            e.end();
            return e;
        }

        public static HappyBirthdayEventHelper DatingBirthday_Harvey()
        {
            List<EventPrecondition> conditions = new List<EventPrecondition>();
            conditions.Add(new FarmerBirthdayPrecondition());
            conditions.Add(new GameLocationPrecondition(Game1.getLocationFromName("HarveyRoom")));
            conditions.Add(new TimeOfDayPrecondition(600, 2600));

            NPC harvey = Game1.getCharacterFromName("Harvey");

            conditions.Add(new DatingNPCEventPrecondition(harvey));

            HappyBirthdayEventHelper e = new HappyBirthdayEventHelper(EventIds.BirthdayDatingHarvey, "Omegasis.HappyBirthday.DatingHarveyBirthdayParty", 2, conditions, new EventStartData("playful", 6, 6, new EventStartData.FarmerData(6, 11, EventHelper.FacingDirection.Up), new List<EventStartData.NPCData>() {
                new EventStartData.NPCData(harvey,3,6, EventHelper.FacingDirection.Down),
            }));
            e.globalFadeIn();

            //Dialogue here.
            e.moveFarmerUp(5, EventHelper.FacingDirection.Up, false);
            e.moveFarmerLeft(2, EventHelper.FacingDirection.Left, false);
            e.npcFaceDirection(harvey, EventHelper.FacingDirection.Right);
            e.speakWithTranslatedMessage(harvey, "DatingHarveyBirthday_Harvey:0");
            e.speakWithTranslatedMessage(harvey, "DatingHarveyBirthday_Harvey:1");
            e.emoteFarmer_QuestionMark();
            e.speakWithTranslatedMessage(harvey, "DatingHarveyBirthday_Harvey:2");
            e.speakWithTranslatedMessage(harvey, "DatingHarveyBirthday_Harvey:3");


            e.emoteFarmer_Heart();
            e.emote_Heart("Harvey");
            e.globalFadeOut(0.010);
            e.setViewportPosition(-100, -100);
            e.addTranslatedMessageToBeShown("DatingHarveyBirthday_Finish:0"); //abi party finish 0
            e.addTranslatedMessageToBeShown("DatingHarveyBirthday_Finish:1"); //abi party finish 0
            e.addObjectToPlayersInventory(237, 1, false);
            e.addObjectToPlayersInventory(348, 1, false);
            e.addTranslatedMessageToBeShown("PartyOver");
            e.end();
            return e;
        }


        public static HappyBirthdayEventHelper DatingBirthday_Alex()
        {
            List<EventPrecondition> conditions = new List<EventPrecondition>();
            conditions.Add(new FarmerBirthdayPrecondition());
            conditions.Add(new GameLocationPrecondition(Game1.getLocationFromName("JoshHouse")));
            conditions.Add(new TimeOfDayPrecondition(600, 2600));

            NPC alex = Game1.getCharacterFromName("Alex");

            conditions.Add(new DatingNPCEventPrecondition(alex));

            HappyBirthdayEventHelper e = new HappyBirthdayEventHelper(EventIds.BirthdayDatingAlex, "Omegasis.HappyBirthday.DatingAlexBirthdayParty", 2, conditions, new EventStartData("playful", 3, 20, new EventStartData.FarmerData(7, 19, EventHelper.FacingDirection.Left), new List<EventStartData.NPCData>() {
                new EventStartData.NPCData(alex,3,19, EventHelper.FacingDirection.Down),
            }));
            e.globalFadeIn();

            //Dialogue here.
            e.moveFarmerLeft(3, EventHelper.FacingDirection.Left, false);
            e.npcFaceDirection(alex, EventHelper.FacingDirection.Right);

            e.speakWithTranslatedMessage(alex, "DatingAlexBirthday_Alex:0");
            e.speakWithTranslatedMessage(alex, "DatingAlexBirthday_Alex:1");
            e.speakWithTranslatedMessage(alex, "DatingAlexBirthday_Alex:2");
            e.speakWithTranslatedMessage(alex, "DatingAlexBirthday_Alex:3");
            e.emoteFarmer_Heart();
            e.emote_Heart("Alex");
            e.globalFadeOut(0.010);
            e.setViewportPosition(-100, -100);
            e.addTranslatedMessageToBeShown("DatingAlexBirthday_Finish:0"); //sam party finish 0
            e.addTranslatedMessageToBeShown("DatingAlexBirthday_Finish:1"); //sam party finish 0
            e.addObjectToPlayersInventory(206, 1, false);
            e.addObjectToPlayersInventory(167, 1, false);
            e.addTranslatedMessageToBeShown("PartyOver");
            e.end();
            return e;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static HappyBirthdayEventHelper SaloonBirthday_Year2()
        {
            HappyBirthdayEventHelper e = SaloonBirthday(new List<EventStartData.NPCData>()
            {
                new EventStartData.NPCData(Game1.getCharacterFromName("Kent"),3,23, EventHelper.FacingDirection.Right)
            });
            e.stardewEventId = "Omegasis.HappyBirthday.SaloonYear2CommunityBirthdayParty";
            e.eventStringId = EventIds.SaloonBirthdayParty_Year2;
            e.addEventPrecondition(new YearPrecondition(2, Enums.ComparisonType.GreaterThanOrEqualTo));

            return e;
        }


        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static HappyBirthdayEventHelper SaloonBirthday_Year1()
        {
            HappyBirthdayEventHelper e = SaloonBirthday(new List<EventStartData.NPCData>());
            e.addEventPrecondition(new YearPrecondition(1, Enums.ComparisonType.EqualTo));

            return e;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static HappyBirthdayEventHelper SaloonBirthday(List<EventStartData.NPCData> additionalNpcs)
        {
            List<EventPrecondition> conditions = new List<EventPrecondition>();
            conditions.Add(new FarmerBirthdayPrecondition());
            conditions.Add(new GameLocationPrecondition(Game1.getLocationFromName("Saloon")));
            conditions.Add(new TimeOfDayPrecondition(600, 2600));
            conditions.Add(new VillagersHaveEnoughFriendshipBirthdayPrecondition());
            //conditions.Add(new HasUnlockedCommunityCenter()); //Infered by the fact that you must enter the community center to trigger this event anyways.

            NPC lewis = Game1.getCharacterFromName("Lewis");

            List<EventStartData.NPCData> npcs = new List<EventStartData.NPCData>()
            {

                 new EventStartData.NPCData(lewis,14,21, EventHelper.FacingDirection.Down),

                new EventStartData.NPCData(Game1.getCharacterFromName("Gus"),14,18, EventHelper.FacingDirection.Down),
                new EventStartData.NPCData(Game1.getCharacterFromName("Emily"),16,18, EventHelper.FacingDirection.Down),
                new EventStartData.NPCData(Game1.getCharacterFromName("Sandy"),18,18, EventHelper.FacingDirection.Down),



                new EventStartData.NPCData(Game1.getCharacterFromName("Alex"),6,21, EventHelper.FacingDirection.Right),
                new EventStartData.NPCData(Game1.getCharacterFromName("George"),7,2, EventHelper.FacingDirection.Right),
                new EventStartData.NPCData(Game1.getCharacterFromName("Evelyn"),6,22, EventHelper.FacingDirection.Right),

                new EventStartData.NPCData(Game1.getCharacterFromName("Harvey"),6,17, EventHelper.FacingDirection.Right),


                new EventStartData.NPCData(Game1.getCharacterFromName("Marnie"),9,22, EventHelper.FacingDirection.Right),
                new EventStartData.NPCData(Game1.getCharacterFromName("Shane"),11,23, EventHelper.FacingDirection.Right),
                new EventStartData.NPCData(Game1.getCharacterFromName("Jas"),10,23, EventHelper.FacingDirection.Right),


                new EventStartData.NPCData(Game1.getCharacterFromName("Pierre"),17,20, EventHelper.FacingDirection.Down),
                new EventStartData.NPCData(Game1.getCharacterFromName("Caroline"),18,20, EventHelper.FacingDirection.Down),

                new EventStartData.NPCData(Game1.getCharacterFromName("Penny"),10,20, EventHelper.FacingDirection.Down),
                new EventStartData.NPCData(Game1.getCharacterFromName("Pam"),11,20, EventHelper.FacingDirection.Down),

                new EventStartData.NPCData(Game1.getCharacterFromName("Abigail"),22,18, EventHelper.FacingDirection.Left),
                new EventStartData.NPCData(Game1.getCharacterFromName("Sebastian"),23,18, EventHelper.FacingDirection.Left),
                new EventStartData.NPCData(Game1.getCharacterFromName("Sam"),22,19, EventHelper.FacingDirection.Left),

                new EventStartData.NPCData(Game1.getCharacterFromName("Haley"),8,20, EventHelper.FacingDirection.Down),

                new EventStartData.NPCData(Game1.getCharacterFromName("Elliott"),4,18, EventHelper.FacingDirection.Right),
                new EventStartData.NPCData(Game1.getCharacterFromName("Leah"),5,19, EventHelper.FacingDirection.Right),

                new EventStartData.NPCData(Game1.getCharacterFromName("Robin"),19,22, EventHelper.FacingDirection.Left),
                new EventStartData.NPCData(Game1.getCharacterFromName("Demetrius"),19,23, EventHelper.FacingDirection.Left),
                new EventStartData.NPCData(Game1.getCharacterFromName("Maru"),18,21, EventHelper.FacingDirection.Left),


                new EventStartData.NPCData(Game1.getCharacterFromName("Linus"),27,23, EventHelper.FacingDirection.Left),

                new EventStartData.NPCData(Game1.getCharacterFromName("Clint"),20,17, EventHelper.FacingDirection.Left),

                new EventStartData.NPCData(Game1.getCharacterFromName("Vincent"),2,21, EventHelper.FacingDirection.Right),
                new EventStartData.NPCData(Game1.getCharacterFromName("Jodi"),3,22, EventHelper.FacingDirection.Right),
                //new EventStartData.NPCData(Game1.getCharacterFromName("Kent"),3,23, EventHelper.FacingDirection.Right),

                new EventStartData.NPCData(Game1.getCharacterFromName("Willy"),20,20, EventHelper.FacingDirection.Left),

                new EventStartData.NPCData(Game1.getCharacterFromName("Wizard"),34,17, EventHelper.FacingDirection.Down),
            };
            npcs.AddRange(additionalNpcs);

            HappyBirthdayEventHelper e = new HappyBirthdayEventHelper(EventIds.SaloonBirthdayParty_Year1, "Omegasis.HappyBirthday.SaloonYear1CommunityBirthdayParty", 2, conditions, new EventStartData("playful", -100, -100, new EventStartData.FarmerData(14, 23, EventHelper.FacingDirection.Up), npcs));

            e.globalFadeIn();

            e.addTranslatedMessageToBeShown("CommunityBirthdayParty_0");
            e.addTranslatedMessageToBeShown("CommunityBirthdayParty_1");
            e.setViewportPosition(14, 23);

            //Figure out real sound for this.
            //e.playSound("furnace");

            e.emoteFarmer_ExclamationMark();
            e.addTranslatedMessageToBeShown("CommunityBirthdayParty_2");
            e.speakWithTranslatedMessage(lewis, "CommunityBirthdayParty_3");
            e.speakWithTranslatedMessage(lewis, "CommunityBirthdayParty_4");
            e.emoteFarmer_Heart();
            e.globalFadeOut(0.010);
            e.setViewportPosition(-100, -100);
            e.addTranslatedMessageToBeShown("CommunityBirthdayParty_5");
            e.addObjectToPlayersInventory(220, 1, false);

            e.end();

            return e;
        }

        /// <summary>
        /// Birthday event for Joja member.
        /// </summary>
        /// <returns></returns>
        public static HappyBirthdayEventHelper JojaBirthday()
        {
            List<EventPrecondition> conditions = new List<EventPrecondition>();
            conditions.Add(new GameLocationPrecondition(Game1.getLocationFromName("JojaMart")));
            conditions.Add(new TimeOfDayPrecondition(600, 2600));
            conditions.Add(new IsJojaMemberEventPrecondition(true));
            conditions.Add(new FarmerBirthdayPrecondition());

            string morris = "Morris";
            HappyBirthdayEventHelper e = new HappyBirthdayEventHelper(EventIds.JojaMartBirthday, "Omegasis.HappyBirthday.JojaBirthdayParty", 2, conditions, new EventStartData(EventStartData.MusicToPlayType.Continue, 21, 24, new EventStartData.FarmerData(14, 28, EventHelper.FacingDirection.Up), new List<EventStartData.NPCData>()
            {
                new EventStartData.NPCData(morris,-100,-100, EventHelper.FacingDirection.Up)
            }, false));

            e.globalFadeIn();

            e.moveFarmerUp(2, EventHelper.FacingDirection.Up, false);
            e.moveFarmerRight(7, EventHelper.FacingDirection.Up, false);

            //Morris -100 -100 0
            //TODO: Finish this
            e.speakWithTranslatedMessage(morris, "JojaParty_0");
            e.speakWithTranslatedMessage(morris, "JojaParty_1");
            e.speakWithTranslatedMessage(morris, "JojaParty_2");

            e.globalFadeOut(0.010);
            e.setViewportPosition(-400, -400);

            e.showTranslatedMessage("JojaPartyOver");

            e.addObjectToPlayersInventory((int)Gifts.GiftIDS.LegacyGiftIds.Cookie, 1, false);

            e.end();

            return e;

        }

        /// <summary>
        /// The event where Lewis will ask the player for their birthday.
        /// </summary>
        /// <returns></returns>
        public static HappyBirthdayEventHelper LewisAsksPlayerForBirthday()
        {
            List<EventPrecondition> conditions = new List<EventPrecondition>();
            //Need birthdayNotSelected precondition!!!!
            conditions.Add(new GameLocationPrecondition(Game1.getLocationFromName("Farm")));
            conditions.Add(new TimeOfDayPrecondition(600, 2600));
            conditions.Add(new HasChosenBirthdayPrecondition(false));

            NPC lewis = Game1.getCharacterFromName("Lewis");

            HappyBirthdayEventHelper e = new HappyBirthdayEventHelper(EventIds.AskPlayerForBirthday, "Omegasis.HappyBirthday.LewisAsksForYourBirthday", 2, conditions, new EventStartData(EventStartData.MusicToPlayType.Continue, 64, 14, new EventStartData.FarmerData(64, 14, EventHelper.FacingDirection.Down), new List<EventStartData.NPCData>()
            {
                new EventStartData.NPCData(lewis,64,16, EventHelper.FacingDirection.Up),


            }, false));

            e.globalFadeIn();

            e.speakWithTranslatedMessage(lewis.Name, "Lewis_AskPlayerForBirthday_Intro");
            e.addAskForBirthday();

            e.speakIfTodayIsPlayersBirthday(
                lewis.Name,
                "Lewis_AskPlayerForBirthday_TodayIsBirthday",
                "Lewis_AskPlayerForBirthday_Confirmation");

            e.end();

            return e;

        }



        /// <summary>
        /// An event wrapper that manages the birthday party event for the player for when they are married for when the farmhouse is level 1.
        /// </summary>
        /// <returns></returns>
        public static HappyBirthdayEventHelper MarriedBirthday_farmhouseLevel1()
        {
            return MarriedBirthday_AllSpouses(EventIds.Married_BirthdayParty_Farmhouse_2, 1);
        }

        /// <summary>
        /// An event wrapper that manages the birthday party event for the player for when they are married for when the farmhouse is level 2.
        /// </summary>
        /// <returns></returns>
        public static HappyBirthdayEventHelper MarriedBirthday_farmhouseLevel2()
        {
            return MarriedBirthday_AllSpouses(EventIds.Married_BirthdayParty_Farmhouse_2, 2);
        }

        /// <summary>
        /// An event wrapper for the birthday event when the spouse asks the player for their prefered birthday gift when the farmhouse is level 1.
        /// </summary>
        /// <returns></returns>
        public static HappyBirthdayEventHelper SpouseAsksPlayerForFavoriteGift_farmhouseLevel1()
        {
            return AnySpouseAsksPlayerForFavoriteGift(EventIds.AskPlayerForFavoriteGift_Farmhouse_1, 1);
        }

        /// <summary>
        /// An event wrapper for the birthday event when the spouse asks the player for their prefered birthday gift when the farmhouse is level 2.
        /// </summary>
        /// <returns></returns>
        public static HappyBirthdayEventHelper SpouseAsksPlayerForFavoriteGift_farmhouseLevel2()
        {
            return AnySpouseAsksPlayerForFavoriteGift(EventIds.AskPlayerForFavoriteGift_Farmhouse_2, 1);
        }

        /// <summary>
        /// The actual creation of the event data for the birthday party for when the player is married.
        /// </summary>
        /// <param name="EventId"></param>
        /// <param name="EventIntId"></param>
        /// <param name="FarmHouseLevel"></param>
        /// <returns></returns>
        public static HappyBirthdayEventHelper MarriedBirthday_AllSpouses(string EventId, int FarmHouseLevel)
        {
            string spouseName = HappyBirthdayEventHelper.SPOUSE_IDENTIFIER_TOKEN;

            List<EventPrecondition> conditions = new List<EventPrecondition>();
            conditions.Add(new FarmerBirthdayPrecondition());
            conditions.Add(new TimeOfDayPrecondition(600, 2600));
            conditions.Add(new GameLocationIsHomePrecondition());

            conditions.Add(new FarmHouseLevelPrecondition(FarmHouseLevel));
            conditions.Add(new IsMarriedPrecondition());


            Vector2 spouseStartTile;
            Vector2 playerStartTile;
            if (FarmHouseLevel == 2)
            {
                spouseStartTile = new Vector2(7, 14);
                playerStartTile = new Vector2(10, 14);
            }
            else
            {
                //Level 1
                spouseStartTile = new Vector2(6, 5);
                playerStartTile = new Vector2(9, 5);
            }

            HappyBirthdayEventHelper e = new HappyBirthdayEventHelper(EventId, EventId, 2, conditions, new EventStartData("playful", (int)spouseStartTile.X, (int)spouseStartTile.Y, new EventStartData.FarmerData((int)playerStartTile.X, (int)playerStartTile.Y, EventHelper.FacingDirection.Up), new List<EventStartData.NPCData>() {
                new EventStartData.NPCData(spouseName,(int)spouseStartTile.X,(int)spouseStartTile.Y, EventHelper.FacingDirection.Up),
            }));
            e.playerFaceDirection(EventHelper.FacingDirection.Left);


            if (FarmHouseLevel == 2)
                e.makeAllObjectsTemporarilyInvisible(new List<Vector2>()
                {
                    new Vector2(7,14),
                    new Vector2(8,14),
                    new Vector2(9,14),
                    new Vector2(10,14),
                });
            else
                e.makeAllObjectsTemporarilyInvisible(new List<Vector2>()
                {
                    new Vector2(6,5),
                    new Vector2(7,5),
                    new Vector2(8,5),
                    new Vector2(9,5),
                });
            e.globalFadeIn();

            e.moveFarmerLeft(2, EventHelper.FacingDirection.Left, false);
            e.npcFaceDirection(spouseName, EventHelper.FacingDirection.Right);

            e.speakWithTranslatedMessage(spouseName, "SpouseBirthdayEvent_" + spouseName + "_0");
            e.speakWithTranslatedMessage(spouseName, "SpouseBirthdayEvent_" + spouseName + "_1");

            //Add player's favorite gift to inventory.
            e.givePlayerFavoriteGift();

            e.speakWithTranslatedMessage(spouseName, "SpouseBirthdayEvent_" + spouseName + "_2");
            e.speakWithTranslatedMessage(spouseName, "SpouseBirthdayEvent_" + spouseName + "_3");

            e.emoteFarmer_Heart();
            e.emote_Heart(spouseName);

            e.globalFadeOut(0.010);
            e.setViewportPosition(-400, -400);

            e.addTranslatedMessageToBeShown("SpousePartyOver");

            e.end();

            return e;

        }

        /// <summary>
        /// The actual creation of the event where the player's spouse will ask them what their favorite gift is.
        /// </summary>
        /// <param name="EventId"></param>
        /// <param name="EventIntId"></param>
        /// <param name="FarmHouseLevel"></param>
        /// <returns></returns>
        public static HappyBirthdayEventHelper AnySpouseAsksPlayerForFavoriteGift(string EventId, int FarmHouseLevel)
        {
            string spouseName = HappyBirthdayEventHelper.SPOUSE_IDENTIFIER_TOKEN;

            List<EventPrecondition> conditions = new List<EventPrecondition>();
            conditions.Add(new TimeOfDayPrecondition(600, 2600));
            conditions.Add(new GameLocationIsHomePrecondition());
            conditions.Add(new HasChosenFavoriteGiftPrecondition(false));

            conditions.Add(new FarmHouseLevelPrecondition(FarmHouseLevel));
            conditions.Add(new IsMarriedPrecondition());


            Vector2 spouseStartTile;
            Vector2 playerStartTile;
            if (FarmHouseLevel == 2)
            {
                spouseStartTile = new Vector2(7, 14);
                playerStartTile = new Vector2(10, 14);
            }
            else
            {
                //Level 1
                spouseStartTile = new Vector2(6, 5);
                playerStartTile = new Vector2(9, 5);
            }

            HappyBirthdayEventHelper e = new HappyBirthdayEventHelper(EventId, EventId, 2, conditions, new EventStartData("playful", (int)spouseStartTile.X, (int)spouseStartTile.Y, new EventStartData.FarmerData((int)playerStartTile.X, (int)playerStartTile.Y, EventHelper.FacingDirection.Up), new List<EventStartData.NPCData>() {
                new EventStartData.NPCData(spouseName,(int)spouseStartTile.X,(int)spouseStartTile.Y, EventHelper.FacingDirection.Up),
            }, false));

            e.playerFaceDirection(EventHelper.FacingDirection.Left);

            e.globalFadeIn();

            e.moveFarmerLeft(2, EventHelper.FacingDirection.Left, false);
            e.npcFaceDirection(spouseName, EventHelper.FacingDirection.Right);

            e.speakWithTranslatedMessage(spouseName, "SpouseAskPlayerForFavoriteGift_" + spouseName + "_0");
            e.addAskForFavoriteGift();
            e.speakWithTranslatedMessage(spouseName, "SpouseAskPlayerForFavoriteGift_" + spouseName + "_1");

            e.globalFadeOut(0.010);
            e.setViewportPosition(-100, -100);
            e.end();

            return e;

        }

    }
}
