using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;
using StardewModdingAPI;
using System.IO;
using Newtonsoft.Json;
/*TODO:
*/
/*
Issues:
-Mail can't be wiped without destroying all mail.
-Lighting transition does not work if it is raining.
        -set the weather to clear if you are stayig up late.
        -transition still doesnt work. However atleast it is dark now.

-Known glitched

*/



namespace Stardew_NightOwl
{
    public class Class1 : Mod
    {
        int player_x;
        int player_y;
        string prior_map;

        bool reset_check;
        bool was_raining;
        bool time_reset;
        int prior_money;
        int post_money;
        float pre_stam;
        int pre_health;

        bool lighting_transition; //if true transition happens. If false, game starts out bright at 2AM. Good to remove that awkward change from dark to bright.
        bool warp;
        bool stay_up;

        bool wipe_mail;
        bool protect_money;
        bool persistant_health;
        bool persistant_stamina;

        bool game_loaded;

        bool once;
        bool up_late;

        bool first_check;

        bool warped_check;

        bool super_map_warping_check;

       

       public static string ModPath;
       public static string Error_Path;
        public override void Entry(IModHelper helper)
        {
            StardewModdingAPI.Events.TimeEvents.TimeOfDayChanged += TimeEvents_TimeOfDayChanged;
            StardewModdingAPI.Events.TimeEvents.AfterDayStarted += TimeEvents_AfterDayStarted;
            StardewModdingAPI.Events.SaveEvents.AfterLoad += PlayerEvents_LoadedGame;
            StardewModdingAPI.Events.GameEvents.FourthUpdateTick += GameEvents_FourthUpdateTick;
            ModPath =Helper.DirectoryPath;
            Error_Path = Path.Combine(ModPath, "Error_Logs");
        }


        public void GameEvents_FourthUpdateTick(object sender, EventArgs e)
        {
            /*
            IMPLEMENT TRY CATCH SYSTEM HERE;

            */
            try
            {

                if (Game1.eventUp == true) return;
            if (game_loaded == false) return;

            if (Game1.timeOfDay == 600)
            {
                if (once==true)
                {
                  //  Log.AsyncM("NEW MONEY" + Game1.player.money);
                   // Game1.player.money = Game1.player.money + post_money;
                    once = false;
                }

            }
            if (warped_check == false)
            {
                if (warp == true)
                {
                    Monitor.Log("Warping!!!");
                    if (prior_map == null)
                    {
                        warped_check = true;
                        super_map_warping_check = true;
                        return;
                    }
                    Game1.warpFarmer(prior_map, player_x, player_y, false);
                    warped_check = true;
                    super_map_warping_check = true;
                    prior_map = null;
                }

                }
            }
            catch (Exception ex)
            {
                try
                {
                    Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                    serializer.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    serializer.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All;
                    serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
                    serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    using (StreamWriter sw = new StreamWriter(Path.Combine(Error_Path, "Mod_State.json")))
                    {
                        using (Newtonsoft.Json.JsonWriter writer2 = new Newtonsoft.Json.JsonTextWriter(sw))
                        {
                            serializer.Serialize(writer2, this);
                        }
                    }
                }
                catch (Exception exc)
                {
                    Monitor.Log(exc.ToString());
                }
            }
        }

        public void PlayerEvents_LoadedGame(object sender, EventArgs e)
        {
            DataLoader();
            MyWritter();
            game_loaded = true;
            once = false;
            up_late = false;
            first_check = false;
            warped_check = true;
            super_map_warping_check = true;
        }

