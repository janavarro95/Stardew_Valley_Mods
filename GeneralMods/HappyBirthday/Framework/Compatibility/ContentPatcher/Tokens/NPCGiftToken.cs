using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using StardewValley;
using StardewValley.GameData.Objects;

namespace Omegasis.HappyBirthday.Framework.Compatibility.ContentPatcher.Tokens
{
    public class NPCGiftToken : ContentPatcherTokenBase
    {

        public string pickedGift = "";
        public string npcName = "";


        public NPCGiftToken() : base()
        {


        }

        public override bool IsReady()
        {
            if (Context.IsWorldReady)
                return true;

            return false;
        }

        public override bool AllowsInput()
        {
            return true;
        }

        public override bool RequiresInput()
        {
            return true;
        }

        public override bool TryValidateInput(string input, [NotNullWhen(false)] out string error)
        {
            int? result = Game1.player.tryGetFriendshipLevelForNPC(input);
            if (result.HasValue)
            {

                error = "";
                return true;
            }
            foreach (GameLocation gameLocation in Game1.locations)
            {
                if (gameLocation != null)
                {
                    foreach (NPC npc in gameLocation.characters)
                    {
                        if (npc != null)
                        {
                            if (npc.Name == input)
                            {
                                error = "";
                                return true;

                            }
                        }
                    }
                }
            }

            error = string.Format("Warning: No NPC with name {0} could be found", input);
            return true;
        }

        public override bool TryValidateValues(string input, IEnumerable<string> values, [NotNullWhen(false)] out string error)
        {
            error = "";
            return true;
        }

        public override bool CanHaveMultipleValues(string input = null)
        {
            return false;
        }

        public override bool HasBoundedRangeValues(string input, out int min, out int max)
        {
            min = 1;
            max = 1;
            return true;
        }

        public override IEnumerable<string> GetValidInputs()
        {
            return base.GetValidInputs();
        }


        /// <inheritdoc />
        public override bool UpdateContext()
        {
            if (this.IsReady())
            {
                Item item = HappyBirthdayModCore.Instance.giftManager.getNextBirthdayGift(this.npcName);

                this.pickedGift = item.QualifiedItemId + " "+ item.Stack.ToString();
            }
            return true;
        }

        public override IEnumerable<string> GetValues(string input)
        {
            this.npcName = input;
            this.UpdateContext();

            return new List<string>(){
                this.pickedGift
            };
        }
    }
}
