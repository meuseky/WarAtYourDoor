using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Home_Front
{
    //Holds Player info
    static class Player
    {
        #region Variables

        //Current Map Tile
        public static Point gridPosition;

        //Vector Relative to Map origin
        public static Vector2 mapVector;

        //Vector of screen position
        public static Vector2 screenVector;

        //Player collision rectangle
        private static Rectangle collisionRect;

        //Direction Moving
        private static MovementType playerMovement;

        //Represents facing direction: Up=0,Right=1,Down=2,Left=3
        private static int faceDirection;

        //Holds amount moved during each grid traversal.
        private static int amountMoved;

        //Scores
        public static int[] highScores;

        //Temperary Score Holder
        public static int tempScore;

        //Animation
        private static int fps;
        private static int totalFrame;
        private static int currentFrame;
        private static float updateTrigger;
        private static float stoppedTimer;

        //Player Stats
        private static float hunger;
        public static int smokebombAmount;
        public static int fireworkAmount;
        private static int score;
        private static float scoreTimer;
        private static bool alive;
        #endregion

        #region Properties

        public static float Hunger
        {
            get { return hunger; }
            set { hunger = value; }
        }

        public static bool Alive
        {
            get { return alive; }
            set { alive = value; }
        }

        public static int FacingDirection
        {
            get { return faceDirection; }
            set { faceDirection = value; }
        }

        public static int Score
        {
            get { return score; }
            set { score = value; }
        }

        public static int[] HighScores
        {
            get { return highScores; }
            set { highScores = value; }
        }

        #endregion

        #region Methods
        //Constructor
        static Player()
        {
            //Position
            gridPosition = new Point(2, 2);
            mapVector = new Vector2(200, 200);
            screenVector = new Vector2(600, 300);

            amountMoved = 0;            
            collisionRect = new Rectangle((int)mapVector.X + 10, (int)mapVector.Y + 10, 80, 80);

            //Animation
            fps = 6;
            totalFrame = 6;
            currentFrame = 0;
            updateTrigger = 0;
            stoppedTimer = 0.2f;

            //Movement
            playerMovement = MovementType.None;
            faceDirection = 2;

            //Scores
            score = 0;
            scoreTimer = 1;
            hunger = 100;
            alive = true;

            //Items
            smokebombAmount = 3;
            fireworkAmount = 3;

            //Set Highscores
            highScores = new int[5];
            highScores = IOHandler.GetHighScores();
        }

        //Update Player State
        public static void Update(GameTime gameTime)
        {

            UpdateMovement(gameTime);
            UpdateAnimation(gameTime);
            UpdateHunger(gameTime);
            CheckEnemyCollision();

            //Game Over
            if (alive == false)
            {
                tempScore = score;
                UpdateHighScores();
                Player.Reset();
                Items.ResetItems();
                AudioHandler.StopSong(InPlay.currentLevel);
                InPlay.inPlayState = InPlayState.Death;
            }

            //Increment timer Score
            if (scoreTimer <= 0)
            {
                scoreTimer = 1f;
                score += 10;
            }
            else
            {
                scoreTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        //Draw Player Sprite
        public static void Draw(SpriteBatch spriteBatch)
        {
            PlayerSprite.Draw(spriteBatch, screenVector, currentFrame, faceDirection);
        }

        //Move Player to New Position
        public static void ResetPosition(Point newPos)
        {
            gridPosition = newPos;
            mapVector.X = newPos.X * 100;
            mapVector.Y = newPos.Y * 100;
        }

        //Update Movement from Input
        public static void UpdateMovement(GameTime gameTime)
        {
            switch (playerMovement)
            {
                case MovementType.None:
                   
                    //Run Animation until fully stopped
                    if (stoppedTimer < 0)
                    {
                        currentFrame = 0;
                    }
                    else
                    {
                        stoppedTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds * fps;
                    }

                    #region Move In New Direction
                    if (InputHandler.newKeyboardState.IsKeyDown(Keys.Up) == true)
                    {
                        faceDirection = 0;

                        if (gridPosition.Y > 0)
                        {
                            if (Map.m_Maps[InPlay.currentLevel].MapTile[gridPosition.X, gridPosition.Y - 1] < 11)
                            {
                                playerMovement = MovementType.Up;
                            }
                            if (Map.m_Maps[InPlay.currentLevel].MapTile[gridPosition.X, gridPosition.Y - 1] < 11)
                            {
                                AudioHandler.PlayEffect(3);
                                stoppedTimer = 0.2f;
                            }
                        }                       
                    }
                    if (InputHandler.newKeyboardState.IsKeyDown(Keys.Right) == true)
                    {
                        faceDirection = 1;

                        if (gridPosition.X < Map.m_Maps[InPlay.currentLevel].Height - 1)
                        {
                            if (Map.m_Maps[InPlay.currentLevel].MapTile[gridPosition.X + 1, gridPosition.Y] < 11)
                            {
                                playerMovement = MovementType.Right;
                            }
                            if (Map.m_Maps[InPlay.currentLevel].MapTile[gridPosition.X + 1, gridPosition.Y] < 11)
                            {
                                AudioHandler.PlayEffect(3);
                                stoppedTimer = 0.2f;
                            }
                        }                       
                    }
                    if (InputHandler.newKeyboardState.IsKeyDown(Keys.Down) == true)
                    {
                        faceDirection = 2;

                        if (gridPosition.Y < Map.m_Maps[InPlay.currentLevel].Width - 1)
                        {
                            if (Map.m_Maps[InPlay.currentLevel].MapTile[gridPosition.X, gridPosition.Y + 1] < 11)
                            {
                                playerMovement = MovementType.Down;
                            }
                            if (Map.m_Maps[InPlay.currentLevel].MapTile[gridPosition.X, gridPosition.Y + 1] < 11)
                            {
                                AudioHandler.PlayEffect(3);
                                stoppedTimer = 0.2f;
                            }
                        }                       
                    }

                    if (InputHandler.newKeyboardState.IsKeyDown(Keys.Left) == true)
                    {
                        faceDirection = 3;

                        if (gridPosition.X > 0)
                        {
                            if (Map.m_Maps[InPlay.currentLevel].MapTile[gridPosition.X - 1, gridPosition.Y] < 11)
                            {
                                playerMovement = MovementType.Left;
                            }
                            if (Map.m_Maps[InPlay.currentLevel].MapTile[gridPosition.X - 1, gridPosition.Y] < 11)
                            {
                                AudioHandler.PlayEffect(3);
                                stoppedTimer = 0.2f;
                            }
                        }                       
                    }
                    #endregion

                    break;

                case MovementType.Up:

                    if (amountMoved >= 90)
                    {
                        gridPosition.Y--;
                        mapVector.Y = gridPosition.Y * 100;
                        amountMoved = 0;
                        playerMovement = MovementType.None;
                    }
                    else
                    {
                        mapVector.Y -= gameTime.ElapsedGameTime.Milliseconds / 2;
                        amountMoved += gameTime.ElapsedGameTime.Milliseconds / 2;
                    }

                    break;

                case MovementType.Right:

                    if (amountMoved >= 90)
                    {
                        gridPosition.X++;
                        mapVector.X = gridPosition.X * 100;
                        amountMoved = 0;
                        playerMovement = MovementType.None;
                    }
                    else
                    {
                        mapVector.X += gameTime.ElapsedGameTime.Milliseconds / 2;
                        amountMoved += gameTime.ElapsedGameTime.Milliseconds / 2;
                    }

                    break;

                case MovementType.Down:

                    if (amountMoved >= 90)
                    {
                        gridPosition.Y++;
                        mapVector.Y = gridPosition.Y * 100;
                        amountMoved = 0;
                        playerMovement = MovementType.None;
                    }
                    else
                    {
                        mapVector.Y += gameTime.ElapsedGameTime.Milliseconds / 2;
                        amountMoved += gameTime.ElapsedGameTime.Milliseconds / 2;
                    }

                    break;

                case MovementType.Left:

                    if (amountMoved >= 90)
                    {
                        gridPosition.X--;
                        mapVector.X = gridPosition.X * 100;
                        amountMoved = 0;
                        playerMovement = MovementType.None;
                    }
                    else
                    {
                        mapVector.X -= gameTime.ElapsedGameTime.Milliseconds / 2;
                        amountMoved += gameTime.ElapsedGameTime.Milliseconds / 2;
                    }

                    break;
            }
            
            //Update Collision Rectangle
            collisionRect.X = (int)mapVector.X + 10;
            collisionRect.Y = (int)mapVector.Y + 10;
        }

        //Update Player Life Level
        public static void UpdateHunger(GameTime gameTime)
        {
            hunger -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Kill Player if too hungry
            if (hunger <= 0)
            {
                AudioHandler.PlayEffectInstance(0);
                alive = false;
            }

            //Clamp hunger Max
            if (Player.Hunger > 100)
            {
                Player.Hunger = 100;
            }
        }

        //Update Player Animation
        public static void UpdateAnimation(GameTime gameTime)
        {
            updateTrigger += (float)gameTime.ElapsedGameTime.TotalSeconds * fps;

            if (updateTrigger >= 0.8f)
            {
                updateTrigger = 0;

                if (playerMovement != MovementType.None)
                    currentFrame = (currentFrame + 1) % totalFrame;
            }
        }

        //Reset Player Info
        public static void Reset()
        {
            gridPosition = new Point(5, 6);
            mapVector = new Vector2(200, 200);
            screenVector = new Vector2(600, 300);

            amountMoved = 0;
            collisionRect = new Rectangle((int)mapVector.X + 10, (int)mapVector.Y + 10, 80, 80);

            currentFrame = 0;
            updateTrigger = 0;

            playerMovement = MovementType.None;
            faceDirection = 2;
            smokebombAmount = 3;
            fireworkAmount = 3;

            score = 0;
            hunger = 100;
            alive = true;
        }

        //Check collision box against Enemy locations
        public static void CheckEnemyCollision()
        {
            foreach (Enemy enemy in EnemyHandler.gameEnemy)
            {
                Vector2 tempVec = enemy.ScreenVector - screenVector;

                float vecLength = (float)tempVec.Length();

                if ((vecLength < 30) && (Items.smokeIsActive == false))
                {
                    //Death SFX
                    AudioHandler.PlayEffectInstance(0);
                    alive = false;
                }
            }
        }

        //Update & Sort Highscore table
        public static void UpdateHighScores()
        {
            int[] tempArray = new int[6];

            for (int i = 0; i < 5; i++)
            {
                tempArray[i] = highScores[i];
            }

            tempArray[5] = score;

            tempArray = tempArray.OrderByDescending(c => c).ToArray();

            for (int i = 0; i < 5; i++)
            {
                highScores[i] = tempArray[i];
            }
        }
        #endregion
    }

    class PlayerSprite
    {
        #region Variables

        static public Texture2D spriteTexture;
        static public int tileWidth = 100;
        static public int tileHeight = 100;

        #endregion

        #region Methods
        //Draw Player Sprite
        public static void Draw(SpriteBatch spriteBatch, Vector2 screenPos, int tileFrame, int tileDirection)
        {
            spriteBatch.Draw(spriteTexture, screenPos, new Rectangle(tileFrame * 100, tileDirection * 100, tileWidth, tileHeight), Color.White);
        }
        #endregion
    }
}
