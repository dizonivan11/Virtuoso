using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using MNote = Melanchall.DryWetMidi.Interaction.Note;
using Melanchall.DryWetMidi.MusicTheory;
using Microsoft.Xna.Framework.Input;
using NAudio.Wave;

namespace FullKeyMania.Components {
    public class Beatmap {
        public static readonly string MUSIC_FILE_EXTENSION = ".mp3";
        public static readonly string MIDI_FILE_EXTENSION = ".mid";

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

            // Note timing processing
            MidiFile midiFile = MidiFile.Read(beatmapPath + @"\" + Name + MIDI_FILE_EXTENSION);
            var notes = midiFile.GetNotes().ToArray();
            for (int n = 0; n < notes.Length; n++) {
                MNote note = notes[n];
                MetricTimeSpan mts = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, midiFile.GetTempoMap());
                Notes.Add(new Note(mts.Minutes * 60d + mts.Seconds + mts.Milliseconds / 1000d, ToKey(note)));
            }
        }

        public static Keys ToKey(MNote note) {
            int offset;
            switch (note.NoteName) {
                case NoteName.C:
                    offset = 0; break;
                case NoteName.CSharp:
                    offset = 1; break;
                case NoteName.D:
                    offset = 2; break;
                case NoteName.DSharp:
                    offset = 3; break;
                case NoteName.E:
                    offset = 4; break;
                case NoteName.F:
                    offset = 5; break;
                case NoteName.FSharp:
                    offset = 6; break;
                case NoteName.G:
                    offset = 7; break;
                case NoteName.GSharp:
                    offset = 8; break;
                case NoteName.A:
                    offset = 9; break;
                case NoteName.ASharp:
                    offset = 10; break;
                case NoteName.B:
                    offset = 11; break;
                default:
                    offset = 0; break;
            }

            int noteValue = offset + ((note.Octave - 1) * 12) - 12;

            switch (noteValue) {
                case -3: case 13: case 29: return Keys.Q;
                case -2: case 14: case 30: return Keys.W;
                case -1: case 15: case 31: return Keys.E;
                case 0: case 16: case 32: return Keys.R;
                case 1: case 17: case 33: return Keys.T;
                case 2: case 18: case 34: return Keys.Y;

                case 3: case 19: case 35: return Keys.A;
                case 4: case 20: case 36: return Keys.S;
                case 5: case 21: case 37: return Keys.D;
                case 6: case 22: case 38: return Keys.F;
                case 7: case 23: case 39: return Keys.G;

                case 8: case 24: case 40: return Keys.Z;
                case 9: case 25: return Keys.X;
                case 10: case 26: return Keys.C;
                case 11: case 27: return Keys.V;
                case 12: case 28: return Keys.B;

                case 41: case 58: case 75: return Keys.U;
                case 42: case 59: case 76: return Keys.I;
                case 43: case 60: case 77: return Keys.O;
                case 44: case 61: case 78: return Keys.P;
                case 45: case 62: case 79: return Keys.OemOpenBrackets;
                case 46: case 63: case 80: return Keys.OemCloseBrackets;

                case 47: case 64: case 81: return Keys.H;
                case 48: case 65: case 82: return Keys.J;
                case 49: case 66: case 83: return Keys.K;
                case 50: case 67: case 84: return Keys.L;
                case 51: case 68: return Keys.OemSemicolon;
                case 52: case 69: return Keys.OemQuotes;

                case 53: case 70: return Keys.N;
                case 54: case 71: return Keys.M;
                case 55: case 72: return Keys.OemComma;
                case 56: case 73: return Keys.OemPeriod;
                case 57: case 74: return Keys.OemQuestion;

                default: return Keys.Q;
            }
        }
    }
}
