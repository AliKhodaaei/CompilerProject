using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectCore
{
    public class Parser
    {
        private static Token lookahead;
        private Token pl; //Previous Lookahead
        private readonly Lexer lexer;
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
        public List<string> Output { get; set; }

        public Parser(string input)
        {
            Walls = new List<(int x, int y)>();
            Output = new List<string>();
            lexer = new Lexer(input);
            lookahead = lexer.GetToken();
        }

        public void A()
        {
            Match(Tag.Lx);
            Match(':');
            Match(Tag.num);
            Lx = ((Num)pl).value;
            B();
        }

        void B()
        {
            Match(Tag.Ux);
            Match(':');
            Match(Tag.num);
            Ux = ((Num)pl).value;
            if (Ux <= Lx) throw new Exception("Logical error: Ux must be greater than Lx");
            C();
        }

        void C()
        {
            Match(Tag.Ly);
            Match(':');
            Match(Tag.num);
            Ly = ((Num)pl).value;
            D();
        }

        void D()
        {
            Match(Tag.Uy);
            Match(':');
            Match(Tag.num);
            Uy = ((Num)pl).value;
            if (Uy <= Ly) throw new Exception("Logical error: Uy must be greater than Ly");
            E();
        }

        void E()
        {
            Match(Tag.Ox);
            Match(':');
            Match(Tag.num);
            Ox = ((Num)pl).value;
            F();
        }
        void F()
        {
            Match(Tag.Oy);
            Match(':');
            Match(Tag.num);
            Oy = ((Num)pl).value;
            if (!CheckBounds(Ox, Oy)) throw new Exception("Logical error: Start Point (Ox, Oy) is out of range");
            G();
        }

        void G()
        {
            Match(Tag.N);
            Match(':');
            Match(Tag.num);
            N = ((Num)pl).value;
            H();
        }

        void H()
        {
            for (int i = 0; i < N; i++)
            {
                int x, y;
                Match(Tag.num);
                x = ((Num)pl).value;
                Match(Tag.num);
                y = ((Num)pl).value;
                if (!CheckBounds(x, y))
                    throw new Exception($"Logical error: wall ({x},{y}) is out of range");
                Walls.Add((x, y));
            }
            S();
            En();
        }

        void S()
        {
            Match(Tag.begin);
            X = Ox; Y = Oy;
            int cmd = 1;
            Output.Add($"({X},{Y})");

            L = 0;
            while (true)
            {
                switch (lookahead.tag)
                {
                    case Tag.north:
                        Match(Tag.north);
                        if (Walls.Any(wall => wall == (X, Y + 1)) || !CheckBounds(X, Y + 1))
                            Output.Add($"ERROR in instr {cmd}");
                        else
                        {
                            Output.Add($"({X},{++Y})");
                            L++;
                        }
                        break;
                    case Tag.east:
                        Match(Tag.east);
                        if (Walls.Any(wall => wall == (X + 1, Y)) || !CheckBounds(X + 1, Y))
                            Output.Add($"ERROR in instr {cmd}");
                        else
                        {
                            Output.Add($"({++X},{Y})");
                            L++;
                        }
                        break;
                    case Tag.west:
                        Match(Tag.west);
                        if (Walls.Any(wall => wall == (X - 1, Y)) || !CheckBounds(X - 1, Y))
                            Output.Add($"ERROR in instr {cmd}");
                        else
                        {
                            Output.Add($"({--X},{Y})");
                            L++;
                        }
                        break;
                    case Tag.south:
                        Match(Tag.south);
                        if (Walls.Any(wall => wall == (X, Y - 1)) || !CheckBounds(X, Y - 1))
                            Output.Add($"ERROR in instr {cmd}");
                        else
                        {
                            Output.Add($"({X},{--Y})");
                            L++;
                        }
                        break;
                    default:
                        return;
                }
                cmd++;
            }
        }

        void En()
        {
            Match(Tag.end);
            Output.Add($"L={L}");
            Output.Add($"D={Math.Sqrt(Math.Pow(Y - Oy, 2) + Math.Pow(X - Ox, 2)):0.0000}");
        }

        void Match(int t)
        {
            if (lookahead.tag == t)
            {
                pl = lookahead;
                lookahead = lexer.GetToken();
            }
            else
                throw new Exception($"Syntax Error in line {lexer.line}");
        }

        bool CheckBounds(int x, int y) => x <= Ux && x >= Lx && y <= Uy && y >= Ly;
    }
}
