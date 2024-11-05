using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Netcode;
using Omegasis.StardustCore.Utilities;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Characters;

namespace Omegasis.StardustCore.Events
{
    /// <summary>
    /// Contains functions that are used to parse event data and do additional things.
    /// </summary>
    public class ExtraEventActions
    {

        public static Dictionary<string, JunimoAdvanceMoveData> junimoLerpData = new Dictionary<string, JunimoAdvanceMoveData>();


        public static void addObjectToPlayerInventory(Event gameEvent, string[] Args, EventContext eventContext)
        {
            string name = Args[0];
            //On the off chance this is an int for legacy reasons, put it back into a string.
            string qualifiedItemId = Convert.ToString(Args[1]);
            if (!ItemRegistry.IsQualifiedItemId(qualifiedItemId))
            {
                qualifiedItemId = ItemRegistry.QualifyItemId(qualifiedItemId);
            }
            int amount = Convert.ToInt32(Args[2]);
            bool makeActiveObject = Convert.ToBoolean(Args[3]);
            StardewValley.Object obj = ItemRegistry.Create<StardewValley.Object>(qualifiedItemId, amount);
            Game1.player.addItemToInventoryBool(obj, makeActiveObject);
            Game1.CurrentEvent.CurrentCommand++;
        }

        /// <summary>
        /// Adds in a junimo actor at the current location. Allows for multiple.
        /// </summary>
        /// <param name="EventManager"></param>
        /// <param name="EventData"></param>
        public static void AddInJumimoActorForEvent(Event gameEvent, string[] Args, EventContext eventContext)
        {
            string name = Args[0];

            string actorName = Args[1];
            int xPos = Convert.ToInt32(Args[2]);
            int yPos = Convert.ToInt32(Args[3]);
            Color color = new Color(Convert.ToInt32(Args[4]), Convert.ToInt32(Args[5]), Convert.ToInt32(Args[6]));
            bool flipped = Convert.ToBoolean(Args[7]);

            List<NPC> actors = Game1.CurrentEvent.actors;
            Junimo junimo = new Junimo(new Vector2(xPos * 64, yPos * 64), -1, false);
            junimo.Name = actorName;
            junimo.EventActor = true;
            junimo.flip = flipped;

            IReflectedField<NetColor> colorF=StardustCore.StardustCoreModCore.ModHelper.Reflection.GetField<NetColor>(junimo, "color", true);
            NetColor c = colorF.GetValue();
            c.R = color.R;
            c.G = color.G;
            c.B = color.B;
            colorF.SetValue(c);

            actors.Add((NPC)junimo);
            ++Game1.CurrentEvent.CurrentCommand; //I've been told ++<int> is more efficient than <int>++;
        }

        /// <summary>
        /// Flip a given junimo actor. Necessary to make junimos face left.
        /// </summary>
        /// <param name="EventManager"></param>
        /// <param name="EventData"></param>
        public static void FlipJunimoActor(Event gameEvent, string[] Args, EventContext eventContext)
        {
            string name = Args[0];
            string actorName = Args[1];
            bool flipped = Convert.ToBoolean(Args[2]);
            NPC junimo=Game1.CurrentEvent.actors.Find(i => i.Name.Equals(actorName));
            junimo.flip = flipped;
            ++Game1.CurrentEvent.CurrentCommand; //I've been told ++<int> is more efficient than <int>++;
        }

        /// <summary>
        /// Adds the concurrent event to handle junimo movement.
        /// </summary>
        /// <param name="EventManager"></param>
        /// <param name="EventData"></param>
        public static void SetUpAdvanceJunimoMovement(Event gameEvent, string[] Args, EventContext eventContext)
        {
            string name = Args[0];
            ++Game1.CurrentEvent.CurrentCommand; //I've been told ++<int> is more efficient than <int>++;
            EventManager.Instance.addConcurrentEvent(new ConcurrentEventInformation("AdvanceJunimoMove",Args, AdvanceJunimoMovement));
        }

        /// <summary>
        /// Finishes handling advvance junimo movement.
        /// </summary>
        /// <param name="EventManager"></param>
        /// <param name="EventData"></param>
        public static void FinishAdvanceJunimoMovement(Event gameEvent, string[] Args, EventContext eventContext)
        {
            ++Game1.CurrentEvent.CurrentCommand; //I've been told ++<int> is more efficient than <int>++;
            EventManager.Instance.finishConcurrentEvent("AdvanceJunimoMove");
        }

        public static void AddInJunimoAdvanceMove(Event gameEvent, string[] Args, EventContext eventContext)
        {

            if (EventManager.Instance.concurrentEventActions.ContainsKey("Omegasis.EventFramework.SetUpAdvanceJunimoMovement")==false)
            {
                EventManager.Instance.addConcurrentEvent(new ConcurrentEventInformation("AdvanceJunimoMove",null, AdvanceJunimoMovement));
            }
            string name = Args[0];

            string actorName = Args[1];
            int MaxFrames = Convert.ToInt32(Args[2]);
            int Speed = Convert.ToInt32(Args[3]);
            bool Loop = Convert.ToBoolean(Args[4]);

            List<Point> points = new List<Point>();
            for(int i = 5; i < Args.Length; i++)
            {
                string pointData = Args[i];
                string[] point = pointData.Split('_');
                int x = Convert.ToInt32(point[0]);
                int y = Convert.ToInt32(point[1]);
                points.Add(new Point(x, y));
            }

            junimoLerpData.Add(actorName, new JunimoAdvanceMoveData(actorName,points,MaxFrames,Speed,Loop));

            ++Game1.CurrentEvent.CurrentCommand; //I've been told ++<int> is more efficient than <int>++;
        }

        /// <summary>
        /// Updates all of the junimo movement logic.
        /// </summary>
        /// <param name="EventManager"></param>
        /// <param name="EventData"></param>
        public static void AdvanceJunimoMovement(Event gameEvent, string[] Args, EventContext eventContext)
        {
            foreach(KeyValuePair<string,JunimoAdvanceMoveData> pair in junimoLerpData)
            {
                pair.Value.update();
            }
        }

        /// <summary>
        /// Removes, aka stops the junimo actor from doing their advance movement.
        /// </summary>
        /// <param name="EventManager"></param>
        /// <param name="EventData"></param>
        public static void RemoveAdvanceJunimoMovement(Event gameEvent, string[] Args, EventContext eventContext)
        {
            string name = Args[0];
            string actorName = Args[1];
            if (junimoLerpData.ContainsKey(actorName))
            {
                junimoLerpData.Remove(actorName);
            }

            ++Game1.CurrentEvent.CurrentCommand; //I've been told ++<int> is more efficient than <int>++;
        }
    }
}
