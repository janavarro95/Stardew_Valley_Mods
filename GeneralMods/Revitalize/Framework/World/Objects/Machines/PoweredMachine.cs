using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using Omegasis.Revitalize.Framework.Constants;
using Omegasis.Revitalize.Framework.Constants.Ids.Items;
using Omegasis.Revitalize.Framework.HUD;
using Omegasis.Revitalize.Framework.Player;
using Omegasis.Revitalize.Framework.Utilities.JsonContentLoading;
using Omegasis.Revitalize.Framework.World.Objects.InformationFiles;
using Omegasis.Revitalize.Framework.World.Objects.Interfaces;
using Omegasis.Revitalize.Framework.World.WorldUtilities;
using StardewValley;

namespace Omegasis.Revitalize.Framework.World.Objects.Machines
{
    /// <summary>
    /// Machines that are powered and used the machine tier system.
    /// </summary>
    ///
    [XmlType("Mods_Revitalize.Framework.World.Objects.Machines.PoweredMachine")]
    public class PoweredMachine : Machine
    {
        public enum PoweredMachineTier
        {
            Coal,
            Electric,
            Nuclear,
            Magical,
            /// <summary>
            /// If ever implemented, this will have machines process instantly.
            /// </summary>
            Galaxy
        }

        public readonly NetEnum<PoweredMachineTier> machineTier = new NetEnum<PoweredMachineTier>();
        public readonly NetInt fuelChargesRemaining = new NetInt(0);

        [XmlIgnore]
        public int FuelChargesRemaining
        {
            get
            {
                return this.fuelChargesRemaining.Value;
            }
            set
            {
                this.fuelChargesRemaining.Value = value;
            }
        }

        [XmlIgnore]
        public PoweredMachineTier MachineTier
        {
            get
            {
                return this.machineTier.Value;
            }
            set
            {
                this.machineTier.Value = value;
            }
        }

        public PoweredMachine()
        {

        }

        public PoweredMachine(BasicItemInformation info, PoweredMachineTier machineTier) : base(info)
        {
            this.createStatusBubble();
            this.machineTier.Value = machineTier;
        }

        public PoweredMachine(BasicItemInformation info, Vector2 TileLocation, PoweredMachineTier machineTier) : base(info, TileLocation)
        {
            this.createStatusBubble();
            this.machineTier.Value = machineTier;
        }

        protected override void initializeNetFieldsPostConstructor()
        {
            base.initializeNetFieldsPostConstructor();
            this.NetFields.AddFields(this.machineTier, this.fuelChargesRemaining);
        }

        /// <summary>
        /// Performed when dropping in an object into the mining drill.
        /// </summary>
        /// <param name="dropInItem"></param>
        /// <param name="probe"></param>
        /// <param name="who"></param>
        /// <returns></returns>
        public override bool performObjectDropInAction(Item dropInItem, bool probe, Farmer who)
        {
            //Prevent overriding and destroying the previous operation.
            if (this.heldObject.Value != null && who!=null)
            {
                Game1.player.addItemToInventory(this.heldObject.Value);
                this.heldObject.Value = null;
            }
            bool success = base.performObjectDropInAction(dropInItem, probe, who) && this.useFuelItemToIncreaseCharges(who, false, true);
            if (!success) return false;

            this.processInput(dropInItem, who, true);
            return false;
        }


        /// <summary>
        /// Consumes a single charge of fuel used on this funace.
        /// </summary>
        public virtual void consumeFuelCharge()
        {
            if (this.machineTier.Value == PoweredMachineTier.Magical) return;
            this.FuelChargesRemaining--;
            if (this.FuelChargesRemaining <= 0) this.FuelChargesRemaining = 0;
        }


        public virtual int getCoalFuelChargeIncreaseAmount()
        {
            return 1;
        }

        public virtual int getElectricFuelChargeIncreaseAmount()
        {
            return 3;
        }

        public virtual int getNuclearFuelChargeIncreaseAmount()
        {
            return 14;
        }

        public virtual int getMagicalFuelChargeIncreaseAmount()
        {
            return int.MaxValue;
        }

        public virtual int getGalaxyFuelChargeIncreaseAmount()
        {
            return int.MaxValue;
        }


        /// <summary>
        /// Increases the fuel type for the furnace. Public for automate compatibility
        /// </summary>
        public virtual void increaseFuelCharges()
        {
            if (this.machineTier.Value == PoweredMachineTier.Coal)
            {
                this.FuelChargesRemaining = this.getCoalFuelChargeIncreaseAmount();
            }
            if (this.machineTier.Value == PoweredMachineTier.Electric)
            {
                this.FuelChargesRemaining = this.getElectricFuelChargeIncreaseAmount();
            }
            if (this.machineTier.Value == PoweredMachineTier.Nuclear)
            {
                this.FuelChargesRemaining = this.getNuclearFuelChargeIncreaseAmount();
            }
            if (this.machineTier.Value == PoweredMachineTier.Magical)
            {
                this.FuelChargesRemaining = this.getMagicalFuelChargeIncreaseAmount();
            }
            if (this.machineTier.Value == PoweredMachineTier.Galaxy)
            {
                this.FuelChargesRemaining = this.getGalaxyFuelChargeIncreaseAmount();
            }
        }

