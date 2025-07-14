using Microsoft.Xna.Framework.Graphics;
using Omegasis.CasinoExpansion.Constants;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.GameData;
using StardewValley.GameData.BigCraftables;
using StardewValley.GameData.Machines;
using StardewValley.GameData.Shops;
using StardewValley.Menus;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Omegasis.CasinoExpansion
{
    public class CasinoExpansion : Mod
    {

        public static CasinoExpansion? Instance { get; private set; }

        public override void Entry(IModHelper helper)
        {
            this.Helper.Events.Content.AssetRequested += this.checkIfAssetCanBeEdited;
            Instance = this;

        }

        private void checkIfAssetCanBeEdited(object? sender, AssetRequestedEventArgs e)
        {
            if (e.NameWithoutLocale.IsEquivalentTo("Data/BigCraftables"))
            {
                e.Edit(this.addItemsToObjectRegistry);
            }

            if (e.NameWithoutLocale.IsEquivalentTo("Data/Machines"))
            {
                e.Edit(this.addMachineData);
            }

            if (e.NameWithoutLocale.IsEquivalentTo("Data/Shops"))
            {
                e.Edit(this.updateShops);
            }

            if (e.NameWithoutLocale.IsEquivalentTo("Omegasis_CasinoExpansion/Assets/Graphics/TravelingCartGamblingMachine.png"))
            {
                e.LoadFromModFile<Texture2D>("Assets/Graphics/TravelingCartGamblingMachine.png", AssetLoadPriority.High);
            }
        }

        private void addItemsToObjectRegistry(IAssetData data)
        {
            IDictionary<string, BigCraftableData> objectDictionary = data.AsDictionary<string, BigCraftableData>().Data;

            BigCraftableData objectData = new BigCraftableData();
            objectData.Name = ModConstants.TRAVELING_CART_GAMBLING_MACHINE;
            objectData.DisplayName = "Traveling Cart Gambling Machine";
            objectData.Description = "A machine that allows you to gamble casino tokens for items!";
            objectData.Texture = "Omegasis_CasinoExpansion/Assets/Graphics/Machines/TravelingCartGamblingMachine.png";
            objectData.SpriteIndex = 0;
            objectData.Price = 0;
            objectDictionary.Add(ModConstants.TRAVELING_CART_GAMBLING_MACHINE, objectData);
        }

        private void addMachineData(IAssetData data)
        {
            MachineData travelingCartGamblingMachine = new MachineData();
            travelingCartGamblingMachine.WobbleWhileWorking = true;
            travelingCartGamblingMachine.ReadyTimeModifiers = new List<StardewValley.GameData.QuantityModifier>();
            travelingCartGamblingMachine.InteractMethod = "Omegasis.CasinoExpansion, CasinoExpansion : TravelingCartMachineGambling";
            travelingCartGamblingMachine.HasInput = true;
            travelingCartGamblingMachine.HasOutput = true;
            travelingCartGamblingMachine.OutputRules = new List<MachineOutputRule>
            {
               new MachineOutputRule()
                {

                }
            }

            IDictionary<string, MachineData> machineDataDictionary = data.AsDictionary<string, MachineData>().Data;

            machineDataDictionary.Add(ModConstants.TRAVELING_CART_GAMBLING_MACHINE, travelingCartGamblingMachine);
        }

        public void updateShops(IAssetData data)
        {
            IDictionary<string, ShopData> shopDataDictionary = data.AsDictionary<string, ShopData>().Data;

            if (shopDataDictionary.ContainsKey("Casino"))
            {
                shopDataDictionary["Casino"].Items.Add(new ShopItemData()
                {
                    Id = ModConstants.TRAVELING_CART_GAMBLING_MACHINE,
                    TradeItemAmount = 1,
                    ItemId = ModConstants.TRAVELING_CART_GAMBLING_MACHINE,
                    Price = 2500,
                });
            }
        }

        public static void TravelingCartMachineGambling(StardewValley.Object machine, GameLocation location, Farmer who)
        {
            DataLoader.Shops(Game1.content).TryGetValue("Traveler", out ShopData shopData);
            if (shopData != null)
            {
                shopData.Currency = ShopMenu.currency_qiCoins;
                shopData.Items[0].MaxItems = 100;
            }

            Dictionary<ISalable,ItemStockInformation> stock = StardewValley.Internal.ShopBuilder.GetShopStock(ModConstants.TRAVELING_CART_GAMBLING_MACHINE_SHOP_DATA, shopData);
            Game1.showGlobalMessage("Start gambling!");
            machine.heldObject.Value = (StardewValley.Object)stock.Keys.ElementAt(Game1.random.Next(0, stock.Count));
            machine.MinutesUntilReady = 10;
        }

    }
}
