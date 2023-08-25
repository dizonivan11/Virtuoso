using FullKeyMania.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.IO;

namespace FullKeyMania.Scenes {
    public class GameScene : Scene {
        Conductor conductor;
        int currentHitTime;

        Texture2D[] keyGraphics = new Texture2D[33];
        Texture2D background;
        Texture2D approachCircle;
        Color[] keyGraphicColors = new Color[33];
        Color idleColor, pressedColor;
        int keyPadding = 12;
        int[] layerKeyCounts = new int[] { 12, 11, 10 };

        public GameScene(MainScene main) : base(main) {
            idleColor = Color.FromNonPremultiplied(255, 255, 255, (int)(main.Settings.NoteOpacity * 255d));
            pressedColor = Color.White;

            for (int k = 0; k < keyGraphics.Length; k++) {
                using (FileStream fs = new FileStream(@"Skins\" + main.Settings.SelectedSkin + @"\note-" + k + ".png", FileMode.Open, FileAccess.Read)) {
                    keyGraphicColors[k] = idleColor;
                    keyGraphics[k] = Texture2D.FromStream(main.GraphicsDevice, fs);
                    fs.Close();
                }
            }
            approachCircle = Graphics.GetTexture2DFromFile(main.GraphicsDevice, @"Skins\" + main.Settings.SelectedSkin + @"\approach.png");

            // Beatmap division = new Beatmap(@"Songs\Black Lotus Audio - Division\");
            Beatmap division = new Beatmap(@"Songs\Sereno - World's End Waltz\");
            conductor = new Conductor(division);
            background = Graphics.GetTexture2DFromFile(main.GraphicsDevice, conductor.Beatmap.DIR + "background.jpg");
            conductor.OutputDevice.PlaybackStopped += OutputDevice_PlaybackStopped;
        }

        private void OutputDevice_PlaybackStopped(object sender, StoppedEventArgs e) {
            MainScene.ChangeScene(new HomeScene(MainScene));
        }

        internal override void Update(GameTime gameTime, InputState input) {

            double arInSec = conductor.Beatmap.AR / 1000d;
            int currentKeyIndex = 0;
            for (int c = 0; c < layerKeyCounts.Length; c++) {
                int keyCount = layerKeyCounts[c];

                for (int k = 0; k < keyCount; k++) {
                    if (currentKeyIndex < keyGraphics.Length) {
                        if (conductor.KeyTimingLayer[currentKeyIndex].Count > 0) {
                            double nextKeyTime = conductor.KeyTimingLayer[currentKeyIndex][0];
                            double endOpacityTime = nextKeyTime + arInSec;
                            double hitTime = Math.Abs(Math.Abs(nextKeyTime - conductor.SongPosition) + (MainScene.Settings.GlobalOffset / 1000d));

                            // Full Combo Bot
                            if (hitTime < Conductor.HW_EXCELLENT && hitTime <= Conductor.HWS_EXACT) {
                                conductor.KeyTimingLayer[currentKeyIndex].RemoveAt(0);

                                conductor.HitCount[0]++;
                                conductor.CurrentCombo++;
                                if (conductor.CurrentCombo > conductor.HighestCombo) conductor.HighestCombo++;
                                conductor.Score += Conductor.HWS_EXACT + conductor.CurrentCombo;

                                WaveOutEvent hitSound = new WaveOutEvent();
                                hitSound.Init(new AudioFileReader(MainScene.Settings.HitSoundPath));
                                hitSound.Play();
                                hitSound.PlaybackStopped += delegate { hitSound.Dispose(); };
                            }

                            Keys bind = Conductor.BINDS[currentKeyIndex];
                            if (hitTime < Conductor.HW_MISS && Input.JustKeyPressed(input, bind)) {
                                keyGraphicColors[currentKeyIndex] = pressedColor;
                                conductor.KeyTimingLayer[currentKeyIndex].RemoveAt(0);

                                currentHitTime = (int)(hitTime * 1000d);
                                if (hitTime <= Conductor.HW_EXACT) {
                                    conductor.HitCount[0]++;
                                    conductor.CurrentCombo++;
                                    if (conductor.CurrentCombo > conductor.HighestCombo) conductor.HighestCombo++;
                                    conductor.Score += Conductor.HWS_EXACT + conductor.CurrentCombo;

                                } else if (hitTime > Conductor.HW_EXACT && hitTime <= Conductor.HW_EXCELLENT) {
                                    conductor.HitCount[1]++;
                                    conductor.CurrentCombo++;
                                    if (conductor.CurrentCombo > conductor.HighestCombo) conductor.HighestCombo++;
                                    conductor.Score += Conductor.HWS_EXCELLENT + (conductor.CurrentCombo / 2);

                                } else if (hitTime > Conductor.HW_EXCELLENT && hitTime <= Conductor.HW_PERFECT) {
                                    conductor.HitCount[2]++;
                                    conductor.CurrentCombo++;
                                    if (conductor.CurrentCombo > conductor.HighestCombo) conductor.HighestCombo++;
                                    conductor.Score += Conductor.HWS_PERFECT + (conductor.CurrentCombo / 6);

                                } else if (hitTime > Conductor.HW_PERFECT && hitTime <= Conductor.HW_GOOD) {
                                    conductor.HitCount[3]++;
                                    conductor.CurrentCombo++;
                                    if (conductor.CurrentCombo > conductor.HighestCombo) conductor.HighestCombo++;
                                    conductor.Score += Conductor.HWS_GOOD + (conductor.CurrentCombo / 10);

                                } else if (hitTime > Conductor.HW_GOOD && hitTime <= Conductor.HW_BAD) {
                                    conductor.HitCount[4]++;
                                    conductor.CurrentCombo = 0;
                                    conductor.Score += Conductor.HWS_BAD;
  
                                } else if (hitTime > Conductor.HW_BAD) {
                                    conductor.HitCount[5]++;
                                    conductor.CurrentCombo = 0;
                                    conductor.Score += Conductor.HWS_MISS;
                                }

                                WaveOutEvent hitSound = new WaveOutEvent();
                                hitSound.Init(new AudioFileReader(MainScene.Settings.HitSoundPath));
                                hitSound.Play();
                                hitSound.PlaybackStopped += delegate { hitSound.Dispose(); };
                            } else {
                                keyGraphicColors[currentKeyIndex] = idleColor;

                                if (conductor.SongPosition >= endOpacityTime) {
                                    conductor.HitCount[5]++;
                                    conductor.CurrentCombo = 0;
                                    conductor.Score += Conductor.HWS_MISS;
                                    conductor.KeyTimingLayer[currentKeyIndex].RemoveAt(0);
                                }
                            }
                        }

                        if (input.CurrentKeyboardState.IsKeyDown(Conductor.BINDS[currentKeyIndex])) {
                            keyGraphicColors[currentKeyIndex] = pressedColor;
                        } else {
                            keyGraphicColors[currentKeyIndex] = idleColor;
                        }

                        currentKeyIndex++;
                    }
                }
            }
            conductor.Update(gameTime);

            if (Input.KeyPressed(input, Keys.Escape)) {
                conductor.OutputDevice.Stop();
            }
        }

        internal override void Draw() {
            // BACKGROUND SECTION
            MainScene.Editor.spriteBatch.Draw(background,
                new Vector2(MainScene.Editor.graphics.Viewport.Width / 2, MainScene.Editor.graphics.Viewport.Height / 2),
                new Rectangle(0, 0, background.Width, background.Height),
                Color.FromNonPremultiplied(255, 255, 255, (int)(MainScene.Settings.BackgroundOpacity * 255d)),
                0f, new Vector2(background.Width / 2, background.Height / 2), 1f, SpriteEffects.None, 0f);

            // NOTE SECTION
            double arInSec = conductor.Beatmap.AR / 1000;
            int currentKeyIndex = 0;
            int currentLayerIndex = 0;
            int x = 12;
            int y = 250;
            for (int c = 0; c < layerKeyCounts.Length; c++) {
                int keyCount = layerKeyCounts[c];

                for (int k = 0; k < keyCount; k++) {
                    if (currentKeyIndex < keyGraphics.Length) {
                        MainScene.Editor.spriteBatch.Draw(keyGraphics[currentKeyIndex], new Vector2(x, y), keyGraphicColors[currentKeyIndex]);

                        // If there's still notes left on this key layer
                        if (conductor.KeyTimingLayer[currentKeyIndex].Count > 0) {
                            double nextKeyTime = conductor.KeyTimingLayer[currentKeyIndex][0];
                            double startOpacityTime = nextKeyTime - arInSec;
                            double endOpacityTime = nextKeyTime + arInSec;
                            Color noteStateColor;
                            Color approachStateColor;
                            double opacity = 0;

                            // Note Fade In
                            if (conductor.SongPosition <= nextKeyTime) {
                                opacity = (conductor.SongPosition - startOpacityTime) / arInSec;
                                int opacityColor = (int)(opacity * 255d);
                                noteStateColor = Color.FromNonPremultiplied(255, 255, 255, opacityColor);
                                approachStateColor = noteStateColor;

                            // Note Fade Out
                            } else if (conductor.SongPosition > nextKeyTime && conductor.SongPosition < endOpacityTime) {
                                opacity = Math.Abs((conductor.SongPosition - endOpacityTime) / arInSec);
                                int opacityColor = (int)(opacity * 255d);
                                noteStateColor = Color.FromNonPremultiplied(255, opacityColor, opacityColor, opacityColor);
                                approachStateColor = Color.FromNonPremultiplied(opacityColor, opacityColor, opacityColor, opacityColor);

                            // Missed Note Removal
                            } else {
                                noteStateColor = Color.Transparent;
                                approachStateColor = Color.Transparent;
                            }

                            MainScene.Editor.spriteBatch.Draw(keyGraphics[currentKeyIndex], new Vector2(x, y), noteStateColor);

                            MainScene.Editor.spriteBatch.Draw(
                                approachCircle,
                                new Vector2(x + 32, y + 32),
                                new Rectangle(0, 0, 256, 256),
                                approachStateColor, 0f, new Vector2(128, 128), 1.25f - (float)opacity, SpriteEffects.None, 0f);

                            // Editor.spriteBatch.DrawString(Editor.Font, "T: " + opacity, new Vector2(x, y), Color.White);
                        }

                        x += keyGraphics[currentKeyIndex].Width + keyPadding;
                        currentKeyIndex++;
                    }
                }
                x = 12 + (32 * ++currentLayerIndex);
                y += 64 + keyPadding;
            }

            x = 5;
            y = 5;
            MainScene.Editor.spriteBatch.DrawString(MainScene.Editor.Font, "Song Position:" + conductor.SongPosition.ToString(), new Vector2(x, y), Color.White);
            y = UINewLine(y);
            MainScene.Editor.spriteBatch.DrawString(MainScene.Editor.Font, "Current Beat:" + conductor.CurrentBeat.ToString(), new Vector2(x, y), Color.White);
            y = UINewLine(y);
            MainScene.Editor.spriteBatch.DrawString(MainScene.Editor.Font, "BPM: " + conductor.Beatmap.BPM.ToString(), new Vector2(x, y), Color.White);
            y = UINewLine(y);
            MainScene.Editor.spriteBatch.DrawString(MainScene.Editor.Font, "AR: " + conductor.Beatmap.AR.ToString(), new Vector2(x, y), Color.White);
            y = UINewLine(y);
            MainScene.Editor.spriteBatch.DrawString(MainScene.Editor.Font, "Global Offset: " + MainScene.Settings.GlobalOffset.ToString(), new Vector2(x, y), Color.White);
            y = UINewLine(y);
            MainScene.Editor.spriteBatch.DrawString(MainScene.Editor.Font, "Beatmap Offset: " + conductor.Beatmap.Offset.ToString(), new Vector2(x, y), Color.White);
            y = UINewLine(y);
            MainScene.Editor.spriteBatch.DrawString(MainScene.Editor.Font, "Hit Time: " + currentHitTime + "ms", new Vector2(x, y), Color.White);
            y = UINewLine(y);
            MainScene.Editor.spriteBatch.DrawString(MainScene.Editor.Font,
                string.Format(
                    "Score: {0} | Current Combo: {1} | Highest Combo: {2} | Exact: {3} | Excellent: {4} | Perfect: {5} | Good: {6} | Bad: {7} | Missed: {8}",
                    conductor.Score,
                    conductor.CurrentCombo,
                    conductor.HighestCombo,
                    conductor.Exact,
                    conductor.Excellent,
                    conductor.Perfect,
                    conductor.Good,
                    conductor.Bad,
                    conductor.Missed),
                new Vector2(x, y), Color.White);
        }

        int UINewLine(int y) { return y + (int)MainScene.Editor.FontHeight + 5; }
    }
}
