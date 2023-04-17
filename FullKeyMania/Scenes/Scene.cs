using FullKeyMania.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FullKeyMania.Scenes {
    public abstract class Scene {
        public MainScene MainScene { get; internal set; }

        public Scene(MainScene main) {
            MainScene = main;
        }

        internal abstract void Update(GameTime gameTime, InputState input);
        internal abstract void Draw();
    }
}
