using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Home_Front
{
    //Sends Game Loop into either Transition, Menu, or InPlay State
    static class StateHandler
    {
        #region Variables
        public static GameState gameState;
        #endregion

        #region Methods
        //Constructor
        static StateHandler()
        {
            gameState = GameState.Transition;
        }

        //Routes Game Loop Updates
        public static void Update(GameTime gameTime)
        {
            switch (gameState)
            {
                case GameState.Transition:
                    Transition.Update(gameTime);
                    break;

                case GameState.Menu:
                    Menu.Update(gameTime);
                    break;

                case GameState.InPlay:
                    InPlay.Update(gameTime);;
                    break;
            }
        }

        //Routes Draw Updates
        public static void Draw(SpriteBatch spriteBatch)
        {
            switch (gameState)
            {
                case GameState.Transition:
                    Transition.Draw(spriteBatch);
                    break;

                case GameState.Menu:
                    Menu.Draw(spriteBatch);
                    break;

                case GameState.InPlay:
                    InPlay.Draw(spriteBatch); ;
                    break;
            }
        }
        #endregion
    }
}
