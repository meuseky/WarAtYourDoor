using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Home_Front
{
    //Manages Item arrays
    static class Items
    {
        #region Variables

        //Amounts of Items
        public static int smokeBombAmount;
        public static int fireWorkAmount;

        //List of Items
        public static List<SmokeBomb> activeSmoke;
        public static List<FireWork> activeFire;

        //Item Activity
        public static bool smokeIsActive;
        public static bool fireIsActive;

        #endregion

        #region Methods
        //Constructor
        static Items()
        {
            smokeBombAmount = 0;
            fireWorkAmount = 0;
            activeSmoke = new List<SmokeBomb>();
            activeFire = new List<FireWork>();
            smokeIsActive = false;
            fireIsActive = false;
        }

        //Updates Item state
        public static void Update(GameTime gameTime)
        {
            //Places Smoke Bomb on Map
            if ((InputHandler.newKeyboardState.IsKeyDown(Keys.X) == true
                && InputHandler.oldKeyboardState.IsKeyDown(Keys.X) == false) && (activeSmoke.Count < 1))
            {
                if (Player.smokebombAmount > 0)
                {
                    AudioHandler.PlayEffectInstance(10);
                    PlaceItem(0);
                    smokeIsActive = true;
                    Player.smokebombAmount--;
                }

            }

            //Places Firework on Map
            if ((InputHandler.newKeyboardState.IsKeyDown(Keys.Z) == true
                && InputHandler.oldKeyboardState.IsKeyDown(Keys.Z) == false) && (activeFire.Count < 1))
            {
                if (Player.fireworkAmount > 0)
                {
                    AudioHandler.PlayEffectInstance(11);
                    PlaceItem(1);
                    fireIsActive = true;
                    Player.fireworkAmount--;
                }
            }

            //Removes unactive smokebombs
            for (int i = activeSmoke.Count - 1; i >= 0; i--)
            {
                activeSmoke[i].Update(gameTime);
                if (activeSmoke[i].alive == false)
                {
                    activeSmoke.RemoveAt(i);
                    smokeIsActive = false;
                    foreach (Enemy enemy in EnemyHandler.gameEnemy)
                    {
                        enemy.smokeInSight = false;
                    }
                }
            }

            //Removes unactive fireworks
            for (int i = activeFire.Count - 1; i >= 0; i--)
            {
                activeFire[i].Update(gameTime);
                if (activeFire[i].alive == false)
                {
                    AudioHandler.StopEffectInstance(11);
                    activeFire.RemoveAt(i);
                    fireIsActive = false;
                    foreach (Enemy enemy in EnemyHandler.gameEnemy)
                    {
                        enemy.fireNearby = false;
                    }
                }
            }
        }

        //Draws Smokebomb animation
        public static void DrawSmoke(SpriteBatch spriteBatch)
        {
            foreach (SmokeBomb sm in activeSmoke)
            {
                sm.Draw(spriteBatch);
            }
        }

        //Draws Firework animation
        public static void DrawFire(SpriteBatch spriteBatch)
        {
            foreach (FireWork fw in activeFire)
            {
                fw.Draw(spriteBatch);
            }
        }

        //Places Item InPlay & on Map
        static void PlaceItem(int type)
        {
            switch (type)
            {
                case 0:
                    SmokeBomb tempSmoke = new SmokeBomb();
                    activeSmoke.Add(tempSmoke);
                    smokeIsActive = true;
                    break;
                case 1:
                    FireWork tempFire = new FireWork();
                    activeFire.Add(tempFire);
                    fireIsActive = true;
                    break;
                default:
                    break;
            }
        }

        //Clear Item Arrays
        public static void ResetItems()
        {
            activeSmoke.Clear();
            activeFire.Clear();
        }
        #endregion
    }

    //Implements Smokebomb
    class SmokeBomb
    {
        #region Variables

        //Animation
        public float timer;
        public float animTimer;
        public int animState;

        //Activity
        public bool alive;

        //Position
        public Vector2 mapPosition;
        public Vector2 spritePosition;
        public Point location;

        #endregion

        #region Methods
        //Constructor
        public SmokeBomb()
        {
            timer = 3;
            animTimer = 0.1f;
            animState = 0;
            alive = true;
            location = Player.gridPosition;

            //Position depending on Player Facing direction
            switch (Player.FacingDirection)
            {
                case 0:
                    location.Y--;
                    break;
                case 1:
                    location.X++;
                    break;
                case 2:
                    location.Y++;
                    break;
                case 3:
                    location.X--;
                    break;
                default:
                    break;
            }

            mapPosition.X = location.X * 100;
            mapPosition.Y = location.Y * 100;
        }


        //Update Animation State
        public void Update(GameTime gameTime)
        {
            if (animTimer > 0)
            {
                animTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;               
            }
            else
            {
                animTimer = 0.1f;
                animState = (animState + 1) % 10;
            }
            if (timer <= 0)
            {
                alive = false;
            }
            UpdateScreenPosition();
        }

        //Draw Animation
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(InPlay.inPlayTexture[11], new Rectangle((int)spritePosition.X, (int)spritePosition.Y, 100, 100),
                new Rectangle(animState * 100, 0, 100, 100), Color.White);
        }

        //Adjust screen position
        private void UpdateScreenPosition()
        {
            spritePosition = new Vector2(Player.screenVector.X - Player.mapVector.X + mapPosition.X,
                                                        Player.screenVector.Y - Player.mapVector.Y + mapPosition.Y);
        }

        #endregion
    }

    //Implements Firework
    class FireWork
    {
        #region Variables

        //Animation
        public float timer;
        public float animTimer;
        public int animState;

        //Activity
        public bool alive;

        //Position
        public Vector2 mapPosition;
        public Vector2 spritePosition;
        public Point location;

        #endregion

        #region Methods
        //Constructor
        public FireWork()
        {
            timer = 3;
            animTimer = 0.1f;
            animState = 0;
            alive = true;
            location = Player.gridPosition;

            //Position depending on Player Facing direction
            switch (Player.FacingDirection)
            {
                case 0:
                    location.Y--;
                    break;
                case 1:
                    location.X++;
                    break;
                case 2:
                    location.Y++;
                    break;
                case 3:
                    location.X--;
                    break;
                default:
                    break;
            }
            mapPosition.X = location.X * 100;
            mapPosition.Y = location.Y * 100;
        }

        //Update Animation State
        public void Update(GameTime gameTime)
        {
            if (animTimer > 0)
            {
                animTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                animTimer = 0.1f;
                animState = (animState + 1) % 4;
            }
            if (timer <= 0)
            {
                alive = false;
            }
            UpdateScreenPosition();
        }

        //Draw Animation
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(InPlay.inPlayTexture[10], new Rectangle((int)spritePosition.X, (int)spritePosition.Y, 100, 100),
                new Rectangle(animState * 100, 0, 100, 100), Color.White);
        }

        //Adjust screen position
        private void UpdateScreenPosition()
        {
            spritePosition = new Vector2(Player.screenVector.X - Player.mapVector.X + mapPosition.X,
                                                        Player.screenVector.Y - Player.mapVector.Y + mapPosition.Y);
        }

        #endregion
    }
}
