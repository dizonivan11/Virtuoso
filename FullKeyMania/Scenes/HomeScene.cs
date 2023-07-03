using FullKeyMania.Components;
using FullKeyMania.Components.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;
using Button = FullKeyMania.Components.GameObjects.Button;

namespace FullKeyMania.Scenes {
    internal class HomeScene : Scene {
        Texture2D pixel;
        Texture2D background;
        Texture2D logo;
        Button home_solo_play;
        Button home_multi_play;
        Button home_quit;

        public HomeScene(MainScene main) : base(main) {
            pixel = Graphics.GetTexture2DFromFile(main.GraphicsDevice, @"Skins\" + main.Settings.SelectedSkin + @"\pixel.png");
            background = Graphics.GetTexture2DFromFile(main.GraphicsDevice, @"Skins\" + main.Settings.SelectedSkin + @"\home_background.jpg");
            logo = Graphics.GetTexture2DFromFile(main.GraphicsDevice, @"Skins\" + main.Settings.SelectedSkin + @"\game_logo.png");
            Texture2D home_solo_play_idle = Graphics.GetTexture2DFromFile(main.GraphicsDevice, @"Skins\" + main.Settings.SelectedSkin + @"\home_solo_play_idle.png");
            Texture2D home_solo_play_hover = Graphics.GetTexture2DFromFile(main.GraphicsDevice, @"Skins\" + main.Settings.SelectedSkin + @"\home_solo_play_hover.png");
            Texture2D home_solo_play_active = Graphics.GetTexture2DFromFile(main.GraphicsDevice, @"Skins\" + main.Settings.SelectedSkin + @"\home_solo_play_active.png");
            Texture2D home_multi_play_idle = Graphics.GetTexture2DFromFile(main.GraphicsDevice, @"Skins\" + main.Settings.SelectedSkin + @"\home_multi_play_idle.png");
            Texture2D home_multi_play_hover = Graphics.GetTexture2DFromFile(main.GraphicsDevice, @"Skins\" + main.Settings.SelectedSkin + @"\home_multi_play_hover.png");
            Texture2D home_multi_play_active = Graphics.GetTexture2DFromFile(main.GraphicsDevice, @"Skins\" + main.Settings.SelectedSkin + @"\home_multi_play_active.png");
            Texture2D home_quit_idle = Graphics.GetTexture2DFromFile(main.GraphicsDevice, @"Skins\" + main.Settings.SelectedSkin + @"\home_quit_idle.png");
            Texture2D home_quit_hover = Graphics.GetTexture2DFromFile(main.GraphicsDevice, @"Skins\" + main.Settings.SelectedSkin + @"\home_quit_hover.png");
            Texture2D home_quit_active = Graphics.GetTexture2DFromFile(main.GraphicsDevice, @"Skins\" + main.Settings.SelectedSkin + @"\home_quit_active.png");

            home_solo_play = new Button(
                home_solo_play_idle,
                home_solo_play_hover,
                home_solo_play_active,
                main.GraphicsDevice.Viewport.Width / 2,
                main.GraphicsDevice.Viewport.Height * 0.5f,
                0.25f);
            home_solo_play.ButtonClicked += StartSoloPlay;

            home_multi_play = new Button(
                home_multi_play_idle,
                home_multi_play_hover,
                home_multi_play_active,
                main.GraphicsDevice.Viewport.Width / 2,
                main.GraphicsDevice.Viewport.Height * 0.65f,
                0.25f);
            home_multi_play.ButtonClicked += StartMultiPlay;

            home_quit = new Button(
                home_quit_idle,
                home_quit_hover,
                home_quit_active,
                main.GraphicsDevice.Viewport.Width / 2,
                main.GraphicsDevice.Viewport.Height * 0.8f,
                0.25f);
            home_quit.ButtonClicked += QuitGame;

            main.Invalidated += RepositionGameObjects;
            main.SizeChanged += RepositionGameObjects;
        }

        private void RepositionGameObjects(object sender, System.EventArgs e) {
            home_solo_play.Position = new Vector2(MainScene.GraphicsDevice.Viewport.Width / 2, MainScene.GraphicsDevice.Viewport.Height * 0.5f);
            home_multi_play.Position = new Vector2(MainScene.GraphicsDevice.Viewport.Width / 2, MainScene.GraphicsDevice.Viewport.Height * 0.65f);
            home_quit.Position = new Vector2(MainScene.GraphicsDevice.Viewport.Width / 2, MainScene.GraphicsDevice.Viewport.Height * 0.8f);
        }

        private void StartSoloPlay(MouseButton mouseButton) {
            if (mouseButton == MouseButton.Left)
                MainScene.ChangeScene(new GameScene(MainScene));
        }

        private void StartMultiPlay(MouseButton mouseButton) {
            // TODO....
        }

        private void QuitGame(MouseButton mouseButton) {
            if (mouseButton == MouseButton.Left)
                Application.Exit();
        }

        internal override void Update(GameTime gameTime, InputState input) {
            home_solo_play.Update(gameTime, input, MainScene.GraphicsDevice);
            home_multi_play.Update(gameTime, input, MainScene.GraphicsDevice);
            home_quit.Update(gameTime, input, MainScene.GraphicsDevice);
        }

        internal override void Draw() {
            // BACKGROUND SECTION
            MainScene.Editor.spriteBatch.Draw(background,
                new Vector2(MainScene.Editor.graphics.Viewport.Width / 2, MainScene.Editor.graphics.Viewport.Height / 2),
                new Rectangle(0, 0, background.Width, background.Height),
                Color.White, 0f, new Vector2(background.Width / 2, background.Height / 2), 1f, SpriteEffects.None, 0f);

            // LOGO SECTION
            MainScene.Editor.spriteBatch.Draw(logo,
                new Vector2(MainScene.Editor.graphics.Viewport.Width / 2, MainScene.Editor.graphics.Viewport.Height * 0.25f),
                new Rectangle(0, 0, logo.Width, logo.Height),
                Color.White,
                0f, new Vector2(logo.Width / 2, logo.Height / 2), new Vector2(MainScene.GraphicsDevice.Viewport.Width / MainScene.REF_WIDTH_SCALE * .4f, MainScene.GraphicsDevice.Viewport.Height / MainScene.REF_HEIGHT_SCALE * .4f), SpriteEffects.None, 0f);

            // SOLO PLAY SECTION
            home_solo_play.Draw(MainScene.Editor.spriteBatch);
            home_solo_play.DrawDebug(MainScene.Editor.spriteBatch, pixel);

            // MULTI PLAY SECTION
            home_multi_play.Draw(MainScene.Editor.spriteBatch);
            home_multi_play.DrawDebug(MainScene.Editor.spriteBatch, pixel);

            // QUIT SECTION
            home_quit.Draw(MainScene.Editor.spriteBatch);
            home_quit.DrawDebug(MainScene.Editor.spriteBatch, pixel);
        }
    }
}
