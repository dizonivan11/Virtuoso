using Microsoft.Xna.Framework.Input;

namespace FullKeyMania.Components {
    public static class Input {
        // x = trigger
        // p = pressed
        // r = released
        // h = hold
        // - = no input

        public static bool JustLeftClicked(MouseState previous, MouseState current) {
            // x hhhhhhhhhh r -------
            return previous.LeftButton == ButtonState.Released && current.LeftButton == ButtonState.Pressed;
        }
        public static bool HoldLeftClicked(MouseState previous, MouseState current) {
            // p xxxxxxxxxx r -------
            return previous.LeftButton == ButtonState.Pressed && current.LeftButton == ButtonState.Pressed;
        }
        public static bool LeftClicked(MouseState previous, MouseState current) {
            // p hhhhhhhhhh x -------
            return previous.LeftButton == ButtonState.Pressed && current.LeftButton == ButtonState.Released;
        }

        public static bool JustRightClicked(MouseState previous, MouseState current) {
            // x hhhhhhhhhh r -------
            return previous.RightButton == ButtonState.Released && current.RightButton == ButtonState.Pressed;
        }
        public static bool HoldRightClicked(MouseState previous, MouseState current) {
            // p xxxxxxxxxx r -------
            return previous.RightButton == ButtonState.Pressed && current.RightButton == ButtonState.Pressed;
        }
        public static bool RightClicked(MouseState previous, MouseState current) {
            // p hhhhhhhhhh x -------
            return previous.RightButton == ButtonState.Pressed && current.RightButton == ButtonState.Released;
        }

        public static bool JustMiddleClicked(MouseState previous, MouseState current) {
            // x hhhhhhhhhh r -------
            return previous.MiddleButton == ButtonState.Released && current.MiddleButton == ButtonState.Pressed;
        }
        public static bool HoldMiddleClicked(MouseState previous, MouseState current) {
            // p xxxxxxxxxx r -------
            return previous.MiddleButton == ButtonState.Pressed && current.MiddleButton == ButtonState.Pressed;
        }
        public static bool MiddleClicked(MouseState previous, MouseState current) {
            // p hhhhhhhhhh x -------
            return previous.MiddleButton == ButtonState.Pressed && current.MiddleButton == ButtonState.Released;
        }

        public static bool JustKeyPressed(KeyboardState previous, KeyboardState current, Keys key) {
            // x hhhhhhhhhh r -------
            return previous.IsKeyUp(key) && current.IsKeyDown(key);
        }
        public static bool HoldKeyPressed(KeyboardState previous, KeyboardState current, Keys key) {
            // p xxxxxxxxxx r -------
            return previous.IsKeyDown(key) && current.IsKeyDown(key);
        }
        public static bool KeyPressed(KeyboardState previous, KeyboardState current, Keys key) {
            // p hhhhhhhhhh x -------
            return previous.IsKeyDown(key) && current.IsKeyUp(key);
        }
    }
}
