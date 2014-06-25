using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Home_Front
{
    //Used to draw Debug Info onscreen during development
    static class Debug
    {
        #region Variables
        public static SpriteFont debugFont;
        #endregion

        #region Methods
        //Constructor
        static Debug()
        {
        }

        //Draws text on-screen
        public static void Draw(SpriteBatch spriteBatch)
        {
            /*
            if (StateHandler.gameState == GameState.InPlay)
            {
                //spriteBatch.DrawString(debugFont, "Level: " + InPlay.currentLevel.ToString(), new Vector2(50, 200), Color.White);

                /*
                spriteBatch.DrawString(debugFont, "Score: " + Player.Score.ToString(), new Vector2(50, 200), Color.White);
                                
                for (int i = 0; i < 5; i++)
                {
                    spriteBatch.DrawString(debugFont, (i + 1).ToString() + ": " + Player.HighScores[i].ToString().PadLeft(5,'0'), new Vector2(50, 250 + (i * 25)), Color.White);
                }
                
                
                /*
                spriteBatch.DrawString(debugFont, "PlayerGridPos: " + Player.gridPosition + "\nPlayerVectPos: " + Player.mapVector
                                        + "\nEnemy1GridPos: " + EnemyHandler.gameEnemy[0].GridPosition + "\nEnemy1VectPos: " + EnemyHandler.gameEnemy[0].MapVector
                                        + "\nEnemy1PatrolPoint: " + EnemyHandler.gameEnemy[0].PatrolPoint, 
                                        new Vector2(50, 50), Color.White);
                spriteBatch.DrawString(debugFont, "Hunger: " + Player.Hunger, new Vector2(50, 200), Color.White);
                foreach (Quest quest in QuestHandler.gameQuests)
                {
                    spriteBatch.DrawString(debugFont, "QuestID: " + quest.QuestID, new Vector2(50, 400), Color.White);
                    spriteBatch.DrawString(debugFont, "QuestLevel: " + quest.Level, new Vector2(50, 600), Color.White);
                    spriteBatch.DrawString(debugFont, "QuestActive: " + quest.Active, new Vector2(50, 625), Color.White);
                    for (int i = 0; i < quest.SubQuests.Length; i++)
                    {
                        spriteBatch.DrawString(debugFont, "SQPos: " + quest.SubQuests[i].MapPosition.X + "," + quest.SubQuests[i].MapPosition.X, new Vector2(50, 450 + (i * 25)), Color.White);
                    }
                    spriteBatch.DrawString(debugFont, "CurrentSQ: " + quest.CurrentSubQ, new Vector2(50, 700), Color.White);
                }
                spriteBatch.DrawString(debugFont, "InPlayLevel: " + InPlay.currentLevel, new Vector2(50, 650), Color.White);
                spriteBatch.DrawString(debugFont, "PopUpFlag: " + QuestHandler.popUpFlag, new Vector2(50, 675), Color.White); 
                
            }
            */
        }

        public static void Update(GameTime gameTime)
        {
            if (InputHandler.newKeyboardState.IsKeyDown(Keys.Q))
            {
                IOHandler.WriteHighScore(Player.HighScores);
                IOHandler.WriteAchievements(Achievements.Achievement);
                Game1.exitGame = true;
            }
            if (InputHandler.newKeyboardState.IsKeyDown(Keys.W))
            {
                Player.tempScore = Player.Score;
                Player.UpdateHighScores();
                StateHandler.gameState = GameState.InPlay;
                InPlay.inPlayState = InPlayState.Win;
            }
			if (InputHandler.newKeyboardState.IsKeyDown(Keys.N)
                && InputHandler.oldKeyboardState.IsKeyUp(Keys.N))
            {
                if (InPlay.currentLevel < 6)
                {
                    InPlay.currentLevel++;
                    Items.ResetItems();
                    InPlay.inPlayState = InPlayState.NewLevel;
                }
            }
            if (InputHandler.newKeyboardState.IsKeyDown(Keys.E))
            {
                for (int i = 0; i < 5; i++)
                {
                    Player.highScores[i] = (i + 100) * (i + 50);
                }
                for (int i = 0; i < 4; i++)
                {
                    Achievements.Achievement[i] = 1;
                }
            }
        }
        #endregion
    }
}