        public void TimeEvents_AfterDayStarted(object sender, EventArgs e)
        {
            /*
            IMPLEMENT TRY CATCH SYSTEM HERE

            */
            try { 

            if (game_loaded == false) return;
            if (first_check == false)
            {
                first_check = true;
                Monitor.Log("first");
                return;
            }
            else
            {
                first_check = false;
                Monitor.Log("Second");
            }
          if(Game1.farmerShouldPassOut== true)  Game1.farmerShouldPassOut = false; //make the farmer collapse.
           
            reset_check = false;
            //Game1.farmerShouldPassOut = false; //make the farmer collapse.
            DataLoader();
            MyWritter();
            //List<string> newmail = new List<string>();
            Queue<string> newmail = new Queue<string>();
           
            up_late = false;
            
            if (time_reset == true)
            {
                was_raining = false;
                if (persistant_stamina==true) Game1.player.stamina = pre_stam; //reset health and stam upon collapsing
                if (persistant_health == true) Game1.player.health = pre_health;
                //post_money = Game1.player.money;
                if (protect_money == true) {

                    Game1.player.money += post_money;
                    once = true;
                

                } //add the money back from colapsing.
                time_reset = false; //reset my bool.
                if(warp==true)Game1.warpFarmer(prior_map, player_x, player_y, false);
               // very_old_money = (safety_cash - Game1.player.money);
                //Game1.player.money += very_old_money;
            }

            if (wipe_mail == true)
            {
                foreach (var i in Game1.mailbox) //delete those annoying charge messages. If only I could do this with mail IRL.
                { 
                    Monitor.Log(i);

                    if (i.Contains("passedOut"))
                    {
                        Monitor.Log("Found bad mail");
                    }
                    else {
                        newmail.Enqueue(i);
                    }
                }
                Game1.mailbox.Clear();
                foreach (string mail in newmail)
                {
                    Game1.mailbox.Enqueue(mail);
                }
            }
            warped_check = false;
                //prior_map==null; //prevents multiple warping when sleeping
            }
            catch (Exception ex)
            {
                try
                {
                    Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                    serializer.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    serializer.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All;
                    serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
                    serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    using (StreamWriter sw = new StreamWriter(Path.Combine(Error_Path, "Mod_State.json")))
                    {
                        using (Newtonsoft.Json.JsonWriter writer2 = new Newtonsoft.Json.JsonTextWriter(sw))
                        {
                            serializer.Serialize(writer2, this);
                        }
                    }
                }
                catch (Exception exc)
                {
                    Monitor.Log(exc.ToString());
                }
            }
        }


        public void TimeEvents_TimeOfDayChanged(object sender, StardewModdingAPI.Events.EventArgsIntChanged e)
        {
            /*
            Implement try catch system here.

            */
            try { 
            if (game_loaded == false) return;
            float color_mod;
            //transition the lighting for more "realistic" lighting from 2am to 11am. There is an awkward screen shuffle that happens because of this update.
          
            if (lighting_transition == true)
            {
                if (Game1.timeOfDay > 400 && Game1.timeOfDay < 600)
                {
                    color_mod = (1300 - Game1.timeOfDay) / 1000f; //.7f
                    Game1.outdoorLight = Game1.ambientLight * color_mod;

                }
            }


            if (stay_up == true)
            {
                if (Game1.timeOfDay == 2550)
                {
                    if (StardewValley.Game1.isRaining == true)
                    {
                        was_raining = true;
                        StardewValley.Game1.isRaining = false; //regardless make sure I change the weather. Otherwise lighting gets screwy.
                    }
                    StardewValley.Game1.updateWeatherIcon();
                    Game1.timeOfDay = 150; //change it from 1:50 am late, to 1:50 am early
                }
            }


            if (Game1.timeOfDay == 550) //if the game time is 5:50 AM
            {
                up_late = true;
            }

            if (Game1.timeOfDay == 600) //when time is 6:00 AM after staying up
            {

                if (up_late == false)
                {
                    return;
                }

     
                if (super_map_warping_check == true)
                {
                    if (Game1.currentMinigame != null) Game1.currentMinigame = null;
                    //if (time_reset == false)  return;
                    time_reset = true;
                    player_x = Game1.player.getTileX();
                    player_y = Game1.player.getTileY();
                    prior_map = Game1.player.currentLocation.name;

                    Monitor.Log(prior_map);
                    Monitor.Log(player_x.ToString());
                    Monitor.Log(player_y.ToString());
                    super_map_warping_check = false;


                    Game1.farmerShouldPassOut = true; //make the farmer collapse.

                    pre_stam = Game1.player.stamina;
                    pre_health = Game1.player.health;
                    prior_money = Game1.player.money;

                    if (Game1.player.money <= 10000)
                    {
                        post_money = prior_money / 10;
                    }
                    else
                    {
                        post_money = 1000;
                    }
                }
            }
            }
            catch (Exception ex)
            {
                try
                {
                    Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                    serializer.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    serializer.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All;
                    serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
                    serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    using (StreamWriter sw = new StreamWriter(Path.Combine(Error_Path, "Mod_State.json")))
                    {
                        using (Newtonsoft.Json.JsonWriter writer2 = new Newtonsoft.Json.JsonTextWriter(sw))
                        {
                            serializer.Serialize(writer2, this);
                        }
                    }
                }
                catch (Exception exc)
                {
                    Monitor.Log(exc.ToString());
                }
            }
        }
        

