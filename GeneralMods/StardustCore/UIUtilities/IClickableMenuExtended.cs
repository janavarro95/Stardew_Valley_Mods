using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace StardustCore.UIUtilities
{
    public class IClickableMenuExtended : StardewValley.Menus.IClickableMenu
    {
        public List<SpriteFonts.Components.TexturedString> texturedStrings;
        public List<MenuComponents.Button> buttons;
        public Color dialogueBoxBackgroundColor;
        public List<Texture2DExtended> menuTextures;

        public bool showRightCloseButton;

        public IClickableMenuExtended() { }

        public IClickableMenuExtended(int x, int y, int width, int height, bool showCloseButton)
            : base(x, y, width, height, showCloseButton)
        {
            this.showRightCloseButton = showCloseButton;
        }


        public virtual IClickableMenuExtended clone()
        {
            return new IClickableMenuExtended(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, this.showRightCloseButton);
        }

        /// <summary>Draws a dialogue box background with the menu's position and dimentions as the paramaters for size and position.</summary>
        public virtual void drawDialogueBoxBackground()
        {
            this.drawDialogueBoxBackground(this.dialogueBoxBackgroundColor);
        }

        /// <summary>Draws a dialogue box background.</summary>
        public virtual void drawDialogueBoxBackground(Color color)
        {
            this.drawDialogueBoxBackground(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, color);
        }

        /// <summary>Draws a dialogue box background.</summary>
        public virtual void drawDialogueBoxBackground(int xPosition, int yPosition, int width, int height)
        {
            if (this.dialogueBoxBackgroundColor == null) this.dialogueBoxBackgroundColor = Color.White;
            this.drawDialogueBoxBackground(xPosition, yPosition, width, height, false, true, this.dialogueBoxBackgroundColor);
        }

        /// <summary>Draws a dialogue box background.</summary>
        public virtual void drawDialogueBoxBackground(int xPosition, int yPosition, int width, int height, Color color)
        {
            this.drawDialogueBoxBackground(xPosition, yPosition, width, height, false, true, color);
        }

        public virtual void drawOnlyDialogueBoxBackground(int x, int y, int width, int height, Color color, float depth)
        {
            width = Math.Min(Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Width, width);

            Rectangle rectangle1 = new Rectangle(Game1.tileSize, Game1.tileSize * 2, Game1.tileSize, Game1.tileSize);

            depth += 0.001f;
            Game1.spriteBatch.Draw(Game1.menuTexture, new Rectangle(x + Game1.tileSize / 2, y + Game1.tileSize / 2, width, height), rectangle1, color, 0f, Vector2.Zero, SpriteEffects.None, depth);
            rectangle1.Y = 0;
            rectangle1.X = 0;
            depth += 0.001f;

            //Draw the corners
            Game1.spriteBatch.Draw(Game1.menuTexture, new Rectangle(x, y, rectangle1.Width, rectangle1.Height), rectangle1, color, 0f, Vector2.Zero, SpriteEffects.None, depth);
            rectangle1.X = Game1.tileSize * 3;
            depth += 0.001f;
            Game1.spriteBatch.Draw(Game1.menuTexture, new Rectangle((x + width), (y), rectangle1.Width, rectangle1.Height), rectangle1, color, 0f, Vector2.Zero, SpriteEffects.None, depth);
            rectangle1.Y = Game1.tileSize * 3;
            depth += 0.001f;
            Game1.spriteBatch.Draw(Game1.menuTexture, new Rectangle((x + width), (y + height), rectangle1.Width, rectangle1.Height), rectangle1, color, 0f, Vector2.Zero, SpriteEffects.None, depth);
            rectangle1.X = 0;
            depth += 0.001f;
            Game1.spriteBatch.Draw(Game1.menuTexture, new Rectangle((x), (y + height), rectangle1.Width, rectangle1.Height), rectangle1, color, 0f, Vector2.Zero, SpriteEffects.None, depth);


            rectangle1.X = Game1.tileSize * 2;
            rectangle1.Y = 0;
            depth += 0.001f;
            //top
            Game1.spriteBatch.Draw(Game1.menuTexture, new Rectangle(x + Game1.tileSize, y, width - Game1.tileSize, Game1.tileSize), rectangle1, color, 0f, Vector2.Zero, SpriteEffects.None, depth);
            rectangle1.Y = 3 * Game1.tileSize;
            depth += 0.001f;
            //bottom??
            Game1.spriteBatch.Draw(Game1.menuTexture, new Rectangle(Game1.tileSize + x, y + height, width - Game1.tileSize, Game1.tileSize), rectangle1, color, 0f, Vector2.Zero, SpriteEffects.None, depth);
            rectangle1.Y = Game1.tileSize * 2;
            rectangle1.X = 0;
            depth += 0.001f;
            //left
            Game1.spriteBatch.Draw(Game1.menuTexture, new Rectangle(x, y + Game1.tileSize, Game1.tileSize, height - Game1.tileSize), rectangle1, color, 0f, Vector2.Zero, SpriteEffects.None, depth);
            rectangle1.X = 3 * Game1.tileSize;
            depth += 0.001f;
            //right
            Game1.spriteBatch.Draw(Game1.menuTexture, new Rectangle(x + width, y + Game1.tileSize, Game1.tileSize, height - Game1.tileSize), rectangle1, color, 0f, Vector2.Zero, SpriteEffects.None, depth);

        }

        /// <summary>Draws the dialogue box background. Takes in a color.</summary>
        public virtual void drawDialogueBoxBackground(int x, int y, int width, int height, bool speaker, bool drawOnlyBox, Color color, string message = null, bool objectDialogueWithPortrait = false)
        {
            if (!drawOnlyBox)
                return;
            int height1 = Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Height;
            int width1 = Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Width;
            int dialogueX = 0;
            int num1 = y > Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Y ? 0 : Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Y;
            int num2 = 0;
            width = Math.Min(Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Width, width);
            if (!Game1.isQuestion && Game1.currentSpeaker == null && (Game1.currentObjectDialogue.Count > 0 && !drawOnlyBox))
            {
                width = (int)Game1.dialogueFont.MeasureString(Game1.currentObjectDialogue.Peek()).X + Game1.tileSize * 2;
                height = (int)Game1.dialogueFont.MeasureString(Game1.currentObjectDialogue.Peek()).Y + Game1.tileSize;
                x = width1 / 2 - width / 2;
                num2 = height > Game1.tileSize * 4 ? -(height - Game1.tileSize * 4) : 0;
            }
            Rectangle rectangle1 = new Rectangle(0, 0, Game1.tileSize, Game1.tileSize);
            int addedTileHeightForQuestions = -1;
            if (Game1.questionChoices.Count >= 3)
                addedTileHeightForQuestions = Game1.questionChoices.Count - 3;
            if (!drawOnlyBox && Game1.currentObjectDialogue.Count > 0)
            {
                if ((double)Game1.dialogueFont.MeasureString(Game1.currentObjectDialogue.Peek()).Y >= (double)(height - Game1.tileSize * 2))
                {
                    addedTileHeightForQuestions -= (int)(((double)(height - Game1.tileSize * 2) - (double)Game1.dialogueFont.MeasureString(Game1.currentObjectDialogue.Peek()).Y) / (double)Game1.tileSize) - 1;
                }
                else
                {
                    height += (int)Game1.dialogueFont.MeasureString(Game1.currentObjectDialogue.Peek()).Y / 2;
                    num2 -= (int)Game1.dialogueFont.MeasureString(Game1.currentObjectDialogue.Peek()).Y / 2;
                    if ((int)Game1.dialogueFont.MeasureString(Game1.currentObjectDialogue.Peek()).Y / 2 > Game1.tileSize)
                        addedTileHeightForQuestions = 0;
                }
            }
            if (Game1.currentSpeaker != null && Game1.isQuestion && Game1.currentSpeaker.CurrentDialogue.Peek().getCurrentDialogue().Substring(0, Game1.currentDialogueCharacterIndex).Contains(Environment.NewLine))
                ++addedTileHeightForQuestions;
            rectangle1.Width = Game1.tileSize;
            rectangle1.Height = Game1.tileSize;
            rectangle1.X = Game1.tileSize;
            rectangle1.Y = Game1.tileSize * 2;
            Game1.spriteBatch.Draw(Game1.menuTexture, new Rectangle(28 + x + dialogueX, 28 + y - Game1.tileSize * addedTileHeightForQuestions + num1 + num2, width - Game1.tileSize, height - Game1.tileSize + addedTileHeightForQuestions * Game1.tileSize), rectangle1, color);
            rectangle1.Y = 0;
            rectangle1.X = 0;
            Game1.spriteBatch.Draw(Game1.menuTexture, new Vector2((float)(x + dialogueX), (float)(y - Game1.tileSize * addedTileHeightForQuestions + num1 + num2)), rectangle1, color);
            rectangle1.X = Game1.tileSize * 3;
            Game1.spriteBatch.Draw(Game1.menuTexture, new Vector2((float)(x + width + dialogueX - Game1.tileSize), (float)(y - Game1.tileSize * addedTileHeightForQuestions + num1 + num2)), rectangle1, color);
            rectangle1.Y = Game1.tileSize * 3;
            Game1.spriteBatch.Draw(Game1.menuTexture, new Vector2((float)(x + width + dialogueX - Game1.tileSize), (float)(y + height + num1 - Game1.tileSize + num2)), rectangle1, color);
            rectangle1.X = 0;
            Game1.spriteBatch.Draw(Game1.menuTexture, new Vector2((float)(x + dialogueX), (float)(y + height + num1 - Game1.tileSize + num2)), rectangle1, color);
            rectangle1.X = Game1.tileSize * 2;
            rectangle1.Y = 0;
            Game1.spriteBatch.Draw(Game1.menuTexture, new Rectangle(Game1.tileSize + x + dialogueX, y - Game1.tileSize * addedTileHeightForQuestions + num1 + num2, width - Game1.tileSize * 2, Game1.tileSize), rectangle1, color);
            rectangle1.Y = 3 * Game1.tileSize;
            Game1.spriteBatch.Draw(Game1.menuTexture, new Rectangle(Game1.tileSize + x + dialogueX, y + height + num1 - Game1.tileSize + num2, width - Game1.tileSize * 2, Game1.tileSize), rectangle1, color);
            rectangle1.Y = Game1.tileSize * 2;
            rectangle1.X = 0;
            Game1.spriteBatch.Draw(Game1.menuTexture, new Rectangle(x + dialogueX, y - Game1.tileSize * addedTileHeightForQuestions + num1 + Game1.tileSize + num2, Game1.tileSize, height - Game1.tileSize * 2 + addedTileHeightForQuestions * Game1.tileSize), rectangle1, color);
            rectangle1.X = 3 * Game1.tileSize;
            Game1.spriteBatch.Draw(Game1.menuTexture, new Rectangle(x + width + dialogueX - Game1.tileSize, y - Game1.tileSize * addedTileHeightForQuestions + num1 + Game1.tileSize + num2, Game1.tileSize, height - Game1.tileSize * 2 + addedTileHeightForQuestions * Game1.tileSize), rectangle1, color);
            if (objectDialogueWithPortrait && Game1.objectDialoguePortraitPerson != null || speaker && Game1.currentSpeaker != null && (Game1.currentSpeaker.CurrentDialogue.Count > 0 && Game1.currentSpeaker.CurrentDialogue.Peek().showPortrait))
            {
                Rectangle rectangle2 = new Rectangle(0, 0, 64, 64);
                NPC npc = objectDialogueWithPortrait ? Game1.objectDialoguePortraitPerson : Game1.currentSpeaker;
                string s = objectDialogueWithPortrait ? (Game1.objectDialoguePortraitPerson.Name.Equals(Game1.player.spouse) ? "$l" : "$neutral") : npc.CurrentDialogue.Peek().CurrentEmotion;
                switch (s)
                {
                    case "$a":
                        rectangle2 = new Rectangle(64, 128, 64, 64);
                        break;

                    case "$u":
                        rectangle2 = new Rectangle(64, 64, 64, 64);
                        break;

                    case "$s":
                        rectangle2 = new Rectangle(0, 64, 64, 64);
                        break;

                    case "$h":
                        rectangle2 = new Rectangle(64, 0, 64, 64);
                        break;

                    case "$l":
                        rectangle2 = new Rectangle(0, 128, 64, 64);
                        break;

                    default:
                        rectangle2 = (s == "$k" || s == "$neutral" ? new Rectangle(0, 0, 64, 64) : Game1.getSourceRectForStandardTileSheet(npc.Portrait, Convert.ToInt32(npc.CurrentDialogue.Peek().CurrentEmotion.Substring(1)), -1, -1));
                        break;
                }

                Game1.spriteBatch.End();
                Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, (DepthStencilState)null, (RasterizerState)null);
                if (npc.Portrait != null)
                {
                    Game1.spriteBatch.Draw(Game1.mouseCursors, new Vector2((float)(dialogueX + x + Game1.tileSize * 12), (float)(height1 - 5 * Game1.tileSize - Game1.tileSize * addedTileHeightForQuestions - 256 + num1 + Game1.tileSize / 4 - 60 + num2)), new Rectangle(333, 305, 80, 87), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 0.98f);
                    Game1.spriteBatch.Draw(npc.Portrait, new Vector2((float)(dialogueX + x + Game1.tileSize * 12 + 32), (float)(height1 - 5 * Game1.tileSize - Game1.tileSize * addedTileHeightForQuestions - 256 + num1 + Game1.tileSize / 4 - 60 + num2)), rectangle2, Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 0.99f);
                }
                Game1.spriteBatch.End();
                Game1.spriteBatch.Begin();
                if (Game1.isQuestion)
                    Game1.spriteBatch.DrawString(Game1.dialogueFont, npc.displayName, new Vector2((float)(Game1.tileSize * 14 + Game1.tileSize / 2) - Game1.dialogueFont.MeasureString(npc.displayName).X / 2f + (float)dialogueX + (float)x, (float)(height1 - 5 * Game1.tileSize - Game1.tileSize * addedTileHeightForQuestions) - Game1.dialogueFont.MeasureString(npc.displayName).Y + (float)num1 + (float)(Game1.tileSize / 3) + (float)num2) + new Vector2(2f, 2f), new Color(150, 150, 150));
                Game1.spriteBatch.DrawString(Game1.dialogueFont, npc.Name.Equals("DwarfKing") ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3754") : (npc.Name.Equals("Lewis") ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3756") : npc.displayName), new Vector2((float)(dialogueX + x + Game1.tileSize * 14 + Game1.tileSize / 2) - Game1.dialogueFont.MeasureString(npc.Name.Equals("Lewis") ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3756") : npc.displayName).X / 2f, (float)(height1 - 5 * Game1.tileSize - Game1.tileSize * addedTileHeightForQuestions) - Game1.dialogueFont.MeasureString(npc.Name.Equals("Lewis") ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3756") : npc.displayName).Y + (float)num1 + (float)(Game1.tileSize / 3) + (float)(Game1.tileSize / 8) + (float)num2), Game1.textColor);
            }
            if (drawOnlyBox || Game1.nameSelectUp && (!Game1.messagePause || Game1.currentObjectDialogue == null))
                return;
            string text = "";
            if (Game1.currentSpeaker != null && Game1.currentSpeaker.CurrentDialogue.Count > 0)
            {
                if (Game1.currentSpeaker.CurrentDialogue.Peek() == null || Game1.currentSpeaker.CurrentDialogue.Peek().getCurrentDialogue().Length < Game1.currentDialogueCharacterIndex - 1)
                {
                    Game1.dialogueUp = false;
                    Game1.currentDialogueCharacterIndex = 0;
                    Game1.playSound("dialogueCharacterClose");
                    Game1.player.forceCanMove();
                    return;
                }
                text = Game1.currentSpeaker.CurrentDialogue.Peek().getCurrentDialogue().Substring(0, Game1.currentDialogueCharacterIndex);
            }
            else if (message != null)
                text = message;
            else if (Game1.currentObjectDialogue.Count > 0)
                text = Game1.currentObjectDialogue.Peek().Length <= 1 ? "" : Game1.currentObjectDialogue.Peek().Substring(0, Game1.currentDialogueCharacterIndex);
            Vector2 position = (double)Game1.dialogueFont.MeasureString(text).X <= (double)(width1 - Game1.tileSize * 4 - dialogueX) ? (Game1.currentSpeaker == null || Game1.currentSpeaker.CurrentDialogue.Count <= 0 ? (message == null ? (!Game1.isQuestion ? new Vector2((float)(width1 / 2) - Game1.dialogueFont.MeasureString(Game1.currentObjectDialogue.Count == 0 ? "" : Game1.currentObjectDialogue.Peek()).X / 2f + (float)dialogueX, (float)(y + Game1.pixelZoom + num2)) : new Vector2((float)(width1 / 2) - Game1.dialogueFont.MeasureString(Game1.currentObjectDialogue.Count == 0 ? "" : Game1.currentObjectDialogue.Peek()).X / 2f + (float)dialogueX, (float)(height1 - Game1.tileSize * addedTileHeightForQuestions - 4 * Game1.tileSize - (Game1.tileSize / 4 + (Game1.questionChoices.Count - 2) * Game1.tileSize) + num1 + num2))) : new Vector2((float)(width1 / 2) - Game1.dialogueFont.MeasureString(text).X / 2f + (float)dialogueX, (float)(y + Game1.tileSize * 3 / 2 + Game1.pixelZoom))) : new Vector2((float)(width1 / 2) - Game1.dialogueFont.MeasureString(Game1.currentSpeaker.CurrentDialogue.Peek().getCurrentDialogue()).X / 2f + (float)dialogueX, (float)(height1 - Game1.tileSize * addedTileHeightForQuestions - 4 * Game1.tileSize - Game1.tileSize / 4 + num1 + num2))) : new Vector2((float)(Game1.tileSize * 2 + dialogueX), (float)(height1 - Game1.tileSize * addedTileHeightForQuestions - 4 * Game1.tileSize - Game1.tileSize / 4 + num1 + num2));
            if (!drawOnlyBox)
            {
                Game1.spriteBatch.DrawString(Game1.dialogueFont, text, position + new Vector2(3f, 0.0f), Game1.textShadowColor);
                Game1.spriteBatch.DrawString(Game1.dialogueFont, text, position + new Vector2(3f, 3f), Game1.textShadowColor);
                Game1.spriteBatch.DrawString(Game1.dialogueFont, text, position + new Vector2(0.0f, 3f), Game1.textShadowColor);
                Game1.spriteBatch.DrawString(Game1.dialogueFont, text, position, Game1.textColor);
            }
            if ((double)Game1.dialogueFont.MeasureString(text).Y <= (double)Game1.tileSize)
                num1 += Game1.tileSize;
            if (Game1.isQuestion && !Game1.dialogueTyping)
            {
                for (int index = 0; index < Game1.questionChoices.Count; ++index)
                {
                    if (Game1.currentQuestionChoice == index)
                    {
                        position.X = (float)(Game1.tileSize * 5 / 4 + dialogueX + x);
                        position.Y = (float)(height1 - (5 + addedTileHeightForQuestions + 1) * Game1.tileSize) + (text.Trim().Length > 0 ? Game1.dialogueFont.MeasureString(text).Y : 0.0f) + (float)(Game1.tileSize * 2) + (float)((Game1.tileSize / 2 + Game1.tileSize / 4) * index) - (float)(Game1.tileSize / 4 + (Game1.questionChoices.Count - 2) * Game1.tileSize) + (float)num1 + (float)num2;
                        Game1.spriteBatch.End();
                        Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, (DepthStencilState)null, (RasterizerState)null);
                        Game1.spriteBatch.Draw(Game1.objectSpriteSheet, position + new Vector2((float)Math.Cos((double)Game1.currentGameTime.TotalGameTime.Milliseconds * Math.PI / 512.0) * 3f, 0.0f), GameLocation.getSourceRectForObject(26), Color.White, 0.0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
                        Game1.spriteBatch.End();
                        Game1.spriteBatch.Begin();
                        position.X = (float)(Game1.tileSize * 5 / 2 + dialogueX + x);
                        position.Y = (float)(height1 - (5 + addedTileHeightForQuestions + 1) * Game1.tileSize) + (text.Trim().Length > 1 ? Game1.dialogueFont.MeasureString(text).Y : 0.0f) + (float)(Game1.tileSize * 3 / 2 + Game1.tileSize / 2) - (float)((Game1.questionChoices.Count - 2) * Game1.tileSize) + (float)((Game1.tileSize / 2 + Game1.tileSize / 4) * index) + (float)num1 + (float)num2;
                        Game1.spriteBatch.DrawString(Game1.dialogueFont, Game1.questionChoices[index].responseText, position, Game1.textColor);
                    }
                    else
                    {
                        position.X = (float)(Game1.tileSize * 2 + dialogueX + x);
                        position.Y = (float)(height1 - (5 + addedTileHeightForQuestions + 1) * Game1.tileSize) + (text.Trim().Length > 1 ? Game1.dialogueFont.MeasureString(text).Y : 0.0f) + (float)(Game1.tileSize * 3 / 2 + Game1.tileSize / 2) - (float)((Game1.questionChoices.Count - 2) * Game1.tileSize) + (float)((Game1.tileSize / 2 + Game1.tileSize / 4) * index) + (float)num1 + (float)num2;
                        Game1.spriteBatch.DrawString(Game1.dialogueFont, Game1.questionChoices[index].responseText, position, Game1.unselectedOptionColor);
                    }
                }
            }
            else if (Game1.numberOfSelectedItems != -1 && !Game1.dialogueTyping)
                this.drawItemSelectDialogue(x, y, dialogueX, num1 + num2, height1, addedTileHeightForQuestions, text);
            if (drawOnlyBox || Game1.dialogueTyping || message != null)
                return;
            Game1.spriteBatch.Draw(Game1.mouseCursors, new Vector2((float)(x + dialogueX + width - Game1.tileSize * 3 / 2), (float)(y + height + num1 + num2 - Game1.tileSize * 3 / 2) - Game1.dialogueButtonScale), Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, Game1.dialogueButtonShrinking || (double)Game1.dialogueButtonScale >= (double)(Game1.tileSize / 8) ? 2 : 3, -1, -1), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9999999f);
        }
        
        /// <summary>Draws the dialogue box background. Takes in a color.</summary>
        /// <param name="texture">A custom menu texture to use.</param>
        public virtual void drawDialogueBoxBackground(Texture2DExtended texture, int x, int y, int width, int height, bool speaker, bool drawOnlyBox, Color color, string message = null, bool objectDialogueWithPortrait = false)
        {
            if (!drawOnlyBox)
                return;
            int height1 = Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Height;
            int width1 = Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Width;
            int dialogueX = 0;
            int num1 = y > Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Y ? 0 : Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Y;
            int num2 = 0;
            width = Math.Min(Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Width, width);
            if (!Game1.isQuestion && Game1.currentSpeaker == null && (Game1.currentObjectDialogue.Count > 0 && !drawOnlyBox))
            {
                width = (int)Game1.dialogueFont.MeasureString(Game1.currentObjectDialogue.Peek()).X + Game1.tileSize * 2;
                height = (int)Game1.dialogueFont.MeasureString(Game1.currentObjectDialogue.Peek()).Y + Game1.tileSize;
                x = width1 / 2 - width / 2;
                num2 = height > Game1.tileSize * 4 ? -(height - Game1.tileSize * 4) : 0;
            }
            Rectangle rectangle1 = new Rectangle(0, 0, Game1.tileSize, Game1.tileSize);
            int addedTileHeightForQuestions = -1;
            if (Game1.questionChoices.Count >= 3)
                addedTileHeightForQuestions = Game1.questionChoices.Count - 3;
            if (!drawOnlyBox && Game1.currentObjectDialogue.Count > 0)
            {
                if ((double)Game1.dialogueFont.MeasureString(Game1.currentObjectDialogue.Peek()).Y >= (double)(height - Game1.tileSize * 2))
                {
                    addedTileHeightForQuestions -= (int)(((double)(height - Game1.tileSize * 2) - (double)Game1.dialogueFont.MeasureString(Game1.currentObjectDialogue.Peek()).Y) / (double)Game1.tileSize) - 1;
                }
                else
                {
                    height += (int)Game1.dialogueFont.MeasureString(Game1.currentObjectDialogue.Peek()).Y / 2;
                    num2 -= (int)Game1.dialogueFont.MeasureString(Game1.currentObjectDialogue.Peek()).Y / 2;
                    if ((int)Game1.dialogueFont.MeasureString(Game1.currentObjectDialogue.Peek()).Y / 2 > Game1.tileSize)
                        addedTileHeightForQuestions = 0;
                }
            }
            if (Game1.currentSpeaker != null && Game1.isQuestion && Game1.currentSpeaker.CurrentDialogue.Peek().getCurrentDialogue().Substring(0, Game1.currentDialogueCharacterIndex).Contains(Environment.NewLine))
                ++addedTileHeightForQuestions;
            rectangle1.Width = Game1.tileSize;
            rectangle1.Height = Game1.tileSize;
            rectangle1.X = Game1.tileSize;
            rectangle1.Y = Game1.tileSize * 2;
            Game1.spriteBatch.Draw(texture.getTexture(), new Rectangle(28 + x + dialogueX, 28 + y - Game1.tileSize * addedTileHeightForQuestions + num1 + num2, width - Game1.tileSize, height - Game1.tileSize + addedTileHeightForQuestions * Game1.tileSize), rectangle1, color);
            rectangle1.Y = 0;
            rectangle1.X = 0;
            Game1.spriteBatch.Draw(texture.getTexture(), new Vector2((float)(x + dialogueX), (float)(y - Game1.tileSize * addedTileHeightForQuestions + num1 + num2)), rectangle1, color);
            rectangle1.X = Game1.tileSize * 3;
            Game1.spriteBatch.Draw(texture.getTexture(), new Vector2((float)(x + width + dialogueX - Game1.tileSize), (float)(y - Game1.tileSize * addedTileHeightForQuestions + num1 + num2)), rectangle1, color);
            rectangle1.Y = Game1.tileSize * 3;
            Game1.spriteBatch.Draw(texture.getTexture(), new Vector2((float)(x + width + dialogueX - Game1.tileSize), (float)(y + height + num1 - Game1.tileSize + num2)), rectangle1, color);
            rectangle1.X = 0;
            Game1.spriteBatch.Draw(texture.getTexture(), new Vector2((float)(x + dialogueX), (float)(y + height + num1 - Game1.tileSize + num2)), rectangle1, color);
            rectangle1.X = Game1.tileSize * 2;
            rectangle1.Y = 0;
            Game1.spriteBatch.Draw(texture.getTexture(), new Rectangle(Game1.tileSize + x + dialogueX, y - Game1.tileSize * addedTileHeightForQuestions + num1 + num2, width - Game1.tileSize * 2, Game1.tileSize), rectangle1, color);
            rectangle1.Y = 3 * Game1.tileSize;
            Game1.spriteBatch.Draw(texture.getTexture(), new Rectangle(Game1.tileSize + x + dialogueX, y + height + num1 - Game1.tileSize + num2, width - Game1.tileSize * 2, Game1.tileSize), rectangle1, color);
            rectangle1.Y = Game1.tileSize * 2;
            rectangle1.X = 0;
            Game1.spriteBatch.Draw(texture.getTexture(), new Rectangle(x + dialogueX, y - Game1.tileSize * addedTileHeightForQuestions + num1 + Game1.tileSize + num2, Game1.tileSize, height - Game1.tileSize * 2 + addedTileHeightForQuestions * Game1.tileSize), rectangle1, color);
            rectangle1.X = 3 * Game1.tileSize;
            Game1.spriteBatch.Draw(texture.getTexture(), new Rectangle(x + width + dialogueX - Game1.tileSize, y - Game1.tileSize * addedTileHeightForQuestions + num1 + Game1.tileSize + num2, Game1.tileSize, height - Game1.tileSize * 2 + addedTileHeightForQuestions * Game1.tileSize), rectangle1, color);
            if (objectDialogueWithPortrait && Game1.objectDialoguePortraitPerson != null || speaker && Game1.currentSpeaker != null && (Game1.currentSpeaker.CurrentDialogue.Count > 0 && Game1.currentSpeaker.CurrentDialogue.Peek().showPortrait))
            {
                Rectangle rectangle2 = new Rectangle(0, 0, 64, 64);
                NPC npc = objectDialogueWithPortrait ? Game1.objectDialoguePortraitPerson : Game1.currentSpeaker;
                string s = objectDialogueWithPortrait ? (Game1.objectDialoguePortraitPerson.Name.Equals(Game1.player.spouse) ? "$l" : "$neutral") : npc.CurrentDialogue.Peek().CurrentEmotion;
                switch (s)
                {
                    case "$a":
                        rectangle2 = new Rectangle(64, 128, 64, 64);
                        break;

                    case "$u":
                        rectangle2 = new Rectangle(64, 64, 64, 64);
                        break;

                    case "$s":
                        rectangle2 = new Rectangle(0, 64, 64, 64);
                        break;

                    case "$h":
                        rectangle2 = new Rectangle(64, 0, 64, 64);
                        break;

                    case "$l":
                        rectangle2 = new Rectangle(0, 128, 64, 64);
                        break;

                    default:
                        rectangle2 = (s == "$k" || s == "$neutral" ? new Rectangle(0, 0, 64, 64) : Game1.getSourceRectForStandardTileSheet(npc.Portrait, Convert.ToInt32(npc.CurrentDialogue.Peek().CurrentEmotion.Substring(1)), -1, -1));
                        break;
                }

                Game1.spriteBatch.End();
                Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, (DepthStencilState)null, (RasterizerState)null);
                if (npc.Portrait != null)
                {
                    Game1.spriteBatch.Draw(Game1.mouseCursors, new Vector2((float)(dialogueX + x + Game1.tileSize * 12), (float)(height1 - 5 * Game1.tileSize - Game1.tileSize * addedTileHeightForQuestions - 256 + num1 + Game1.tileSize / 4 - 60 + num2)), new Rectangle(333, 305, 80, 87), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 0.98f);
                    Game1.spriteBatch.Draw(npc.Portrait, new Vector2((float)(dialogueX + x + Game1.tileSize * 12 + 32), (float)(height1 - 5 * Game1.tileSize - Game1.tileSize * addedTileHeightForQuestions - 256 + num1 + Game1.tileSize / 4 - 60 + num2)), rectangle2, Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 0.99f);
                }
                Game1.spriteBatch.End();
                Game1.spriteBatch.Begin();
                if (Game1.isQuestion)
                    Game1.spriteBatch.DrawString(Game1.dialogueFont, npc.displayName, new Vector2((float)(Game1.tileSize * 14 + Game1.tileSize / 2) - Game1.dialogueFont.MeasureString(npc.displayName).X / 2f + (float)dialogueX + (float)x, (float)(height1 - 5 * Game1.tileSize - Game1.tileSize * addedTileHeightForQuestions) - Game1.dialogueFont.MeasureString(npc.displayName).Y + (float)num1 + (float)(Game1.tileSize / 3) + (float)num2) + new Vector2(2f, 2f), new Color(150, 150, 150));
                Game1.spriteBatch.DrawString(Game1.dialogueFont, npc.Name.Equals("DwarfKing") ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3754") : (npc.Name.Equals("Lewis") ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3756") : npc.displayName), new Vector2((float)(dialogueX + x + Game1.tileSize * 14 + Game1.tileSize / 2) - Game1.dialogueFont.MeasureString(npc.Name.Equals("Lewis") ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3756") : npc.displayName).X / 2f, (float)(height1 - 5 * Game1.tileSize - Game1.tileSize * addedTileHeightForQuestions) - Game1.dialogueFont.MeasureString(npc.Name.Equals("Lewis") ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3756") : npc.displayName).Y + (float)num1 + (float)(Game1.tileSize / 3) + (float)(Game1.tileSize / 8) + (float)num2), Game1.textColor);
            }
            if (drawOnlyBox || Game1.nameSelectUp && (!Game1.messagePause || Game1.currentObjectDialogue == null))
                return;
            string text = "";
            if (Game1.currentSpeaker != null && Game1.currentSpeaker.CurrentDialogue.Count > 0)
            {
                if (Game1.currentSpeaker.CurrentDialogue.Peek() == null || Game1.currentSpeaker.CurrentDialogue.Peek().getCurrentDialogue().Length < Game1.currentDialogueCharacterIndex - 1)
                {
                    Game1.dialogueUp = false;
                    Game1.currentDialogueCharacterIndex = 0;
                    Game1.playSound("dialogueCharacterClose");
                    Game1.player.forceCanMove();
                    return;
                }
                text = Game1.currentSpeaker.CurrentDialogue.Peek().getCurrentDialogue().Substring(0, Game1.currentDialogueCharacterIndex);
            }
            else if (message != null)
                text = message;
            else if (Game1.currentObjectDialogue.Count > 0)
                text = Game1.currentObjectDialogue.Peek().Length <= 1 ? "" : Game1.currentObjectDialogue.Peek().Substring(0, Game1.currentDialogueCharacterIndex);
            Vector2 position = (double)Game1.dialogueFont.MeasureString(text).X <= (double)(width1 - Game1.tileSize * 4 - dialogueX) ? (Game1.currentSpeaker == null || Game1.currentSpeaker.CurrentDialogue.Count <= 0 ? (message == null ? (!Game1.isQuestion ? new Vector2((float)(width1 / 2) - Game1.dialogueFont.MeasureString(Game1.currentObjectDialogue.Count == 0 ? "" : Game1.currentObjectDialogue.Peek()).X / 2f + (float)dialogueX, (float)(y + Game1.pixelZoom + num2)) : new Vector2((float)(width1 / 2) - Game1.dialogueFont.MeasureString(Game1.currentObjectDialogue.Count == 0 ? "" : Game1.currentObjectDialogue.Peek()).X / 2f + (float)dialogueX, (float)(height1 - Game1.tileSize * addedTileHeightForQuestions - 4 * Game1.tileSize - (Game1.tileSize / 4 + (Game1.questionChoices.Count - 2) * Game1.tileSize) + num1 + num2))) : new Vector2((float)(width1 / 2) - Game1.dialogueFont.MeasureString(text).X / 2f + (float)dialogueX, (float)(y + Game1.tileSize * 3 / 2 + Game1.pixelZoom))) : new Vector2((float)(width1 / 2) - Game1.dialogueFont.MeasureString(Game1.currentSpeaker.CurrentDialogue.Peek().getCurrentDialogue()).X / 2f + (float)dialogueX, (float)(height1 - Game1.tileSize * addedTileHeightForQuestions - 4 * Game1.tileSize - Game1.tileSize / 4 + num1 + num2))) : new Vector2((float)(Game1.tileSize * 2 + dialogueX), (float)(height1 - Game1.tileSize * addedTileHeightForQuestions - 4 * Game1.tileSize - Game1.tileSize / 4 + num1 + num2));
            if (!drawOnlyBox)
            {
                Game1.spriteBatch.DrawString(Game1.dialogueFont, text, position + new Vector2(3f, 0.0f), Game1.textShadowColor);
                Game1.spriteBatch.DrawString(Game1.dialogueFont, text, position + new Vector2(3f, 3f), Game1.textShadowColor);
                Game1.spriteBatch.DrawString(Game1.dialogueFont, text, position + new Vector2(0.0f, 3f), Game1.textShadowColor);
                Game1.spriteBatch.DrawString(Game1.dialogueFont, text, position, Game1.textColor);
            }
            if ((double)Game1.dialogueFont.MeasureString(text).Y <= (double)Game1.tileSize)
                num1 += Game1.tileSize;
            if (Game1.isQuestion && !Game1.dialogueTyping)
            {
                for (int index = 0; index < Game1.questionChoices.Count; ++index)
                {
                    if (Game1.currentQuestionChoice == index)
                    {
                        position.X = (float)(Game1.tileSize * 5 / 4 + dialogueX + x);
                        position.Y = (float)(height1 - (5 + addedTileHeightForQuestions + 1) * Game1.tileSize) + (text.Trim().Length > 0 ? Game1.dialogueFont.MeasureString(text).Y : 0.0f) + (float)(Game1.tileSize * 2) + (float)((Game1.tileSize / 2 + Game1.tileSize / 4) * index) - (float)(Game1.tileSize / 4 + (Game1.questionChoices.Count - 2) * Game1.tileSize) + (float)num1 + (float)num2;
                        Game1.spriteBatch.End();
                        Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, (DepthStencilState)null, (RasterizerState)null);
                        Game1.spriteBatch.Draw(Game1.objectSpriteSheet, position + new Vector2((float)Math.Cos((double)Game1.currentGameTime.TotalGameTime.Milliseconds * Math.PI / 512.0) * 3f, 0.0f), GameLocation.getSourceRectForObject(26), Color.White, 0.0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
                        Game1.spriteBatch.End();
                        Game1.spriteBatch.Begin();
                        position.X = (float)(Game1.tileSize * 5 / 2 + dialogueX + x);
                        position.Y = (float)(height1 - (5 + addedTileHeightForQuestions + 1) * Game1.tileSize) + (text.Trim().Length > 1 ? Game1.dialogueFont.MeasureString(text).Y : 0.0f) + (float)(Game1.tileSize * 3 / 2 + Game1.tileSize / 2) - (float)((Game1.questionChoices.Count - 2) * Game1.tileSize) + (float)((Game1.tileSize / 2 + Game1.tileSize / 4) * index) + (float)num1 + (float)num2;
                        Game1.spriteBatch.DrawString(Game1.dialogueFont, Game1.questionChoices[index].responseText, position, Game1.textColor);
                    }
                    else
                    {
                        position.X = (float)(Game1.tileSize * 2 + dialogueX + x);
                        position.Y = (float)(height1 - (5 + addedTileHeightForQuestions + 1) * Game1.tileSize) + (text.Trim().Length > 1 ? Game1.dialogueFont.MeasureString(text).Y : 0.0f) + (float)(Game1.tileSize * 3 / 2 + Game1.tileSize / 2) - (float)((Game1.questionChoices.Count - 2) * Game1.tileSize) + (float)((Game1.tileSize / 2 + Game1.tileSize / 4) * index) + (float)num1 + (float)num2;
                        Game1.spriteBatch.DrawString(Game1.dialogueFont, Game1.questionChoices[index].responseText, position, Game1.unselectedOptionColor);
                    }
                }
            }
            else if (Game1.numberOfSelectedItems != -1 && !Game1.dialogueTyping)
                this.drawItemSelectDialogue(x, y, dialogueX, num1 + num2, height1, addedTileHeightForQuestions, text);
            if (drawOnlyBox || Game1.dialogueTyping || message != null)
                return;
            Game1.spriteBatch.Draw(Game1.mouseCursors, new Vector2((float)(x + dialogueX + width - Game1.tileSize * 3 / 2), (float)(y + height + num1 + num2 - Game1.tileSize * 3 / 2) - Game1.dialogueButtonScale), Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, Game1.dialogueButtonShrinking || (double)Game1.dialogueButtonScale >= (double)(Game1.tileSize / 8) ? 2 : 3, -1, -1), color, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9999999f);
        }

        /// <summary>Work around drawing function instead of using Game1.drawItemSelectDialogue.</summary>
        public virtual void drawItemSelectDialogue(int x, int y, int dialogueX, int dialogueY, int screenHeight, int addedTileHeightForQuestions, string text)
        {
            string selectedItemsType = Game1.selectedItemsType;
            string text1;
            if (selectedItemsType != "flutePitch" && selectedItemsType != "drumTome")
            {
                if (selectedItemsType == "jukebox")
                    text1 = "@ " + Game1.player.songsHeard.ElementAt(Game1.numberOfSelectedItems) + " >  ";
                else
                    text1 = "@ " + Game1.numberOfSelectedItems + " >  " + Game1.priceOfSelectedItem * Game1.numberOfSelectedItems + "g";
            }
            else
                text1 = "@ " + Game1.numberOfSelectedItems + " >  ";
            if (Game1.currentLocation.Name.Equals("Club"))
                text1 = "@ " + Game1.numberOfSelectedItems + " >  ";
            Game1.spriteBatch.DrawString(Game1.dialogueFont, text1, new Vector2(dialogueX + x + Game1.tileSize, screenHeight - (5 + addedTileHeightForQuestions + 1) * Game1.tileSize + Game1.dialogueFont.MeasureString(text).Y + (Game1.tileSize * 3 / 2 + Game1.tileSize / 8) + dialogueY), Game1.textColor);
        }

        public override void update(GameTime time) { }
    }
}
