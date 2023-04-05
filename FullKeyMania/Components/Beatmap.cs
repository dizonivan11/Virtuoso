using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Input;
using NAudio.Wave;

namespace FullKeyMania.Components {
    public class Beatmap {
        public static readonly string MUSIC_FILE_EXTENSION = ".mp3";

        public string DIR { get; private set; }
        public string Name { get; private set; }
        public int BPM { get; private set; } // Beats per Minute
        public int BeatValue { get; private set; }
        public int BeatCount { get; private set; }
        public double AR { get; private set; } // Appoach Rate (in ms; the time the notes will appear prior to their hit window)
        public double Offset { get; set; } // The start offset of the song (to fix the beat of a song with silence at the beginning)

        public AudioFileReader Song { get; private set; }
        public List<Note> Notes { get; private set; }

        public Beatmap(string beatmapPath) {
            DIR = beatmapPath;
            Name = Path.GetFileName(Path.GetDirectoryName(beatmapPath));
            Song = new AudioFileReader(beatmapPath + @"\" + Name + MUSIC_FILE_EXTENSION);
            Notes = new List<Note>();

            SettingsCollection settings = Settings.Parse(beatmapPath + @"\settings.ini");
            BPM = int.Parse(settings["bpm"]);
            BeatValue = int.Parse(settings["beatvalue"]);
            BeatCount = int.Parse(settings["beatcount"]);
            AR = double.Parse(settings["ar"]);
            Offset = double.Parse(settings["offset"]);

            StreamReader sr = new StreamReader(beatmapPath + @"\notes.ini");
            string[] notes = sr.ReadToEnd().Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            for (int n = 0; n < notes.Length; n++) {
                string[] note = notes[n].Split('=');
                Notes.Add(new Note(double.Parse(note[0]), ToKey(note[1])));
            }
        }

        public static Keys ToKey(string key) {
            switch (key) {
                case "q": return Keys.Q;
                case "w": return Keys.W;
                case "e": return Keys.E;
                case "r": return Keys.R;
                case "t": return Keys.T;
                case "y": return Keys.Y;
                case "u": return Keys.U;
                case "i": return Keys.I;
                case "o": return Keys.O;
                case "p": return Keys.P;
                case "[": return Keys.OemOpenBrackets;
                case "]": return Keys.OemCloseBrackets;
                case "a": return Keys.A;
                case "s": return Keys.S;
                case "d": return Keys.D;
                case "f": return Keys.F;
                case "g": return Keys.G;
                case "h": return Keys.H;
                case "j": return Keys.J;
                case "k": return Keys.K;
                case "l": return Keys.L;
                case ";": return Keys.OemSemicolon;
                case "'": return Keys.OemQuotes;
                case "z": return Keys.Z;
                case "x": return Keys.X;
                case "c": return Keys.C;
                case "v": return Keys.V;
                case "b": return Keys.B;
                case "n": return Keys.N;
                case "m": return Keys.M;
                case ",": return Keys.OemComma;
                case ".": return Keys.OemPeriod;
                case "/": return Keys.OemQuestion;
                default: return Keys.None;
            }
        }
    }
}
