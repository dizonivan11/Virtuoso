using Microsoft.Xna.Framework.Input;

namespace FullKeyMania.Components {
    public class Note {
        public double Time { get; private set; }
        public Keys KeyAssigned { get; private set; }

        public Note(double time, Keys keyAssigned) {
            Time = time;
            KeyAssigned = keyAssigned;
        }
    }
}
