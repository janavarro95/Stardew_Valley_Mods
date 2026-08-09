using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using StardewValley;
using StardewValley.GameData.Objects;

namespace Omegasis.HappyBirthday.Framework.Compatibility.ContentPatcher.Tokens
{
    public class GenericNPCGiftToken : ContentPatcherTokenBase
    {
        public string pickedGift = "";
        public int matchingItemCategory = int.MinValue;

        public bool useCategoryMatching = false;


        public GenericNPCGiftToken() : base() {
            

        }

        public GenericNPCGiftToken(int matchingCategory) : base()
        {
            this.matchingItemCategory = matchingCategory;
            this.useCategoryMatching  = true;

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

        public override IEnumerable<string> GetValues(string input)
        {
            this.UpdateContext();
            return new List<string>(){
                this.pickedGift
            };
        }

    }
}
