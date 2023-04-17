using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FullKeyMania.Components.GameObjects {
    public abstract class GameObject {
        internal abstract void Update(GameTime gameTime, InputState input);
        internal abstract void Draw(SpriteBatch spriteBatch);
    }
}
