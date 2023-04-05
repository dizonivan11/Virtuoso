using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace FullKeyMania.Components {
    public static class Graphics {
        public static Texture2D GetTexture2DFromFile(GraphicsDevice gd, string filePath) {
            Texture2D result = null;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read)) {
                result = Texture2D.FromStream(gd, fs);
                fs.Close();
            }
            return result;
        }
    }
}
