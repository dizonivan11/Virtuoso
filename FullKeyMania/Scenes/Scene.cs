using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FullKeyMania.Scenes {
    public abstract class Scene {

        internal abstract void Update(
            MainScene main,
            GameTime gameTime,
            KeyboardState previousKeyState,
            KeyboardState currentKeyState,
            MouseState previousMouseState,
            MouseState currentMouseState);

        internal abstract void Draw(MainScene main);
    }
}
