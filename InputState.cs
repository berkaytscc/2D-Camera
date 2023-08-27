using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;

namespace _2D_Camera
{
    public static class InputState
    {
        public static bool IsScrollLeft()
        {
            return IsKeyPressed(Keys.A);
        }
        public static bool IsScrollRight()
        {
            return IsKeyPressed(Keys.D);
        }
        public static bool IsScrollUp()
        {
            return IsKeyPressed(Keys.W);
        }
        public static bool IsScrollDown()
        {
            return IsKeyPressed(Keys.S);
        }
        public static bool IsZoomOut()
        {
            return IsNewKeyPress(Keys.OemPeriod);
        }
        public static bool IsZoomIn()
        {
            return IsNewKeyPress(Keys.OemComma);
        }

        private static bool IsKeyPressed(Keys key)
        {
            return kState.IsKeyDown(key);
        }

        private static bool IsNewKeyPress(Keys oem)
        {
            return kState.IsKeyDown(oem);
        }

        private static KeyboardState kState;


        public static void Update()
        {
            kState = Keyboard.GetState();
        }
    }
}
