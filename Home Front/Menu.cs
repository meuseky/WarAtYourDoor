using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Home_Front
{
    //Handles Menu States
    //Main, Settings, Highscore
    static class Menu
    {
        #region Variables

        public static MenuState menuState;

        public static Texture2D[] menuTexture;

        #endregion

        #region Methods
        //Constructor
        static Menu()
        {
            menuState = MenuState.Main;
        }

        //Updates Menu State
        public static void Update(GameTime gameTime)
        {
            switch (menuState)
            {
                case MenuState.Main:

                    ButtonManager.gameButton[0].Update();
                    ButtonManager.gameButton[1].Update();
                    ButtonManager.gameButton[2].Update();
                    ButtonManager.gameButton[3].Update();

                    //New Game Button
                    if (ButtonManager.gameButton[0].IsClicked())
                    {
                        AudioHandler.StopEffectInstance(1);
                        NewGame();
                        StateHandler.gameState = GameState.InPlay;
                        InPlay.inPlayState = InPlayState.NewGame;
                    }

                    //Highscore Button
                    if (ButtonManager.gameButton[1].IsClicked())
                    {
                        menuState = MenuState.HighScore;                        
                    }

                    //Settings Button
                    if (ButtonManager.gameButton[2].IsClicked())
                    {
                        menuState = MenuState.Settings;
                    }

                    //Exit Game Button
                    if (ButtonManager.gameButton[3].IsClicked())
                    {
                        AudioHandler.StopEffectInstance(1);
                        IOHandler.WriteHighScore(Player.HighScores);
                        IOHandler.WriteAchievements(Achievements.Achievement);
                        Transition.transitionState = TransitionState.Outro;
                        StateHandler.gameState = GameState.Transition;
                        Transition.init = true;
                    } 

                    break;

                case MenuState.Settings:

                    ButtonManager.gameButton[4].Update();
                    ButtonManager.gameButton[12].Update();
                    ButtonManager.gameButton[13].Update();

                    //Main Manu Button
                    if (ButtonManager.gameButton[4].IsClicked())
                    {
                        menuState = MenuState.Main;
                    }

                    //Reset Highscores
                    if (ButtonManager.gameButton[12].IsClicked())
                    {
                        int[] tempHold = new int[5];
                        for (int i = 0; i < 5; i++)
                        {
                            tempHold[i] = 0;
                            Player.highScores[i] = 0;
                        }
                        IOHandler.WriteHighScore(tempHold);
                    }

                    //Reset Achievements
                    if (ButtonManager.gameButton[13].IsClicked())
                    {
                        int[] tempHold = new int[4];
                        for (int i = 0; i < 4; i++)
                        {
                            tempHold[i] = 0;
                        }
                        IOHandler.WriteAchievements(tempHold);
                        Achievements.ResetAll();
                    }

                    break;

                case MenuState.HighScore:

                    ButtonManager.gameButton[4].Update();

                    //Main Mneu Button
                    if (ButtonManager.gameButton[4].IsClicked())
                    {
                        menuState = MenuState.Main;
                    }

                    break;
            }
        }

        //Draws Mneu State
        public static void Draw(SpriteBatch spriteBatch)
        {
            switch (menuState)
            {
                case MenuState.Main:

                    //Background
                    spriteBatch.Draw(menuTexture[0], new Rectangle(0, 0, 1366, 768), new Rectangle(455, 0, 455, 256), Color.White);
                    //Title
                    spriteBatch.Draw(menuTexture[5], new Rectangle(60, 55, 973, 65), Color.White);

                    ButtonManager.gameButton[0].Draw(spriteBatch);
                    ButtonManager.gameButton[1].Draw(spriteBatch);
                    ButtonManager.gameButton[2].Draw(spriteBatch);
                    ButtonManager.gameButton[3].Draw(spriteBatch);

                    break;

                case MenuState.Settings:

                    //Background
                    spriteBatch.Draw(menuTexture[0], new Rectangle(0, 0, 1366, 768), new Rectangle(0, 0, 455, 256), Color.White);
                    //Boxes
                    spriteBatch.Draw(menuTexture[4], new Rectangle(50, 50, 650, 450), Color.White);
                    spriteBatch.Draw(menuTexture[3], new Rectangle(750, 50, 500, 598), Color.White);

                    //Main Menu Button
                    ButtonManager.gameButton[4].Draw(spriteBatch);
                    //Reset Buttons
                    ButtonManager.gameButton[12].Draw(spriteBatch);
                    ButtonManager.gameButton[13].Draw(spriteBatch);

                    //Settings Text
                    spriteBatch.DrawString(PopUp.popUpFonts[0], "Press to Reset HighScores:", new Vector2(190, 155), Color.SeaShell);
                    spriteBatch.DrawString(PopUp.popUpFonts[0], "Reset", new Vector2(335, 220), new Color(50, 50, 50));
                    spriteBatch.DrawString(PopUp.popUpFonts[0], "Press to Reset Achievements:", new Vector2(190, 305), Color.SeaShell);
                    spriteBatch.DrawString(PopUp.popUpFonts[0], "Reset", new Vector2(335, 370), new Color(50,50,50));

                    //Instructions Text
                    spriteBatch.DrawString(PopUp.popUpFonts[0], "Complete quests to progress \nthrough each level before\nyour stamina depletes.\n\nDistract enemies with Fireworks, \navoid with Smokebombs.",
                        new Vector2(790, 140), Color.SeaShell);
                    spriteBatch.Draw(InPlay.inPlayTexture[12], new Rectangle(775, 350, 70, 70), new Rectangle(300, 0, 100, 100), Color.White);
                    spriteBatch.DrawString(PopUp.popUpFonts[0], "- Quest Marker", new Vector2(845, 360), Color.SeaShell);
                    spriteBatch.DrawString(PopUp.popUpFonts[0], "Controls:\n\nEscape - Quest Menu\nZ - Set Fire Work\nX - Set Smoke Bomb\nUp/Down/Left/Right - Movement", new Vector2(790, 425), Color.SeaShell);
                    
                    break;

                case MenuState.HighScore:

                    //Background
                    spriteBatch.Draw(menuTexture[0], new Rectangle(0, 0, 1366, 768), new Rectangle(910, 0, 455, 256), Color.White);
                    //Boxes
                    spriteBatch.Draw(menuTexture[1], new Vector2(50,50), Color.White);
                    spriteBatch.Draw(menuTexture[2], new Vector2(750, 50), Color.White);

                    //Main Menu Button
                    ButtonManager.gameButton[4].Draw(spriteBatch);

                    //Write Highscores
                    for (int i = 0; i < 5; i++)
                    {
                        spriteBatch.DrawString(PopUp.popUpFonts[0], Player.HighScores[i].ToString().PadLeft(6, ' '), new Vector2(400, 170 + (50 * i)), Color.SeaShell);
                    }                     
                    for (int i = 0; i < 5; i++)
                    {
                        spriteBatch.DrawString(PopUp.popUpFonts[0], (i + 1).ToString() + ":", new Vector2(250, 170 + (50 * i)), Color.White);
                    }

                    //Draw Achievements
                    for (int i = 0; i < 4; i++)
                    {
                        int temp1, temp2, temp3;
                        temp1 = i % 2;
                        temp2 = (i < 2) ? 0 : 1;
                        temp3 = (Achievements.Achievement[i] == 0) ? 0 : (i + 1);
                        spriteBatch.Draw(menuTexture[6], new Rectangle(850 + (temp1 * 200), 170 + (temp2 * 200), 100, 150), 
                            new Rectangle(0 + (temp3 * 100), 0, 100, 150), Color.White);
                    }

                    break;
            }
        }

        //Reset Game
        public static void NewGame()
        {
            Player.UpdateHighScores();
            Player.Reset();
            EnemyHandler.Reset();
            InPlay.Reset();
        }
        #endregion
    }
}
