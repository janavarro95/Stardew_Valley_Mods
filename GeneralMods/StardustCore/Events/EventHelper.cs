using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using StardewValley;
using System.Text.Json.Serialization;
using Omegasis.StardustCore.Events.Preconditions;
using Omegasis.StardustCore.Events.Preconditions.TimeSpecific;

namespace Omegasis.StardustCore.Events
{
    /// <summary>
    /// 
    /// Helps in creating events based in code for stardew valley.
    /// https://stardewvalleywiki.com/Modding:Event_data
    ///
    /// A lot of the function comments were taken from the SDV wiki.
    /// </summary>
    public class EventHelper
    {
        public const string commandArgsSplitter = " ";

        /// <summary>
        /// Nexus user id for Omegasis (aka me).
        /// </summary>
        protected int nexusUserId = 32171640;

        /// <summary>
        /// Wraps SDV facing direction.
        /// </summary>
        public enum FacingDirection
        {
            Up,
            Right,
            Down,
            Left
        }

        public enum Layers
        {
            Back,
            Paths,
            Buildings,
            Front,
            AlwaysFront
        }

        public enum FestivalItemType
        {
            Pan,
            Sculpture,
            Rod,
            Sword,
            Hero,
            Joja,
            SlimeEgg
        }


        protected StringBuilder eventData = new StringBuilder();
        protected StringBuilder eventPreconditionData = new StringBuilder();

        /// <summary>
        /// The event data for a given event.
        /// </summary>
        public string EventData
        {
            get
            {
                return this.getEventString();
            }
            set
            {
                this.eventData.Clear();
                this.eventData.Append(value);
            }
        }

        /// <summary>
        /// The event data for a given event.
        /// </summary>
        public string EventPreconditionData
        {
            get
            {
                return this.eventPreconditionData.ToString();
            }
            set
            {
                this.eventPreconditionData.Clear();
                this.eventPreconditionData.Append(value);
            }
        }

        protected List<EventPrecondition> eventPreconditions;
        public string stardewEventId;
        public string eventStringId;

        public int version;

        public EventHelper()
        {
            this.eventData = new StringBuilder();
            this.eventPreconditions = new List<EventPrecondition>();
        }

        public EventHelper(string EventName, string EventId, int version, GameLocationPrecondition Location, TimeOfDayPrecondition Time, DayOfWeekPrecondition NotTheseDays, EventStartData StartData)
        {
            this.eventStringId = EventName;
            this.eventData = new StringBuilder();
            this.eventPreconditions = new List<EventPrecondition>();
            this.stardewEventId = EventId;
            this.version = version;
            this.addEventPrecondition(Location);
            this.addEventPrecondition(Time);
            this.addEventPrecondition(NotTheseDays);
            this.addEventData(StartData.ToString());
        }

        public EventHelper(string EventName, string EventId, int version, List<EventPrecondition> Conditions, EventStartData StartData)
        {
            this.eventStringId = EventName;
            this.stardewEventId = EventId;
            this.eventData = new StringBuilder();
            this.eventPreconditions = new List<EventPrecondition>();
            this.version = version;
            foreach (var v in Conditions)
            {
                this.addEventPrecondition(v);
            }
            this.addEventData(StartData.ToString());
        }

        /// <summary>
        /// Adds in the event precondition data to the string builder and appends seperators as necessary.
        /// </summary>
        /// <param name="Data"></param>
        public virtual void addEventPrecondition(EventPrecondition Data, bool AddPreconditionData = true)
        {
            this.eventPreconditions.Add(Data);
            if (AddPreconditionData)
            {
                this.eventPreconditionData.Append(Data.ToString());
                this.eventPreconditionData.Append(this.getCommandSeperator());
            }

        }

        /// <summary>
        /// Adds in the data to the event data.Aka what happens during the event.
        /// </summary>
        /// <param name="Data"></param>
        public virtual void addEventData(string Data)
        {

            if (this.eventData.Length > 0)
            {
                this.eventData.Append(this.getCommandSeperator());
            }
            this.eventData.Append(Data);
        }

        /// <summary>
        /// Adds in the data to the event data. Aka what happens during the event.
        /// </summary>
        /// <param name="Builder"></param>
        public virtual void addEventData(StringBuilder Builder)
        {
            this.addEventData(Builder.ToString());
        }

        public virtual void parseEventPreconditionsFromPreconditionStrings(EventManager eventManager)
        {
            string[] preconditions = this.EventPreconditionData.Split(this.getCommandSeperator());
            foreach (string precondition in preconditions)
            {
                string[] data = precondition.Split(" ");
                string preconditionId = data[0];

                if (string.IsNullOrEmpty(preconditionId))
                {
                    continue;
                }

                if (eventManager.eventPreconditionParsingMethods.ContainsKey(preconditionId))
                {
                    EventPrecondition condition = eventManager.eventPreconditionParsingMethods[preconditionId].Invoke(data);
                    this.addEventPrecondition(condition, false);
                }
                else
                {
                    throw new Exception(string.Format("Unknown event precondition command {0}. Does it need to be registered in the EventManager?", precondition));
                }
            }
        }


        /// <summary>
        /// Converts the direction to enum.
        /// </summary>
        /// <param name="Dir"></param>
        /// <returns></returns>
        public virtual int getFacingDirectionNumber(FacingDirection Dir)
        {
            return (int)Dir;
        }

        /// <summary>
        /// Gets the layer string from the Layer enum. Has weird values???/
        /// </summary>
        /// <param name="Layer"></param>
        /// <returns></returns>
        public virtual int getLayerName(Layers Layer)
        {
            return 724;
            //if (Layer == Layers.AlwaysFront) return "AlwaysFront";
            //if (Layer == Layers.Back) return "Back";
            //if (Layer == Layers.Buildings) return "Buildings";
            //if (Layer == Layers.Front) return "Front";
            //if (Layer == Layers.Paths) return "Paths";
            //return "";
        }

        /// <summary>
        /// Gets the even parsing seperator.
        /// </summary>
        /// <returns></returns>
        public virtual string getCommandSeperator()
        {
            return "/";
        }

        /// <summary>
        /// Gets the starting event numbers based off of my nexus user id.
        /// </summary>
        /// <returns></returns>
        public virtual string getUniqueEventStartID()
        {
            string s = this.nexusUserId.ToString();
            return s.Substring(0, 4);
        }

        /// <summary>
        /// Checks to ensure I don't create a id value that is too big for nexus.
        /// </summary>
        /// <param name="IDToCheck"></param>
        /// <returns></returns>
        public virtual bool isIdValid(int IDToCheck)
        {
            if (IDToCheck > 2147483647 || IDToCheck < 0) return false;
            else return true;
        }

        /// <summary>
        /// Checks to ensure I don't create a id value that is too big for nexus.
        /// </summary>
        /// <param name="IDToCheck"></param>
        /// <returns></returns>
        public virtual bool isIdValid(string IDToCheck)
        {
            if (Convert.ToInt32(IDToCheck) > 2147483647 || Convert.ToInt32(IDToCheck) < 0) return false;
            else return true;
        }
        /// <summary>
        /// Gets the string representation for the event's data.
        /// </summary>
        /// <returns></returns>
        public virtual string getEventString()
        {
            return this.eventData.ToString();
        }
        /// <summary>
        /// Creates the Stardew Valley event from the given event data.
        /// </summary>
        /// <param name="PlayerActor"></param>
        /// <returns></returns>
        public virtual StardewValley.Event getEvent(Farmer PlayerActor = null)
        {
            return new StardewValley.Event(this.getEventString(), null, this.stardewEventId, PlayerActor);
        }

        /// <summary>
        /// Checks to see if all of the event preconditions have been met and starts the event if so.
        /// </summary>
        public virtual bool startEventAtLocationifPossible()
        {
            if (this.canEventOccur())
            {
                //Game1.player.currentLocation.currentEvent = this.getEvent();
                Game1.player.currentLocation.startEvent(this.getEvent());
                return true;
            }
            return false;
        }


