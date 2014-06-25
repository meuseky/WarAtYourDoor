using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Home_Front
{
    //Handles all player input
    static class InputHandler
    {
        #region Variables

        //Keyboard states
        public static KeyboardState newKeyboardState;
        public static KeyboardState oldKeyboardState;

        //Mouse states
        public static MouseState newMouseState;
        public static MouseState oldMouseState;

        #endregion

        #region Methods
        //Constructor
        static InputHandler()
        {
            oldKeyboardState = newKeyboardState;
            newKeyboardState = Keyboard.GetState();
            oldMouseState = newMouseState;
            newMouseState = Mouse.GetState();
        }

        //Refreshes user input
        public static void Update()
        {
            oldKeyboardState = newKeyboardState;
            newKeyboardState = Keyboard.GetState();
            oldMouseState = newMouseState;
            newMouseState = Mouse.GetState();
        }

        public static bool IsLeftButtonDown()
        {
            if (newMouseState.LeftButton == ButtonState.Pressed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsLeftButtonClicked()
        {
            if (newMouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool KeyPress(int keyIndex)
        {
            switch (keyIndex)
            {
                case 1:
                    //Corresponds to hitting the 'Escape' Key
                    if(newKeyboardState.IsKeyDown(Keys.Escape) && oldKeyboardState.IsKeyDown(Keys.Escape))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case 2:
                    //Corresponds to hitting the 'S' Key
                    if (newKeyboardState.IsKeyDown(Keys.S) && oldKeyboardState.IsKeyDown(Keys.S))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                default:
                    return false;
            }
        }

        //Returns mouse cursor position
        public static int[] GetMousePos()
        {
            int cursorX = newMouseState.X;
            int cursorY = newMouseState.Y;

            int[] mousePos = new int[] { cursorX, cursorY };
            return mousePos;
        }
        #endregion
    }
}
