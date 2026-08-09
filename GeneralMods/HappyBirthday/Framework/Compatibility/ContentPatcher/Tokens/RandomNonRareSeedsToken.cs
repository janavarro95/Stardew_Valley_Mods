using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;
using StardewValley.GameData.Objects;

namespace Omegasis.HappyBirthday.Framework.Compatibility.ContentPatcher.Tokens
{
    public class RandomNonRareSeedsToken : GenericNPCGiftToken 
    {

        public RandomNonRareSeedsToken() : base(StardewValley.Object.SeedsCategory) {

        }

        /// <inheritdoc />
        public override bool UpdateContext()
        {
            List<string> list = new List<string>();

            foreach (string id in Game1.objectData.Keys)
            {
                ObjectData d = Game1.objectData[id];
                if (this.useCategoryMatching)
                {
                    if (d.Category == this.matchingItemCategory)
                    {

                        if (id == "347")
                        {
                            //Sweet Gem Berry
                            continue;
                        }
                        if (id == "499")
                        {
                            //Ancient Seeds
                            continue;
                        }
                        if (d.Name.Contains("Sapling"))
                        {
                            //Don't include saplings.
                            continue;
                        }

                        list.Add(id);
                    }
                }
                else
                {
                    list.Add(id);
                }
            }
            int index = Game1.random.Next(list.Count);
            string chosenId = list[index];
            this.pickedGift = chosenId;
            return true;
        }

    }
}
