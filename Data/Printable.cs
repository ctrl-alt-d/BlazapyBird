using System.Collections.Generic;
using System.Linq;

namespace BlazapyBird.Data
{
    public class Printable : GameElement
    {
        public Printable() {}
        public Printable(int x, int y, string image)
        {
            this.X = x;
            this.Y = y;
            this.Image = image;
        }
    }
}