        //~~~~~~~~~~~~~~~~//
        //   Validation   //
        //~~~~~~~~~~~~~~~~//

        /// <summary>
        /// Checks to see if the event can occur.
        /// </summary>
        /// <returns></returns>
        public virtual bool canEventOccur()
        {
            foreach (EventPrecondition eve in this.eventPreconditions)
            {
                if (eve.meetsCondition() == false)
                {
                    //ModCore.log("Failed event precondition for precondition type: " + eve.GetType());
                    return false;
                }
            }

            return true;
        }

        //~~~~~~~~~~~~~~~~//
        //      Actions   //
        //~~~~~~~~~~~~~~~~//

        /// <summary>
        /// Adds an object at the specified tile from the TileSheets\Craftables.png sprite sheet
        /// </summary>
        /// <param name="xTile"></param>
        /// <param name="yTile"></param>
        /// <param name="ID"></param>
        public virtual void addBigProp(int xTile, int yTile, int ID)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addBigProp ");
            b.Append(xTile.ToString());
            b.Append(" ");
            b.Append(yTile.ToString());
            b.Append(" ");
            b.Append(ID.ToString());
            this.addEventData(b);
        }

        /// <summary>
        /// Starts an active dialogue event with the given ID and a length of 4 days.
        /// </summary>
        /// <param name="ID"></param>
        public virtual void addConversationTopic(string ID)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addBigProp ");
            b.Append(ID);
            this.addEventData(b);
        }

        /// <summary>
        /// Adds the specified cooking recipe to the player.
        /// </summary>
        /// <param name="Recipe"></param>
        public virtual void addCookingRecipe(string Recipe)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addCookingRecipe ");
            b.Append(Recipe);
            this.addEventData(b);
        }

        /// <summary>
        /// Adds the specified crafting recipe to the player.
        /// </summary>
        /// <param name="Recipe"></param>
        public virtual void addCraftingRecipe(string Recipe)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addCraftingRecipe ");
            b.Append(Recipe);
            this.addEventData(b);
        }

        /// <summary>
        /// Add a non-solid prop from the current festival texture. Default solid width/height is 1. Default display height is solid height.
        /// </summary>
        public virtual void addFloorProp(int PropIndex, int XTile, int YTile, int SolidWidth, int SolidHeight, int DisplayHeight)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addFloorProp ");
            b.Append(PropIndex.ToString());
            b.Append(" ");
            b.Append(XTile.ToString());
            b.Append(" ");
            b.Append(YTile.ToString());
            b.Append(" ");
            b.Append(SolidWidth.ToString());
            b.Append(" ");
            b.Append(SolidHeight.ToString());
            b.Append(" ");
            b.Append(DisplayHeight.ToString());
            this.addEventData(b);
        }

        /// <summary>
        /// Adds a glowing temporary sprite at the specified tile from the Maps\springobjects.png sprite sheet. A light radius of 0 just places the sprite.
        /// </summary>
        /// <param name="ItemID"></param>
        /// <param name="XPosition"></param>
        /// <param name="YPosition"></param>
        /// <param name="LightRadius"></param>
        public virtual void addLantern(int ItemID, int XPosition, int YPosition, float LightRadius)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addLantern ");
            b.Append(ItemID.ToString());
            b.Append(" ");
            b.Append(XPosition.ToString());
            b.Append(" ");
            b.Append(YPosition.ToString());
            b.Append(" ");
            b.Append(LightRadius.ToString());
            this.addEventData(b);
        }

        /// <summary>
        /// 	Set a letter as received.
        /// </summary>
        /// <param name="ID"></param>
        public virtual void addMailReceived(string ID)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addMailReceived ");
            b.Append(ID);
            this.addEventData(b);
        }

        /// <summary>
        /// Adds a temporary sprite at the specified tile from the Maps\springobjects.png sprite sheet.
        /// </summary>
        /// <param name="XTile"></param>
        /// <param name="YTile"></param>
        /// <param name="ParentSheetIndex"></param>
        /// <param name="Layer"></param>
        public virtual void addObject(int XTile, int YTile, int ParentSheetIndex)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addObject ");
            b.Append(XTile.ToString());
            b.Append(" ");
            b.Append(YTile.ToString());
            b.Append(" ");
            b.Append(ParentSheetIndex);
            this.addEventData(b);
        }

        /// <summary>
        /// Add a solid prop from the current festival texture. Default solid width/height is 1. Default display height is solid height.
        /// </summary>
        /// <param name="Index"></param>
        /// <param name="XTile"></param>
        /// <param name="YTile"></param>
        /// <param name="SolidWidth"></param>
        /// <param name="SolidHeight"></param>
        /// <param name="DisplayHeight"></param>
        public virtual void addProp(int Index, int XTile, int YTile, int SolidWidth, int SolidHeight, int DisplayHeight)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addProp ");
            b.Append(Index.ToString());
            b.Append(" ");
            b.Append(XTile.ToString());
            b.Append(" ");
            b.Append(YTile.ToString());
            b.Append(" ");
            b.Append(SolidWidth.ToString());
            b.Append(" ");
            b.Append(SolidHeight.ToString());
            b.Append(" ");
            b.Append(DisplayHeight.ToString());
            this.addEventData(b);
        }

        /// <summary>
        /// Add the specified quest to the quest log.
        /// </summary>
        /// <param name="QuestID"></param>
        public virtual void addQuest(int QuestID)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addQuest ");
            b.Append(QuestID.ToString());
            this.addEventData(b);
        }

        /// <summary>
        /// Add a temporary actor. 'breather' is boolean. The category determines where the texture will be loaded from, default is Character. Animal name only applies to animal.
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="SpriteWidth"></param>
        /// <param name="SpriteHeight"></param>
        /// <param name="TileX"></param>
        /// <param name="TileY"></param>
        /// <param name="Direction"></param>
        /// <param name="Breather"></param>
        public virtual void addTemporaryActor_NPC(NPC npc, int SpriteWidth, int SpriteHeight, int TileX, int TileY, FacingDirection Direction, bool Breather)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addTemporaryActor ");
            b.Append(npc.Name);
            b.Append(" ");
            b.Append(SpriteWidth.ToString());
            b.Append(" ");
            b.Append(SpriteHeight.ToString());
            b.Append(" ");
            b.Append(TileX.ToString());
            b.Append(" ");
            b.Append(TileY.ToString());
            b.Append(" ");
            b.Append(this.getFacingDirectionNumber(Direction));
            b.Append(" ");
            b.Append(Breather);
            b.Append(" ");
            b.Append("Character");
            this.addEventData(b);
        }


        public virtual void addTemporaryActor_NPC(string npc, int SpriteWidth, int SpriteHeight, int TileX, int TileY, FacingDirection Direction, bool Breather)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addTemporaryActor ");
            b.Append(npc);
            b.Append(" ");
            b.Append(SpriteWidth.ToString());
            b.Append(" ");
            b.Append(SpriteHeight.ToString());
            b.Append(" ");
            b.Append(TileX.ToString());
            b.Append(" ");
            b.Append(TileY.ToString());
            b.Append(" ");
            b.Append(this.getFacingDirectionNumber(Direction));
            b.Append(" ");
            b.Append(Breather);
            b.Append(" ");
            b.Append("Character");
            this.addEventData(b);
        }
        /// <summary>
        /// Add a temporary actor. 'breather' is boolean. The category determines where the texture will be loaded from, default is Character. Animal name only applies to animal.
        /// </summary>
        /// <param name="character"></param>
        /// <param name="SpriteWidth"></param>
        /// <param name="SpriteHeight"></param>
        /// <param name="TileX"></param>
        /// <param name="TileY"></param>
        /// <param name="Direction"></param>
        /// <param name="Breather"></param>
        /// <param name="AnimalName"></param>
        public virtual void addTemporaryActor_Animal(string character, int SpriteWidth, int SpriteHeight, int TileX, int TileY, FacingDirection Direction, bool Breather, string AnimalName)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addTemporaryActor ");
            b.Append(character);
            b.Append(" ");
            b.Append(SpriteWidth.ToString());
            b.Append(" ");
            b.Append(SpriteHeight.ToString());
            b.Append(" ");
            b.Append(TileX.ToString());
            b.Append(" ");
            b.Append(TileY.ToString());
            b.Append(" ");
            b.Append(Direction);
            b.Append(" ");
            b.Append(Breather);
            b.Append(" ");
            b.Append("Animal");
            b.Append(" ");
            b.Append(AnimalName);
            this.addEventData(b);
        }

        /// <summary>
        /// Add a temporary actor. 'breather' is boolean. The category determines where the texture will be loaded from, default is Character. Animal name only applies to animal.
        /// </summary>
        /// <param name="character"></param>
        /// <param name="SpriteWidth"></param>
        /// <param name="SpriteHeight"></param>
        /// <param name="TileX"></param>
        /// <param name="TileY"></param>
        /// <param name="Direction"></param>
        /// <param name="Breather"></param>
        public virtual void addTemporaryActor_Monster(string character, int SpriteWidth, int SpriteHeight, int TileX, int TileY, FacingDirection Direction, bool Breather)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addTemporaryActor ");
            b.Append(character);
            b.Append(" ");
            b.Append(SpriteWidth.ToString());
            b.Append(" ");
            b.Append(SpriteHeight.ToString());
            b.Append(" ");
            b.Append(TileX.ToString());
            b.Append(" ");
            b.Append(TileY.ToString());
            b.Append(" ");
            b.Append(Direction);
            b.Append(" ");
            b.Append(Breather);
            b.Append(" ");
            b.Append("Monster");
            this.addEventData(b);
        }

        /// <summary>
        /// Places on object on the furniture at a position. If the location is FarmHouse, then it will always be placed on the initial table.
        /// </summary>
        /// <param name="XPosition"></param>
        /// <param name="YPosition"></param>
        /// <param name="ObjectParentSheetIndex"></param>
        public virtual void addToTable(int XPosition, int YPosition, int ObjectParentSheetIndex)
        {
            StringBuilder b = new StringBuilder();
            b.Append("addToTable ");
            b.Append(XPosition);
            b.Append(" ");
            b.Append(YPosition);
            b.Append(" ");
            b.Append(ObjectParentSheetIndex);
            this.addEventData(b);
        }

        /// <summary>
        /// Adds the Return Scepter to the player's inventory.
        /// </summary>
        public virtual void addTool_ReturnScepter()
        {
            StringBuilder b = new StringBuilder();
            b.Append("addTool Wand");
            this.addEventData(b);
        }

        /// <summary>
        /// 	Set multiple movements for an NPC. You can set True to have NPC walk the path continuously. Example: /advancedMove Robin false 0 3 2 0 0 2 -2 0 0 -2 2 0/
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="Loop"></param>
        /// <param name="TilePoints"></param>
        public virtual void advanceMove(NPC npc, bool Loop, List<Point> TilePoints)
        {
            StringBuilder b = new StringBuilder();
            b.Append("advancedMove ");
            b.Append(npc.Name);
            b.Append(" ");
            b.Append(Loop.ToString());
            b.Append(" ");
            for (int i = 0; i < TilePoints.Count; i++)
            {
                b.Append(TilePoints[i].X);
                b.Append(" ");
                b.Append(TilePoints[i].Y);
                if (i != TilePoints.Count - 1)
                {
                    b.Append(" ");
                }
            }
            this.addEventData(b);
        }

        public virtual void advanceMove(string Actor, bool Loop, List<Point> TilePoints)
        {
            StringBuilder b = new StringBuilder();
            b.Append("advancedMove ");
            b.Append(Actor);
            b.Append(" ");
            b.Append(Loop.ToString());
            b.Append(" ");
            for (int i = 0; i < TilePoints.Count; i++)
            {
                b.Append(TilePoints[i].X);
                b.Append(" ");
                b.Append(TilePoints[i].Y);
                if (i != TilePoints.Count - 1)
                {
                    b.Append(" ");
                }
            }

            StardustCoreModCore.ModMonitor.Log(b.ToString(), StardewModdingAPI.LogLevel.Info);

            this.addEventData(b);
        }

        /// <summary>
        /// Modifies the ambient light level, with RGB values from 0 to 255. Note that it works by removing colors from the existing light ambience, so ambientLight 1 80 80 would reduce green and blue and leave the light with a reddish hue.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        public virtual void setAmbientLight(int r, int g, int b)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("ambientLight ");
            builder.Append(r);
            builder.Append(" ");
            builder.Append(g);
            builder.Append(" ");
            builder.Append(b);
            this.addEventData(builder);
        }

        /// <summary>
        /// Modifies the ambient light level, with RGB values from 0 to 255. Note that it works by removing colors from the existing light ambience, so ambientLight 1 80 80 would reduce green and blue and leave the light with a reddish hue.
        /// </summary>
        /// <param name="color"></param>
        public virtual void setAmbientLight(Color color)
        {
            this.setAmbientLight(color.R, color.G, color.B);
        }

        //animalNaming

        public virtual void animalNaming()
        {
            StringBuilder b = new StringBuilder();
            b.Append("animalNaming");
            this.addEventData(b);
        }

        /// <summary>
        /// Animate a named actor, using one or more <frames> from their sprite sheet, for <frame duration> milliseconds per frame. <flip> indicates whether to flip the sprites along the Y axis; <loop> indicates whether to repeat the animation until stopAnimation is used.
        /// </summary>
        /// <param name="ActorName"></param>
        /// <param name="Flip"></param>
        /// <param name="Loop"></param>
        /// <param name="FrameDuration">In milliseconds</param>
        /// <param name="Frames"></param>
        public virtual void animate(string ActorName, bool Flip, bool Loop, int FrameDuration, List<int> Frames)
        {
            StringBuilder b = new StringBuilder();
            b.Append("animate ");
            b.Append(ActorName);
            b.Append(" ");
            b.Append(Flip.ToString().ToLowerInvariant());
            b.Append(" ");
            b.Append(Loop.ToString().ToLowerInvariant());
            b.Append(" ");
            b.Append(FrameDuration);
            b.Append(" ");
            for (int i = 0; i < Frames.Count; i++)
            {
                b.Append(Frames[i]);
                if (i != Frames.Count - 1)
                {
                    b.Append(" ");
                }
            }
            this.addEventData(b);
        }

        /// <summary>
        /// Attach an actor to the most recent temporary sprite.
        /// </summary>
        /// <param name="Actor"></param>
        public virtual void attachCharacterToTempSprite(string Actor)
        {
            StringBuilder b = new StringBuilder();
            b.Append("attachCharacterToTempSprite ");
            b.Append(Actor);
            this.addEventData(b);
        }

        /// <summary>
        /// Awards the festival prize to the winner for the easter egg hunt and ice fishing contest.
        /// </summary>
        public virtual void awardFestivalPrize()
        {
            StringBuilder b = new StringBuilder();
            b.Append("awardFestivalPrize");
            this.addEventData(b);
        }

        /// <summary>
        /// Awards the specified item to the player. Possible item types are "pan", "sculpture", "rod", "sword", "hero", "joja", and "slimeegg".
        /// </summary>
        /// <param name="ItemType"></param>
        public virtual void awardFestivalPrize(FestivalItemType ItemType)
        {
            StringBuilder b = new StringBuilder();
            b.Append("awardFestivalPrize ");

            if (ItemType == FestivalItemType.Hero)
            {
                b.Append("hero");
            }
            else if (ItemType == FestivalItemType.Joja)
            {
                b.Append("joja");
            }
            else if (ItemType == FestivalItemType.Pan)
            {
                b.Append("pan");
            }
            else if (ItemType == FestivalItemType.Rod)
            {
                b.Append("rod");
            }
            else if (ItemType == FestivalItemType.Sculpture)
            {
                b.Append("sculpture");
            }
            else if (ItemType == FestivalItemType.SlimeEgg)
            {
                b.Append("slimeegg");
            }
            else if (ItemType == FestivalItemType.Sword)
            {
                b.Append("sword");
            }

            this.addEventData(b);
        }

        /// <summary>
        /// Causes the event to be seen by all players when triggered.
        /// </summary>
        public virtual void broadcastEvent()
        {
            StringBuilder b = new StringBuilder();
            b.Append("broadcastEvent");
            this.addEventData(b);
        }

        /// <summary>
        /// Trigger question about adopting your pet.
        /// </summary>
        public virtual void petAdoptionQuestion()
        {
            StringBuilder b = new StringBuilder();
            b.Append("catQuestion");
            this.addEventData(b);
        }

        /// <summary>
        /// Trigger the question for the farm cave type. This will work again later, however changing from bats to mushrooms will not remove the mushroom spawning objects.
        /// </summary>
        public virtual void farmCaveTypeQuestion()
        {
            StringBuilder b = new StringBuilder();
            b.Append("cave");
            this.addEventData(b);
        }

        /// <summary>
        /// Change to another location and run the remaining event script there.
        /// </summary>
        /// <param name="location"></param>
        public virtual void changeLocation(GameLocation location)
        {
            StringBuilder b = new StringBuilder();
            b.Append("changeLocation ");
            b.Append(location.NameOrUniqueName);
            this.addEventData(b);
        }


        /// <summary>
        /// Change to another location and run the remaining event script there.
        /// </summary>
        /// <param name="location"></param>
        public virtual void changeLocation(string location)
        {
            StringBuilder b = new StringBuilder();
            b.Append("changeLocation ");
            b.Append(location);
            this.addEventData(b);
        }

        /// <summary>
        /// Change the specified tile to a particular value.
        /// </summary>
        /// <param name="l"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="TileIndex"></param>
        public virtual void changeMapTile(Layers l, int X, int Y, int TileIndex)
        {
            StringBuilder b = new StringBuilder();
            b.Append("changeMapTile ");
            b.Append(this.getLayerName(l));
            b.Append(" ");
            b.Append(X);
            b.Append(" ");
            b.Append(Y);
            b.Append(" ");
            b.Append(TileIndex);
            this.addEventData(b);
        }

        /// <summary>
        /// Change the NPC's portrait to be from "Portraits/<actor>_<Portrait>".
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="Portrait"></param>
        public virtual void changePortrait(string npc, string Portrait)
        {
            StringBuilder b = new StringBuilder();
            b.Append("changePortrait ");
            b.Append(npc);
            b.Append(" ");
            b.Append(Portrait);
            this.addEventData(b);
        }

        /// <summary>
        /// Change the actor's sprite to be from "Characters/<actor>_<sprite>".
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="Sprite"></param>
        public virtual void changeSprite(string npc, string Sprite)
        {
            StringBuilder b = new StringBuilder();
            b.Append("changeSprite ");
            b.Append(npc);
            b.Append(" ");
            b.Append(Sprite);
            this.addEventData(b);
        }

        /// <summary>
        /// 	Change the location to a temporary one loaded from the map file specified by <map>. The [pan] argument indicates the tile coordinates to pan to (defaults to 0, 0).
        /// </summary>
        /// <param name="Map"></param>
        public virtual void changeToTemporaryMap(string Map)
        {
            StringBuilder b = new StringBuilder();
            b.Append("changeToTemporaryMap ");
            b.Append(Map);
            this.addEventData(b);
        }
        /// <summary>
        /// 	Change the location to a temporary one loaded from the map file specified by <map>. The [pan] argument indicates the tile coordinates to pan to (defaults to 0, 0).
        /// </summary>
        /// <param name="Map"></param>
        /// <param name="PanPosition"></param>
        public virtual void changeToTemporaryMap(string Map, Point PanPosition)
        {
            StringBuilder b = new StringBuilder();
            b.Append("changeToTemporaryMap ");
            b.Append(Map);
            b.Append(" ");
            b.Append(PanPosition.X);
            b.Append(" ");
            b.Append(PanPosition.Y);
            this.addEventData(b);
        }

        /// <summary>
        /// Changes the NPC's vertical texture offset. Example: changeYSourceRectOffset Abigail 96 will offset her sprite sheet, showing her looking left instead of down. This persists for the rest of the event. This is only used in Emily's Clothing Therapy event to display the various outfits properly.
        /// </summary>
        /// <param name="NPC"></param>
        /// <param name="YOffset"></param>
        public virtual void changeYSourceRectOffset(string NPC, int YOffset)
        {
            StringBuilder b = new StringBuilder();
            b.Append("changeYSourceRectOffset ");
            b.Append(NPC);
            b.Append(" ");
            b.Append(YOffset);
            this.addEventData(b);
        }

        /// <summary>
        /// Acts as if the player had clicked the specified x/y coordinate and triggers any relevant action. It is commonly used to open doors from inside events, but it can be used for other purposes. If you use it on an NPC you will talk to them, and if the player is holding an item they will give that item as a gift. doAction activates objects in the main game world (their actual location outside of the event), so activating NPCs like this is very tricky, and their reaction varies depending on what the player is holding.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public virtual void doAction(int x, int y)
        {
            StringBuilder b = new StringBuilder();
            b.Append("doAction ");
            b.Append(x);
            b.Append(" ");
            b.Append(y);
            this.addEventData(b);
        }

        /// <summary>
        /// Make the given NPC name perform an emote, which is a little icon shown above the NPC's head. Emotes are stored in Content\TileSheets\emotes.xnb (see list of emotes).
        /// </summary>
        /// <param name="Actor"></param>
        /// <param name="EmoteID"></param>
        public virtual void emote(string Actor, int EmoteID)
        {
            StringBuilder b = new StringBuilder();
            b.Append("emote ");
            b.Append(Actor);
            b.Append(" ");
            b.Append(EmoteID * 4);
            this.addEventData(b);
        }

        public virtual void emoteFarmer(int EmoteID)
        {
            this.emote("farmer", EmoteID);
        }

        public virtual void emote_Empty(string Actor)
        {
            this.emote(Actor, 0);
        }

        public virtual void emote_NoWater(string Actor)
        {
            this.emote(Actor, 1);
        }

        public virtual void emote_QuestionMark(string Actor)
        {
            this.emote(Actor, 2);
        }

        public virtual void emote_Angry(string Actor)
        {
            this.emote(Actor, 3);
        }

        public virtual void emote_ExclamationMark(string Actor)
        {
            this.emote(Actor, 4);
        }
        public virtual void emote_Heart(string Actor)
        {
            this.emote(Actor, 5);
        }
        public virtual void emote_Sleeping(string Actor)
        {
            this.emote(Actor, 6);
        }
        public virtual void emote_Sad(string Actor)
        {
            this.emote(Actor, 7);
        }
        public virtual void emote_Happy(string Actor)
        {
            this.emote(Actor, 8);
        }
        public virtual void emote_No(string Actor)
        {
            this.emote(Actor, 9);
        }
        public virtual void emote_Pause(string Actor)
        {
            this.emote(Actor, 10);
        }
        public virtual void emote_Thinking(string Actor)
        {
            this.emote(Actor, 10);
        }
        public virtual void emote_Fishing(string Actor)
        {
            this.emote(Actor, 11);
        }

        public virtual void emote_CommunityCenterTablet(string Actor)
        {
            this.emote(Actor, 12);
        }

        public virtual void emote_Gaming(string Actor)
        {
            this.emote(Actor, 13);
        }
        public virtual void emote_MusicNote(string Actor)
        {
            this.emote(Actor, 14);
        }
        public virtual void emote_Blushing(string Actor)
        {
            this.emote(Actor, 15);
        }
        public virtual void emote_Embarrased(string Actor)
        {
            this.emote_Blushing(Actor);
        }

        public virtual void emoteFarmer_Empty()
        {
            this.emoteFarmer(0);
        }

        public virtual void emoteFarmer_NoWater()
        {
            this.emoteFarmer(1);
        }

        public virtual void emoteFarmer_QuestionMark()
        {
            this.emoteFarmer(2);
        }

        public virtual void emoteFarmer_Angry()
        {
            this.emoteFarmer(3);
        }

        public virtual void emoteFarmer_ExclamationMark()
        {
            this.emoteFarmer(4);
        }
        public virtual void emoteFarmer_Heart()
        {
            this.emoteFarmer(5);
        }
        public virtual void emoteFarmer_Sleeping()
        {
            this.emoteFarmer(6);
        }
        public virtual void emoteFarmer_Sad()
        {
            this.emoteFarmer(7);
        }
        public virtual void emoteFarmer_Happy()
        {
            this.emoteFarmer(8);
        }
        public virtual void emoteFarmer_No()
        {
            this.emoteFarmer(9);
        }
        public virtual void emoteFarmer_Pause()
        {
            this.emoteFarmer(10);
        }
        public virtual void emoteFarmer_Thinking()
        {
            this.emoteFarmer_Pause();
        }
        public virtual void emoteFarmer_Fishing()
        {
            this.emoteFarmer(11);
        }

        public virtual void emoteFarmer_CommunityCenterTablet()
        {
            this.emoteFarmer(12);
        }

        public virtual void emoteFarmer_Gaming()
        {
            this.emoteFarmer(13);
        }
        public virtual void emoteFarmer_MusicNote()
        {
            this.emoteFarmer(14);
        }
        public virtual void emoteFarmer_Blushing()
        {
            this.emoteFarmer(15);
        }
        public virtual void emote_Embarrased()
        {
            this.emoteFarmer_Blushing();
        }


        /// <summary>
        /// Ends the current event by fading out, then resumes the game world and places the player on the square where they entered the zone. All end parameters do this by default unless otherwise stated.
        /// </summary>
        public virtual void end()
        {
            StringBuilder b = new StringBuilder();
            b.Append("end");
            this.addEventData(b);
        }

        /// <summary>
        /// Same as end, and additionally clears the existing NPC dialogue for the day and replaces it with the line(s) specified at the end of the command. Example usage: end dialogue Abigail "It was fun talking to you today.$h"
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="Dialogue"></param>
        public virtual void endDialogue(NPC npc, string Dialogue)
        {
            StringBuilder b = new StringBuilder();
            b.Append("end dialogue ");
            b.Append(npc.Name);
            b.Append(" ");
            b.Append(Dialogue);
            this.addEventData(b);
        }

        /// <summary>
        /// Ends the event, sets npc dialogue, and warps the player out of the location.
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="Dialogue"></param>
        public virtual void endDialogueWarpOut(NPC npc, string Dialogue)
        {
            StringBuilder b = new StringBuilder();
            b.Append("end dialogueWarpOut ");
            b.Append(npc.Name);
            b.Append(" ");
            b.Append(Dialogue);
            this.addEventData(b);
        }

        /// <summary>
        /// Same as end, and additionally turns the specified NPC invisible (cannot be interacted with until the next day).
        /// </summary>
        /// <param name="npc"></param>

        public virtual void endInvisible(NPC npc)
        {
            StringBuilder b = new StringBuilder();
            b.Append("end invisible ");
            b.Append(npc.Name);
            b.Append(" ");
            this.addEventData(b);
        }

        public virtual void endInvisibleWarpOut(NPC npc)
        {
            StringBuilder b = new StringBuilder();
            b.Append("end invisibleWarpOut ");
            b.Append(npc.Name);
            b.Append(" ");
            this.addEventData(b);
        }

        /// <summary>
        /// Ends both the event and the day (warping player to their bed, saving the game, selling everything in the shipping box, etc).
        /// </summary>

        public virtual void endNewDay()
        {
            StringBuilder b = new StringBuilder();
            b.Append("end newDay");
            this.addEventData(b);
        }

        /// <summary>
        /// Same as end, and additionally warps the player to the map coordinates specified in x y.
        /// </summary>
        /// <param name="xTile"></param>
        /// <param name="yTile"></param>
        public virtual void endPosition(int xTile, int yTile)
        {
            StringBuilder b = new StringBuilder();
            b.Append("end position");
            b.Append(xTile);
            b.Append(" ");
            b.Append(yTile);
            this.addEventData(b);
        }

        public virtual void endWarpOut()
        {
            StringBuilder b = new StringBuilder();
            b.Append("end warpOut");
            this.addEventData(b);
        }

        public virtual void playerFaceDirection(FacingDirection Dir)
        {
            this.actorFaceDirection("farmer", Dir);
        }

        public virtual void npcFaceDirection(NPC NPC, FacingDirection Dir)
        {
            this.actorFaceDirection(NPC.Name, Dir);
        }

        public virtual void npcFaceDirection(string NPC, FacingDirection Dir)
        {
            this.actorFaceDirection(NPC, Dir);
        }

        public virtual void actorFaceDirection(string Actor, FacingDirection Dir)
        {
            StringBuilder b = new StringBuilder();
            b.Append("faceDirection ");
            b.Append(Actor);
            b.Append(" ");
            b.Append(this.getFacingDirectionNumber(Dir).ToString());
            b.Append(" ");
            b.Append(true);
            this.addEventData(b);
        }


        /// <summary>
        /// Special code to make junimos face a direction because it doesn't work the same as npcs.
        /// </summary>
        /// <param name="Actor">The name of the junimo actor.</param>
        /// <param name="Dir">The direction for the junimo to face.</param>
        public virtual void junimoFaceDirection(string Actor, FacingDirection Dir)
        {
            this.actorFaceDirection(Actor, Dir);
            int frame = 0;
            bool flip = false;
            if (Dir.Equals(FacingDirection.Down))
            {
                frame = 0;
            }
            else if (Dir.Equals(FacingDirection.Left))
            {
                frame = 16;
                flip = true;
            }
            else if (Dir.Equals(FacingDirection.Right))
            {
                frame = 16;
            }
            else if (Dir.Equals(FacingDirection.Up))
            {
                frame = 32;
            }
            this.animate(Actor, flip, true, 250, new List<int>() { frame, frame });
        }

        /// <summary>
        /// Fades out the screen.
        /// </summary>
        public virtual void fadeOut()
        {
            StringBuilder b = new StringBuilder();
            b.Append("fade true");
            this.addEventData(b);
        }

        public virtual void fadeIn()
        {
            StringBuilder b = new StringBuilder();
            b.Append("fade");
            this.addEventData(b);
        }

        /// <summary>
        /// Makes the farmer eat an object.
        /// </summary>
        /// <param name="ID"></param>
        public virtual void farmerEatObject(int ID)
        {
            StringBuilder b = new StringBuilder();
            b.Append("farmerEat ");
            b.Append(ID);
            this.addEventData(b);
        }

        //TODO: Support event forking.

        /// <summary>
        /// Add the given number of friendship points with the named NPC. (There are 250 points per heart.)
        /// </summary>
        /// <param name="NPC"></param>
        /// <param name="Amount"></param>
        public virtual void addFriendship(NPC NPC, int Amount)
        {
            StringBuilder b = new StringBuilder();
            b.Append("friendship ");
            b.Append(NPC.Name);
            b.Append(" ");
            b.Append(Amount);
            this.addEventData(b);
        }

        /// <summary>
        /// Fade to black at a particular speed (default 0.007). If no speed is specified, the event will continue immediately; otherwise, it will continue after the fade is finished. The fade effect disappears when this command is done; to avoid that, use the viewport command to move the camera off-screen.
        /// </summary>
        /// <param name="speed"></param>
        public virtual void globalFadeOut(double speed = 0.007)
        {
            StringBuilder b = new StringBuilder();
            b.Append("globalFade ");
            b.Append(speed);
            this.addEventData(b);
        }

        public virtual void globalFadeIn(double speed = 0.007)
        {
            StringBuilder b = new StringBuilder();
            b.Append("globalFadeToClear ");
            b.Append(speed);
            this.addEventData(b);
        }
        /// <summary>
        /// Fade to clear (unfade?) at a particular speed (default 0.007). If no speed is specified, the event will continue immediately; otherwise, it will continue after the fade is finished.
        /// </summary>
        /// <param name="speed"></param>
        public virtual void globalFadeToClear(double speed)
        {
            StringBuilder b = new StringBuilder();
            b.Append("globalFadeToClear ");
            b.Append(speed);
            this.addEventData(b);
        }

        /// <summary>
        /// Make the screen glow once, fading into and out of the <r> <g> <b> values over the course of a second. If <hold> is true it will fade to and hold that color until stopGlowing is used.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="Hold"></param>
        public virtual void glow(Color color, bool Hold)
        {
            StringBuilder b = new StringBuilder();
            b.Append("glow ");
            b.Append(color.R);
            b.Append(" ");
            b.Append(color.G);
            b.Append(" ");
            b.Append(color.B);
            b.Append(" ");
            b.Append(Hold);
            this.addEventData(b);
        }

        /// <summary>
        /// Stops npc movement
        /// </summary>
        public virtual void stopNPCMovement()
        {
            StringBuilder b = new StringBuilder();
            b.Append("halt");
            this.addEventData(b);
        }
        /// <summary>
        /// Stops npc movement.
        /// </summary>
        public virtual void halt()
        {
            this.stopNPCMovement();
        }

        /// <summary>
        /// Make a the named NPC jump. The default intensity is 8.
        /// </summary>
        /// <param name="ActorName"></param>
        /// <param name="Intensity"></param>
        public virtual void actorJump(string ActorName, int Intensity = 8)
        {
            StringBuilder b = new StringBuilder();
            b.Append("jump ");
            b.Append(Intensity);
            this.addEventData(b);
        }

        /// <summary>
        /// 	Queue a letter to be received tomorrow (see Content\Data\mail.xnb for available mail).
        /// </summary>
        /// <param name="LetterID"></param>
        public virtual void addMailForTomorrow(string LetterID)
        {
            StringBuilder b = new StringBuilder();
            b.Append("mail ");
            b.Append(LetterID);
            this.addEventData(b);
        }

        /// <summary>
        /// Show a dialogue box (no speaker). See dialogue format for the <text> format.
        /// </summary>
        /// <param name="Message"></param>
        public virtual void showMessage(string Message)
        {
            StringBuilder b = new StringBuilder();
            b.Append("message ");
           // b.Append("\\\"");
            b.Append(Message);
           // b.Append("\\\"");

            StardustCore.StardustCoreModCore.ModMonitor.Log(b.ToString());

            this.addEventData(b);
        }

        public virtual void showTranslatedMessage(string MessageKey)
        {
            StringBuilder b = new StringBuilder();

        }

        /// <summary>
        /// Move the given actor a certain amount of tiles.
        /// </summary>
        /// <param name="Actor"></param>
        /// <param name="xOffset"></param>
        /// <param name="yOffset"></param>
        /// <param name="Dir"></param>
        /// <param name="Continue"></param>
        public virtual void moveActor(string Actor, int xOffset, int yOffset, FacingDirection Dir, bool Continue)
        {
            StringBuilder b = new StringBuilder();
            b.Append("move ");
            b.Append(Actor);
            b.Append(" ");
            b.Append(xOffset);
            b.Append(" ");
            b.Append(yOffset);
            b.Append(" ");
            b.Append(this.getFacingDirectionNumber(Dir));
            b.Append(" ");
            b.Append(Continue);
            this.addEventData(b);
        }

        public virtual void moveActorUp(string Actor, int TileAmount, FacingDirection FinishingFacingDirection, bool EventDoesntPause)
        {
            this.moveActor(Actor, 0, -TileAmount, FinishingFacingDirection, EventDoesntPause);
        }

        public virtual void moveActorDown(string Actor, int TileAmount, FacingDirection FinishingFacingDirection, bool EventDoesntPause)
        {
            this.moveActor(Actor, 0, TileAmount, FinishingFacingDirection, EventDoesntPause);
        }

        public virtual void moveActorLeft(string Actor, int TileAmount, FacingDirection FinishingFacingDirection, bool EventDoesntPause)
        {
            this.moveActor(Actor, -TileAmount, 0, FinishingFacingDirection, EventDoesntPause);
        }

        public virtual void moveActorRight(string Actor, int TileAmount, FacingDirection FinishingFacingDirection, bool EventDoesntPause)
        {
            this.moveActor(Actor, TileAmount, 0, FinishingFacingDirection, EventDoesntPause);
        }

        /// <summary>
        /// Make a named NPC move by the given tile offset from their current position (along one axis only), and face the given direction when they're done. To move along multiple axes, you must specify multiple move commands. By default the event pauses while a move command is occurring, but if <continue> is set to true the movement is asynchronous and will run simultaneously with other event commands.
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="xOffset"></param>
        /// <param name="yOffset"></param>
        /// <param name="Dir"></param>
        /// <param name="Continue"></param>
        public virtual void moveNPC(NPC npc, int xOffset, int yOffset, FacingDirection Dir, bool Continue)
        {
            StringBuilder b = new StringBuilder();
            b.Append("move ");
            b.Append(npc.Name);
            b.Append(" ");
            b.Append(xOffset);
            b.Append(" ");
            b.Append(yOffset);
            b.Append(" ");
            b.Append(this.getFacingDirectionNumber(Dir));
            b.Append(" ");
            b.Append(Continue);
            this.addEventData(b);
        }

        public virtual void moveNPCUp(NPC npc, int TileAmount, FacingDirection FinishingFacingDirection, bool EventDoesntPause)
        {
            this.moveNPC(npc, 0, -TileAmount, FinishingFacingDirection, EventDoesntPause);
        }

        public virtual void moveNPCDown(NPC npc, int TileAmount, FacingDirection FinishingFacingDirection, bool EventDoesntPause)
        {
            this.moveNPC(npc, 0, TileAmount, FinishingFacingDirection, EventDoesntPause);
        }

        public virtual void moveNPCLeft(NPC npc, int TileAmount, FacingDirection FinishingFacingDirection, bool EventDoesntPause)
        {
            this.moveNPC(npc, -TileAmount, 0, FinishingFacingDirection, EventDoesntPause);
        }

        public virtual void moveNPCRight(NPC npc, int TileAmount, FacingDirection FinishingFacingDirection, bool EventDoesntPause)
        {
            this.moveNPC(npc, TileAmount, 0, FinishingFacingDirection, EventDoesntPause);
        }

        public virtual void moveFarmer(int xOffset, int yOffset, FacingDirection Dir, bool Continue)
        {
            StringBuilder b = new StringBuilder();
            b.Append("move ");
            b.Append("farmer");
            b.Append(" ");
            b.Append(xOffset);
            b.Append(" ");
            b.Append(yOffset);
            b.Append(" ");
            b.Append(this.getFacingDirectionNumber(Dir));
            b.Append(" ");
            b.Append(Continue);
            this.addEventData(b);
        }

        public virtual void moveFarmerUp(int TileAmount, FacingDirection FinishingFacingDirection, bool EventDoesntPause)
        {
            this.moveFarmer(0, -TileAmount, FinishingFacingDirection, EventDoesntPause);
        }
        public virtual void moveFarmerDown(int TileAmount, FacingDirection FinishingFacingDirection, bool EventDoesntPause)
        {
            this.moveFarmer(0, TileAmount, FinishingFacingDirection, EventDoesntPause);
        }

        public virtual void moveFarmerLeft(int TileAmount, FacingDirection FinishingFacingDirection, bool EventDoesntPause)
        {
            this.moveFarmer(-TileAmount, 0, FinishingFacingDirection, EventDoesntPause);
        }

        public virtual void moveFarmerRight(int TileAmount, FacingDirection FinishingFacingDirection, bool EventDoesntPause)
        {
            this.moveFarmer(TileAmount, 0, FinishingFacingDirection, EventDoesntPause);
        }



        /// <summary>
        /// Pause the game for the given number of milliseconds.
        /// </summary>
        /// <param name="Milliseconds"></param>
        public virtual void pauseGame(int Milliseconds)
        {
            StringBuilder b = new StringBuilder();
            b.Append("pause ");
            b.Append(Milliseconds);
            this.addEventData(b);
        }

        /// <summary>
        /// 	Play the specified music track ID. If the track is 'samBand', the track played will change depend on certain dialogue answers (76-79).
        /// </summary>
        /// <param name="id"></param>
        public virtual void playMusic(string id)
        {
            StringBuilder b = new StringBuilder();
            b.Append("playMusic ");
            b.Append(id);
            this.addEventData(b);
        }

        /// <summary>
        /// Play a given sound ID from the game's sound bank.
        /// </summary>
        /// <param name="id"></param>
        public virtual void playSound(string id)
        {
            StringBuilder b = new StringBuilder();
            b.Append("playSound ");
            b.Append(id);
            this.addEventData(b);
        }

        /// <summary>
        /// Give the player control back.
        /// </summary>
        public virtual void givePlayerControl()
        {
            StringBuilder b = new StringBuilder();
            b.Append("playSound ");
            this.addEventData(b);
        }

        /// <summary>
        /// Offset the position of the named NPC by the given number of pixels. This happens instantly, with no walking animation.
        /// </summary>
        /// <param name="ActorName"></param>
        /// <param name="PixelsX"></param>
        /// <param name="PixelsY"></param>
        public virtual void positionOffset(string ActorName, int PixelsX, int PixelsY)
        {
            StringBuilder b = new StringBuilder();
            b.Append("positionOffset ");
            b.Append(PixelsX);
            b.Append(" ");
            b.Append(PixelsY);
            this.addEventData(b);
        }

        /// <summary>
        /// Show a dialogue box with some answers and an optional question. When the player chooses an answer, the event script continues with no other effect.
        /// </summary>
        /// <param name="Question"></param>
        /// <param name="Answer1"></param>
        /// <param name="Answer2"></param>
        public virtual void questionNoFork(string Question, string Answer1, string Answer2)
        {
            StringBuilder b = new StringBuilder();
            b.Append("question null ");
            b.Append(Question);
            b.Append("#");
            b.Append(Answer1);
            b.Append("#");
            b.Append(Answer2);
            this.addEventData(b);
        }

        /// <summary>
        /// Remove the first of an object from a player's inventory.
        /// </summary>
        /// <param name="ParentSheetIndex"></param>
        public virtual void removeItem(int ParentSheetIndex)
        {

            StringBuilder b = new StringBuilder();
            b.Append("removeItem ");
            b.Append(ParentSheetIndex);
            this.addEventData(b);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TileX"></param>
        /// <param name="TileY"></param>
        public virtual void removeObject(int TileX, int TileY)
        {
            StringBuilder b = new StringBuilder();
            b.Append("removeObject ");
            b.Append(TileX);
            b.Append(" ");
            b.Append(TileY);
            this.addEventData(b);
        }

        /// <summary>
        /// Remove the specified quest from the quest log.
        /// </summary>
        /// <param name="ID"></param>
        public virtual void removeQuest(int ID)
        {
            StringBuilder b = new StringBuilder();
            b.Append("removeQuest ");
            b.Append(ID);
            this.addEventData(b);
        }

        /// <summary>
        /// Remove the temporary sprite at a position.
        /// </summary>
        /// <param name="XTile"></param>
        /// <param name="YTile"></param>
        public virtual void removeSprite(int XTile, int YTile)
        {
            StringBuilder b = new StringBuilder();
            b.Append("removeSprite ");
            b.Append(" ");
            b.Append(XTile);
            b.Append(" ");
            b.Append(YTile);
            this.addEventData(b);
        }

        /// <summary>
        /// Remove all temporary sprites.
        /// </summary>
        public virtual void removeTemporarySprites()
        {
            StringBuilder b = new StringBuilder();
            b.Append("removeTemporarySprites");
            this.addEventData(b);
        }

        /// <summary>
        /// Flashes the screen white for an instant. An alpha value from 0 to 1 adjusts the brightness, and values from 1 and out flashes pure white for x seconds.
        /// </summary>
        /// <param name="Alpha"></param>
        public virtual void screenFlash(float Alpha)
        {
            StringBuilder b = new StringBuilder();
            b.Append("screenFlash ");
            b.Append(Alpha);
            this.addEventData(b);
        }

        public virtual void setPlayerRunning()
        {
            StringBuilder b = new StringBuilder();
            b.Append("setRunning");
            this.addEventData(b);
        }

        public virtual void shakeNPC(NPC npc, int Milliseconds)
        {
            StringBuilder b = new StringBuilder();
            b.Append("shake ");
            b.Append(npc.Name);
            b.Append(" ");
            b.Append(Milliseconds);
            this.addEventData(b);
        }

        public virtual void shakeFarmer(int Milliseconds)
        {
            StringBuilder b = new StringBuilder();
            b.Append("shake ");
            b.Append("farmer");
            b.Append(" ");
            b.Append(Milliseconds);
            this.addEventData(b);
        }

        /// <summary>
        /// Set the named NPC's current frame in their Content\Characters\*.xnb spritesheet. Note that setting the farmer's sprite only changes parts of the sprite (some times arms, some times arms and legs and torso but not the head, etc). To rotate the whole sprite, use faceDirection farmer <0/1/2/3> first before modifying the sprite with showFrame. Frame ID starts from 0. If farmer is the one whose frame is being set, "farmer" can be eliminated, i.e. both showFrame farmer <frame ID> and showFrame <frame ID> would work.
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="FrameIndex"></param>
        public virtual void showNPCFrame(NPC npc, int FrameIndex)
        {
            StringBuilder b = new StringBuilder();
            b.Append("showFrame ");
            b.Append(npc.Name);
            b.Append(" ");
            b.Append(FrameIndex);
            this.addEventData(b);
        }

        /// <summary>
        /// Shows the given frame for the farmer. Forces the farmer to rotate first as specified by the wiki,
        /// </summary>
        /// <param name="Direction"></param>
        /// <param name="FrameIndex"></param>
        public virtual void showFrameFarmer(FacingDirection Direction, int FrameIndex)
        {
            this.playerFaceDirection(Direction);
            StringBuilder b = new StringBuilder();
            b.Append("showFrame ");
            b.Append(FrameIndex);
            this.addEventData(b);
        }

        //Skippable is enabled by default.


        /// <summary>
        /// Show dialogue text from a named NPC; see dialogue format.
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="Message"></param>
        public virtual void speak(NPC npc, string Message)
        {
            StringBuilder b = new StringBuilder();
            b.Append("speak ");
            b.Append(npc.Name);
            b.Append(" ");
            b.Append('"');
            b.Append(Message);
            b.Append('"');
            this.addEventData(b);
        }

        public virtual void speak(string npcName, string Message)
        {
            StringBuilder b = new StringBuilder();
            b.Append("speak ");
            b.Append(npcName);
            b.Append(" ");
            b.Append('"');
            b.Append(Message);
            b.Append('"');
            this.addEventData(b);
        }

        /// <summary>
        /// 	Make the player start jittering.
        /// </summary>
        public virtual void playerJitter()
        {
            StringBuilder b = new StringBuilder();
            b.Append("startJittering");
            this.addEventData(b);
        }

        /// <summary>
        /// 	Stop movement from advancedMove.
        /// </summary>
        public virtual void stopAdvancedMoves()
        {
            StringBuilder b = new StringBuilder();
            b.Append("stopAdvancedMoves");
            this.addEventData(b);
        }

        /// <summary>
        /// 	Stop the farmer's current animation.
        /// </summary>
        public virtual void stopFarmerAnimation()
        {
            StringBuilder b = new StringBuilder();
            b.Append("stopAnimation farmer");
            this.addEventData(b);
        }

        /// <summary>
        /// Stop the named NPC's current animation. Not applicable to the farmer.
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="EndFrame"></param>
        public virtual void stopNPCAnimation(NPC npc, int EndFrame)
        {
            StringBuilder b = new StringBuilder();
            b.Append("stopAnimation ");
            b.Append(npc.Name);
            b.Append(" ");
            b.Append(EndFrame);
            this.addEventData(b);
        }

        /// <summary>
        /// 	Make the screen stop glowing.
        /// </summary>
        public virtual void stopGlowing()
        {
            StringBuilder b = new StringBuilder();
            b.Append("stopGlowing");
            this.addEventData(b);
        }

        /// <summary>
        /// 	Make the player stop jittering.
        /// </summary>
        public virtual void stopJittering()
        {
            StringBuilder b = new StringBuilder();
            b.Append("stopJittering");
            this.addEventData(b);
        }

        /// <summary>
        /// 	Stop any currently playing music.
        /// </summary>
        public virtual void stopMusic()
        {
            StringBuilder b = new StringBuilder();
            b.Append("stopMusic");
            this.addEventData(b);
        }

        /// <summary>
        /// 	Make the farmer stop running.
        /// </summary>
        public virtual void stopRunning()
        {
            StringBuilder b = new StringBuilder();
            b.Append("stopRunning");
            this.addEventData(b);
        }

        /// <summary>
        /// 	Make the farmer stop swimming.
        /// </summary>
        public virtual void stopFarmerSwimming()
        {
            StringBuilder b = new StringBuilder();
            b.Append("stopSwimming farmer");
            this.addEventData(b);
        }

        /// <summary>
        /// 	Make the npc stop swimming.
        /// </summary>
        public virtual void stopNPCSwimming(NPC npc)
        {
            StringBuilder b = new StringBuilder();
            b.Append("stopSwimming ");
            b.Append(npc.Name);
            this.addEventData(b);
        }

        /// <summary>
        /// 	Make the farmer start swimming.
        /// </summary>
        public virtual void startFarmerSwimming()
        {
            StringBuilder b = new StringBuilder();
            b.Append("swimming farmer");
            this.addEventData(b);
        }


        /// <summary>
        /// Make the npc start swimming.
        /// </summary>
        /// <param name="npc"></param>
        public virtual void startNPCSwimming(NPC npc)
        {
            StringBuilder b = new StringBuilder();
            b.Append("swimming ");
            b.Append(npc.Name);
            this.addEventData(b);
        }

        /// <summary>
        /// 	Changes the current event (ie. event commands) to another event in the same location.
        /// </summary>
        /// <param name="ID"></param>
        public virtual void switchEvent(int ID)
        {
            StringBuilder b = new StringBuilder();
            b.Append("switchEvent ");
            b.Append(ID);
            this.addEventData(b);
        }


        /// <summary>
        /// Create a temporary sprite with the given parameters.
        /// </summary>
        /// <param name="XPos"></param>
        /// <param name="YPos"></param>
        /// <param name="Index"></param>
        /// <param name="AnimationLength"></param>
        /// <param name="AnimationInterval"></param>
        /// <param name="Looped"></param>
        /// <param name="LoopCount"></param>
        public virtual void temporarySprite(int XPos, int YPos, int Index, int AnimationLength, int AnimationInterval, bool Looped, int LoopCount)
        {
            StringBuilder b = new StringBuilder();
            b.Append("temporarySprite ");
            b.Append(XPos);
            b.Append(" ");
            b.Append(YPos);
            b.Append(" ");
            b.Append(Index);
            b.Append(" ");
            b.Append(AnimationLength);
            b.Append(" ");
            b.Append(AnimationInterval);
            b.Append(" ");
            b.Append(Looped);
            b.Append(" ");
            b.Append(LoopCount);
            this.addEventData(b);
        }

        /// <summary>
        /// 	Show a small text bubble over the named NPC's head with the given text; see dialogue format.
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="Message"></param>
        public virtual void showSpeechBubble(NPC npc, string Message)
        {
            StringBuilder b = new StringBuilder();
            b.Append("textAboveHead ");
            b.Append(npc.Name);
            b.Append(" ");
            b.Append(Message);
            this.addEventData(b);
        }

        /// <summary>
        /// Pan the the camera in the direction (and with the velocity) defined by x/y for the given duration in milliseconds. Example: "viewport move 2 -1 5000" moves the camera 2 pixels right and 1 pixel up for 5 seconds.
        /// </summary>
        /// <param name="xPixelAmount"></param>
        /// <param name="yPixelAmount"></param>
        /// <param name="MillisecondDuration"></param>
        public virtual void panViewport(int xPixelAmount, int yPixelAmount, int MillisecondDuration)
        {
            StringBuilder b = new StringBuilder();
            b.Append("viewport move ");
            b.Append(xPixelAmount);
            b.Append(" ");
            b.Append(yPixelAmount);
            b.Append(" ");
            b.Append(MillisecondDuration);
            this.addEventData(b);
        }

        /// <summary>
        /// Instantly reposition the camera to center on the given X, Y tile position. TODO: explain other parameters.
        /// </summary>
        /// <param name="XPosition"></param>
        /// <param name="YPosition"></param>
        public virtual void setViewportPosition(int XPosition, int YPosition)
        {
            StringBuilder b = new StringBuilder();
            b.Append("viewport ");
            b.Append(XPosition);
            b.Append(" ");
            b.Append(YPosition);
            b.Append(" ");
            b.Append("true");
            this.addEventData(b);
        }

        /// <summary>
        /// Wait for other players (vanilla MP).
        /// </summary>
        public virtual void waitForAllPlayers()
        {
            StringBuilder b = new StringBuilder();
            b.Append("waitForOtherPlayers");
            this.addEventData(b);
        }

        /// <summary>
        /// Warp the named NPC to a position to the given X, Y tile coordinate. This can be used to warp characters off-screen.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public virtual void warpPlayer(int x, int y)
        {
            StringBuilder b = new StringBuilder();
            b.Append("warp farmer ");
            b.Append(x);
            b.Append(" ");
            b.Append(y);
            this.addEventData(b);
        }

        /// <summary>
        /// Warp the named NPC to a position to the given X, Y tile coordinate. This can be used to warp characters off-screen.
        /// </summary>
        /// <param name="ActorName"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public virtual void warpActor(string ActorName, int x, int y)
        {
            StringBuilder b = new StringBuilder();
            b.Append("warp ");
            b.Append(ActorName);
            b.Append(" ");
            b.Append(x);
            b.Append(" ");
            b.Append(y);
            this.addEventData(b);
        }

        public virtual void warpNPC(NPC NPC, int x, int y)
        {
            StringBuilder b = new StringBuilder();
            b.Append("warp ");
            b.Append(NPC.Name);
            b.Append(" ");
            b.Append(x);
            b.Append(" ");
            b.Append(y);
            this.addEventData(b);
        }
    }
}
