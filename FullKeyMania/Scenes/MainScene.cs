using FullKeyMania.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Forms.Controls;

namespace FullKeyMania.Scenes {
    public class MainScene : MonoGameControl {
        public static readonly float REF_WIDTH_SCALE = 800;
        public static readonly float REF_HEIGHT_SCALE = 600;

        InputState GameInput;

        public Settings Settings { get; private set; }
        public Scene Scene { get; private set; }

        protected override void Initialize() {
            base.Initialize();
            GraphicsProfile = GraphicsProfile.HiDef;
            Editor.ShowFPS = true;

            GameInput = new InputState();
            Settings = new Settings(@"settings.ini");
            Scene = new HomeScene(this);
        }

        public void ChangeScene(Scene newScene) {
            Scene = newScene;
        }

        protected override void Update(GameTime gameTime) {
            base.Update(gameTime);

            // Store Current Input States
            GameInput.UpdateCurrentStates(Keyboard.GetState(), Mouse.GetState());

            // Update Current Scene
            Scene.Update(gameTime, GameInput);

            // Store Previous Input States
            GameInput.UpdatePreviousStates();
        }

        protected override void Draw() {
            base.Draw();

            Editor.spriteBatch.GraphicsDevice.Clear(Color.DarkSlateBlue);
            Editor.spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.NonPremultiplied,
                SamplerState.AnisotropicClamp,
                DepthStencilState.Default,
                RasterizerState.CullCounterClockwise, null, null);

            // Render Current Scene
            Scene.Draw();

            Editor.spriteBatch.End();
        }
    }
}
