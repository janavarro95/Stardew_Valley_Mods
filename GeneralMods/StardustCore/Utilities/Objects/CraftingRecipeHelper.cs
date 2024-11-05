using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace Omegasis.StardustCore.Utilities.Objects
{
    /// <summary>
    /// A helper class to help create recipe formats for Stardew Valley.
    /// </summary>
    public class CraftingRecipeHelper
    {

        /// <summary>
        /// The list of ingredients that are used for this crafting recipe.
        /// </summary>
        public List<ItemWithAmount>? Ingredients { get; set; }

        /// <summary>
        /// The output item for this crafting recipe.
        /// </summary>
        public ItemWithAmount? OutputItem { get; set; }

        /// <summary>
        /// Is this crafting recipe a big craftable or not?
        /// </summary>
        public bool IsBigCraftable { get; set; }

        /// <summary>
        /// The unlock conditions for this crafting recipe, such as Farming 8.
        /// </summary>
        public List<string>? UnlockConditions { get; set; }

        /// <summary>
        /// The alternative display name for this crafting recipe instead of the output item.
        /// </summary>
        public string? DisplayName { get; set; }

        /// <summary>
        /// Outputs this helper's contents into a format that can be understood by Stardew Valley.
        /// </summary>
        /// <returns></returns>
        public virtual string toCraftingRecipeFormat()
        {
            StringBuilder craftingRecipeStringBuilder= new StringBuilder();
            for (int i = 0; i < this.Ingredients.Count; i++)
            {
                craftingRecipeStringBuilder.Append(this.Ingredients[i].toRecipeFormat());
                if (i != this.Ingredients.Count - 1)
                {
                    craftingRecipeStringBuilder.Append(" ");
                }
            }
            
            craftingRecipeStringBuilder.Append("/");
            //Unused.
            craftingRecipeStringBuilder.Append("Home/");
            craftingRecipeStringBuilder.Append(this.OutputItem.toRecipeFormat(false));
            craftingRecipeStringBuilder.Append("/");
            craftingRecipeStringBuilder.Append(this.IsBigCraftable.ToString());
            craftingRecipeStringBuilder.Append("/");
            if (this.UnlockConditions!=null && this.UnlockConditions.Count > 0)
            {
                foreach (string unlockCondition in this.UnlockConditions)
                {
                    craftingRecipeStringBuilder.Append(unlockCondition);
                }
            }
            else
            {
                craftingRecipeStringBuilder.Append("null");
            }
            craftingRecipeStringBuilder.Append("/");

            if (!string.IsNullOrEmpty(this.DisplayName))
            {
                craftingRecipeStringBuilder.Append(this.DisplayName);
            }
            return craftingRecipeStringBuilder.ToString();
        }
    }
}
