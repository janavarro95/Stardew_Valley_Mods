using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.GameData.Objects;
using StardewValley.GameData.Shops;
using StardewValley.Menus;
using StardewValley.Triggers;

namespace RefillSilos
{
    /// <summary>
    /// A mod used to add an item to be bought from Marnie to refill all of the silos on the Player's farm automatically.
    /// </summary>
    public class RefillSilos : Mod
    {
        /// <summary>
        /// The action id to be used for triggering refilling a silo for the player.
        /// </summary>
        public const string BUY_SILO_REFILL_ACTION = "Omegasis.RefillSilos.OnRefillSiloItemObtained";
        public const string REFILL_SILO_ITEM_ID = "Omegasis.RefillSilo.Item";
        public const string REFILL_SILO_ITEM_QUALIFIED_ID = "(O)"+ REFILL_SILO_ITEM_ID;

        /// <summary>
        /// Used to keep track of how much hay to actually provide when purchasing the silo refill option.
        /// </summary>
        public int numberOfHayToRefill = 0;

        public override void Entry(IModHelper helper)
        {
            TriggerActionManager.RegisterAction(BUY_SILO_REFILL_ACTION, new TriggerActionDelegate(this.onRefilSiloItemObtained));

            this.Helper.Events.Display.MenuChanged += this.updateAnimalShopForSiloRefillItem;
            this.Helper.Events.Content.AssetRequested += this.checkIfAssetCanBeEdited;
            this.Helper.Events.Player.InventoryChanged += this.Player_InventoryChanged;
        }

        private void Player_InventoryChanged(object? sender, InventoryChangedEventArgs e)
        {
            if (e.Player.currentLocation.NameOrUniqueName == "AnimalShop" && e.Added.Count() == 1 && e.Added.ElementAt(0).Name == REFILL_SILO_ITEM_ID)
            {
                this.tryToRefillHay();
                e.Player.removeItemFromInventory(e.Added.ElementAt(0));
            }

        }

        /// <summary>
        /// CHeck
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkIfAssetCanBeEdited(object? sender, AssetRequestedEventArgs e)
        {

            if (e.NameWithoutLocale.IsEquivalentTo("Data/Objects"))
            {
                e.Edit(this.addItemToObjectRegistry);
            }
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="data"></param>
        public void addItemToObjectRegistry(IAssetData data)
        {
            IDictionary<string, ObjectData> objectDictionary = data.AsDictionary<string, ObjectData>().Data;
            ObjectData objectData = new ObjectData();
            objectData.Name = REFILL_SILO_ITEM_ID;
            objectData.DisplayName = "Refill Silos";
            objectData.Texture = null;
            objectData.Description = "Refills all of the silos full of hay, as much as possible.";
            objectData.ExcludeFromFishingCollection = true;
            objectData.ExcludeFromRandomSale = true;
            objectData.ExcludeFromShippingCollection = true;
            objectData.SpriteIndex = 178;
            objectData.Price = 0;
            objectDictionary.Add(REFILL_SILO_ITEM_ID, objectData);
        }

        private void updateAnimalShopForSiloRefillItem(object? sender, MenuChangedEventArgs e)
        {
            if (e.NewMenu != null && e.NewMenu is ShopMenu)
            {
                ShopMenu shopMenu = (ShopMenu)e.NewMenu;
                if (shopMenu.ShopId.Equals("AnimalShop"))
                {
                    //Reset the counter for number of hay to refill.
                    this.numberOfHayToRefill = 0;

                    int index = 0;
                    foreach (ISalable key in shopMenu.itemPriceAndStock.Keys)
                    {

                        ItemStockInformation itemStockInformation = shopMenu.itemPriceAndStock[key];

                        //ShopItemData hayObjectForSale = shopMenu.ShopData.Items[index];
                        //Find the hay object.
                        if (key.QualifiedItemId.Equals("(O)178"))
                        {
                            ItemStockInformation refillItemStockInformation = new ItemStockInformation(this.numberOfHayToRefill * itemStockInformation.Price, 1);
                            refillItemStockInformation.ActionsOnPurchase = new List<string>() { BUY_SILO_REFILL_ACTION };


                            //Calculate how many pieces of hay need to be purchased.
                            foreach (GameLocation location in Game1.locations)
                            {
                                this.numberOfHayToRefill += location.GetHayCapacity() - location.piecesOfHay.Value;
                            }

                            //Cap the number of hay pieces the farmer can by by either the max number of peices they can buy, or by how much money they have, so that way the refill amount is always purchasable.
                            this.numberOfHayToRefill = Math.Min(this.numberOfHayToRefill, Game1.player.Money / itemStockInformation.Price);

                            //Get the price of hay and multiply by the number of pieces to refill for the silo.
                            refillItemStockInformation.TradeItemCount = 1;

                            //Only add the refill option if the number of hay pieces are greater than zero.
                            if (this.numberOfHayToRefill > 0)
                            {
                                //Insert the new item after the index of the hay item for better visibility.
                                Item refillItem = ItemRegistry.Create(REFILL_SILO_ITEM_QUALIFIED_ID);
                                refillItem.salePrice();

                                shopMenu.forSale.Insert(index + 1, refillItem);
                                shopMenu.itemPriceAndStock.Add(refillItem, refillItemStockInformation);

                                break;
                            }
                            else
                            {
                                this.Monitor.Log("Refill item can't bought! Not enough hay space available.", LogLevel.Info);
                                break;
                            }
                        }
                        index++;
                    }
                }
            }
        }

        /// <summary>
        /// Triggers when the player buys the silo refil item from marnie.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="triggerActionContext"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        private bool onRefilSiloItemObtained(string[] args, TriggerActionContext triggerActionContext, out string error)
        {
            this.tryToRefillHay();

            error = "Refilling the silos was successful, but for for some reason the game throws this error message. I can confirm that everything still works as intended.";
            return true;
        }

        /// <summary>
        /// Attempts to refill the hay for the player.
        /// </summary>
        private void tryToRefillHay()
        {
            int initialiNumberOfPiecesOfHayToFill = this.numberOfHayToRefill;
            foreach (GameLocation location in Game1.locations)
            {
                if (location.GetHayCapacity() <= 0)
                {
                    continue;
                }
                //The return amount is always the number of hay that can't be stored.
                this.numberOfHayToRefill = location.tryToAddHay(this.numberOfHayToRefill);
                if (this.numberOfHayToRefill <= 0)
                {
                    break;
                }
            }

            Game1.hudMessages.Add(new HUDMessage(string.Format("Bought {0} pieces of hay.", initialiNumberOfPiecesOfHayToFill)));

            this.numberOfHayToRefill = 0;

            if (Game1.activeClickableMenu != null)
            {
                if (Game1.activeClickableMenu is ShopMenu)
                {
                    ShopMenu shopMenu = (ShopMenu)Game1.activeClickableMenu;
                    //Since the item purchased is a fake item as it's just supposed to refill the silos, don't allow the player to add it to their inventory.
                    shopMenu.heldItem = null;
                }
            }
        }
    }
}
