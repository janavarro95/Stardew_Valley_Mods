using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley.GameData.BigCraftables;
using StardewValley.GameData.Machines;
using StardewValley.GameData.Shops;
using StardewValley.GameData;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.Locations;
using Omegasis.StardustCore.Utilities.Objects;
using Microsoft.Xna.Framework;

namespace AdditionalGenerators
{
    //TODO: Make sure this works in shops/crafting.
    public class AdditionalGenerators : Mod
    {
        public override void Entry(IModHelper helper)
        {
            this.Helper.Events.Content.AssetRequested += this.checkIfAssetCanBeEdited;

            GameStateQuery.Register("Omegasis.AdditionalGenerators.IsLavaHere", new StardewValley.Delegates.GameStateQueryDelegate(this.isLavaHere));
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

            if (e.NameWithoutLocale.IsEquivalentTo("Omegasis_AdditionalGenerators/Assets/Graphics/BiofuelGenerator.png"))
            {
                e.LoadFromModFile<Texture2D>("Assets/Graphics/BiofuelGenerator.png", AssetLoadPriority.High);
            }
            if (e.NameWithoutLocale.IsEquivalentTo("Omegasis_AdditionalGenerators/Assets/Graphics/GeothermalGenerator.png"))
            {
                e.LoadFromModFile<Texture2D>("Assets/Graphics/GeothermalGenerator.png", AssetLoadPriority.High);
            }

            if (e.NameWithoutLocale.IsEquivalentTo("Data/CraftingRecipes"))
            {
                e.Edit(this.addCraftingRecipe);
            }

            if (e.NameWithoutLocale.IsEquivalentTo("Data/Shops"))
            {
                e.Edit(this.addGeneratorsToShops);
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
            objectData.Name = ModConstants.BioFuelGeneratorObjectId;
            objectData.DisplayName = "Biofuel Generator";
            objectData.Description = "Takes in various biomass and turns it into battery packs.";
            objectData.Texture = "Omegasis_AdditionalGenerators/Assets/Graphics/BiofuelGenerator.png";
            objectData.SpriteIndex = 0;
            objectData.Price = 0;
            objectDictionary.Add(ModConstants.BioFuelGeneratorObjectId, objectData);


            BigCraftableData geothermalGenerator = new BigCraftableData();
            geothermalGenerator.Name = ModConstants.GeothermalGeneratorObjectId;
            geothermalGenerator.DisplayName = "Geothermal Generator";
            geothermalGenerator.Description = "Takes in heat in order to produce battery packs. Only works in very warm places, such as deep in the mines, or the volcano.";
            geothermalGenerator.Texture = "Omegasis_AdditionalGenerators/Assets/Graphics/GeothermalGenerator.png";
            geothermalGenerator.SpriteIndex = 0;
            geothermalGenerator.Price = 0;
            objectDictionary.Add(ModConstants.GeothermalGeneratorObjectId, geothermalGenerator);


        }

        /// <summary>
        /// Adds the necessary machine data to make the crystal refiner work.
        /// </summary>
        /// <param name="data"></param>
        public void addMachineData(IAssetData data)
        {
            IDictionary<string, MachineData> machineDataDictionary = data.AsDictionary<string, MachineData>().Data;



            machineDataDictionary.Add(ModConstants.GeothermalGeneratorQualifiedObjectId, this.createGeothermalGeneratorMachineData());
            machineDataDictionary.Add(ModConstants.BioFuelGeneratorQualifiedObjectId, this.createBiofuelGeneratorMachineData());
        }

        private MachineData createGeothermalGeneratorMachineData()
        {
            MachineData objectData = new MachineData();
            objectData.AllowFairyDust = true;
            objectData.WobbleWhileWorking = true;
            objectData.ReadyTimeModifiers = new List<StardewValley.GameData.QuantityModifier>();
            objectData.OutputRules = new List<MachineOutputRule>();
            objectData.WorkingEffects = this.createGeneratorWorkingEffect();
            objectData.WorkingEffectChance = 0.05f;
            objectData.ShowNextIndexWhileWorking = true;

            MachineOutputRule machineOutputRule = new MachineOutputRule();
            //Add triggers for when the machine should work.
            machineOutputRule.Triggers = new List<MachineOutputTriggerRule>()
            {
                new MachineOutputTriggerRule(){
                    Id = "IsCaldera",
                    Trigger = MachineOutputTrigger.MachinePutDown,
                    Condition = "ANY \"Omegasis.AdditionalGenerators.IsLavaHere Here\""
                },
                new MachineOutputTriggerRule(){
                    Id = "TakeOutItem",
                    Trigger = MachineOutputTrigger.OutputCollected,
                },
            };

            //Add the output that can be processed by this machine.
            machineOutputRule.OutputItem = new List<MachineItemOutput>();
            MachineItemOutput machineItemOutput = new MachineItemOutput();
            machineItemOutput.PriceModifierMode = QuantityModifier.QuantityModifierMode.Stack;
            machineItemOutput.Id = "Default";
            //Battery Pack
            machineItemOutput.ItemId = "(O)787";
            machineOutputRule.DaysUntilReady = 7;

            machineOutputRule.OutputItem.Add(machineItemOutput);

            objectData.OutputRules.Add(machineOutputRule);
            return objectData;
        }

        private MachineData createBiofuelGeneratorMachineData()
        {
            MachineData objectData = new MachineData();
            objectData.AllowFairyDust = true;
            objectData.WobbleWhileWorking = true;
            objectData.ReadyTimeModifiers = new List<StardewValley.GameData.QuantityModifier>();
            objectData.OutputRules = new List<MachineOutputRule>();
            objectData.LoadEffects = this.createGeneratorWorkingEffect();
            objectData.WorkingEffects = this.createGeneratorWorkingEffect();
            objectData.WorkingEffectChance = 0.05f;
            objectData.ShowNextIndexWhileWorking = true;

            MachineOutputRule machineOutputRule = new MachineOutputRule();
            //Add triggers for when the machine should work.
            machineOutputRule.Triggers = new List<MachineOutputTriggerRule>() {
            new MachineOutputTriggerRule()
            {
                Id="Coal",
                Trigger = MachineOutputTrigger.ItemPlacedInMachine,
                RequiredCount = 3,
                RequiredItemId = "(O)382",
            },
            new MachineOutputTriggerRule()
            {
                Id="Seaweed",
                Trigger = MachineOutputTrigger.ItemPlacedInMachine,
                RequiredCount = 5,
                RequiredItemId = "(O)152",
            },
            new MachineOutputTriggerRule(){
                Id="GreenAlgae",
                Trigger = MachineOutputTrigger.ItemPlacedInMachine,
                RequiredCount = 5,
                RequiredItemId = "(O)153",
            },
            new MachineOutputTriggerRule(){
                Id="WhiteAlgae",
                Trigger = MachineOutputTrigger.ItemPlacedInMachine,
                RequiredCount = 5,
                RequiredItemId = "(O)157",
            },
            new MachineOutputTriggerRule(){
                Id="Fiber",
                Trigger = MachineOutputTrigger.ItemPlacedInMachine,
                RequiredCount = 25,
                RequiredItemId = "(O)771",
            },
            new MachineOutputTriggerRule(){
                Id="Moss",
                Trigger = MachineOutputTrigger.ItemPlacedInMachine,
                RequiredCount = 5,
                RequiredItemId = "Moss",
            },
            new MachineOutputTriggerRule(){
                Id="Slime",
                Trigger = MachineOutputTrigger.ItemPlacedInMachine,
                RequiredCount = 50,
                RequiredItemId = ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.Slime),
            }
            };


            //Add the output that can be processed by this machine.
            machineOutputRule.OutputItem = new List<MachineItemOutput>();
            MachineItemOutput machineItemOutput = new MachineItemOutput();
            machineItemOutput.PriceModifierMode = QuantityModifier.QuantityModifierMode.Stack;
            machineItemOutput.Id = "Default";
            //Battery Pack
            machineItemOutput.ItemId = "(O)787";
            //Adding the condition to the actual output rule itself determines if the trigger for item quality works.
            machineOutputRule.MinutesUntilReady = 360; //Takes 6 ingame hours to produce it.
            machineOutputRule.OutputItem.Add(machineItemOutput);
            objectData.OutputRules.Add(machineOutputRule);
            return objectData;
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
                    new ItemWithAmount(ObjectIds.StardewObjectIds.IronBar,5),
                    new ItemWithAmount(ObjectIds.StardewObjectIds.CopperBar,2),
                    new ItemWithAmount(ObjectIds.StardewObjectIds.Stone,25),
                    new ItemWithAmount(ObjectIds.StardewObjectIds.RefinedQuartz,2),
                },
                OutputItem = new ItemWithAmount()
                {
                    Id = ModConstants.BioFuelGeneratorObjectId,
                    Amount = 1
                },
                IsBigCraftable = true,

            };
            objectDictionary.Add(ModConstants.BioFuelGeneratorObjectId, craftingRecipeHelper.toCraftingRecipeFormat());


