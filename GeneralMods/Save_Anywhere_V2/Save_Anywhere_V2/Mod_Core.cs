using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using StardewModdingAPI.Events;
using System.Reflection;
using System.Globalization;
using System.IO;
using StardewValley.Characters;
using Microsoft.Xna.Framework;
using Save_Anywhere_V2.Save_Utilities;

namespace Save_Anywhere_V2
{
    public class Mod_Core : StardewModdingAPI.Mod
    {
        
        public static string mod_path;
        public static string player_path;
        public static string animal_path;
        public static string npc_path;
        public static bool npc_warp;
        public static int checking_time;
        public static bool once;
        public static bool new_day;
        public static bool anywhereSaved = false;
        Dictionary<string, string> npc_key_value_pair;

        public static IMonitor thisMonitor;

        public override void Entry(IModHelper helper)
        {
            try {
                StardewModdingAPI.Events.ControlEvents.KeyPressed += KeyPressed_Save_Load_Menu;
                StardewModdingAPI.Events.SaveEvents.AfterLoad += PlayerEvents_LoadedGame;
                StardewModdingAPI.SaveEvents.AfterSave += SaveEvents_AfterSave;
                StardewModdingAPI.Events.GameEvents.UpdateTick += Warp_Check;
                StardewModdingAPI.Events.TimeEvents.TimeOfDayChanged += NPC_scheduel_update;
                StardewModdingAPI.Events.TimeEvents.AfterDayStarted += TimeEvents_DayOfMonthChanged;
                StardewModdingAPI.Events.TimeEvents.AfterDayStarted += TimeEvents_OnNewDay;
                mod_path = Helper.DirectoryPath;
                npc_key_value_pair = new Dictionary<string, string>();
                thisMonitor = Monitor;
            }
            catch(Exception x)
            {
                Monitor.Log(x.ToString());
            }
        }

        private void SaveEvents_AfterSave(object sender, EventArgs e)
        {
            if (anywhereSaved)
            {
                Monitor.Log("Anywhere saved.");
                anywhereSaved = false;
            }
            else
            {
                Monitor.Log("Game normal saved.");
                var name = Game1.player.name;
                var player_path = Path.Combine(mod_path, "Save_Data", name);
                Directory.Delete(player_path, true);
                Monitor.Log("Anywhere saved data cleared.");
            }
        }

        //done
        private void TimeEvents_OnNewDay(object sender, EventArgs e)
        {
            try {
                //Log.Info("Day of Month Changed");
                new_day = true;
                string name = Game1.player.name;
                Save_Anywhere_V2.Mod_Core.player_path = Path.Combine(Save_Anywhere_V2.Mod_Core.mod_path, "Save_Data", name);
              
            }
            catch(Exception err)
            {
                Monitor.Log(err.ToString());
            }
        }

        //done
        private void TimeEvents_DayOfMonthChanged(object sender, EventArgs e)
        {
            try {
                //new_day = true;
                // Log.Info("Day of Month Changed");
                npc_key_value_pair.Clear();
                foreach (var loc in Game1.locations)
                {
                    foreach (var character in loc.characters)
                    {

                       if(!npc_key_value_pair.ContainsKey(character.name)) npc_key_value_pair.Add(character.name, parseSchedule(character));
                      //  Monitor.Log(parseSchedule(character));
                    }
                }
            }
            catch(Exception err)
            {
                Monitor.Log(err.ToString());
            }
        }

