using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Home_Front
{
    //Manages Transition Screens
    //Intro, Loading, Outro
    static class Transition
    {
        #region Variables

        public static TransitionState transitionState;

        //Art Assets
        public static Texture2D[] transitionTexture;

        //Amount if time transition runs
        public static float transitionTimer;

        //Used to initialise in first loop of update
        public static bool init, initSound;

        //Texture rotation
        private static float rotationAngle1, rotationAngle2, rotationAngle3;

        //Animation
        private static int outroAnim;

        #endregion

        #region Methods
        //Constructor
        static Transition()
        {
            transitionState = TransitionState.Intro;

            transitionTimer = 2.5f;

            rotationAngle1 = 0;
            rotationAngle2 = 0;
            rotationAngle3 = 0;

            init = true;
            initSound = true;

            outroAnim = 0;
        }

        //Updates Transition State
        public static void Update(GameTime gameTime)
        {
            switch (transitionState)
            {
                case TransitionState.Intro:

                    //Initialize Audio
                    if (init)
                    {
                        AudioHandler.PlayEffectInstance(8);
                        init = false;
                    }

                    //Run timer
                    if (transitionTimer > 0)
                    {
                        transitionTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    else
                    {
                        transitionTimer = 7f;
                        init = true;
                        transitionState = TransitionState.Loading;
                        AudioHandler.StopEffectInstance(8);
                    }

                    //Skip if Enter is Pressed
                    if (InputHandler.newKeyboardState.IsKeyDown(Keys.Enter) && InputHandler.oldKeyboardState.IsKeyDown(Keys.Enter))
                    {
                        AudioHandler.StopEffectInstance(8);
                        transitionTimer = 7f;
                        init = true;
                        transitionState = TransitionState.Loading;
                    }

                    break;

                case TransitionState.Loading:

                    //Initialize Audio
                    if (init)
                    {
                        AudioHandler.PlayEffectInstance(12);
                        init = false;
                    }

                    //Run timer
                    if (transitionTimer < 1)
                    {
                        if (initSound)
                        {
                            AudioHandler.PlayEffectInstance(13);
                            initSound = false;
                        }
                    }
                    if (transitionTimer > 0)
                    {
                        transitionTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    else
                    {
                        init = true;
                        transitionTimer = 2.45f;
                        StateHandler.gameState = GameState.Menu;
                        AudioHandler.StopEffectInstance(12);
                        AudioHandler.PlayEffectInstance(1);
                    }

                    //Update Animation
                    float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                    rotationAngle1 += elapsed * 3;
                    rotationAngle2 -= elapsed * 2;
                    rotationAngle3 += elapsed * 5;
                    float circle = MathHelper.Pi * 2;
                    rotationAngle1 = rotationAngle1 % circle;
                    rotationAngle2 = rotationAngle2 % circle;
                    rotationAngle3 = rotationAngle3 % circle;

                    break;

                case TransitionState.Outro:

                    //Initialize Audio
                    if (init)
                    {
                        AudioHandler.PlayEffectInstance(9);
                        init = false;
                    }

                    //Run timer
                    if (transitionTimer > 0)
                    {
                        transitionTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                        outroAnim += (int)gameTime.ElapsedGameTime.TotalMilliseconds / 10;
                    }
                    else
                    {
                        AudioHandler.StopEffectInstance(9);

                        //Exit Game
                        Game1.exitGame = true;
                    }
                    break;
            }
        }

        //Draw Transition Screen
        public static void Draw(SpriteBatch spriteBatch)
        {
            switch (transitionState)
            {
                case TransitionState.Intro:

                    spriteBatch.Draw(transitionTexture[0], new Rectangle(0, 0, 1366, 768), Color.White);

                    break;

                case TransitionState.Loading:

                    spriteBatch.Draw(transitionTexture[2], new Rectangle(0, 0, 1366, 768), Color.White);
                    spriteBatch.Draw(transitionTexture[3], new Rectangle(408, 100, 550, 550), Color.White);

                    //Spinning textures
                    spriteBatch.Draw(transitionTexture[4], new Vector2(423 + (transitionTexture[4].Width / 2), 115 + (transitionTexture[4].Width / 2)), null, Color.White, rotationAngle1,
                    new Vector2(transitionTexture[4].Width / 2, transitionTexture[4].Height / 2), 1.0f, SpriteEffects.None, 0f);
                    spriteBatch.Draw(transitionTexture[5], new Vector2(508 + (transitionTexture[5].Width / 2), 200 + (transitionTexture[5].Width / 2)), null, Color.White, rotationAngle2,
                    new Vector2(transitionTexture[5].Width / 2, transitionTexture[5].Height / 2), 1.0f, SpriteEffects.None, 0f);
                    spriteBatch.Draw(transitionTexture[6], new Vector2(568 + (transitionTexture[6].Width / 2), 260 + (transitionTexture[6].Width / 2)), null, Color.White, rotationAngle3,
                    new Vector2(transitionTexture[6].Width / 2, transitionTexture[6].Height / 2), 1.0f, SpriteEffects.None, 0f);

                    //Fill progress bar
                    int barSize = (int)(((7 - transitionTimer) / 7) * 580);
                    if (barSize > 488)
                    {
                        barSize = 488;
                    }
                    
                    spriteBatch.Draw(transitionTexture[7], new Vector2(433, 700), Color.White);
                    spriteBatch.Draw(transitionTexture[8], new Rectangle(439, 706, barSize, 18), Color.White);

                    break;

                case TransitionState.Outro:

                    spriteBatch.Draw(transitionTexture[1], new Rectangle(0, 0, 1366, 768), Color.White);

                    //Moving textures
                    spriteBatch.Draw(transitionTexture[9], new Vector2(500 + outroAnim, 25), Color.White);
                    spriteBatch.Draw(transitionTexture[10], new Vector2(550, 250 + (outroAnim * 2)), Color.White);

                    break;
            }
        }
        #endregion
    }
}
