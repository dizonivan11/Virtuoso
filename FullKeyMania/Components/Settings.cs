using System;
using System.IO;

namespace FullKeyMania.Components {
    public class Settings {
        public int GlobalOffset { private set; get; }
        public double BackgroundOpacity { private set; get; }
        public double NoteOpacity { private set; get; }
        public string SelectedSkin { private set; get; }
        public string HitSoundPath { private set; get; }

        public Settings(string filePath) {
            SettingsCollection result = Parse(filePath);

            int.TryParse(result["globaloffset"], out int globalOffset);
            double.TryParse(result["backgroundopacity"], out double backgroundOpacity);
            double.TryParse(result["noteopacity"], out double noteOpacity);

            GlobalOffset = globalOffset;
            BackgroundOpacity = backgroundOpacity;
            NoteOpacity = noteOpacity;
            SelectedSkin = result["skin"];
            HitSoundPath = @"Skins\" + SelectedSkin + @"\normal-hitclap.mp3";
        }

        public static SettingsCollection Parse(string filePath) {
            SettingsCollection result = new SettingsCollection();

            StreamReader sr = new StreamReader(filePath);
            string[] settings = sr.ReadToEnd().Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            for (int s = 0; s < settings.Length; s++) {
                string[] setting = settings[s].Split('=');
                result.Add(setting[0], setting[1]);
            }

            return result;
        }
    }
}
