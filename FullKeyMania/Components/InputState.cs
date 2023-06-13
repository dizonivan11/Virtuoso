using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FullKeyMania.Components {
    public class InputState {
        public KeyboardState PreviousKeyboardState { get; private set; }
        public KeyboardState CurrentKeyboardState { get; private set; }
        public MouseState PreviousMouseState { get; private set; }
        public MouseState CurrentMouseState { get; private set; }
        public Point MousePosition { get { return CurrentMouseState.Position; } }

        public void UpdateCurrentStates(KeyboardState cks, MouseState cms) {
            CurrentKeyboardState = cks;
            CurrentMouseState = cms;
        }

        public void UpdatePreviousStates() {
            PreviousKeyboardState = CurrentKeyboardState;
            PreviousMouseState = CurrentMouseState;
        }
    }
}
