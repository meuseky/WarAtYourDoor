using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Home_Front
{
    //Routes all 'In Game' logic
    static class InPlay
    {
        #region Variables
        
        //Holds current InPlay state
        public static InPlayState inPlayState;

        //Holds InPlay textures
        public static Texture2D[] inPlayTexture;

        //Current Level
        public static int currentLevel;

        //Button selected in PopUp screen
        public static bool popUpChoice;

        //Level introduction info
        private static string[] newLevelInfo;

        #endregion

        #region Methods
        //Constructor
        static InPlay()
        {
            inPlayState = InPlayState.NewGame;
            currentLevel = 0;

            popUpChoice = true;

            newLevelInfo = new string[6];
            newLevelInfo[0] = "You wake up at the sound of war alarms echoing\nthroughout the city.You exit your house trying\nto find out what is happening. You see people\nrunning around in a frantic way and you approach\none to ask. He tells you that the city is under\nattack but he doesn't know any more details than\nthis. Perhaps you should go to the nearest police\nstation if you want more details.";
            newLevelInfo[1] = "You reach the bus station but something is terribly\nwrong.The area lies in ruins.You see several destroyed\nbuses but no other people except from two soldiers.\nThey tell you that the the bus station was bombarded.\nThe evacuation plan is being changed but they don't\nknow anything else.Seek information at the Public\nService.";
            newLevelInfo[2] = "After you spend the night at the abandoned house\nyou decide to hear today's broadcast before\ncontinuing on your journey. Everything seems to\ngo wrong as you hear the news: 'The evacuation\nplan is postponed until further notice. All capable\ncitizens are required to report on the new military\nbase located to the building on Carlton road'.Not\nonly you can't escape the city but you are being\nsummoned as well..You don't have any option in this.";
            newLevelInfo[3] = "You are being transferred at the new army HQs along a\ndozen other civilians.Why they chose you is beyond\nyour knowledge but the fact that you approach the war\nfront instead of drawing away terrifies you.People die\nevery day in a war and while you were lucky so far\ndoes not mean you will be forever..";
            newLevelInfo[4] = "The army has dragged you step by step nearer the front.\nIt is as if they gave you much choice, just orders.\nThe terrors of war have started to get the best of you.\nYou feel despair creeping in as your plan of fleeing\nthe city seems more distant every day.You are now\nbasically a soldier even though no-one seems to admit\nit.";
            newLevelInfo[5] = "It can get worse than this.You are trapped in the\nmiddle of the warzone and you are alone.You have to\nmuster all your courage and determination if you want\nto leave this place alive.Sheer adrenaline is the\nonly force that drives you on";
        }

        //Updates InPlay State
        //NewGame, Playing, PlayMenuPopUpScreen, NewLevel, Death, Win
        public static void Update(GameTime gameTime)
        {
            switch (inPlayState)
            {

                //Initalizes New Game
                case InPlayState.NewGame:

                    //Sets first achievement to true
                    Achievements.UpdateAchievements(0);

                    inPlayState = InPlayState.NewLevel;

                    break;

                //Main Gameplay Loop
                case InPlayState.Playing:

                    Player.Update(gameTime);
                    Items.Update(gameTime);
                    EnemyHandler.Update(gameTime);
                    QuestHandler.Update(gameTime);

                    //If Escape pressed switch to Quest menu
                    if (InputHandler.KeyPress(1))
                    {
                        inPlayState = InPlayState.PlayMenu;
                    }

                    break;

                //Shows current quests & allows return to Main Menu
                case InPlayState.PlayMenu:
                                       
                    ButtonManager.gameButton[5].Update();
                    ButtonManager.gameButton[6].Update();

                    //Back to game
                    if (ButtonManager.gameButton[5].IsClicked())
                    {
                        inPlayState = InPlayState.Playing;
                    }

                    //Return to Main Menu
                    if (ButtonManager.gameButton[6].IsClicked())
                    {
                        QuestHandler.ResetAllQuests();
                        AudioHandler.StopSong(currentLevel);
                        AudioHandler.PlayEffectInstance(4);
                        StateHandler.gameState = GameState.Menu;
                        AudioHandler.PlayEffectInstance(1);
                    }

                    break;

                //Quest Popup screens
                case InPlayState.PopUpScreen:

                    //Handles input & outcomes from Quest popup screens
                    if (popUpChoice)
                    {
                        PopUp.Update();
                    }
                    else
                    {
                        PopUp.UpdateOutcome();
                    }

                    break;
                    
                //Handles New Level transitions
                case InPlayState.NewLevel:   
                 
                    //Checks if New Level Info Box is Finished
                    ButtonManager.gameButton[11].Update();

                    //Sets second achievement to true
                    if (currentLevel == 1)
                    {                       
                        Achievements.UpdateAchievements(1);
                    }

                    //Prevents going beyond Level 6
                    if (currentLevel > 5)
                    {
                        inPlayState = InPlayState.Playing;
                    }
                    else
                    {
                        //Checks if New Level Info Box is Finished
                        if(ButtonManager.gameButton[11].IsClicked())
                        {
                            //Resets player position
                            Point tempPoint = Map.startingPoints[currentLevel];
                            Player.ResetPosition(tempPoint);

                            //Stops previous song playing
                            if (currentLevel != 0)
                            {
                                AudioHandler.StopSong(currentLevel);
                            }

                            //Plays new song
                            AudioHandler.PlaySong(currentLevel);

                            //Start game play
                            inPlayState = InPlayState.Playing;
                        }
                    }        
            
                    break;

                //Handles player death
                case InPlayState.Death:

                    
                    ButtonManager.gameButton[10].Update();
                    if (ButtonManager.gameButton[10].IsClicked())
                    {
                        QuestHandler.ResetAllQuests();
                        StateHandler.gameState = GameState.Menu;
                        inPlayState = InPlayState.Playing;
                        AudioHandler.PlayEffectInstance(1);
                    }
                    
                    break;

                //Handles player win
                case InPlayState.Win:

                    //Return to Main Menu
                    ButtonManager.gameButton[10].Update();
                    if (ButtonManager.gameButton[10].IsClicked())
                    {
                        Achievements.UpdateAchievements(2);
                        QuestHandler.ResetAllQuests();                        
                        AudioHandler.StopSong(currentLevel);
                        StateHandler.gameState = GameState.Menu;
                        inPlayState = InPlayState.Playing;
                        AudioHandler.PlayEffectInstance(1);
                    }

                    break;
            }
        }

        //Draws InPlay State
        public static void Draw(SpriteBatch spriteBatch)
        {
            switch (inPlayState)
            {
                case InPlayState.NewGame:
                    //No drawing necessary
                    break;

                case InPlayState.Playing:

                    Tile.DrawBorder(spriteBatch);
                    Map.Draw(spriteBatch);
                    Items.DrawSmoke(spriteBatch);
                    EnemyHandler.Draw(spriteBatch);
                    Player.Draw(spriteBatch);
                    Items.DrawFire(spriteBatch);
                    QuestHandler.Draw(spriteBatch);

                    spriteBatch.Draw(inPlayTexture[2], new Rectangle(0, 0, 1366, 75), Color.White);
                    spriteBatch.Draw(inPlayTexture[8], new Rectangle(20, 10, 50, 50), Color.White);
                    spriteBatch.Draw(inPlayTexture[9], new Rectangle(130, 10, 50, 50), Color.White);
                    spriteBatch.Draw(inPlayTexture[3], new Rectangle(500, 20, 366, 30), Color.White);
                    spriteBatch.Draw(inPlayTexture[4], new Rectangle(500, 20
                                                                        , 366 - (int)(366 * (101 - Player.Hunger) / 100), 30)
                                                                        , Color.White);

                    spriteBatch.DrawString(PopUp.popUpFonts[3], "Stamina: ", new Vector2(345, 22), Color.Black);
                    spriteBatch.DrawString(PopUp.popUpFonts[3], "Score: ", new Vector2(1065, 23), Color.Black);
                    spriteBatch.DrawString(PopUp.popUpFonts[3], "x" + Player.fireworkAmount.ToString(), new Vector2(75, 21), Color.DarkRed);
                    spriteBatch.DrawString(PopUp.popUpFonts[3], "x" + Player.smokebombAmount.ToString(), new Vector2(186, 21), Color.DarkRed);

                    string scoreString;
                    scoreString = Player.Score.ToString();
                    scoreString = scoreString.PadLeft(7, '0');
                    spriteBatch.DrawString(PopUp.popUpFonts[0], scoreString, new Vector2(1180, 20), Color.Black);
                    break;

                case InPlayState.PlayMenu:
                    //spriteBatch.Draw(inPlayTexture[0], new Rectangle(0, 0, 1366, 768), Color.White);
                    Tile.DrawBorder(spriteBatch);
                    Map.Draw(spriteBatch);
                    EnemyHandler.Draw(spriteBatch);
                    Player.Draw(spriteBatch);
                    spriteBatch.Draw(inPlayTexture[1], new Rectangle(270, 100, 750, 600), Color.White);
                    ButtonManager.gameButton[5].Draw(spriteBatch);
                    ButtonManager.gameButton[6].Draw(spriteBatch);

                    int temp = 0;
                    foreach (Quest quest in QuestHandler.gameQuests)
                    {
                        if (quest.Level == InPlay.currentLevel)
                        {
                            if (quest.Active)
                            {
                                spriteBatch.DrawString(PopUp.popUpFonts[0], quest.QuestName, new Vector2(310, 225 + (temp * 75)), new Color(50,50,50));
                                spriteBatch.DrawString(PopUp.popUpFonts[0], quest.QuestDescription, new Vector2(500, 225 + (temp * 75)), Color.Black);
                                temp++;
                            }
                        }
                    }
                    break;

                case InPlayState.PopUpScreen:
                    //Draw the Game in the background of the PopUp Box
                    //spriteBatch.Draw(inPlayTexture[0], new Rectangle(0, 0, 1366, 768), Color.White);
                    Tile.DrawBorder(spriteBatch);
                    Map.Draw(spriteBatch);
                    EnemyHandler.Draw(spriteBatch);
                    Player.Draw(spriteBatch);                    
                    if (popUpChoice)
                    {
                        PopUp.Draw(spriteBatch);
                    }
                    else
                    {
                        PopUp.DrawOutcome(spriteBatch);
                    }
                    break;

                case InPlayState.NewLevel:
                    //spriteBatch.Draw(inPlayTexture[0], new Rectangle(0, 0, 1366, 768), Color.White);
                    Tile.DrawBorder(spriteBatch);
                    spriteBatch.Draw(inPlayTexture[7], new Rectangle(290, 140, 800, 500), Color.White);
                    spriteBatch.DrawString(PopUp.popUpFonts[2], "Level " + (currentLevel + 1).ToString(), 
                        new Vector2(572, 180), Color.Black);
                    spriteBatch.DrawString(PopUp.popUpFonts[0], newLevelInfo[currentLevel],
                        new Vector2(350, 250), Color.Black);
                    ButtonManager.gameButton[11].Draw(spriteBatch);
                    break;

                case InPlayState.Death:
                    //spriteBatch.Draw(inPlayTexture[0], new Rectangle(0, 0, 1366, 768), Color.White);
                    Tile.DrawBorder(spriteBatch);
                    spriteBatch.Draw(inPlayTexture[5], new Rectangle(290, 140, 800, 500), Color.White);
                    spriteBatch.DrawString(PopUp.popUpFonts[2], "Score: " + Player.tempScore.ToString(), new Vector2(563, 400), Color.Red);
                    ButtonManager.gameButton[10].Draw(spriteBatch);
                    break;

                case InPlayState.Win:
                    //spriteBatch.Draw(inPlayTexture[0], new Rectangle(0, 0, 1366, 768), Color.White);
                    Tile.DrawBorder(spriteBatch);
                    spriteBatch.Draw(inPlayTexture[6], new Rectangle(290, 140, 800, 500), Color.White);
                    spriteBatch.DrawString(PopUp.popUpFonts[2], "Score: " + Player.tempScore.ToString(), new Vector2(570, 420), Color.Blue);
                    ButtonManager.gameButton[10].Draw(spriteBatch);
                    break;
            }
        }

        //Sets current level to first level
        public static void Reset()
        {
            currentLevel = 0;
        }

        #endregion
    }
}
