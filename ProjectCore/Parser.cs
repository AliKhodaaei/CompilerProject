using System;
using System.Linq;
using System.Collections.Generic;

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
            Match(Tag.num);
            Lx = ((Num)pl).value;
            B();
        }

        void B()
        {
            Match(Tag.Ux);
            Match(Tag.num);
            Ux = ((Num)pl).value;
            if (Ux <= Lx) throw new Exception("Bounds error");
            C();
        }

        void C()
        {
            Match(Tag.Ly);
            Match(Tag.num);
            Ly = ((Num)pl).value;
            D();
        }

        void D()
        {
            Match(Tag.Uy);
            Match(Tag.num);
            Uy = ((Num)pl).value;
            if (Uy <= Ly) throw new Exception("Bounds error");
            E();
        }

        void E()
        {
            Match(Tag.Ox);
            Match(Tag.num);
            Ox = ((Num)pl).value;
            F();
        }
        void F()
        {
            Match(Tag.Oy);
            Match(Tag.num);
            Oy = ((Num)pl).value;
            if (!CheckBounds(Ox, Oy)) throw new Exception("Bounds error");
            G();
        }

        void G()
        {
            Match(Tag.N);
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
                            Output.Add($"ERROR int instr {cmd}");
                        else
                        {
                            Output.Add($"({X},{++Y})");
                            L++;
                        }
                        break;
                    case Tag.east:
                        Match(Tag.east);
                        if (Walls.Any(wall => wall == (X + 1, Y)) || !CheckBounds(X + 1, Y))
                            Output.Add($"ERROR int instr {cmd}");
                        else
                        {
                            Output.Add($"({++X},{Y})");
                            L++;
                        }
                        break;
                    case Tag.west:
                        Match(Tag.west);
                        if (Walls.Any(wall => wall == (X - 1, Y)) || !CheckBounds(X - 1, Y))
                            Output.Add($"ERROR int instr {cmd}");
                        else
                        {
                            Output.Add($"({--X},{Y})");
                            L++;
                        }
                        break;
                    case Tag.south:
                        Match(Tag.south);
                        if (Walls.Any(wall => wall == (X, Y - 1)) || !CheckBounds(X, Y - 1))
                            Output.Add($"ERROR int instr {cmd}");
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
            var result = Math.Sqrt(Math.Pow(Y - Oy, 2) + Math.Pow(X - Ox, 2)).ToString("0.0000");
            Output.Add($"D={result}");
        }

        void Match(int t)
        {
            if (lookahead.tag == Tag.end) return;
            if (lookahead.tag == t)
            {
                pl = lookahead;
                lookahead = lexer.GetToken();
            }
            else
                throw new Exception("Syntax Error!");
        }

        bool CheckBounds(int x, int y)
        {
            return (x <= Ux && x >= Lx && y <= Uy && y >= Ly);
        }
    }
}
