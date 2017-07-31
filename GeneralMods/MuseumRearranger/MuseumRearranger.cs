﻿using System;
using System.IO;
using Omegasis.MuseumRearranger.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Locations;

namespace Omegasis.MuseumRearranger
{
    /// <summary>The mod entry point.</summary>
    public class MuseumRearranger : Mod
    {
        /*********
        ** Properties
        *********/
        /// <summary>The key which shows the museum rearranging menu.</summary>
        private string ShowMenuKey = "R";

        /// <summary>The key which toggles the inventory box when the menu is open.</summary>
        private string ToggleInventoryKey = "T";

        /// <summary>Whether the player loaded a save.</summary>
        private bool IsGameLoaded;

        /// <summary>The open museum menu (if any).</summary>
        private NewMuseumMenu OpenMenu;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            SaveEvents.AfterLoad += this.SaveEvents_AfterLoad;
            ControlEvents.KeyPressed += this.ControlEvents_KeyPressed;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>The method invoked when the presses a keyboard button.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void ControlEvents_KeyPressed(object sender, EventArgsKeyPressed e)
        {
            if (Game1.player == null || Game1.player.currentLocation == null || !this.IsGameLoaded)
                return;

            // open menu
            if (e.KeyPressed.ToString() == this.ShowMenuKey)
            {
                if (Game1.activeClickableMenu != null)
                    return;
                if (Game1.player.currentLocation is LibraryMuseum)
                    Game1.activeClickableMenu = this.OpenMenu = new NewMuseumMenu(this.Helper.Reflection);
                else
                    this.Monitor.Log("You can't rearrange the museum here.");
            }

            // toggle inventory box
            if (e.KeyPressed.ToString() == this.ToggleInventoryKey)
                this.OpenMenu?.ToggleInventory();
        }

        /// <summary>The method invoked after the player loads a save.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void SaveEvents_AfterLoad(object sender, EventArgs e)
        {
            this.IsGameLoaded = true;
            this.LoadConfig();
            this.WriteConfig();
        }

        /// <summary>Load the configuration settings.</summary>
        private void LoadConfig()
        {
            string path = Path.Combine(Helper.DirectoryPath, "Museum_Rearrange_Config.txt");
            if (!File.Exists(path)) //if not data.json exists, initialize the data variables to the ModConfig data. I.E. starting out.
            {
                this.ShowMenuKey = "R";
                this.ToggleInventoryKey = "T";
            }
            else
            {
                string[] text = File.ReadAllLines(path);
                this.ShowMenuKey = Convert.ToString(text[3]);
                this.ToggleInventoryKey = Convert.ToString(text[5]);
            }
        }

        /// <summary>Save the configuration settings.</summary>
        private void WriteConfig()
        {
            string path = Path.Combine(Helper.DirectoryPath, "Museum_Rearrange_Config.txt");
            string[] text = new string[20];
            if (!File.Exists(path))
            {
                this.Monitor.Log("Museum Rearranger: Config not found. Creating it now.");

                text[0] = "Config: Museum_Rearranger. Feel free to mess with these settings.";
                text[1] = "====================================================================================";
                text[2] = "Key binding for rearranging the museum.";
                text[3] = this.ShowMenuKey;
                text[4] = "Key binding for showing the menu when rearranging the museum.";
                text[5] = this.ToggleInventoryKey;
                File.WriteAllLines(path, text);
            }
            else
            {
                //write out the info to a text file at the end of a day. This will run if it doesnt exist.
                text[0] = "Config: Save_Anywhere Info. Feel free to mess with these settings.";
                text[1] = "====================================================================================";
                text[2] = "Key binding for rearranging the museum.";
                text[3] = this.ShowMenuKey;
                text[4] = "Key binding for showing the menu when rearranging the museum.";
                text[5] = this.ToggleInventoryKey;
                File.WriteAllLines(path, text);
            }
        }
    }
}