        void MyWritter()
        {
            //saves the BuildEndurance_data at the end of a new day;
            string myname = Game1.player.name;
            string mylocation = Path.Combine(Helper.DirectoryPath, "Night_Owl_Config_");
            //string mylocation2 = mylocation + myname;
            string mylocation3 = mylocation + ".txt";
            string[] mystring3 = new string[20];
            if (!File.Exists(mylocation3))
            {
              Monitor.Log("The data file for Night Owl wasn't found. Time to create it!");
                mystring3[0] = "Player: Night Owl Config. Feel free to edit.";
                mystring3[1] = "====================================================================================";
                mystring3[2] = "Stay up to 6 AM?: Do you wan't to stay up till 6 AM the next morning?";
                mystring3[3] = stay_up.ToString();
                mystring3[4] = "Custom Lighting Transition?: Do you want to have a lighting transition from 2AM to 5:50 AM? Setting this to false will keep the world dark until the player passes out or goes to bed.";
                mystring3[5] = lighting_transition.ToString();
                mystring3[6] = "Warp to collapse position?: True: Stay in the same place. False: Warp back home.";
                mystring3[7] = warp.ToString();
                mystring3[8] = "Keep Money?: True: Don't be charged for passing out.";
                mystring3[9] = protect_money.ToString();
                mystring3[10] = "Keep stamina when staying up? When the farmer passes out at 6, should their stamina be their stamina before collapsing?";
                mystring3[11] = persistant_stamina.ToString();
                mystring3[12] = "Keep health when staying up? When the farmer passes out at 6, should their health be their health before collapsing?";
                mystring3[13] = persistant_health.ToString();
                mystring3[14] = "Clean out charges mail?: Get rid of the annoying We charged X gold for your health fees, etc.";
                mystring3[15] = wipe_mail.ToString();
                File.WriteAllLines(mylocation3, mystring3);
            }

            else
            {
                //    Console.WriteLine("HEY IM SAVING DATA");
                //write out the info to a text file at the end of a day.
                mystring3[0] = "Player: Night Owl Config. Feel free to edit.";
                mystring3[1] = "====================================================================================";
                mystring3[2] = "Stay up to 6 AM?: Do you wan't to stay up till 6 AM the next morning?";
                mystring3[3] = stay_up.ToString();
                mystring3[4] = "Custom Lighting Transition?: Do you want to have a lighting transition from 2AM to 5:50 AM? Setting this to false will keep the world dark until the player passes out or goes to bed.";
                mystring3[5] = lighting_transition.ToString();
                mystring3[6] = "Warp to collapse position?: True: Stay in the same place. False: Warp back home.";
                mystring3[7] = warp.ToString();
                mystring3[8] = "Keep Money?: True: Don't be charged for passing out.";
                mystring3[9] = protect_money.ToString();
                mystring3[10] = "Keep stamina when staying up? When the farmer passes out at 6, should their stamina be their stamina before collapsing?";
                mystring3[11] = persistant_stamina.ToString();
                mystring3[12] = "Keep health when staying up? When the farmer passes out at 6, should their health be their health before collapsing?";
                mystring3[13] = persistant_health.ToString();
                mystring3[14] = "Clean out charges mail?: Get rid of the annoying We charged X gold for your health fees, etc.";
                mystring3[15] = wipe_mail.ToString();

                File.WriteAllLines(mylocation3, mystring3);
            }
        }
        void DataLoader()
        {
            //loads the data to the variables upon loading the game.
            string myname = StardewValley.Game1.player.name;
            string mylocation = Path.Combine(Helper.DirectoryPath, "Night_Owl_Config_");
            //string mylocation2 = mylocation + myname;
            string mylocation3 = mylocation + ".txt";
            if (!File.Exists(mylocation3)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {
               Monitor.Log("Loading Night_Owl_Config");
                lighting_transition = true;
                warp = true;
                stay_up = true;

                persistant_health = true;
                persistant_stamina = true;
                wipe_mail = true;
                protect_money = true;

            }

            else
            {
                string[] readtext = File.ReadAllLines(mylocation3);
                stay_up = Convert.ToBoolean(readtext[3]);//will the player stay up?
                lighting_transition = Convert.ToBoolean(readtext[5]);  //does the lighting transition occur?
                warp = Convert.ToBoolean(readtext[7]); //does the player warp back to the spot they passed out at?
                protect_money = Convert.ToBoolean(readtext[9]); //Is their money safe from stealing?
                persistant_stamina = Convert.ToBoolean(readtext[11]); //Does the player have the same stam when collapsing?
                persistant_health = Convert.ToBoolean(readtext[13]); //Ditto but with health.
                if (readtext[15] == "") wipe_mail = true;
               else wipe_mail = Convert.ToBoolean(readtext[15]); //will I clean out their mailbox?
            }
        }
    }
}
