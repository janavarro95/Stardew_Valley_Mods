using Microsoft.Xna.Framework.Graphics;
using Omegasis.StardustCore.Utilities.Objects;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.GameData;
using StardewValley.GameData.BigCraftables;
using StardewValley.GameData.Machines;
using StardewValley.GameData.Objects;
using StardewValley.GameData.Shops;
using StardewValley.Triggers;

namespace CrystalRefiner
{
    /// <summary>
    /// A mod to add a crystal refiner machine which can increase the quality of gems and minerals.
    /// </summary>
    public class CrystalRefiner : Mod
    {
        public override void Entry(IModHelper helper)
        {
            this.Helper.Events.Content.AssetRequested += this.checkIfAssetCanBeEdited;
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
                e.Edit(this.addCrystalRefinerMachineData);
            }

            if (e.NameWithoutLocale.IsEquivalentTo("Omegasis_CrystalRefiner/Assets/Graphics/CrystalRefiner.png"))
            {
                e.LoadFromModFile<Texture2D>("Assets/Graphics/CrystalRefiner.png", AssetLoadPriority.High);
            }

            if (e.NameWithoutLocale.IsEquivalentTo("Data/CraftingRecipes"))
            {
                e.Edit(this.addCraftingRecipe);
            }

            if (e.NameWithoutLocale.IsEquivalentTo("Data/Shops"))
            {
                e.Edit(this.addCrystalRefinerToDwarfShop);
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

            objectData.Name = Constants.CrystalRefinerObjectId;
            objectData.DisplayName = "Crystal Refiner";
            objectData.Description = "Refines gems and minerals to be a higher quality.";
            objectData.Texture = "Omegasis_CrystalRefiner/Assets/Graphics/CrystalRefiner.png";
            objectData.SpriteIndex = 0;
            objectData.Price = 0;
            objectDictionary.Add(Constants.CrystalRefinerObjectId, objectData);

        }

        /// <summary>
        /// Adds the necessary machine data to make the crystal refiner work.
        /// </summary>
        /// <param name="data"></param>
        public void addCrystalRefinerMachineData(IAssetData data)
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
                            Id="select"
                        }
                    },
                    Interval=100,
                }
            };

            MachineOutputRule machineOutputRule = new MachineOutputRule();
            machineOutputRule.Triggers = new List<MachineOutputTriggerRule>();

            //Add triggers for when the crystal refiner should turn on.
            MachineOutputTriggerRule machineOutputTriggerRuleGems = new MachineOutputTriggerRule();
            machineOutputTriggerRuleGems.Id = "GemPlacedIntoMachine";
            machineOutputTriggerRuleGems.Trigger = MachineOutputTrigger.ItemPlacedInMachine;
            machineOutputTriggerRuleGems.RequiredCount = 1;
            machineOutputTriggerRuleGems.RequiredTags = new List<string>()
            {
                "category_gem"
            };
            machineOutputTriggerRuleGems.Condition = string.Format("ITEM_CATEGORY INPUT {0}", StardewValley.Object.GemCategory.ToString());

            MachineOutputTriggerRule machineOutputTriggerRuleMinerals = new MachineOutputTriggerRule();
            machineOutputTriggerRuleMinerals.Id = "MineralsPlacedIntoMachine";
            machineOutputTriggerRuleMinerals.Trigger = MachineOutputTrigger.ItemPlacedInMachine;
            machineOutputTriggerRuleMinerals.RequiredCount = 1;
            machineOutputTriggerRuleMinerals.RequiredTags = new List<string>()
            {
                "category_minerals"
            };
            machineOutputTriggerRuleMinerals.Condition = string.Format("ITEM_CATEGORY INPUT {0}", StardewValley.Object.mineralsCategory.ToString());
            machineOutputRule.Triggers.Add(machineOutputTriggerRuleGems);
            machineOutputRule.Triggers.Add(machineOutputTriggerRuleMinerals);

            //Add the output that can be processed by this machine.
            machineOutputRule.OutputItem = new List<MachineItemOutput>();
            MachineItemOutput machineItemOutput = new MachineItemOutput();
            machineItemOutput.PriceModifierMode = QuantityModifier.QuantityModifierMode.Stack;
            machineItemOutput.Id = "Default";
            machineItemOutput.ItemId = "DROP_IN";
            //Adding the condition to the actual output rule itself determines if the trigger for item quality works.
            machineItemOutput.Condition = "ANY \"ITEM_QUALITY Input 0 2\"";
            int maxQualityLevel = QualityUtilities.GetMaxQualityLevel();
            machineItemOutput.Quality = maxQualityLevel;
            machineOutputRule.DaysUntilReady = QualityUtilities.GetQualityProgressionList(false).Count-1; //For every stage, make it "take" 1 day to process to get to the top tier quality.
            machineOutputRule.OutputItem.Add(machineItemOutput);


            objectData.OutputRules.Add(machineOutputRule);

            machineDataDictionary.Add(Constants.CrystalRefinerQualifiedObjectId, objectData);

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
                    new ItemWithAmount()
                    {
                        //Copper bar
                        Id = "(O)334",
                        Amount = 5,
                    },
                    new ItemWithAmount()
                    {
                        //Iron bar
                        Id = "(O)335",
                        Amount = 5,
                    },
                    new ItemWithAmount()
                    {
                        //Battery Pack
                        Id = "(O)787",
                        Amount = 2,
                    },
                    new ItemWithAmount()
                    {
                        //Stone
                        Id = "(O)390",
                        Amount = 100,
                    }
                },
                OutputItem = new ItemWithAmount()
                {
                    Id = Constants.CrystalRefinerObjectId,
                    Amount = 1
                },
                IsBigCraftable = true,

            };
            objectDictionary.Add(Constants.CrystalRefinerObjectId, craftingRecipeHelper.toCraftingRecipeFormat());
        }


        public void addCrystalRefinerToDwarfShop(IAssetData data)
        {
            IDictionary<string, ShopData> shopDataDictionary = data.AsDictionary<string, ShopData>().Data;

            if (shopDataDictionary.ContainsKey("Dwarf"))
            {
                shopDataDictionary["Dwarf"].Items.Add(new ShopItemData()
                {
                    AvailableStock = -1,
                    AvailableStockLimit = LimitedStockMode.Player,
                    Id = Constants.CrystalRefinerObjectId,
                    TradeItemAmount = 1,
                    ItemId = Constants.CrystalRefinerObjectId,
                    Price = 10000,
                    IsRecipe = true,
                });

            }

        }
    }
}
