using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace Omegasis.HappyBirthday.Framework.Utilities
{
    public static class RenderUtilities
    {

        private static Texture2D FarmerTexture = null;

        /// <summary>Raised after drawing the HUD (item toolbar, clock, etc) to the sprite batch, but before it's rendered to the screen.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        public static void OnRenderedHud(object sender, RenderedHudEventArgs e)
        {
            
            if (Game1.activeClickableMenu == null || HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData?.BirthdaySeason?.ToLower() != Game1.currentSeason.ToLower())
                return;

            if (Game1.activeClickableMenu is Billboard billboard)
            {
                if (MenuUtilities.IsDailyQuestBoard || billboard.calendarDays == null)
                    return;

                string hoverText = "";
                List<string> texts = new List<string>();

                foreach (var clicky in billboard.calendarDays)
                {
                    if (clicky.containsPoint(Game1.getMouseX(), Game1.getMouseY()))
                    {
                        if (!string.IsNullOrEmpty(clicky.hoverText))
                            texts.Add(clicky.hoverText); //catches npc birthday names.
                        else if (!string.IsNullOrEmpty(clicky.label))
                            texts.Add(clicky.label); //catches festival dates.
                    }
                }

                for (int i = 0; i < texts.Count; i++)
                {
                    hoverText += texts[i]; //Append text.
                    if (i != texts.Count - 1)
                        hoverText += Environment.NewLine; //Append new line.
                }

                if (!string.IsNullOrEmpty(hoverText))
                {
                    var oldText = HappyBirthdayModCore.Instance.Helper.Reflection.GetField<string>(Game1.activeClickableMenu, "hoverText");
                    oldText.SetValue(hoverText);
                }
            }
            
        }

        /// <summary>When a menu is open (<see cref="Game1.activeClickableMenu"/> isn't null), raised after that menu is drawn to the sprite batch but before it's rendered to the screen.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        public static void OnRenderedActiveMenu(object sender, RenderedActiveMenuEventArgs e)
        {
            
            if (Game1.activeClickableMenu == null || MenuUtilities.IsDailyQuestBoard)
                return;

            //Don't do anything if birthday has not been chosen yet.
            if (HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData == null)
                return;

            if (Game1.activeClickableMenu is Billboard)
            {
               
                if (!string.IsNullOrEmpty(HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData.BirthdaySeason))
                {
                    if (HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData.BirthdaySeason.ToLower() == Game1.currentSeason.ToLower())
                    {
                        DrawPlayerPortraitOnCalendarDay(HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData.BirthdayDay, Game1.player,e.SpriteBatch);
                    }
                }

                foreach (var pair in HappyBirthdayModCore.Instance.birthdayManager.othersBirthdays)
                {
                    DrawPlayerPortraitOnCalendarDay(HappyBirthdayModCore.Instance.birthdayManager.playerBirthdayData.BirthdayDay, Game1.player, e.SpriteBatch);
                }
                (Game1.activeClickableMenu).drawMouse(e.SpriteBatch);

            }
            
        }

        public static void DrawPlayerPortraitOnCalendarDay(int day, Farmer who, SpriteBatch spriteBatch)
        {
            Vector2 drawPosition = new Vector2(Game1.activeClickableMenu.xPositionOnScreen + 196 + (day - 1) % 7 * 32 * 4, Game1.activeClickableMenu.yPositionOnScreen + 222 + (day - 1) / 7 * 32 * 4);

            Game1.player.FarmerRenderer.drawMiniPortrat(spriteBatch, drawPosition, 0.5f, 4f, 2, Game1.player);
            Game1.player.FarmerRenderer.drawHairAndAccesories(spriteBatch, 2, Game1.player, drawPosition, Vector2.Zero, 1f, 0, 0, Color.White, 0.5f);

            //Draw arms.
            if (FarmerTexture == null || FarmerTexture.IsDisposed)
            {
                FarmerTexture = HappyBirthdayModCore.Instance.Helper.Reflection.GetField<Texture2D>(who.FarmerRenderer, "baseTexture").GetValue();
            }
            Game1.spriteBatch.Draw(FarmerTexture, drawPosition + Game1.player.armOffset + new Vector2(0, 4), new Rectangle(96, 64, 16, 32), Color.White, 0, Vector2.Zero, 4f, SpriteEffects.None, 0.5f);

            (Game1.activeClickableMenu as Billboard).drawMouse(spriteBatch);

            string hoverText = HappyBirthdayModCore.Instance.Helper.Reflection.GetField<string>((Game1.activeClickableMenu as Billboard), "hoverText", true).GetValue();
            if (hoverText.Length > 0)
            {
                IClickableMenu.drawHoverText(spriteBatch, hoverText, Game1.dialogueFont, 0, 0, -1, (string)null, -1, (string[])null, (Item)null, 0, null, -1, -1, -1, 1f, (CraftingRecipe)null);
            }
        }

    }
}
