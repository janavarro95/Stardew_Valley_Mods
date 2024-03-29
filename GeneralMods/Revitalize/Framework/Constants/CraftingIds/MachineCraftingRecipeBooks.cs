using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.Revitalize.Framework.Constants.CraftingIds
{
    /// <summary>
    /// Crafting recipe books that are used only for machines and can't be used for crafting by the player directly.
    /// Typically a crafting book will be the same id as the machine itself, but in some cases, it would be wise to use the same id to prevent having to use duplicate recipes all over.
    /// </summary>
    public class MachineCraftingRecipeBooks
    {
        public const string AlloyFurnaceCraftingRecipes = "Omegasis.Revitalize.Crafting.CraftingRecipeBooks.AlloyFurnace";

        public const string ElectricFurnaceCraftingRecipies = "Omegasis.Revitalize.Crafting.CraftingRecipeBooks.RevitalizeMachines.ElectricFurnace";
    }
}
