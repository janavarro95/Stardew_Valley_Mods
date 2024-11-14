using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Omegasis.StardustCore.Utilities.Objects;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.GameData;
using StardewValley.GameData.BigCraftables;
using StardewValley.GameData.Machines;
using StardewValley.GameData.Shops;

namespace Arboretum
{
    public class Arboretum : Mod
    {
        public override void Entry(IModHelper helper)
        {
            this.Helper.Events.Content.AssetRequested += this.checkIfAssetCanBeEdited;

            this.Helper.Events.GameLoop.SaveLoaded += this.GameLoop_SaveLoaded;
        }

        private void GameLoop_SaveLoaded(object? sender, SaveLoadedEventArgs e)
        {
            Game1.player.addItemByMenuIfNecessaryElseHoldUp(ItemRegistry.Create(ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.Acorn), 5));
            Game1.player.addItemByMenuIfNecessaryElseHoldUp(ItemRegistry.Create(ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.PineCone), 5));
            Game1.player.addItemByMenuIfNecessaryElseHoldUp(ItemRegistry.Create(ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.MahoganySeed), 5));
            Game1.player.addItemByMenuIfNecessaryElseHoldUp(ItemRegistry.Create(ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.MapleSeed), 5));

            Game1.player.addItemByMenuIfNecessaryElseHoldUp(ItemRegistry.Create(ModConstants.ArboretumQualifiedObjectId, 20));
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

            if (e.NameWithoutLocale.IsEquivalentTo("Omegasis_Arboretum/Assets/Graphics/Arboretum.png"))
            {
                e.LoadFromModFile<Texture2D>("Assets/Graphics/Arboretum.png", AssetLoadPriority.High);
            }

            if (e.NameWithoutLocale.IsEquivalentTo("Data/CraftingRecipes"))
            {
                e.Edit(this.addCraftingRecipe);
            }

            if (e.NameWithoutLocale.IsEquivalentTo("Data/Shops"))
            {
                e.Edit(this.addToShop);
            }
        }

        /// <summary>
        /// Adds the object(s) to the game's big craftable information.
        /// </summary>
        /// <param name="data"></param>
        public void addItemToObjectRegistry(IAssetData data)
        {
            IDictionary<string, BigCraftableData> objectDictionary = data.AsDictionary<string, BigCraftableData>().Data;

            BigCraftableData objectData = new BigCraftableData();
            objectData.Name = ModConstants.ArboretumObjectId;
            objectData.DisplayName = "Arboretum";
            objectData.Description = "Allows growing tree seeds to create various products.";
            objectData.Texture = "Omegasis_Arboretum/Assets/Graphics/Arboretum.png";
            objectData.SpriteIndex = 0;
            objectData.Price = 0;
            objectDictionary.Add(ModConstants.ArboretumObjectId, objectData);


        }

        /// <summary>
        /// Adds the necessary machine data to make the added machines work.
        /// </summary>
        /// <param name="data"></param>
        public void addMachineData(IAssetData data)
        {
            IDictionary<string, MachineData> machineDataDictionary = data.AsDictionary<string, MachineData>().Data;
            machineDataDictionary.Add(ModConstants.ArboretumQualifiedObjectId, this.createArboretumMachineData());
        }

        private MachineData createArboretumMachineData()
        {
            MachineData objectData = new MachineData();
            objectData.AllowFairyDust = true;
            objectData.WobbleWhileWorking = true;
            objectData.ReadyTimeModifiers = new List<StardewValley.GameData.QuantityModifier>();
            objectData.OutputRules = new List<MachineOutputRule>();
            objectData.LoadEffects = this.createArboretumLoadSeedEffects();
            objectData.WorkingEffects = this.createArboretumWorkingEffects();
            objectData.WorkingEffectChance = 1f;

            objectData.ShowNextIndexWhileWorking = true;
            objectData.ShowNextIndexWhenReady= true;

            MachineOutputRule machineOutputRule = new MachineOutputRule();
            machineOutputRule.Id = "Omegasis.Arboretum.TreeSeeds";
            machineOutputRule.Triggers = new List<MachineOutputTriggerRule>() {
            new MachineOutputTriggerRule()
            {
                Id="Acorn",
                Trigger = MachineOutputTrigger.ItemPlacedInMachine,
                RequiredCount = 1,
                RequiredItemId = ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.Acorn),
            },
            new MachineOutputTriggerRule()
            {
                Id="MapleSeed",
                Trigger = MachineOutputTrigger.ItemPlacedInMachine,
                RequiredCount = 1,
                RequiredItemId = ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.MapleSeed),
            },
            new MachineOutputTriggerRule()
            {
                Id="Pinecone",
                Trigger = MachineOutputTrigger.ItemPlacedInMachine,
                RequiredCount = 1,
                RequiredItemId = ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.PineCone),
            },
            new MachineOutputTriggerRule()
            {
                Id="MahoganySeed",
                Trigger = MachineOutputTrigger.ItemPlacedInMachine,
                RequiredCount = 1,
                RequiredItemId = ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.MahoganySeed),
            },

            };
            //Add the output that can be processed by this machine.
            machineOutputRule.OutputItem = new List<MachineItemOutput>()
            {
                //Acorn outputs
                new MachineItemOutput()
                {
                    Id="Wood_Acorn",
                    ItemId=ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.Wood),
                    MinStack=8,
                    MaxStack=12,
                    Condition="RANDOM 0.8, ITEM_ID Input "+ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.Acorn)
                },
                new MachineItemOutput()
                {
                    Id="Sap_Acorn",
                    ItemId=ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.Sap),
                    MinStack=5,
                    MaxStack=5,
                    Condition="RANDOM 0.1,ITEM_ID Input "+ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.Acorn)
                },
                new MachineItemOutput()
                {
                    Id="Acorn",
                    ItemId=ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.Acorn),
                    MinStack=2,
                    MaxStack=2,
                    Condition="RANDOM 0.1,ITEM_ID Input "+ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.Acorn)
                },

                //Maple seed outputs
                new MachineItemOutput()
                {
                    Id="Wood_MapleSeed",
                    ItemId=ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.Wood),
                    MinStack=8,
                    MaxStack=12,
                    Condition="RANDOM 0.8,ITEM_ID Input "+ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.MapleSeed)
                },
                new MachineItemOutput()
                {
                    Id="Sap_MapleSeed",
                    ItemId=ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.Sap),
                    MinStack=5,
                    MaxStack=5,
                    Condition="RANDOM 0.1,ITEM_ID Input "+ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.MapleSeed)
                },
                new MachineItemOutput()
                {
                    Id="MapleSeed",
                    ItemId=ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.MapleSeed),
                    MinStack=2,
                    MaxStack=2,
                    Condition="RANDOM 0.1,ITEM_ID Input "+ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.MapleSeed)
                },

                //Pinecone outputs.
                new MachineItemOutput()
                {
                    Id="Wood_Pinecone",
                    ItemId=ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.Wood),
                    MinStack=8,
                    MaxStack=12,
                    Condition="RANDOM 0.8,ITEM_ID Input "+ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.PineCone)
                },
                new MachineItemOutput()
                {
                    Id="Sap_PineCone",
                    ItemId=ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.Sap),
                    MinStack=5,
                    MaxStack=5,
                    Condition="RANDOM 0.1,ITEM_ID Input "+ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.PineCone)
                },
                new MachineItemOutput()
                {
                    Id="PineCone",
                    ItemId=ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.PineCone),
                    MinStack=2,
                    MaxStack=2,
                    Condition="RANDOM 0.1,ITEM_ID Input "+ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.PineCone)
                },
                
                //Mahogany outputs.
                new MachineItemOutput()
                {
                    Id="HardWood_Mahogany",
                    ItemId=ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.Hardwood),
                    MinStack=10,
                    MaxStack=10,
                    Condition="RANDOM 0.8,ITEM_ID Input "+ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.MahoganySeed)
                },
                new MachineItemOutput()
                {
                    Id="Sap_Mahogany",
                    ItemId=ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.Sap),
                    MinStack=5,
                    MaxStack=5,
                    Condition="RANDOM 0.1,ITEM_ID Input "+ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.MahoganySeed)
                },
                new MachineItemOutput()
                {
                    Id="MahoganySeed",
                    ItemId=ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.MahoganySeed),
                    MinStack=2,
                    MaxStack=2,
                    Condition="RANDOM 0.1,ITEM_ID Input "+ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.MahoganySeed)
                }
            };
            machineOutputRule.DaysUntilReady = 4;
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
                    new ItemWithAmount(ObjectIds.StardewObjectIds.TreeFertilizer,1),
                    new ItemWithAmount(ObjectIds.StardewObjectIds.Stone,25),
                    new ItemWithAmount(ObjectIds.StardewObjectIds.RefinedQuartz,5),
                },
                OutputItem = new ItemWithAmount()
                {
                    Id = ModConstants.ArboretumObjectId,
                    Amount = 1
                },
                IsBigCraftable = true,

            };
            objectDictionary.Add(ModConstants.ArboretumObjectId, craftingRecipeHelper.toCraftingRecipeFormat());
        }

        public void addToShop(IAssetData data)
        {
            IDictionary<string, ShopData> shopDataDictionary = data.AsDictionary<string, ShopData>().Data;

            if (shopDataDictionary.ContainsKey("Carpenter"))
            {
                shopDataDictionary["Carpenter"].Items.Add(new ShopItemData()
                {
                    AvailableStock = -1,
                    AvailableStockLimit = LimitedStockMode.Player,
                    Id = ModConstants.ArboretumObjectId,
                    TradeItemAmount = 1,
                    ItemId = ModConstants.ArboretumObjectId,
                    Price = 10000,
                    IsRecipe = true,
                });

            }
        }

        public List<MachineEffects> createArboretumWorkingEffects()
        {
            return new List<MachineEffects>()
            {
                new MachineEffects()
                {
                    Id="Acorn",
                    Frames= new List<int>()
                    {
                        1
                    },
                    Condition = "ITEM_ID Input "+ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.Acorn)
                },
                new MachineEffects()
                {
                    Id="MapleSeed",
                    Frames= new List<int>()
                    {
                        2
                    },
                    Condition = "ITEM_ID Input "+ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.MapleSeed)
                },
                new MachineEffects()
                {
                    Id="Pinecone",
                    Frames= new List<int>()
                    {
                        3
                    },
                    Condition = "ITEM_ID Input "+ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.PineCone)
                },
                new MachineEffects()
                {
                    Id="Mahogany",
                    Frames= new List<int>()
                    {
                        4
                    },
                    Condition = "ITEM_ID Input "+ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.MahoganySeed)
                }
            };
        }

        public List<MachineEffects> createArboretumLoadSeedEffects()
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
                            Id="dirtyHit"
                        },
                    },
                },
                new MachineEffects()
                {
                    Id="Acorn",
                    Frames= new List<int>()
                    {
                        1
                    },
                    Condition = "ITEM_ID Input "+ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.Acorn)
                },
                new MachineEffects()
                {
                    Id="MapleSeed",
                    Frames= new List<int>()
                    {
                        2
                    },
                    Condition = "ITEM_ID Input "+ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.MapleSeed)
                },
                new MachineEffects()
                {
                    Id="Pinecone",
                    Frames= new List<int>()
                    {
                        3
                    },
                    Condition = "ITEM_ID Input "+ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.PineCone)
                },
                new MachineEffects()
                {
                    Id="Mahogany",
                    Frames= new List<int>()
                    {
                        4
                    },
                    Condition = "ITEM_ID Input "+ObjectIds.GetQualifiedObjectIdFromStardewObjectId(ObjectIds.StardewObjectIds.MahoganySeed)
                }
            };
        }
    }
}