        private void NPC_scheduel_update(object sender, EventArgs e)
        {

            if (Game1.weatherIcon == 4)
            {
                return;
            }
            if (Game1.isFestival() == true)
            {
                return;
            }
            if (Game1.eventUp == true)
            {
                return;
            }
                //if (once == true) return;
                //FieldInfo field = typeof(NPC).GetField("scheduleTimeToTry", BindingFlags.NonPublic | BindingFlags.Instance);
                // MethodInfo dynMethod = typeof(NPC).GetMethod("prepareToDisembarkOnNewSchedulePath",BindingFlags.NonPublic | BindingFlags.Instance);
                MethodInfo dynMethod2 = typeof(NPC).GetMethod("pathfindToNextScheduleLocation", BindingFlags.NonPublic | BindingFlags.Instance);

                if (npc_warp == false) return;
                if (new_day == true) return;
                List<StardewValley.Characters.Child> child_list = new List<StardewValley.Characters.Child>();
                child_list = StardewValley.Game1.player.getChildren();
                foreach (var loc in Game1.locations)
                {
                    foreach (var npc in loc.characters)
                    {
                        if (npc.DirectionsToNewLocation != null) continue;
                        if (npc.isMoving() == true) continue;
                        if (npc.Schedule == null) continue;
                    if (npc.controller != null) continue;
                        foreach (var child_name in child_list)
                        {
                            if (npc.name == child_name.name) continue;
                        }
                        if (npc.name == Game1.player.getPetName()) continue;
                        Horse horse = StardewValley.Utility.findHorse();
                        if (horse != null)
                        {
                            if (npc.name == horse.name) continue;
                        }
                        // Log.Info("THIS IS MY NPC" + npc.name);
                        //  Monitor.Log("NO SCHEDULE FOUND FOR " + npc.name);


                        //  npc.checkSchedule(Game1.timeOfDay);
                        SchedulePathDescription schedulePathDescription;

                        //int myint = (int)field.GetValue(npc);
                        /*
                        npc.Schedule.TryGetValue(Game1.timeOfDay, out schedulePathDescription);
                        int i = 0;
                        int pseudo_time=0;

                        while (schedulePathDescription == null)
                        {
                            i += 10;
                            pseudo_time = Game1.timeOfDay - i;
                            if (pseudo_time <= 600) { break; }
                            npc.Schedule.TryGetValue(pseudo_time, out schedulePathDescription);
                            checking_time = pseudo_time;
                        }
                        // npc.directionsToNewLocation = schedulePathDescription;
                        // npc.prepareToDisembarkOnNewSchedulePath();

                        //field.SetValue(npc, 9999999);

                        npc.DirectionsToNewLocation = schedulePathDescription;
                       */
                        //////////////////////////////////////////
                        // Log.Info("Does this break here 1");
                        Dictionary<string, string> dictionary;
                        string key_value = "";
                        try
                        {
                            dictionary = Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + npc.name);
                        }
                        catch (Exception ex)
                        {
                        this.Monitor.Log(ex.ToString(), LogLevel.Error);
                            // dictionary = new Dictionary<string, string>();//(Dictionary<int, SchedulePathDescription>)null;
                            continue;
                        }
                        // Log.Info("Does this break here 2");
                        //////////////////////
                        string value;
                        string end_map;
                        int x;
                        int y;
                        int end_dir;
                        npc_key_value_pair.TryGetValue(npc.name, out key_value);
                        if (key_value == "" || key_value == null)
                        {
                            Monitor.Log("THIS IS AWKWARD");
                            continue;
                        }
                        dictionary.TryGetValue(key_value, out value);
                        // Log.Info("Does this break here 3");
                        string[] valueArray1 = value.Split('/');
                        int count1 = 0;
                        foreach (var josh in valueArray1)
                        {
                            string[] valueArray2 = valueArray1[count1].Split(' ');


                        if (valueArray2.Contains("GOTO"))
                        {

                            for (int i = 0; i < valueArray2.Length; i++)
                            {
                                string s = valueArray2.ElementAt(i);
                                if (s == "GOTO")
                                {
                                    dictionary.TryGetValue(valueArray2.ElementAt(i + 1), out value);
                                    // Log.Info("Does this break here 3");
                                    string[] valueArray3 = value.Split('/');
                                    int count10 = 0;
                                    
                                        string[] valueArray4 = valueArray3[count10].Split(' ');
                                        valueArray2 = valueArray4;
                                    
                                }
                            }
                        }

                        try {
                            if (Convert.ToInt32(valueArray2.ElementAt(0)) > Game1.timeOfDay) break;
                            end_map = Convert.ToString(valueArray2.ElementAt(1));
                            x = Convert.ToInt32(valueArray2.ElementAt(2));
                            y = Convert.ToInt32(valueArray2.ElementAt(3));
                            end_dir = Convert.ToInt32(valueArray2.ElementAt(4));
                            //MOST RELIABLE    
                            schedulePathDescription = (SchedulePathDescription)dynMethod2.Invoke(npc, new object[] { npc.currentLocation.name, npc.getTileX(), npc.getTileY(), end_map, x, y, end_dir, null, null });
                            
                            //FASTEST
                             //schedulePathDescription =  pathfindToNextScheduleLocation(npc,npc.currentLocation.name, npc.getTileX(), npc.getTileY(), end_map, x, y, end_dir, null, null );
                            count1++;
                        }
                        catch (Exception err)
                        {
                            this.Monitor.Log(err.ToString(), LogLevel.Error);
                            // Monitor.Log(npc.name);
                            foreach (var v in valueArray2)
                            {
                                //Monitor.Log(v);
                            }
                            schedulePathDescription = null;
                           // Monitor.Log(err);
                        }

                            if (schedulePathDescription == null) continue;
                            //  Log.Info("This works 2");
                            // Utility.getGameLocationOfCharacter(npc);
                            //  Log.Info("This works 3");

                            npc.DirectionsToNewLocation = schedulePathDescription;
                            npc.controller = new PathFindController(npc.DirectionsToNewLocation.route, (Character)npc, Utility.getGameLocationOfCharacter(npc))
                            {
                                finalFacingDirection = npc.DirectionsToNewLocation.facingDirection,
                                endBehaviorFunction = null//npc.getRouteEndBehaviorFunction(npc.DirectionsToNewLocation.endOfRouteBehavior, npc.DirectionsToNewLocation.endOfRouteMessage)
                            };

                        }
                    }
                }
                once = true;
        }


