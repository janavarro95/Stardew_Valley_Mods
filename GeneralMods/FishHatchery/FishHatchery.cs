using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley.GameData.BigCraftables;
using StardewValley.GameData.Machines;
using StardewValley.GameData.Shops;
using StardewValley.GameData;
using StardewValley;
using Omegasis.StardustCore.Utilities.Objects;

namespace FishHatchery
{
    public class FishHatchery : Mod
    {
        public override void Entry(IModHelper helper)
        {
            this.Helper.Events.Content.AssetRequested += this.checkIfAssetCanBeEdited;
            this.Helper.Events.GameLoop.SaveLoaded += this.GameLoop_SaveLoaded;
        }

        private void GameLoop_SaveLoaded(object? sender, SaveLoadedEventArgs e)
        {
            Game1.player.addItemByMenuIfNecessary(ItemRegistry.Create(ModConstants.FishHatcheryQualifiedObjectId, 3));
        }

        /// <summary>
        /// Checks to see if a given asset can be loaded for a specific case.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkIfAssetCanBeEdited(object? sender, AssetRequestedEventArgs e)
        {
            if (e.NameWithoutLocale.IsEquivalentTo("Data/BigCraftables"))
            {
                e.Edit(this.addItemToObjectRegistry);
            }

            if (e.NameWithoutLocale.IsEquivalentTo("Data/Machines"))
            {
                e.Edit(this.addMachineData);
            }

            if (e.NameWithoutLocale.IsEquivalentTo("Omegasis_FishHatchery/Assets/Graphics/FishHatchery.png"))
            {
                e.LoadFromModFile<Texture2D>("Assets/Graphics/FishHatchery.png", AssetLoadPriority.High);
            }

            if (e.NameWithoutLocale.IsEquivalentTo("Data/CraftingRecipes"))
            {
                e.Edit(this.addCraftingRecipe);
            }

            if (e.NameWithoutLocale.IsEquivalentTo("Data/Shops"))
            {
                e.Edit(this.addFishHatcheryToStardewValleyFishShop);
            }
        }

        /// <summary>
        /// Adds the crystal refiner to the game's big craftable information.
        /// </summary>
        /// <param name="data"></param>
        public void addItemToObjectRegistry(IAssetData data)
        {
            IDictionary<string, BigCraftableData> objectDictionary = data.AsDictionary<string, BigCraftableData>().Data;
            BigCraftableData objectData = new BigCraftableData();

            objectData.Name = ModConstants.FishHatcheryObjectId;
            objectData.DisplayName = "Fish Hatchery";
            objectData.Description = "Takes in roe and hatches fish from it after a few days.";
            objectData.Texture = "Omegasis_FishHatchery/Assets/Graphics/FishHatchery.png";
            objectData.SpriteIndex = 0;
            objectData.Price = 0;
            objectDictionary.Add(ModConstants.FishHatcheryObjectId, objectData);

        }

