using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using StardewValley;
using System.Xml.Serialization;
using Netcode;
using Omegasis.Revitalize.Framework.Constants;
using Omegasis.Revitalize.Framework.Utilities;
using Omegasis.Revitalize.Framework.World.Objects.InformationFiles;
using Omegasis.Revitalize.Framework.World.WorldUtilities;

namespace Omegasis.Revitalize.Framework.World.Objects.Machines.EnergyGeneration
{
    [XmlType("Mods_Omegasis.Revitalize.Framework.World.Objects.Machines.EnergyGeneration.Windmill")]
    public class Windmill : Machine
    {

        public readonly NetInt maxDaysToProduceBattery = new NetInt();
        public readonly NetInt daysRemainingToProduceBattery = new NetInt();

        public Windmill() { }

        public Windmill(BasicItemInformation info, Vector2 TileLocation) : base(info, TileLocation)
        {
            this.maxDaysToProduceBattery.Value = 10;
            this.daysRemainingToProduceBattery.Value = this.maxDaysToProduceBattery.Value;
        }

        public override void updateWhenCurrentLocation(GameTime time, GameLocation environment)
        {
            base.updateWhenCurrentLocation(time, environment);
        }

        protected override void initializeNetFieldsPostConstructor()
        {
            base.initializeNetFieldsPostConstructor();
            this.NetFields.AddFields(this.maxDaysToProduceBattery, this.daysRemainingToProduceBattery);
        }

        public override bool canBePlacedHere(GameLocation l, Vector2 tile)
        {
            return l.IsOutdoors && base.canBePlacedHere(l,tile);
        }

        public override Item getOne()
        {
            return new Windmill(this.getItemInformation().Copy(), this.TileLocation);
        }

        public override void doActualDayUpdateLogic(GameLocation location)
        {
            if (!this.getCurrentLocation().IsOutdoors) return;
            if (this.heldObject.Value != null) return;
            if (Game1.weatherIcon == Game1.weather_rain)
                this.daysRemainingToProduceBattery.Value -= 2;
            else if (Game1.weatherIcon == Game1.weather_lightning)
                this.daysRemainingToProduceBattery.Value -= 3;
            else if (Game1.weatherIcon == Game1.weather_debris)
                this.daysRemainingToProduceBattery.Value -= 4;
            else
                this.daysRemainingToProduceBattery.Value -= 1;
            if (this.daysRemainingToProduceBattery.Value <= 0)
            {
                this.daysRemainingToProduceBattery.Value = this.maxDaysToProduceBattery.Value;
                this.heldObject.Value = (StardewValley.Object)RevitalizeModCore.ModContentManager.objectManager.getItem(Enums.SDVObject.BatteryPack, 1);
            }
        }
    }
}