        //done
        /*
        private void NPC_scheduel_update(object sender, EventArgs e)
        {
            /*
            foreach (var key in npc_key_value_pair)
            {
                NPC npc = Game1.getCharacterFromName(key.Key);
                Monitor.Log(npc.name);
                Dictionary<int, SchedulePathDescription> sch =npc.getSchedule(Game1.dayOfMonth);
                if (sch == null) continue;
                foreach (var ehh in sch)
                {
                    Monitor.Log(ehh.Key);
                    Monitor.Log(ehh.Value);
                }
            }


            return;
            
            //if (once == true) return;
            //FieldInfo field = typeof(NPC).GetField("scheduleTimeToTry", BindingFlags.NonPublic | BindingFlags.Instance);
            // MethodInfo dynMethod = typeof(NPC).GetMethod("prepareToDisembarkOnNewSchedulePath",BindingFlags.NonPublic | BindingFlags.Instance);
            //  MethodInfo dynMethod2 = typeof(NPC).GetMethod("pathfindToNextScheduleLocation", BindingFlags.NonPublic | BindingFlags.Instance);

            if (npc_warp == false)
            {
                Monitor.Log("LOL WHUT");
                return;
            }
            if (new_day == true)
            {
                Monitor.Log("Interesting");
                return;
            }
                List<StardewValley.Characters.Child> child_list = new List<StardewValley.Characters.Child>();
                child_list = StardewValley.Game1.player.getChildren();
                foreach (var loc in Game1.locations)
                {
                foreach (var npc in loc.characters)
                {
                    // if (npc.DirectionsToNewLocation != null) continue;
                    if (npc.isMoving() == true)
                    {
                        Monitor.Log("I AM MOVING");
                        continue;
                    }
                    //if (npc.Schedule == null) continue;
                    foreach (var child_name in child_list)
                    {
                        if (npc.name == child_name.name)
                        {
                            Monitor.Log("I AM A CHILD");
                            continue;
                        }
                    }
                    if (Game1.player.hasPet() == true) {
                        if (npc.name == Game1.player.getPetName())
                        {
                            Monitor.Log("I AM A PET");
                            continue;
                        }
                    }
                    Horse horse = StardewValley.Utility.findHorse();


                    if (horse != null)
                    {
                        if (npc.name == horse.name) continue;
                    }
                    Log.AsyncR("AM I GETTING TO STEP 1?");

                   // System.Threading.Thread.Sleep(1000);


                    // Log.Info("THIS IS MY NPC" + npc.name);
                    //  Monitor.Log("NO SCHEDULE FOUND FOR " + npc.name);


                    //  npc.checkSchedule(Game1.timeOfDay);
                    SchedulePathDescription schedulePathDescription;

                    //int myint = (int)field.GetValue(npc);
                    
                    npc.Schedule.TryGetValue(Game1.timeOfDay, out schedulePathDescription);
                    int i = 0;
                    int pseudo_time=0;

                    while (schedulePathDescription == null)
                    {
                        i += 10;
                        pseudo_time = Game1.timeOfDay - i;
                        if (pseudo_time <= 600) { break; }
                        npc.Schedule.TryGetValue(pseudo_time, out schedulePathDescription);
                        checking_time = pseudo_time;
                    }
                    // npc.directionsToNewLocation = schedulePathDescription;
                    // npc.prepareToDisembarkOnNewSchedulePath();

                    //field.SetValue(npc, 9999999);

                    npc.DirectionsToNewLocation = schedulePathDescription;
                   
                    //////////////////////////////////////////
                    // Log.Info("Does this break here 1");
                    Dictionary<string, string> dictionary;
                    string key_value = "";
                    try
                    {
                        dictionary = Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + npc.name);
                    }
                    catch (Exception ex)
                    {
                        // dictionary = new Dictionary<string, string>();//(Dictionary<int, SchedulePathDescription>)null;
                        //Monitor.Log(ex);
                        //Monitor.Log("YOU FIX THIS NOW");
                        continue;
                    }
                    // Log.Info("Does this break here 2");
                    //////////////////////
                    string value;
                    string end_map;
                    int x;
                    int y;
                    int end_dir;
                    string behavior;
                    string message;
                    try {
                        npc_key_value_pair.TryGetValue(npc.name, out key_value);
                        if (key_value == "" || key_value == null)
                        {
                            Monitor.Log("NO KEYBLADE");
                            continue;
                        }
                        dictionary.TryGetValue(key_value, out value);

                        //Log.AsyncO(value);
                        // Log.Info("Does this break here 3");
                        string[] valueArray1 = value.Split('/');
                        int count1 = 0;
                        foreach (var josh in valueArray1)
                        {
                            Log.AsyncR("RAWRRRRR");
                            string[] valueArray2 = valueArray1[count1].Split(' ');

                            if (Convert.ToInt32(valueArray2.ElementAt(0)) > Game1.timeOfDay) break;
                            end_map = Convert.ToString(valueArray2.ElementAt(1));
                            x = Convert.ToInt32(valueArray2.ElementAt(2));
                            y = Convert.ToInt32(valueArray2.ElementAt(3));
                            end_dir = Convert.ToInt32(valueArray2.ElementAt(4));
                            schedulePathDescription = pathfindToNextScheduleLocation(npc, npc.currentLocation.name, npc.getTileX(), npc.getTileY(), end_map, x, y, end_dir, null, null);
                            count1++;


                            if (schedulePathDescription == null)
                            {
                                Monitor.Log("WHY???");
                            }
                            //  Log.Info("This works 2");
                            // Utility.getGameLocationOfCharacter(npc);
                            //  Log.Info("This works 3");

                            npc.DirectionsToNewLocation = schedulePathDescription;
                            npc.controller = new PathFindController(npc.DirectionsToNewLocation.route, (Character)npc, Utility.getGameLocationOfCharacter(npc))
                            {
                                finalFacingDirection = npc.DirectionsToNewLocation.facingDirection,
                                endBehaviorFunction = null//npc.getRouteEndBehaviorFunction(npc.DirectionsToNewLocation.endOfRouteBehavior, npc.DirectionsToNewLocation.endOfRouteMessage)
                            };
                            if (npc.controller == null)
                            {
                                Log.AsyncR("CRY");
                            }
                            Monitor.Log("IS THIS RUNNING?");
                            if (npc.name == "Shane") Monitor.Log("IS THIS RUNNING WITH BOOZE?");
                            npc.warpToPathControllerDestination();
                        }
                    }
                    catch(Exception err)
                    {
                       // Monitor.Log(err);
                        continue;
                    }
                    }
                }
                //once = true;
    
        }
        */


