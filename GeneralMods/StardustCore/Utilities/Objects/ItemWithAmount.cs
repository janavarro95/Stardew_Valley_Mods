using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.StardustCore.Utilities.Objects
{
    /// <summary>
    /// Helper class for keeping track of an item id with an amount.
    /// </summary>
    public class ItemWithAmount
    {
        /// <summary>
        /// The fully qualified id of the item to be used as an ingredient.
        /// </summary>
        public string? Id;
        /// <summary>
        /// The amount of the item to require.
        /// </summary>
        public int Amount;

        public virtual string toRecipeFormat(bool IncludeAmountIfOne = true)
        {
            if(!IncludeAmountIfOne && this.Amount == 1)
            {
                return this.Id;
            }
            return string.Format("{0} {1}", this.Id,this.Amount);
        }
    }
}
