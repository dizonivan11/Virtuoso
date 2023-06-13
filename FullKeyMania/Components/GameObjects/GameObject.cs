using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Forms.Controls;

namespace FullKeyMania.Components.GameObjects {
    public abstract class GameObject {
        internal abstract void Update(GameTime gameTime, InputState input, GraphicsDevice graphics);
        internal abstract void Draw(SpriteBatch spriteBatch);
        internal virtual void DrawDebug(SpriteBatch spriteBatch, Texture2D pixel) { }
    }
}
