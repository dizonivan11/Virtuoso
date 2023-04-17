using FullKeyMania.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FullKeyMania.Scenes {
    internal class HomeScene : Scene {
        Texture2D background;
        Texture2D logo;
        Texture2D home_solo_play;
        Texture2D home_multi_play;
        Texture2D home_quit;

        public HomeScene(MainScene main) : base(main) {
            background = Graphics.GetTexture2DFromFile(main.GraphicsDevice, @"Skins\" + main.Settings.SelectedSkin + @"\home_background.jpg");
            logo = Graphics.GetTexture2DFromFile(main.GraphicsDevice, @"Skins\" + main.Settings.SelectedSkin + @"\game_logo.png");
            home_solo_play = Graphics.GetTexture2DFromFile(main.GraphicsDevice, @"Skins\" + main.Settings.SelectedSkin + @"\home_solo_play.png");
            home_multi_play = Graphics.GetTexture2DFromFile(main.GraphicsDevice, @"Skins\" + main.Settings.SelectedSkin + @"\home_multi_play.png");
            home_quit = Graphics.GetTexture2DFromFile(main.GraphicsDevice, @"Skins\" + main.Settings.SelectedSkin + @"\home_quit.png");
        }

        internal override void Update(GameTime gameTime, InputState input) {
            
            if (Input.LeftClicked(input)) {
                MainScene.ChangeScene(new GameScene(MainScene));
            }
        }

        internal override void Draw() {
            // BACKGROUND SECTION
            MainScene.Editor.spriteBatch.Draw(background,
                new Vector2(MainScene.Editor.graphics.Viewport.Width / 2, MainScene.Editor.graphics.Viewport.Height / 2),
                new Rectangle(0, 0, background.Width, background.Height),
                Color.FromNonPremultiplied(255, 255, 255, 128),
                0f, new Vector2(background.Width / 2, background.Height / 2), 1f, SpriteEffects.None, 0f);

            // LOGO SECTION
            MainScene.Editor.spriteBatch.Draw(logo,
                new Vector2(MainScene.Editor.graphics.Viewport.Width / 2, MainScene.Editor.graphics.Viewport.Height * 0.25f),
                new Rectangle(0, 0, logo.Width, logo.Height),
                Color.White,
                0f, new Vector2(logo.Width / 2, logo.Height / 2), 0.4f, SpriteEffects.None, 0f);

            // SOLO PLAY SECTION
            MainScene.Editor.spriteBatch.Draw(home_solo_play,
                new Vector2(MainScene.Editor.graphics.Viewport.Width / 2, MainScene.Editor.graphics.Viewport.Height * 0.5f),
                new Rectangle(0, 0, home_solo_play.Width, home_solo_play.Height),
                Color.White,
                0f, new Vector2(home_solo_play.Width / 2, home_solo_play.Height / 2), 0.25f, SpriteEffects.None, 0f);

            // MULTI PLAY SECTION
            MainScene.Editor.spriteBatch.Draw(home_multi_play,
                new Vector2(MainScene.Editor.graphics.Viewport.Width / 2, MainScene.Editor.graphics.Viewport.Height * 0.65f),
                new Rectangle(0, 0, home_multi_play.Width, home_multi_play.Height),
                Color.White,
                0f, new Vector2(home_multi_play.Width / 2, home_multi_play.Height / 2), 0.25f, SpriteEffects.None, 0f);

            // QUIT SECTION
            MainScene.Editor.spriteBatch.Draw(home_quit,
                new Vector2(MainScene.Editor.graphics.Viewport.Width / 2, MainScene.Editor.graphics.Viewport.Height * 0.80f),
                new Rectangle(0, 0, home_quit.Width, home_quit.Height),
                Color.White,
                0f, new Vector2(home_quit.Width / 2, home_quit.Height / 2), 0.25f, SpriteEffects.None, 0f);
        }
    }
}
