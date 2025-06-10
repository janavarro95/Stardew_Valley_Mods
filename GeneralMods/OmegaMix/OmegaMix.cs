
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley.GameData.FarmAnimals;
using StardewValley.GameData.Machines;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OmegaMix
{
    public class OmegaMix : Mod
    {
        public override void Entry(IModHelper helper)
        {
            this.Helper.Events.Content.AssetRequested += this.checkIfAssetCanBeEdited;
        }

        private void checkIfAssetCanBeEdited(object? sender, AssetRequestedEventArgs e)
        {
            if (e.NameWithoutLocale.IsEquivalentTo("Data/FarmAnimals"))
            {
                e.Edit(this.createFarmAnimalData);
            }
        }

        private void createFarmAnimalData(IAssetData obj)
        {
            IDictionary<string, FarmAnimalData> machineDataDictionary = obj.AsDictionary<string, FarmAnimalData>().Data;

            FarmAnimalData mossySheep = new()
            {
                DisplayName = "Mossy Sheep",
                House = "Barn",
                PurchasePrice = 10_000,
                SellPrice = 20_000,

                //TODO: Add in the face texture for the shop
                ShopTexture = "",
                ShopSourceRect = new(0, 0, 16, 32),
                ShopDisplayName = "Baby Mossy Sheep",
                ShopDescription = "A sheep that produces moss every three days",
                ShopMissingBuildingDescription = "[LocalizedText Strings\\StringsFromCSFiles:Utility.cs.5944]",
                RequiredBuilding = "Deluxe Barn",
                UnlockCondition = "PLAYER_BASE_FARMING_LEVEL 10, PLAYER_BASE_FORAGING_LEVEL 10",
                DaysToMature = 4,
                CanGetPregnant = true,
                DaysToProduce  = 1,
                HarvestType = FarmAnimalHarvestType.HarvestWithTool,
                HarvestTool = "Shears",
                ProduceItemIds = new List<FarmAnimalProduce>()
                {
                    new FarmAnimalProduce()
                    {
                        Id = "Moss",
                        ItemId = "Moss",
                    }
                },
                ProduceOnMature = true,
                CanEatGoldenCrackers = true,
                Sound = "Sheep",
                //TODO: Add in texture
                Texture = "",
                //TODO: Add in harvested texture
                HarvestedTexture = "",
                //TODO: Add in baby texture
                BabyTexture = "",
                UseFlippedRightForLeft = true,
                SpriteWidth = 32,
                SpriteHeight = 32,
                SleepFrame = 12,
                SwimOffset =
                {
                    X = 0,
                    Y = 112
                },
                GrassEatAmount = 4,
                HappinessDrain = 5,
                ShowInSummitCredits = true



            };


        }
    }
}
