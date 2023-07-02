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
                    offset = 3; break;
                case NoteName.CSharp:
                    offset = 4; break;
                case NoteName.D:
                    offset = 5; break;
                case NoteName.DSharp:
                    offset = 6; break;
                case NoteName.E:
                    offset = 7; break;
                case NoteName.F:
                    offset = 8; break;
                case NoteName.FSharp:
                    offset = 9; break;
                case NoteName.G:
                    offset = 10; break;
                case NoteName.GSharp:
                    offset = 11; break;
                case NoteName.A:
                    offset = 0; break;
                case NoteName.ASharp:
                    offset = 1; break;
                case NoteName.B:
                    offset = 2; break;
                default:
                    offset = 0; break;
            }

            int noteValue = note.Octave * 12 + offset - 3;

            switch (noteValue) {
                case 0: case 16: return Keys.Q;
                case 1: case 17: return Keys.W;
                case 2: case 18: return Keys.E;
                case 3: case 19: return Keys.R;
                case 4: case 20: return Keys.T;
                case 5: case 21: return Keys.Y;

                case 32: case 49: return Keys.U;
                case 33: case 50: return Keys.I;
                case 34: case 51: return Keys.O;
                case 35: case 52: return Keys.P;
                case 36: case 53: return Keys.OemOpenBrackets;
                case 37: case 54: case 66: return Keys.OemCloseBrackets;

                case 6: case 22: case 67: return Keys.A;
                case 7: case 23: case 68: return Keys.S;
                case 8: case 24: case 69: return Keys.D;
                case 9: case 25: case 70: return Keys.F;
                case 10: case 26: case 71: return Keys.G;

                case 38: case 55: case 72: return Keys.H;

                case 39: case 56: case 73: return Keys.J;
                case 40: case 57: case 74: return Keys.K;
                case 41: case 58: case 75: return Keys.L;
                case 42: case 59: case 76: return Keys.OemSemicolon;
                case 43: case 60: case 77: return Keys.OemQuotes;

                case 11: case 27: case 78: return Keys.Z;
                case 12: case 28: case 79: return Keys.X;
                case 13: case 29: case 80: return Keys.C;
                case 14: case 30: case 81: return Keys.V;
                case 15: case 31: case 82: return Keys.B;

                case 44: case 61: case 83: return Keys.N;
                case 45: case 62: case 84: return Keys.M;
                case 46: case 63: case 85: return Keys.OemComma;
                case 47: case 64: case 86: return Keys.OemPeriod;
                case 48: case 65: case 87: return Keys.OemQuestion;

                default: return Keys.Q;
            }
        }
    }
}
