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
        public const string MUSIC_FILE_NAME = "audio";
        public const string MUSIC_FILE_EXTENSION = ".mp3";
        public const string MIDI_FILE_NAME = "notes";
        public const string MIDI_FILE_EXTENSION = ".mid";

        public string DIR { get; private set; }
        public string Name { get; private set; }
        public string Author { get; private set; }
        public string Mapper { get; private set; }
        public int BPM { get; private set; } // Beats per Minute
        public int BeatValue { get; private set; }
        public int BeatCount { get; private set; }
        public double AR { get; private set; } // Appoach Rate (in ms; the time the notes will appear prior to their hit window)
        public double Offset { get; set; } // The start offset of the song (to fix the beat of a song with silence at the beginning)

        public AudioFileReader Song { get; private set; }
        public List<Note> Notes { get; private set; }

        public Beatmap(string beatmapPath) {
            DIR = beatmapPath;
            Song = new AudioFileReader(beatmapPath + @"\" + MUSIC_FILE_NAME + MUSIC_FILE_EXTENSION);
            Notes = new List<Note>();

            SettingsCollection settings = Settings.Parse(beatmapPath + @"\settings.ini");
            Name = settings["name"];
            Author = settings["author"];
            Mapper = settings["mapper"];
            BPM = int.Parse(settings["bpm"]);
            BeatValue = int.Parse(settings["beatvalue"]);
            BeatCount = int.Parse(settings["beatcount"]);
            AR = double.Parse(settings["ar"]);
            Offset = double.Parse(settings["offset"]);

            // Note timing processing
            MidiFile midiFile = MidiFile.Read(beatmapPath + @"\" + MIDI_FILE_NAME + MIDI_FILE_EXTENSION);
            var notes = midiFile.GetNotes().ToArray();
            for (int n = 0; n < notes.Length; n++) {
                MNote note = notes[n];
                MetricTimeSpan mts = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, midiFile.GetTempoMap());
                Notes.Add(new Note(mts.Minutes * 60d + mts.Seconds + mts.Milliseconds / 1000d, ToKey(note)));
            }
        }

        public static Keys ToKey(MNote note) {
            switch (note.NoteNumber) {
                case 21: case 37: case 53: return Keys.Q;
                case 22: case 38: case 54: return Keys.W;
                case 23: case 39: case 55: return Keys.E;
                case 24: case 40: case 56: return Keys.R;
                case 25: case 41: case 57: return Keys.T;
                case 26: case 42: case 58: return Keys.Y;

                case 27: case 43: case 59: return Keys.A;
                case 28: case 44: case 60: return Keys.S;
                case 29: case 45: case 61: return Keys.D;
                case 30: case 46: case 62: return Keys.F;
                case 31: case 47: case 63: return Keys.G;

                case 32: case 48: case 64: return Keys.Z;
                case 33: case 49: return Keys.X;
                case 34: case 50: return Keys.C;
                case 35: case 51: return Keys.V;
                case 36: case 52: return Keys.B;

                case 65: case 82: case 99: return Keys.U;
                case 66: case 83: case 100: return Keys.I;
                case 67: case 84: case 101: return Keys.O;
                case 68: case 85: case 102: return Keys.P;
                case 69: case 86: case 103: return Keys.OemOpenBrackets;
                case 70: case 87: case 104: return Keys.OemCloseBrackets;

                case 71: case 88: case 105: return Keys.H;
                case 72: case 89: case 106: return Keys.J;
                case 73: case 90: case 107: return Keys.K;
                case 74: case 91: case 108: return Keys.L;
                case 75: case 92: return Keys.OemSemicolon;
                case 76: case 93: return Keys.OemQuotes;

                case 77: case 94: return Keys.N;
                case 78: case 95: return Keys.M;
                case 79: case 96: return Keys.OemComma;
                case 80: case 97: return Keys.OemPeriod;
                case 81: case 98: return Keys.OemQuestion;

                default: return Keys.None;
            }
        }
    }
}