        //done
        private string get_key_value(NPC npc)
        {
            try
            {


                Dictionary<string, string> dictionary;
                string key_value = "";
                try
                {
                    dictionary = Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + npc.name);
                }
                catch (Exception ex)
                {
                    this.Monitor.Log(ex.ToString(), LogLevel.Error);
                    dictionary = new Dictionary<string, string>();//(Dictionary<int, SchedulePathDescription>)null;
                }
                if (dictionary.ContainsKey(Game1.currentSeason + "_" + Convert.ToString(Game1.dayOfMonth)))
                    key_value = Game1.currentSeason + "_" + Convert.ToString(Game1.dayOfMonth);
                for (int index = !Game1.player.friendships.ContainsKey(npc.name) ? -1 : Game1.player.friendships[npc.name][0] / 250; index > 0; --index)
                {
                    if (dictionary.ContainsKey(Convert.ToString(Game1.dayOfMonth) + "_" + Convert.ToString(index)))
                        key_value = Convert.ToString(Game1.dayOfMonth) + "_" + Convert.ToString(index);
                }
                if (dictionary.ContainsKey(string.Empty + (object)Game1.dayOfMonth))
                    key_value = string.Empty + (object)Game1.dayOfMonth;
                if (npc.name.Equals("Pam") && Game1.player.mailReceived.Contains("ccVault"))
                    key_value = "bus";
                if (Game1.isRaining)
                {
                    if (Game1.random.NextDouble() < 0.5 && dictionary.ContainsKey("rain2"))
                        key_value = "rain2";
                    if (dictionary.ContainsKey("rain"))
                        key_value = "rain";
                }
                List<string> list = new List<string>()
      {
        Game1.currentSeason,
        Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)
      };
                int num1 = !Game1.player.friendships.ContainsKey(npc.name) ? -1 : Game1.player.friendships[npc.name][0] / 250;
                while (num1 > 0)
                {
                    list.Add(string.Empty + (object)num1);
                    if (dictionary.ContainsKey(string.Join("_", (IEnumerable<string>)list)))
                        key_value = string.Join("_", (IEnumerable<string>)list);
                    --num1;
                    list.RemoveAt(Enumerable.Count<string>((IEnumerable<string>)list) - 1);
                }
                if (dictionary.ContainsKey(string.Join("_", (IEnumerable<string>)list)))
                    key_value = string.Join("_", (IEnumerable<string>)list);
                if (dictionary.ContainsKey(Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)))
                    key_value = Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth);
                if (dictionary.ContainsKey(Game1.currentSeason))
                    key_value = Game1.currentSeason;
                if (dictionary.ContainsKey("spring_" + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)))
                    key_value = "spring_" + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth);
                list.RemoveAt(Enumerable.Count<string>((IEnumerable<string>)list) - 1);
                list.Add("spring");
                int num2 = !Game1.player.friendships.ContainsKey(npc.name) ? -1 : Game1.player.friendships[npc.name][0] / 250;
                while (num2 > 0)
                {
                    list.Add(string.Empty + (object)num2);
                    if (dictionary.ContainsKey(string.Join("_", (IEnumerable<string>)list)))
                        key_value = string.Join("_", (IEnumerable<string>)list);
                    --num2;
                    list.RemoveAt(Enumerable.Count<string>((IEnumerable<string>)list) - 1);
                }
                if (dictionary.ContainsKey("spring"))
                    key_value = "spring";

                return key_value;
            }
            catch(Exception err)
            {
                Monitor.Log(err.ToString());
                return null;
            }
        }


        private string parseSchedule(NPC npc)
        {
            if (npc.name.Equals("Robin") || Game1.player.currentUpgrade != null)
            {
                npc.isInvisible = false;
            }
            if (npc.name.Equals("Willy") && Game1.stats.DaysPlayed < 2u)
            {
                npc.isInvisible = true;
            }
            else if (npc.Schedule != null)
            {
                npc.followSchedule = true;
            }

            Dictionary<string, string> dictionary = null;
            string result;
            try
            {
                dictionary = Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + npc.name);
            }
            catch (Exception)
            {
                result = null;
                return "";
            }
            if (npc.isMarried())
            {
                string text = Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth);
                if ((npc.name.Equals("Penny") && (text.Equals("Tue") || text.Equals("Wed") || text.Equals("Fri"))) || (npc.name.Equals("Maru") && (text.Equals("Tue") || text.Equals("Thu"))) || (npc.name.Equals("Harvey") && (text.Equals("Tue") || text.Equals("Thu"))))
                {
                    FieldInfo field = typeof(NPC).GetField("nameofTodaysSchedule", BindingFlags.NonPublic | BindingFlags.Instance);
                    field.SetValue(npc, "marriageJob");
                    // npc.nameOfTodaysSchedule = "marriageJob";
                    return (result = "marriageJob");
                }
                if (!Game1.isRaining && dictionary.ContainsKey("marriage_" + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)))
                {
                    FieldInfo field = typeof(NPC).GetField("nameofTodaysSchedule", BindingFlags.NonPublic | BindingFlags.Instance);
                   field.SetValue(npc,"marriage_" + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth));
                    return result="marriage_" + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth);
                }
                npc.followSchedule = false;
                return null;
            }
            else
            {
                if (dictionary.ContainsKey(Game1.currentSeason + "_" + Game1.dayOfMonth))
                {
                    return result=(Game1.currentSeason + "_" + Game1.dayOfMonth);
                }
                int i;
                for (i = (Game1.player.friendships.ContainsKey(npc.name) ? (Game1.player.friendships[npc.name][0] / 250) : -1); i > 0; i--)
                {
                    if (dictionary.ContainsKey(Game1.dayOfMonth + "_" + i))
                    {
                        return result=Game1.dayOfMonth + "_" + i;
                    }
                }
                if (dictionary.ContainsKey(string.Empty + Game1.dayOfMonth))
                {
                    return result=string.Empty + Game1.dayOfMonth;
                }
                if (npc.name.Equals("Pam") && Game1.player.mailReceived.Contains("ccVault"))
                {
                    return result="bus";
                }
                if (Game1.isRaining)
                {
                    if (Game1.random.NextDouble() < 0.5 && dictionary.ContainsKey("rain2"))
                    {
                        return result="rain2";
                    }
                    if (dictionary.ContainsKey("rain"))
                    {
                        return result="rain";
                    }
                }
                List<string> list = new List<string>
                {
                    Game1.currentSeason,
                    Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)
                };
                i = (Game1.player.friendships.ContainsKey(npc.name) ? (Game1.player.friendships[npc.name][0] / 250) : -1);
                while (i > 0)
                {
                    list.Add(string.Empty + i);
                    if (dictionary.ContainsKey(string.Join("_", list)))
                    {
                        return result=string.Join("_", list);
                    }
                    i--;
                    list.RemoveAt(list.Count - 1);
                }
                if (dictionary.ContainsKey(string.Join("_", list)))
                {
                    return result=string.Join("_", list);
                }
                if (dictionary.ContainsKey(Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)))
                {
                    return result=Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth);
                }
                if (dictionary.ContainsKey(Game1.currentSeason))
                {
                    return result=Game1.currentSeason;
                }
                if (dictionary.ContainsKey("spring_" + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)))
                {
                    return result="spring_" + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth);
                }
                list.RemoveAt(list.Count - 1);
                list.Add("spring");
                i = (Game1.player.friendships.ContainsKey(npc.name) ? (Game1.player.friendships[npc.name][0] / 250) : -1);
                while (i > 0)
                {
                    list.Add(string.Empty + i);
                    if (dictionary.ContainsKey(string.Join("_", list)))
                    {
                        return result=string.Join("_", list);
                    }
                    i--;
                    list.RemoveAt(list.Count - 1);
                }
                if (dictionary.ContainsKey("spring"))
                {
                    return result="spring";
                }
                return null;
            }


        }

        //done
        private void ShippingCheck(object sender, EventArgs e)
        {
            try {
                if (Game1.activeClickableMenu != null) return;
                Save_Anywhere_V2.Save_Utilities.GameUtilities.shipping_check();
            }
            catch(Exception err)
            {
                Monitor.Log(err.ToString());
            }
        }

        //done
        private void Warp_Check(object sender, EventArgs e)
        {
            try
            {
                string name = StardewValley.Game1.player.name;
                Save_Anywhere_V2.Mod_Core.player_path = Path.Combine(Save_Anywhere_V2.Mod_Core.mod_path, "Save_Data", name);
                if (!Directory.Exists(Save_Anywhere_V2.Mod_Core.player_path))
                {
                    //Log.AsyncM(Save_Anywhere_V2.Mod_Core.player_path);
                    //Log.AsyncC("WOOPS");
                    return;
                }

               // Log.AsyncY(Player_Utilities.has_player_warped_yet);

                if (Save_Anywhere_V2.Save_Utilities.Player_Utilities.has_player_warped_yet == false && Game1.player.isMoving() == true)
                {
                    //Log.AsyncM("Ok Good"); 
                    Save_Anywhere_V2.Save_Utilities.Player_Utilities.warp_player();
                    Save_Anywhere_V2.Save_Utilities.Animal_Utilities.load_animal_info();
                    Save_Anywhere_V2.Save_Utilities.NPC_Utilities.Load_NPC_Info();
                    Save_Anywhere_V2.Save_Utilities.Player_Utilities.has_player_warped_yet = true;

                }
            }
            catch (Exception err)
            {
                //7Log.AsyncO("THIS DOESNT MAKE SENSE");
                Monitor.Log(err.ToString());
            }
        }

        //done
        private void PlayerEvents_LoadedGame(object sender, EventArgs e)
        {
            try {
                Save_Anywhere_V2.Save_Utilities.Player_Utilities.load_player_info();
                Save_Anywhere_V2.Save_Utilities.Config_Utilities.DataLoader_Settings();
                Save_Anywhere_V2.Save_Utilities.Config_Utilities.MyWritter_Settings();
            }
            catch (Exception err) {
                Monitor.Log(err.ToString());
            }
        }

        public void KeyPressed_Save_Load_Menu(object sender, StardewModdingAPI.Events.EventArgsKeyPressed e)
        {
            if (e.KeyPressed.ToString() == Save_Anywhere_V2.Save_Utilities.Config_Utilities.key_binding) //if the key is pressed, load my cusom save function
            {
                if (Game1.activeClickableMenu != null) return;
                try {
                    Save_Anywhere_V2.Save_Utilities.GameUtilities.save_game();
                    if (GameUtilities.passiveSave)
                    {
                        Game1.activeClickableMenu = new StardewValley.Menus.SaveGameMenu();
                        GameUtilities.passiveSave = false;
                    }
                    anywhereSaved = true;
                }
                catch(Exception exe)
                {
                    Mod_Core.thisMonitor.Log(exe.ToString(), LogLevel.Error);
                }

                }
        }




        private Dictionary<int, SchedulePathDescription> parseMasterSchedule(NPC npc, string rawData)
        {
            string[] array = rawData.Split(new char[]
            {
                '/'
            });
            Dictionary<int, SchedulePathDescription> dictionary = new Dictionary<int, SchedulePathDescription>();
            int num = 0;
            if (array[0].Contains("GOTO"))
            {
                string text = array[0].Split(new char[]
                {
                    ' '
                })[1];
                if (text.ToLower().Equals("season"))
                {
                    text = Game1.currentSeason;
                }
                try
                {
                    array = Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + npc.name)[text].Split(new char[]
                    {
                        '/'
                    });
                }
                catch (Exception)
                {
                    return parseMasterSchedule(npc,Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + npc.name)["spring"]);
                }
            }
            if (array[0].Contains("NOT"))
            {
                string[] array2 = array[0].Split(new char[]
                {
                    ' '
                });
                string a = array2[1].ToLower();
                if (a == "friendship")
                {
                    string name = array2[2];
                    int num2 = Convert.ToInt32(array2[3]);
                    bool flag = false;
                    using (List<StardewValley.Farmer>.Enumerator enumerator = Game1.getAllFarmers().GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            if (enumerator.Current.getFriendshipLevelForNPC(name) >= num2)
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                    if (flag)
                    {
                        return parseMasterSchedule(npc,Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + npc.name)["spring"]);
                    }
                    num++;
                }
            }
            if (array[num].Contains("GOTO"))
            {
                string text2 = array[num].Split(new char[]
                {
                    ' '
                })[1];
                if (text2.ToLower().Equals("season"))
                {
                    text2 = Game1.currentSeason;
                }
                array = Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + npc.name)[text2].Split(new char[]
                {
                    '/'
                });
                num = 1;
            }
            
            //FieldInfo field = typeof(NPC).GetField("scheduleTimeToTry", BindingFlags.NonPublic | BindingFlags.Instance);
            Point point = npc.isMarried() ? new Point(0, 23) : new Point((int)npc.DefaultPosition.X / Game1.tileSize, (int)npc.DefaultPosition.Y / Game1.tileSize);
            string text3 = npc.isMarried() ? "BusStop" : npc.defaultMap;
            int num3 = num;
            while (num3 < array.Length && array.Length > 1)
            {
                int num4 = 0;
                string[] array3 = array[num3].Split(new char[]
                {
                    ' '
                });
                int key = Convert.ToInt32(array3[num4]);
                num4++;
                string text4 = array3[num4];
                string endBehavior = null;
                string endMessage = null;
                int num5;
                if (int.TryParse(text4, out num5))
                {
                    text4 = text3;
                    num4--;
                }
                num4++;
                int num6 = Convert.ToInt32(array3[num4]);
                num4++;
                int num7 = Convert.ToInt32(array3[num4]);
                num4++;
                int finalFacingDirection = 2;
                try
                {
                    finalFacingDirection = Convert.ToInt32(array3[num4]);
                    num4++;
                }
                catch (Exception)
                {
                    finalFacingDirection = 2;
                }
                if (changeScheduleForLocationAccessibility(npc,ref text4, ref num6, ref num7, ref finalFacingDirection))
                {
                    if (Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + npc.name).ContainsKey("default"))
                    {
                        return parseMasterSchedule(npc,Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + npc.name)["default"]);
                    }
                    return parseMasterSchedule(npc,Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + npc.name)["spring"]);
                }
                else
                {
                    if (num4 < array3.Length)
                    {
                        if (array3[num4].Length > 0 && array3[num4][0] == '"')
                        {
                            endMessage = array[num3].Substring(array[num3].IndexOf('"'));
                        }
                        else
                        {
                            endBehavior = array3[num4];
                            num4++;
                            if (num4 < array3.Length && array3[num4].Length > 0 && array3[num4][0] == '"')
                            {
                                endMessage = array[num3].Substring(array[num3].IndexOf('"')).Replace("\"", "");
                            }
                        }
                    }
                    dictionary.Add(key, pathfindToNextScheduleLocation(npc,text3, point.X, point.Y, text4, num6, num7, finalFacingDirection, endBehavior, endMessage));
                    point.X = num6;
                    point.Y = num7;
                    text3 = text4;
                    num3++;
                }
            }
            return dictionary;
        }

        public Dictionary<int, SchedulePathDescription> getSchedule(NPC npc,int dayOfMonth)
        {
            if (!npc.name.Equals("Robin") || Game1.player.currentUpgrade != null)
            {
                npc.isInvisible = false;
            }
            if (npc.name.Equals("Willy") && Game1.stats.DaysPlayed < 2u)
            {
                npc.isInvisible = true;
            }
            else if (npc.Schedule != null)
            {
                npc.followSchedule = true;
            }
            Dictionary<string, string> dictionary = null;
            Dictionary<int, SchedulePathDescription> result;
            try
            {
                dictionary = Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + npc.name);
            }
            catch (Exception)
            {
                result = null;
                return result;
            }
            if (npc.isMarried())
            {
                string text = Game1.shortDayNameFromDayOfSeason(dayOfMonth);
                if ((npc.name.Equals("Penny") && (text.Equals("Tue") || text.Equals("Wed") || text.Equals("Fri"))) || (npc.name.Equals("Maru") && (text.Equals("Tue") || text.Equals("Thu"))) || (npc.name.Equals("Harvey") && (text.Equals("Tue") || text.Equals("Thu"))))
                {
                    FieldInfo field = typeof(NPC).GetField("nameofTodaysSchedule", BindingFlags.NonPublic | BindingFlags.Instance);
                    field.SetValue(npc, "marriageJob");
                    return parseMasterSchedule(npc, (dictionary["marriageJob"]));
                }
                if (!Game1.isRaining && dictionary.ContainsKey("marriage_" + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)))
                {
                    FieldInfo field = typeof(NPC).GetField("nameofTodaysSchedule", BindingFlags.NonPublic | BindingFlags.Instance);
                    field.SetValue(npc, "marriage_" + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth));
                    return parseMasterSchedule(npc, dictionary["marriage_" + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)]);
                }
                npc.followSchedule = false;
                return null;
            }
            else
            {
                if (dictionary.ContainsKey(Game1.currentSeason + "_" + Game1.dayOfMonth))
                {
                    return parseMasterSchedule(npc, dictionary[Game1.currentSeason + "_" + Game1.dayOfMonth]);
                }
                int i;
                for (i = (Game1.player.friendships.ContainsKey(npc.name) ? (Game1.player.friendships[npc.name][0] / 250) : -1); i > 0; i--)
                {
                    if (dictionary.ContainsKey(Game1.dayOfMonth + "_" + i))
                    {
                        return parseMasterSchedule(npc, dictionary[Game1.dayOfMonth + "_" + i]);
                    }
                }
                if (dictionary.ContainsKey(string.Empty + Game1.dayOfMonth))
                {
                    return parseMasterSchedule(npc, dictionary[string.Empty + Game1.dayOfMonth]);
                }
                if (npc.name.Equals("Pam") && Game1.player.mailReceived.Contains("ccVault"))
                {
                    return parseMasterSchedule(npc, dictionary["bus"]);
                }
                if (Game1.isRaining)
                {
                    if (Game1.random.NextDouble() < 0.5 && dictionary.ContainsKey("rain2"))
                    {
                        return parseMasterSchedule(npc, dictionary["rain2"]);
                    }
                    if (dictionary.ContainsKey("rain"))
                    {
                        return parseMasterSchedule(npc,dictionary["rain"]);
                    }
                }
                List<string> list = new List<string>
                {
                    Game1.currentSeason,
                    Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)
                };
                i = (Game1.player.friendships.ContainsKey(npc.name) ? (Game1.player.friendships[npc.name][0] / 250) : -1);
                while (i > 0)
                {
                    list.Add(string.Empty + i);
                    if (dictionary.ContainsKey(string.Join("_", list)))
                    {
                        return parseMasterSchedule(npc,dictionary[string.Join("_", list)]);
                    }
                    i--;
                    list.RemoveAt(list.Count - 1);
                }
                if (dictionary.ContainsKey(string.Join("_", list)))
                {
                    return parseMasterSchedule(npc,dictionary[string.Join("_", list)]);
                }
                if (dictionary.ContainsKey(Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)))
                {
                    return parseMasterSchedule(npc,dictionary[Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)]);
                }
                if (dictionary.ContainsKey(Game1.currentSeason))
                {
                    return parseMasterSchedule(npc,dictionary[Game1.currentSeason]);
                }
                if (dictionary.ContainsKey("spring_" + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)))
                {
                    return parseMasterSchedule(npc,dictionary["spring_" + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)]);
                }
                list.RemoveAt(list.Count - 1);
                list.Add("spring");
                i = (Game1.player.friendships.ContainsKey(npc.name) ? (Game1.player.friendships[npc.name][0] / 250) : -1);
                while (i > 0)
                {
                    list.Add(string.Empty + i);
                    if (dictionary.ContainsKey(string.Join("_", list)))
                    {
                        return parseMasterSchedule(npc,dictionary[string.Join("_", list)]);
                    }
                    i--;
                    list.RemoveAt(list.Count - 1);
                }
                if (dictionary.ContainsKey("spring"))
                {
                    return parseMasterSchedule(npc,dictionary["spring"]);
                }
                return null;
            }
        }
        private bool changeScheduleForLocationAccessibility(NPC npc,ref string locationName, ref int tileX, ref int tileY, ref int facingDirection)
        {
            string a = locationName;
            if (!(a == "JojaMart") && !(a == "Railroad"))
            {
                if (a == "CommunityCenter")
                {
                    return !Game1.isLocationAccessible(locationName);
                }
            }
            else if (!Game1.isLocationAccessible(locationName))
            {
                if (!Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + npc.name).ContainsKey(locationName + "_Replacement"))
                {
                    return true;
                }
                string[] array = Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + npc.name)[locationName + "_Replacement"].Split(new char[]
                {
                    ' '
                });
                locationName = array[0];
                tileX = Convert.ToInt32(array[1]);
                tileY = Convert.ToInt32(array[2]);
                facingDirection = Convert.ToInt32(array[3]);
            }
            return false;
        }

        private SchedulePathDescription pathfindToNextScheduleLocation(NPC npc,string startingLocation, int startingX, int startingY, string endingLocation, int endingX, int endingY, int finalFacingDirection, string endBehavior, string endMessage)
        {
            Stack<Point> stack = new Stack<Point>();
            Point warpPointTarget = new Point(startingX, startingY);
            List<string> list = (!startingLocation.Equals(endingLocation)) ? getLocationRoute(npc, startingLocation, endingLocation) : null;
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    GameLocation locationFromName = Game1.getLocationFromName(list[i]);
                    if (i < list.Count - 1)
                    {
                        Point warpPointTo = locationFromName.getWarpPointTo(list[i + 1]);
                        if (warpPointTo.Equals(Point.Zero) || warpPointTarget.Equals(Point.Zero))
                        {
                            throw new Exception("schedule pathing tried to find a warp point that doesn't exist.");
                        }
                        stack = addToStackForSchedule(stack, PathFindController.findPathForNPCSchedules(warpPointTarget, warpPointTo, locationFromName, 30000));
                        warpPointTarget = locationFromName.getWarpPointTarget(warpPointTo);
                    }
                    else
                    {
                        stack = addToStackForSchedule(stack, PathFindController.findPathForNPCSchedules(warpPointTarget, new Point(endingX, endingY), locationFromName, 30000));
                    }
                }
            }
            else if (startingLocation.Equals(endingLocation))
            {
                stack = PathFindController.findPathForNPCSchedules(warpPointTarget, new Point(endingX, endingY), Game1.getLocationFromName(startingLocation), 30000);
            }
            return new SchedulePathDescription(stack, finalFacingDirection, endBehavior, endMessage);
        }

        private List<string> getLocationRoute(NPC npc,string startingLocation, string endingLocation)
        {
            FieldInfo field = typeof(NPC).GetField("routesFromLocationToLocation", BindingFlags.NonPublic | BindingFlags.Instance);
            List<List<string>> s = (List<List<string>>)field.GetValue(npc);
            foreach (List<string> current in s)
            {
                if (current.First<string>().Equals(startingLocation) && current.Last<string>().Equals(endingLocation) && (npc.gender == 0 || !current.Contains("BathHouse_MensLocker")) && (npc.gender != 0 || !current.Contains("BathHouse_WomensLocker")))
                {
                    return current;
                }
            }
            return null;
        }


        private Stack<Point> addToStackForSchedule(Stack<Point> original, Stack<Point> toAdd)
        {
            if (toAdd == null)
            {
                return original;
            }
            original = new Stack<Point>(original);
            while (original.Count > 0)
            {
                toAdd.Push(original.Pop());
            }
            return toAdd;
        }
    }
}