        /// <summary>
        /// Adds the necessary machine data to make the crystal refiner work.
        /// </summary>
        /// <param name="data"></param>
        public void addMachineData(IAssetData data)
        {
            IDictionary<string, MachineData> machineDataDictionary = data.AsDictionary<string, MachineData>().Data;
            MachineData objectData = new MachineData();

            objectData.AllowFairyDust = true;
            objectData.WobbleWhileWorking = true;
            objectData.ReadyTimeModifiers = new List<StardewValley.GameData.QuantityModifier>();
            objectData.OutputRules = new List<MachineOutputRule>();
            objectData.LoadEffects = new List<MachineEffects>()
            {
                new MachineEffects()
                {
                    Id="Default",
                    Sounds=new List<MachineSoundData>()
                    {
                        new MachineSoundData()
                        {
                            Id="Ship"
                        }
                    },
                }
            };

            MachineOutputRule machineOutputRule = new MachineOutputRule();
            machineOutputRule.Triggers = new List<MachineOutputTriggerRule>();

            //Add triggers for when fish hatchery should work.
            MachineOutputTriggerRule machineOutputTriggerRule = new MachineOutputTriggerRule();
            machineOutputTriggerRule.Id = "Default";
            machineOutputTriggerRule.Trigger = MachineOutputTrigger.ItemPlacedInMachine;
            machineOutputTriggerRule.RequiredCount = 1;
            machineOutputTriggerRule.RequiredItemId = "(O)812";
            machineOutputRule.Triggers.Add(machineOutputTriggerRule);
            

            //Add the output that can be processed by this machine.
            machineOutputRule.OutputItem = new List<MachineItemOutput>();
            MachineItemOutput machineItemOutput = new MachineItemOutput();
            machineItemOutput.PriceModifierMode = QuantityModifier.QuantityModifierMode.Stack;
            machineItemOutput.Id = "Default";
            //Gets the actual item id that was used to make the preserved item.
            machineItemOutput.ItemId = "DROP_IN_PRESERVE";
            machineItemOutput.QualityModifiers = new List<QuantityModifier> {
                new QuantityModifier(){
                Modification= QuantityModifier.ModificationType.Set,
                Condition= "RANDOM 0.25",
                RandomAmount=new List<float>(){1}
                },
                new QuantityModifier(){
                Modification= QuantityModifier.ModificationType.Set,
                Condition= "RANDOM 0.15",
                RandomAmount=new List<float>(){2}
                },
                new QuantityModifier(){
                Modification= QuantityModifier.ModificationType.Set,
                Condition= "RANDOM 0.1",
                RandomAmount=new List<float>(){4}
                }
            };
            //Adding the condition to the actual output rule itself determines if the trigger for item quality works.
            machineOutputRule.DaysUntilReady = 3; //Make it take 3 days to process.
            machineOutputRule.OutputItem.Add(machineItemOutput);


            objectData.OutputRules.Add(machineOutputRule);

            machineDataDictionary.Add(ModConstants.FishHatcheryQualifiedObjectId, objectData);

        }

        /// <summary>
        /// Adds the crafting recipe for the crystal refiner to the game.
        /// </summary>
        /// <param name="data"></param>
        public void addCraftingRecipe(IAssetData data)
        {
            IDictionary<string, string> objectDictionary = data.AsDictionary<string, string>().Data;

            CraftingRecipeHelper craftingRecipeHelper = new CraftingRecipeHelper()
            {
                Ingredients = new List<ItemWithAmount>()
                {
                    new ItemWithAmount(ObjectIds.StardewObjectIds.RefinedQuartz,10),
                    new ItemWithAmount(ObjectIds.StardewObjectIds.Seaweed,2),
                    new ItemWithAmount(ObjectIds.StardewObjectIds.GreenAlgae,2),
                    new ItemWithAmount(ObjectIds.StardewObjectIds.Stone,25),
                },
                OutputItem = new ItemWithAmount()
                {
                    Id = ModConstants.FishHatcheryObjectId,
                    Amount = 1
                },
                IsBigCraftable = true,

            };
            objectDictionary.Add(ModConstants.FishHatcheryObjectId, craftingRecipeHelper.toCraftingRecipeFormat());
        }


        public void addFishHatcheryToStardewValleyFishShop(IAssetData data)
        {
            IDictionary<string, ShopData> shopDataDictionary = data.AsDictionary<string, ShopData>().Data;

            if (shopDataDictionary.ContainsKey("FishShop"))
            {
                shopDataDictionary["FishShop"].Items.Add(new ShopItemData()
                {
                    AvailableStock = -1,
                    AvailableStockLimit = LimitedStockMode.Player,
                    Id = ModConstants.FishHatcheryObjectId,
                    TradeItemAmount = 1,
                    ItemId = ModConstants.FishHatcheryObjectId,
                    Price = 10000,
                    IsRecipe = true,
                });

            }

        }
    }
}
