using Microsoft.Xna.Framework;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace FullKeyMania.Components {
    public class Conductor {
        public static readonly int START_OFFSET = 2000; // 2 second fixed start offset
        public static readonly Keys[] BINDS = new Keys[] {
            Keys.Q, Keys.W, Keys.E, Keys.R, Keys.T, Keys.Y, Keys.U, Keys.I, Keys.O, Keys.P, Keys.OemOpenBrackets, Keys.OemCloseBrackets,
            Keys.A, Keys.S, Keys.D, Keys.F, Keys.G, Keys.H, Keys.J, Keys.K, Keys.L, Keys.OemSemicolon, Keys.OemQuotes,
            Keys.Z, Keys.X, Keys.C, Keys.V, Keys.B, Keys.N, Keys.M, Keys.OemComma, Keys.OemPeriod, Keys.OemQuestion
        };

        public static readonly double HW_EXACT = 0.016;
        public static readonly double HW_EXCELLENT = 0.032;
        public static readonly double HW_PERFECT = 0.064;
        public static readonly double HW_GOOD = 0.128;
        public static readonly double HW_BAD = 0.164;
        public static readonly double HW_MISS = 0.2;

        public static readonly int HWS_EXACT = 500;
        public static readonly int HWS_EXCELLENT = 400;
        public static readonly int HWS_PERFECT = 300;
        public static readonly int HWS_GOOD = 100;
        public static readonly int HWS_BAD = 50;
        public static readonly int HWS_MISS = 0;

        public WaveOutEvent OutputDevice { get; private set; }
        public Beatmap Beatmap { get; private set; }
        public List<double>[] KeyTimingLayer { get; private set; } // 2D layer for note timings
        public bool IsPlaying { get; private set; }
        public bool IsDone { get; private set; }

        // Session Records
        public int Score { get; internal set; }
        public int[] HitCount = new int[] {
            0, // EXACT
            0, // EXCELLENT
            0, // PERFECT
            0, // GOOD
            0, // BAD
            0 // MISS
        };
        public int Exact { get { return HitCount[0]; } internal set { HitCount[0] = value; } }
        public int Excellent { get { return HitCount[1]; } internal set { HitCount[1] = value; } }
        public int Perfect { get { return HitCount[2]; } internal set { HitCount[2] = value; } }
        public int Good { get { return HitCount[3]; } internal set { HitCount[3] = value; } }
        public int Bad { get { return HitCount[4]; } internal set { HitCount[4] = value; } }
        public int Missed { get { return HitCount[5]; } internal set { HitCount[5] = value; } }

        // Timing Variables
        public double GameTime { get; internal set; }
        public double TimeStarted { get; internal set; }
        public double CurrentTime { get; internal set; }
        public float GlobalOffset { get; private set; } // variable note offset from game settings
        public double SongPosition { get; private set; }
        public double CurrentBeat { get; private set; }
        double secondsPerBeat = 0d;
        // int lastBeat = 0;

        public Conductor(Beatmap beatmap) {
            Beatmap = beatmap;
            KeyTimingLayer = new List<double>[33];
            for (int k = 0; k < KeyTimingLayer.Length; k++) KeyTimingLayer[k] = new List<double>();
            MapNotesToLayer();
            secondsPerBeat = 60d / beatmap.BPM;

            OutputDevice = new WaveOutEvent();
            OutputDevice.PlaybackStopped += OnPlaybackStopped;
            OutputDevice.Init(beatmap.Song);
            IsPlaying = false;
            IsDone = false;
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs args) {
            IsDone = true;
            IsPlaying = false;
            OutputDevice.Dispose();
            Beatmap.Song.Dispose();
        }

        public void Start() {
            OutputDevice.Play();
            IsPlaying = true;
        }

        public void Update(GameTime gameTime) {
            GameTime = gameTime.TotalGameTime.TotalMilliseconds;

            if (TimeStarted == 0d) TimeStarted = GameTime;
            CurrentTime = GameTime - TimeStarted;

            if (!IsDone) {
                if (!IsPlaying) {
                    if (CurrentTime >= START_OFFSET) Start();
                    SongPosition = (CurrentTime - START_OFFSET) / 1000;
                } else {
                    // lastBeat = (int)CurrentBeat;
                    CurrentBeat = SongPosition / secondsPerBeat;
                    SongPosition = OutputDevice.GetPosition() * (1d / OutputDevice.OutputWaveFormat.AverageBytesPerSecond);
                }
            }
        }

        private void MapNotesToLayer() {
            for (int n = 0; n < Beatmap.Notes.Count; n++) {
                Note note = Beatmap.Notes[n];
                KeyTimingLayer[Array.IndexOf(BINDS, note.KeyAssigned)].Add(note.Time + (Beatmap.Offset * 0.001d));
            }
        }
    }
}
