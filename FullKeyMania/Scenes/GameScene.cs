using FullKeyMania.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        public GameScene(MainScene main) {
            idleColor = Color.FromNonPremultiplied(255, 255, 255, (int)(main.Setting.NoteOpacity * 255d));
            pressedColor = Color.White;

            for (int k = 0; k < keyGraphics.Length; k++) {
                using (FileStream fs = new FileStream(@"Skins\" + main.Setting.SelectedSkin + @"\note-" + k + ".png", FileMode.Open, FileAccess.Read)) {
                    keyGraphicColors[k] = idleColor;
                    keyGraphics[k] = Texture2D.FromStream(main.GraphicsDevice, fs);
                    fs.Close();
                }
            }
            approachCircle = Graphics.GetTexture2DFromFile(main.GraphicsDevice, @"Skins\" + main.Setting.SelectedSkin + @"\approach.png");

            Beatmap division = new Beatmap(@"C:\Users\dizon\source\repos\FullKeyMania\FullKeyMania\bin\Debug\Songs\Black Lotus Audio - Division\");
            conductor = new Conductor(division);
            background = Graphics.GetTexture2DFromFile(main.GraphicsDevice, conductor.Beatmap.DIR + "background.jpg");
        }

        internal override void Update(
            MainScene main,
            GameTime gameTime,
            KeyboardState previousKeyState,
            KeyboardState currentKeyState,
            MouseState previousMouseState,
            MouseState currentMouseState) {

            double arInSec = conductor.Beatmap.AR / 1000;
            int currentKeyIndex = 0;
            for (int c = 0; c < layerKeyCounts.Length; c++) {
                int keyCount = layerKeyCounts[c];

                for (int k = 0; k < keyCount; k++) {
                    if (currentKeyIndex < keyGraphics.Length) {
                        if (conductor.KeyTimingLayer[currentKeyIndex].Count > 0) {
                            double nextKeyTime = conductor.KeyTimingLayer[currentKeyIndex][0];
                            double endOpacityTime = nextKeyTime + arInSec;
                            double hitTime = Math.Abs(Math.Abs(nextKeyTime - conductor.SongPosition) + (main.Setting.GlobalOffset / 1000d));

                            Keys bind = Conductor.BINDS[currentKeyIndex];
                            if (hitTime < Conductor.HW_MISS && Input.JustKeyPressed(previousKeyState, currentKeyState, bind)) {
                                keyGraphicColors[currentKeyIndex] = pressedColor;
                                conductor.KeyTimingLayer[currentKeyIndex].RemoveAt(0);

                                currentHitTime = (int)(hitTime * 1000d);
                                if (hitTime <= Conductor.HW_EXACT) {
                                    conductor.HitCount[0]++;
                                    conductor.Score += Conductor.HWS_EXACT;

                                } else if (hitTime > Conductor.HW_EXACT && hitTime <= Conductor.HW_EXCELLENT) {
                                    conductor.HitCount[1]++;
                                    conductor.Score += Conductor.HWS_EXCELLENT;

                                } else if (hitTime > Conductor.HW_EXCELLENT && hitTime <= Conductor.HW_PERFECT) {
                                    conductor.HitCount[2]++;
                                    conductor.Score += Conductor.HWS_PERFECT;

                                } else if (hitTime > Conductor.HW_PERFECT && hitTime <= Conductor.HW_GOOD) {
                                    conductor.HitCount[3]++;
                                    conductor.Score += Conductor.HWS_GOOD;

                                } else if (hitTime > Conductor.HW_GOOD && hitTime <= Conductor.HW_BAD) {
                                    conductor.HitCount[4]++;
                                    conductor.Score += Conductor.HWS_BAD;

                                } else if (hitTime > Conductor.HW_BAD) {
                                    conductor.HitCount[5]++;
                                    conductor.Score += Conductor.HWS_MISS;
                                }
                            } else {
                                keyGraphicColors[currentKeyIndex] = idleColor;

                                if (conductor.SongPosition >= endOpacityTime) {
                                    conductor.HitCount[5]++;
                                    conductor.Score += Conductor.HWS_MISS;
                                    conductor.KeyTimingLayer[currentKeyIndex].RemoveAt(0);
                                }
                            }
                        }

                        if (currentKeyState.IsKeyDown(Conductor.BINDS[currentKeyIndex])) {
                            keyGraphicColors[currentKeyIndex] = pressedColor;
                        } else {
                            keyGraphicColors[currentKeyIndex] = idleColor;
                        }

                        currentKeyIndex++;
                    }
                }
            }
            conductor.Update(gameTime);
        }

        internal override void Draw(MainScene main) {
            // BACKGROUND SECTION
            main.Editor.spriteBatch.Draw(background,
                new Vector2(main.Editor.graphics.Viewport.Width / 2, main.Editor.graphics.Viewport.Height / 2),
                new Rectangle(0, 0, background.Width, background.Height),
                Color.FromNonPremultiplied(255, 255, 255, (int)(main.Setting.BackgroundOpacity * 255d)),
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
                        main.Editor.spriteBatch.Draw(keyGraphics[currentKeyIndex], new Vector2(x, y), keyGraphicColors[currentKeyIndex]);

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

                            main.Editor.spriteBatch.Draw(keyGraphics[currentKeyIndex], new Vector2(x, y), noteStateColor);

                            main.Editor.spriteBatch.Draw(
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
            // Editor.spriteBatch.DrawString(Editor.Font, "Game Time: " + conductor.GameTime.ToString(), new Vector2(x, y), Color.White);
            // y += (int)main.Editor.FontHeight + 5;
            // Editor.spriteBatch.DrawString(Editor.Font, "Current Time: " + conductor.CurrentTime.ToString(), new Vector2(x, y), Color.White);
            // y += (int)main.Editor.FontHeight + 5;
            // Editor.spriteBatch.DrawString(Editor.Font, "Time Started: " + conductor.TimeStarted.ToString(), new Vector2(x, y), Color.White);
            // y += (int)main.Editor.FontHeight + 5;
            main.Editor.spriteBatch.DrawString(main.Editor.Font, "Song Position:" + conductor.SongPosition.ToString(), new Vector2(x, y), Color.White);
            y += (int)main.Editor.FontHeight + 5;
            main.Editor.spriteBatch.DrawString(main.Editor.Font, "Current Beat:" + conductor.CurrentBeat.ToString(), new Vector2(x, y), Color.White);
            y += (int)main.Editor.FontHeight + 5;
            main.Editor.spriteBatch.DrawString(main.Editor.Font, "BPM: " + conductor.Beatmap.BPM.ToString(), new Vector2(x, y), Color.White);
            y += (int)main.Editor.FontHeight + 5;
            main.Editor.spriteBatch.DrawString(main.Editor.Font, "AR: " + conductor.Beatmap.AR.ToString(), new Vector2(x, y), Color.White);
            y += (int)main.Editor.FontHeight + 5;
            main.Editor.spriteBatch.DrawString(main.Editor.Font, "Offset: " + conductor.Beatmap.Offset.ToString(), new Vector2(x, y), Color.White);
            y += (int)main.Editor.FontHeight + 5;
            main.Editor.spriteBatch.DrawString(main.Editor.Font, "Hit Time: " + currentHitTime + "ms", new Vector2(x, y), Color.White);
            y += (int)main.Editor.FontHeight + 5;
            main.Editor.spriteBatch.DrawString(main.Editor.Font,
                string.Format(
                    "Score: {0} | Exact: {1} | Excellent: {2} | Perfect: {3} | Good: {4} | Bad: {5} | Missed: {6}",
                    conductor.Score, conductor.Exact, conductor.Excellent, conductor.Perfect, conductor.Good, conductor.Bad, conductor.Missed),
                new Vector2(x, y), Color.White);
        }
    }
}
