﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Home_Front
{
    static class PopUp
    {
        #region Variables

        //Art Assets
        public static Texture2D[] popUpTxr;
        public static SpriteFont[] popUpFonts;

        //SubQuest for PopUp Box Info
        public static SubQuest sqHolder;

        //Chosen Button
        public static int pickedChoice;

        #endregion

        #region Method
        //Constructor
        static PopUp()
        {
        }

        //Update PopUp Box
        public static void Update()
        {
            //Depends on number of Potential Choices
            switch (sqHolder.Choices.Length)
            {
                case 1:

                    ButtonManager.gameButton[7].Update();

                    if (ButtonManager.gameButton[7].IsClicked())
                    {
                        QuestHandler.RunChoiceOutcomes(sqHolder.Choices[0].ID);
                        pickedChoice = 0;
                        AudioHandler.PlayEffectInstance(4);
                        InPlay.popUpChoice = false;
                    }

                    break;

                case 2:

                    ButtonManager.gameButton[7].Update();
                    ButtonManager.gameButton[8].Update();

                    if (ButtonManager.gameButton[7].IsClicked())
                    {
                        QuestHandler.RunChoiceOutcomes(sqHolder.Choices[0].ID);
                        pickedChoice = 0;
                        AudioHandler.PlayEffectInstance(4);
                        InPlay.popUpChoice = false;
                    }
                    if (ButtonManager.gameButton[8].IsClicked())
                    {
                        QuestHandler.RunChoiceOutcomes(sqHolder.Choices[1].ID);
                        pickedChoice = 1;
                        AudioHandler.PlayEffectInstance(4);
                        InPlay.popUpChoice = false;
                    }

                    break;

                case 3:

                    ButtonManager.gameButton[7].Update();
                    ButtonManager.gameButton[8].Update();
                    ButtonManager.gameButton[9].Update();

                    if (ButtonManager.gameButton[7].IsClicked())
                    {
                        QuestHandler.RunChoiceOutcomes(sqHolder.Choices[0].ID);
                        pickedChoice = 0;
                        AudioHandler.PlayEffectInstance(4);
                        InPlay.popUpChoice = false;
                    }
                    if (ButtonManager.gameButton[8].IsClicked())
                    {
                        QuestHandler.RunChoiceOutcomes(sqHolder.Choices[1].ID);
                        pickedChoice = 1;
                        AudioHandler.PlayEffectInstance(4);
                        InPlay.popUpChoice = false;
                    }
                    if (ButtonManager.gameButton[9].IsClicked())
                    {
                        QuestHandler.RunChoiceOutcomes(sqHolder.Choices[2].ID);
                        pickedChoice = 2;
                        AudioHandler.PlayEffectInstance(4);
                        InPlay.popUpChoice = false;
                    }

                    break;
            }
        }

        //Display Choice Outcome Box
        public static void UpdateOutcome()
        {
            ButtonManager.gameButton[7].Update();

            if (ButtonManager.gameButton[7].IsClicked())
            {
                AudioHandler.PlayEffectInstance(5);
                InPlay.inPlayState = InPlayState.Playing;
                InPlay.popUpChoice = true;
            }
        }

        //Draw PopUp Box
        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(popUpTxr[0], new Rectangle(0, 0, 1366, 768), Color.White);
            spriteBatch.Draw(popUpTxr[1], new Rectangle(270, 100, 750, 600), Color.White);

            spriteBatch.Draw(popUpTxr[QuestHandler.popUpFlag], new Rectangle(370, 150, 550, 200), Color.White);

            spriteBatch.DrawString(popUpFonts[0], sqHolder.Message, new Vector2(375, 375), Color.Black);

            //Draw Buttons with Text
            switch (sqHolder.Choices.Length)
            {
                case 1:

                    ButtonManager.gameButton[7].Draw(spriteBatch);
                    spriteBatch.DrawString(popUpFonts[1], sqHolder.Choices[0].ChoiceText, new Vector2(510, 640), Color.Black);

                    break;

                case 2:

                    ButtonManager.gameButton[7].Draw(spriteBatch);
                    ButtonManager.gameButton[8].Draw(spriteBatch);
                    spriteBatch.DrawString(popUpFonts[1], sqHolder.Choices[0].ChoiceText, new Vector2(510, 640), Color.Black);
                    spriteBatch.DrawString(popUpFonts[1], sqHolder.Choices[1].ChoiceText, new Vector2(510, 560), Color.Black);

                    break;
                    
                case 3:

                    ButtonManager.gameButton[7].Draw(spriteBatch);
                    ButtonManager.gameButton[8].Draw(spriteBatch);
                    ButtonManager.gameButton[9].Draw(spriteBatch);
                    string temp01 = sqHolder.Choices[0].ChoiceText;
                    string temp02 = sqHolder.Choices[1].ChoiceText;
                    string temp03 = sqHolder.Choices[2].ChoiceText;
                    spriteBatch.DrawString(popUpFonts[1], temp01, new Vector2(510, 490), Color.Black);
                    spriteBatch.DrawString(popUpFonts[1], temp02, new Vector2(510, 565), Color.Black);
                    spriteBatch.DrawString(popUpFonts[1], temp03, new Vector2(510, 640), Color.Black);

                    break;
            }
        }

        //Draw Choice Outcome Box
        public static void DrawOutcome(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(popUpTxr[0], new Rectangle(0, 0, 1366, 768), Color.White);
            spriteBatch.Draw(popUpTxr[1], new Rectangle(270, 100, 750, 600), Color.White);
            spriteBatch.Draw(popUpTxr[QuestHandler.popUpFlag], new Rectangle(370, 150, 550, 200), Color.White);
            spriteBatch.DrawString(popUpFonts[0], sqHolder.Choices[pickedChoice].OutcomeText, new Vector2(375, 375), Color.Black);
            ButtonManager.gameButton[7].Draw(spriteBatch);
            spriteBatch.DrawString(popUpFonts[1], "Finish" , new Vector2(510, 640), Color.Black);
        }
        #endregion
    }
}