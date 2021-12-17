using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCore
{
    public class ParserDTO
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Ox { get; set; }
        public int Oy { get; set; }
        public int Ux { get; set; }
        public int Uy { get; set; }
        public int Lx { get; set; }
        public int Ly { get; set; }
        public int N { get; set; }
        public int L { get; set; }
        public List<(int x, int y)> Walls { get; set; }

        public ParserDTO() => Walls = new List<(int x, int y)>();

        public void SetLx(string consoleInput) => Lx = int.Parse(consoleInput);
        public void SetUx(string consoleInput)
        {
            int ux = int.Parse(consoleInput);
            if (Lx >= ux) throw new Exception("Logical error: Ux must be greater that Lx!");
            Ux = ux;
        }
        public void SetLy(string consoleInput) => Ly = int.Parse(consoleInput);
        public void SetUy(string consoleInput)
        {
            int uy = int.Parse(consoleInput);
            if (Ly >= uy) throw new Exception("Logical error: Uy must be greater that Ly!");
            Uy = uy;
        }
        public void SetOx(string consoleInput)
        {
            int ox = int.Parse(consoleInput);
            if (ox > Ux || ox < Lx) throw new Exception("Logical error: Ox is out of bounds!");
            Ox = ox;
        }
        public void SetOy(string consoleInput)
        {
            int oy = int.Parse(consoleInput);
            if (oy > Uy || oy < Ly) throw new Exception("Logical error: Oy is out of bounds!");
            Oy = oy;
        }
        public void SetN(string consoleInput) => N = int.Parse(consoleInput);

        public void AddWall((int x, int y) wall)
        {
            if (wall.x > Ux || wall.x < Lx || wall.y > Uy || wall.y < Ly)
                throw new Exception($"Wall ({wall.x},{wall.y}) is out of range!");
            Walls.Add(wall);
        }
    }
}
