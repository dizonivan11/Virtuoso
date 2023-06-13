using FullKeyMania.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Forms.Controls;

namespace FullKeyMania.Components.GameObjects {
    internal class Button : GameObject {
        // BUTTON DELEGATES
        public delegate void OnButtonJustClicked(MouseButton mouseButton);
        public delegate void OnButtonHoldClicked(MouseButton mouseButton);
        public delegate void OnButtonClicked(MouseButton mouseButton);

        // BUTTON EVENTS
        public event OnButtonJustClicked ButtonJustClicked;
        public event OnButtonHoldClicked ButtonHoldClicked;
        public event OnButtonClicked ButtonClicked;

        // BUTTON STATES
        public static readonly int IDLE = 0;
        public static readonly int HOVER = 1;
        public static readonly int ACTIVE = 2;

        // BUTTON PROPERTIES
        public Texture2D[] Textures { get; private set; }
        public int State { get; private set; }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public float Scale { get; set; }
        private Vector2 finalScale;
        public Rectangle Bounds { get {
                int w = (int)(Textures[State].Width * finalScale.X);
                int h = (int)(Textures[State].Height * finalScale.Y);
                return new Rectangle((int)Position.X - (w / 2), (int)Position.Y - (h / 2), w, h);
            }
        }

        public Button(Texture2D idle, Texture2D hover, Texture2D active, float x, float y, float scale, float rotation = 0f) {
            Textures = new Texture2D[] { idle, hover, active };
            State = IDLE;
            Position = new Vector2(x, y);
            Rotation = rotation;
            Scale = scale;
        }

        internal override void Update(GameTime gameTime, InputState input, GraphicsDevice graphics) {
            int vw = graphics.Viewport.Width;
            int vh = graphics.Viewport.Height;

            if (vw > vh) {
                finalScale = new Vector2(vh / MainScene.REF_HEIGHT_SCALE * Scale);
            } else {
                finalScale = new Vector2(vw / MainScene.REF_WIDTH_SCALE * Scale);
            }

            if (Bounds.Contains(input.MousePosition)) {
                if (Input.JustLeftClicked(input)) ButtonJustClicked?.Invoke(MouseButton.Left);
                if (Input.JustRightClicked(input)) ButtonJustClicked?.Invoke(MouseButton.Right);
                if (Input.HoldLeftClicked(input)) ButtonHoldClicked?.Invoke(MouseButton.Left);
                if (Input.HoldRightClicked(input)) ButtonHoldClicked?.Invoke(MouseButton.Right);
                if (Input.LeftClicked(input)) ButtonClicked?.Invoke(MouseButton.Left);
                if (Input.RightClicked(input)) ButtonClicked?.Invoke(MouseButton.Right);

                if (input.CurrentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed) State = ACTIVE;
                else State = HOVER;
            } else State = IDLE;
        }

        internal override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(Textures[State], Position, Textures[State].Bounds,
                Color.White, Rotation, new Vector2(Textures[State].Width / 2, Textures[State].Height / 2), finalScale, SpriteEffects.None, 0f);
        }

        internal override void DrawDebug(SpriteBatch spriteBatch, Texture2D pixel) {
            Rectangle bounds = Bounds;
            spriteBatch.Draw(pixel, new Vector2(bounds.X, bounds.Y), new Rectangle(0, 0, bounds.Width, 1), Color.LimeGreen);  // top
            spriteBatch.Draw(pixel, new Vector2(bounds.X, bounds.Y), new Rectangle(0, 0, 1, bounds.Height), Color.LimeGreen);  // left
            spriteBatch.Draw(pixel, new Vector2(bounds.X, bounds.Y + bounds.Height), new Rectangle(0, 0, bounds.Width, 1), Color.LimeGreen);  // bottom
            spriteBatch.Draw(pixel, new Vector2(bounds.X + bounds.Width, bounds.Y), new Rectangle(0, 0, 1, bounds.Height), Color.LimeGreen); // right
        }
    }

    public enum MouseButton { Left, Right, Middle }
}