            CraftingRecipeHelper geothermalRecipeHelper = new CraftingRecipeHelper()
            {
                Ingredients = new List<ItemWithAmount>()
                {
                    new ItemWithAmount(ObjectIds.StardewObjectIds.IronBar,10),
                    new ItemWithAmount(ObjectIds.StardewObjectIds.GoldBar,5),
                    new ItemWithAmount(ObjectIds.StardewObjectIds.SolarEssence,10),
                    new ItemWithAmount(ObjectIds.StardewObjectIds.FireQuartz,3),
                },
                OutputItem = new ItemWithAmount()
                {
                    Id = ModConstants.GeothermalGeneratorObjectId,
                    Amount = 1
                },
                IsBigCraftable = true,

            };
            objectDictionary.Add(ModConstants.GeothermalGeneratorObjectId, geothermalRecipeHelper.toCraftingRecipeFormat());
        }


        public void addGeneratorsToShops(IAssetData data)
        {
            IDictionary<string, ShopData> shopDataDictionary = data.AsDictionary<string, ShopData>().Data;

            if (shopDataDictionary.ContainsKey("Carpenter"))
            {
                shopDataDictionary["Carpenter"].Items.Add(new ShopItemData()
                {
                    AvailableStock = -1,
                    AvailableStockLimit = LimitedStockMode.Player,
                    Id = ModConstants.BioFuelGeneratorObjectId,
                    TradeItemAmount = 1,
                    ItemId = ModConstants.BioFuelGeneratorObjectId,
                    Price = 10000,
                    IsRecipe = true,
                });

            }
            if (shopDataDictionary.ContainsKey("VolcanoShop"))
            {
                shopDataDictionary["VolcanoShop"].Items.Add(new ShopItemData()
                {
                    AvailableStock = -1,
                    AvailableStockLimit = LimitedStockMode.Player,
                    Id = ModConstants.GeothermalGeneratorObjectId,
                    TradeItemAmount = 1,
                    ItemId = ModConstants.GeothermalGeneratorObjectId,
                    Price = 20000,
                    IsRecipe = true,
                });

            }
        }

        public bool isLavaHere(string[] args, GameStateQueryContext context)
        {
            if (context.Location != null && context.Location is MineShaft)
            {
                return ((MineShaft)context.Location).mineLevel == 100;
            }
            if (context.Location != null && context.Location is Caldera)
            {
                return true;
            }
            /*
             * Since items placed inside of the volcano dungeon are deleted every day, don't let geothermal generators work here.
            if (context.Location != null && context.Location is VolcanoDungeon)
            {
                return true;
            }
            */
            return false;
        }

        /// <summary>
        /// Helper method to create the effect of a generator working.
        /// </summary>
        /// <returns></returns>
        public List<MachineEffects> createGeneratorWorkingEffect()
        {
            return new List<MachineEffects>()
            {
                new MachineEffects()
                {
                    Id="Default",
                    Sounds=new List<MachineSoundData>()
                    {
                        new MachineSoundData()
                        {
                            Id="fireball"
                        }
                    },
                    Interval=100,
                    TemporarySprites=new List<TemporaryAnimatedSpriteDefinition>{ new TemporaryAnimatedSpriteDefinition()
                    {
                        Id="Default",
                        Texture="Tilesheets\\animations",
                        SourceRect=new Rectangle(0,1728,64,64),
                        Interval=50,
                        Frames=4,
                        Loops=10,
                        PositionOffset=new Vector2(-4,-32),
                        AlphaFade=0.005f,
                        Scale=0.25f,
                    } },
                }
            };
        }
    }
}
