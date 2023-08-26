using FullKeyMania.Components;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;

namespace FullKeyMania.Scenes {
    internal class SelectScene : Scene {
        public const int CARD_HEIGHT = 128;
        public const int CARD_PADDING = 8;

        readonly List<Beatmap> beatmaps;

        public SelectScene(MainScene main) : base(main) {
            beatmaps = new List<Beatmap>();
            LoadAllBeatmaps();
        }

        private void LoadAllBeatmaps() {
            beatmaps.Clear();
            string[] songDirs = Directory.GetDirectories("songs");
            foreach (string songDir in songDirs) {
                beatmaps.Add(new Beatmap(songDir));
            }
        }

        internal override void Update(GameTime gameTime, InputState input) {

        }

        internal override void Draw() {
            Vector2 cardPos = new Vector2(0, CARD_PADDING);
            foreach (var beatmap in beatmaps) {
                MainScene.Editor.spriteBatch.DrawString(MainScene.Editor.Font, beatmap.Name, cardPos, Color.White);
                cardPos.Y += CARD_HEIGHT + CARD_PADDING;
            }
        }
    }
}