        /// <summary>
        /// Attempts to consume the necessary fuel item from the player's inventory.
        /// </summary>
        /// <param name="who"></param>
        /// <param name="RequireFuelToBeActiveObject">Forces the active object from the player to be the correct fuel type to prevent wasting fuel.</param>
        /// <returns></returns>
        protected virtual bool consumeFuelItemFromFarmersInventory(Farmer who, bool RequireFuelToBeActiveObject)
        {
            if (who == null)
            {
                return true; //Used for automate compatibility
            }
            if (this.machineTier.Value == PoweredMachineTier.Magical)
            {
                return true;
            }
            if (this.machineTier.Value == PoweredMachineTier.Coal)
            {
                if (RequireFuelToBeActiveObject)
                {
                    Item item = Game1.player.ActiveObject;
                    if (item.ParentSheetIndex != (int)Enums.SDVObject.Coal)
                    {
                        return false;
                    }
                }
                return PlayerUtilities.ReduceInventoryItemIfEnoughFound(who, Enums.SDVObject.Coal, 1);
            }
            if (this.machineTier.Value == PoweredMachineTier.Electric)
            {
                if (RequireFuelToBeActiveObject)
                {
                    Item item = Game1.player.ActiveObject;
                    if (item.ParentSheetIndex != (int)Enums.SDVObject.BatteryPack)
                    {
                        return false;
                    }
                }
                return PlayerUtilities.ReduceInventoryItemIfEnoughFound(who, Enums.SDVObject.BatteryPack, 1);
            }
            if (this.machineTier.Value == PoweredMachineTier.Nuclear)
            {
                if (RequireFuelToBeActiveObject)
                {
                    Item item = Game1.player.ActiveObject;
                    if (item is IBasicItemInfoProvider)
                    {
                        IBasicItemInfoProvider infoProvider = (IBasicItemInfoProvider)item;
                        //Check to see if the items can stack. If they can simply add them together and then continue on.
                        if (!infoProvider.Id.Equals(MiscItemIds.RadioactiveFuel))
                        {
                            return false;
                        }
                    }
                }
                return PlayerUtilities.ReduceInventoryItemIfEnoughFound(who, MiscItemIds.RadioactiveFuel, 1);
            }
            return true;
            //Magical does not consume fuel.
        }

        /// <summary>
        /// Tries to use the fuel item to increase fuel charges and consumes it in the same process.
        /// </summary>
        /// <param name="who"></param>
        /// <param name="ShowRedMessage"></param>
        /// <returns>True if successful or not necessaryt o consume a fuel charge. False if either the player is null (Automate Compatibility) or the player doesn't have enough fuel in their inventory.</returns>
        public virtual bool useFuelItemToIncreaseCharges(Farmer who, bool RequireFuelToBeActiveObject, bool ShowRedMessage = true)
        {
            if (this.hasFuel())
            {
                return true;
            }
            if (who != null)
            {
                //Make sure enough fuel is present for the furnace to operate (if necessary!)
                bool playerHasItemInInventory = this.consumeFuelItemFromFarmersInventory(who, RequireFuelToBeActiveObject);

                if (playerHasItemInInventory == false && ShowRedMessage)
                {
                    if (ShowRedMessage)
                    {
                        this.showRedMessageForMissingFuel();
                    }
                    return false;
                }
                this.increaseFuelCharges();
                return true;
            }
            return false;
        }

        public virtual bool hasFuel()
        {
            return this.FuelChargesRemaining > 0;
        }


        /// <summary>
        /// Shows an error message if there is no correct fuel present for the furnace.
        /// </summary>
        protected virtual void showRedMessageForMissingFuel()
        {
            if (this.machineTier.Value == PoweredMachineTier.Coal)
            {
                Game1.showRedMessage(JsonContentLoaderUtilities.LoadErrorString(this.getErrorStringFile(), "NeedCoal"));
                return;
            }
            if (this.machineTier.Value == PoweredMachineTier.Electric)
            {
                Game1.showRedMessage(JsonContentLoaderUtilities.LoadErrorString(this.getErrorStringFile(), "NeedBatteryPack"));
                return;
            }
            if (this.machineTier.Value == PoweredMachineTier.Nuclear)
            {
                Game1.showRedMessage(JsonContentLoaderUtilities.LoadErrorString(this.getErrorStringFile(), "NeedNuclearFuel"));
                return;
            }
            Game1.showRedMessage(JsonContentLoaderUtilities.LoadErrorString(this.getErrorStringFile(), "MagicalFurnaceFuelError"));
            return;
        }

        /// <summary>
        /// Gets the relative path to the file to load the error strings from the ErrorStrings directory.
        /// </summary>
        /// <returns></returns>
        public virtual string getErrorStringFile()
        {
            return Path.Combine("Objects", "Machines", "CommonErrorStrings.json");
        }

        public override Item getOne()
        {
            return new PoweredMachine(this.basicItemInformation.Copy(), this.machineTier.Value);
        }
    }
}