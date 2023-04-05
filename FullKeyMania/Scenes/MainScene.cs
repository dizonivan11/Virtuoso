using FullKeyMania.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Forms.Controls;

namespace FullKeyMania.Scenes {
    public class MainScene : MonoGameControl {
        KeyboardState pks, cks;
        MouseState pms, cms;

        public Settings Setting { get; private set; }
        public Scene Scene { get; private set; }

        protected override void Initialize() {
            Setting = new Settings(@"settings.ini");
            Scene = new HomeScene(this);
        }

        public void ChangeScene(Scene newScene) {
            Scene = newScene;
        }

        protected override void Update(GameTime gameTime) {
            // Store Current Input States
            cks = Keyboard.GetState();
            cms = Mouse.GetState();

            // Update Current Scene
            Scene.Update(this, gameTime, pks, cks, pms, cms);

            // Store Input Previous States
            pks = cks;
            pms = cms;
        }

        protected override void Draw() {
            Editor.spriteBatch.GraphicsDevice.Clear(Color.DarkSlateBlue);
            Editor.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, null);

            // Render Current Scene
            Scene.Draw(this);

            Editor.spriteBatch.End();
        }
    }
}
