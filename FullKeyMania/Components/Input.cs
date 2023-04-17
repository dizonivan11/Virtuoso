using Microsoft.Xna.Framework.Input;

namespace FullKeyMania.Components {
    public static class Input {
        // x = trigger
        // p = pressed
        // r = released
        // h = hold
        // - = no input

        public static bool JustLeftClicked(InputState input) {
            // x hhhhhhhhhh r -------
            return input.PreviousMouseState.LeftButton == ButtonState.Released && input.CurrentMouseState.LeftButton == ButtonState.Pressed;
        }
        public static bool HoldLeftClicked(InputState input) {
            // p xxxxxxxxxx r -------
            return input.PreviousMouseState.LeftButton == ButtonState.Pressed && input.CurrentMouseState.LeftButton == ButtonState.Pressed;
        }
        public static bool LeftClicked(InputState input) {
            // p hhhhhhhhhh x -------
            return input.PreviousMouseState.LeftButton == ButtonState.Pressed && input.CurrentMouseState.LeftButton == ButtonState.Released;
        }

        public static bool JustRightClicked(InputState input) {
            // x hhhhhhhhhh r -------
            return input.PreviousMouseState.RightButton == ButtonState.Released && input.CurrentMouseState.RightButton == ButtonState.Pressed;
        }
        public static bool HoldRightClicked(InputState input) {
            // p xxxxxxxxxx r -------
            return input.PreviousMouseState.RightButton == ButtonState.Pressed && input.CurrentMouseState.RightButton == ButtonState.Pressed;
        }
        public static bool RightClicked(InputState input) {
            // p hhhhhhhhhh x -------
            return input.PreviousMouseState.RightButton == ButtonState.Pressed && input.CurrentMouseState.RightButton == ButtonState.Released;
        }

        public static bool JustMiddleClicked(InputState input) {
            // x hhhhhhhhhh r -------
            return input.PreviousMouseState.MiddleButton == ButtonState.Released && input.CurrentMouseState.MiddleButton == ButtonState.Pressed;
        }
        public static bool HoldMiddleClicked(InputState input) {
            // p xxxxxxxxxx r -------
            return input.PreviousMouseState.MiddleButton == ButtonState.Pressed && input.CurrentMouseState.MiddleButton == ButtonState.Pressed;
        }
        public static bool MiddleClicked(InputState input) {
            // p hhhhhhhhhh x -------
            return input.PreviousMouseState.MiddleButton == ButtonState.Pressed && input.CurrentMouseState.MiddleButton == ButtonState.Released;
        }

        public static bool JustKeyPressed(InputState input, Keys key) {
            // x hhhhhhhhhh r -------
            return input.PreviousKeyboardState.IsKeyUp(key) && input.CurrentKeyboardState.IsKeyDown(key);
        }
        public static bool HoldKeyPressed(InputState input, Keys key) {
            // p xxxxxxxxxx r -------
            return input.PreviousKeyboardState.IsKeyDown(key) && input.CurrentKeyboardState.IsKeyDown(key);
        }
        public static bool KeyPressed(InputState input, Keys key) {
            // p hhhhhhhhhh x -------
            return input.PreviousKeyboardState.IsKeyDown(key) && input.CurrentKeyboardState.IsKeyUp(key);
        }
    }
}
