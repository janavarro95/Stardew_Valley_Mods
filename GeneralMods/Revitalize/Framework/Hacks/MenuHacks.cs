using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.World.Objects.Interfaces;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Objects;

namespace Omegasis.Revitalize.Framework.Hacks
{
    /// <summary>
    /// Deals with hijacking menus for custom logic.
    /// </summary>
    public class MenuHacks
    {
        /// <summary>
        /// Checks to see if the mod has had it's custom object processed at the end of the day.
        /// </summary>
        public static bool EndOfDay_HasProcessedModdedItems;
        /// <summary>
        /// Checks to see if the end of day menus are up and running.
        /// </summary>
        /// <returns></returns>
        public static bool EndOfDay_IsShowingEndOfNightMenus()
        {
            return Game1.showingEndOfNightStuff;
        }

        /// <summary>
        /// Checks to see if the current end of day menu is the shippping menu.
        /// </summary>
        /// <returns></returns>
        public static bool EndOfDay_IsEndOfDayMenuShippingMenu()
        {
            if (EndOfDay_IsShowingEndOfNightMenus())
            {
                if (Game1.activeClickableMenu.GetType() == typeof(ShippingMenu)) return true;
                if (Game1.endOfNightMenus.Count == 0 && Game1.activeClickableMenu == null) return false;
                if (Game1.endOfNightMenus.Peek().GetType() == typeof(ShippingMenu))
                    return true;
                else
                    return false;
            }
            else return false;
        }

        /// <summary>
        /// Gets the shipping menu from the end of day menus.
        /// </summary>
        /// <returns></returns>
        public static ShippingMenu EndOfDay_GetShippingMenu()
        {
            if (EndOfDay_IsEndOfDayMenuShippingMenu())
            {
                if (Game1.activeClickableMenu.GetType() == typeof(ShippingMenu))
                    return (ShippingMenu)Game1.activeClickableMenu;
                return (ShippingMenu)Game1.endOfNightMenus.Peek();
            }
            return null;
        }

        /// <summary>
        /// Hijacks the shipping menu to process modded items.
        /// </summary>
        public static void EndOfDay_HackShipping()
        {
            if (EndOfDay_GetShippingMenu() != null)
            {
                //ModCore.log("Hooked shipping menu!");
                ShippingMenu menu = EndOfDay_GetShippingMenu();

                List<int> categoryTotals = new List<int>();
                List<MoneyDial> categoryDials = new List<MoneyDial>();
                List<List<Item>> categoryItems = new List<List<Item>>();

                var CT_R = RevitalizeModCore.ModHelper.Reflection.GetField<List<int>>(menu, "categoryTotals", true);
                var CD_R = RevitalizeModCore.ModHelper.Reflection.GetField<List<MoneyDial>>(menu, "categoryDials", true);
                var CI_R = RevitalizeModCore.ModHelper.Reflection.GetField<List<List<Item>>>(menu, "categoryItems", true);

                categoryTotals = CT_R.GetValue();
                categoryDials = CD_R.GetValue();
                categoryItems = CI_R.GetValue();

                //Recalculate other category.
                foreach (ICommonObjectInterface obj in categoryItems[4])
                {
                    RevitalizeModCore.log(obj.Name);
                    if (obj is StardewValley.Object)
                    {
                        RevitalizeModCore.log(obj.sellToStorePrice());
                        categoryTotals[4] += obj.sellToStorePrice() * obj.Stack;
                        Game1.stats.itemsShipped += (uint)obj.Stack;
                        /*
                        if (o.Category == -75 || o.Category == -79)
                            Game1.stats.CropsShipped += (uint)o.Stack;
                        if (o.countsForShippedCollection())
                            Game1.player.shippedBasic((int)((NetFieldBase<int, NetInt>)o.parentSheetIndex), (int)((NetFieldBase<int, NetInt>)o.stack));
                            */
                    }
                }
                categoryTotals[5] = 0;
                for (int index = 0; index < 5; ++index)
                {
                    categoryTotals[5] += categoryTotals[index];
                    categoryItems[5].AddRange(categoryItems[index]);
                    categoryDials[index].currentValue = categoryTotals[index];
                    categoryDials[index].previousTargetValue = categoryDials[index].currentValue;
                }
                categoryDials[5].currentValue = categoryTotals[5];
                if (Game1.IsMasterGame)
                    Game1.player.Money += categoryTotals[5];
                Game1.setRichPresence("earnings", categoryTotals[5]);

            }
        }

        /// <summary>
        /// Triggers 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="sender"></param>
        public static void EndOfDay_RenderCheck(object o, StardewModdingAPI.Events.RenderedEventArgs sender)
        {
            if (EndOfDay_IsShowingEndOfNightMenus() && EndOfDay_HasProcessedModdedItems == false)
            {
                EndOfDay_HackShipping();
                EndOfDay_HasProcessedModdedItems = true;
            }
        }

        public static void EndOfDay_CleanupForNewDay(object o, StardewModdingAPI.Events.SavedEventArgs sender)
        {
            EndOfDay_HasProcessedModdedItems = false;
        }


    }
}
