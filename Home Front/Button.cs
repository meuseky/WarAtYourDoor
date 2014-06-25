using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Home_Front
{
    //Holds info for individual buttons
    //ButtonHandler class below stores array of all Buttons
    class Button
    {
        #region Variables

        //Button image
        private Texture2D buttonTexture;

        //Button screen position
        private Rectangle screenPosition;

        //0:Normal, 1:MouseOver, 2:MousePressed, 3:MouseReleased
        private int buttonState;

        //Size of animation frame
        private int cellSize;

        #endregion

        #region Methods
        //Constructor
        public Button(Texture2D sentButtonTexture, Rectangle sentScreenPosition)
        {
            buttonTexture = sentButtonTexture;
            screenPosition = sentScreenPosition;
            buttonState = 0;
            cellSize = sentButtonTexture.Height / 4;
        }

        //Updates button state
        //Uses InputHandler to chaeck for interaction
        public void Update()
        {
            if (new Rectangle(InputHandler.newMouseState.X, InputHandler.newMouseState.Y, 1, 1).Intersects(screenPosition))
            {
                buttonState = 1;
                if (InputHandler.IsLeftButtonDown())
                {
                    buttonState = 2;
                }
                if (InputHandler.IsLeftButtonClicked())
                {
                    buttonState = 3;
                }
            }
            else
            {
                buttonState = 0;
            }
        }

        //Updates button animation
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(buttonTexture, screenPosition,
                                new Rectangle(0, cellSize * buttonState, buttonTexture.Width, cellSize), Color.White);
        }

        //Checks if button is clicked
        public bool IsClicked()
        {
            if (InputHandler.IsLeftButtonClicked()
                    && new Rectangle(InputHandler.newMouseState.X, InputHandler.newMouseState.Y, 1, 1).Intersects(screenPosition))
            {
                AudioHandler.PlayEffect(4);
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }

    //Manages state of Buttons
    static class ButtonManager
    {
        #region Variables
        //Array of game buttons
        public static Button[] gameButton;

        //Array of button textures
        public static Texture2D[] buttonTexture;

        #endregion

        #region Methods
        //Constructor
        static ButtonManager()
        {
            gameButton = new Button[14];
        }

        //Initialize all buttons
        public static void InitButtons()
        {
            //Menu Buttons
            gameButton[0] = new Button(buttonTexture[0], new Rectangle(75, 400, 300, 50));
            gameButton[1] = new Button(buttonTexture[1], new Rectangle(75, 475, 300, 50));
            gameButton[2] = new Button(buttonTexture[2], new Rectangle(75, 550, 300, 50));
            gameButton[3] = new Button(buttonTexture[3], new Rectangle(75, 625, 300, 50));
            gameButton[4] = new Button(buttonTexture[4], new Rectangle(75, 625, 300, 50));

            //InPlay Buttons
            gameButton[5] = new Button(buttonTexture[5], new Rectangle(675, 600, 300, 50));
            gameButton[6] = new Button(buttonTexture[4], new Rectangle(325, 600, 300, 50));

            //Quest PopUp Buttons
            gameButton[7] = new Button(buttonTexture[6], new Rectangle(500, 625, 300, 50));
            gameButton[8] = new Button(buttonTexture[6], new Rectangle(500, 550, 300, 50));
            gameButton[9] = new Button(buttonTexture[6], new Rectangle(500, 475, 300, 50));

            //Win/Lose
            gameButton[10] = new Button(buttonTexture[4], new Rectangle(545, 550, 300, 50));
            gameButton[11] = new Button(buttonTexture[7], new Rectangle(545, 550, 300, 50));

            //Settings
            gameButton[12] = new Button(buttonTexture[6], new Rectangle(225, 210, 300, 50));
            gameButton[13] = new Button(buttonTexture[6], new Rectangle(225, 360, 300, 50));
        }
        #endregion
    }
}
