using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectCore
{
    public class Parser
    {
        private static Token lookahead;
        private readonly Lexer lexer;
        private readonly ParserDTO dto;
        public List<string> Output { get; set; }

        public Parser(string input, ParserDTO parserDTO)
        {
            dto = parserDTO;
            Output = new List<string>();
            lexer = new Lexer(input);
            lookahead = lexer.GetToken();
        }

        public void S()
        {
            H();
            En();
        }

        void H()
        {
            Match(Tag.begin);
            dto.X = dto.Ox; dto.Y = dto.Oy;
            int cmd = 1;
            Output.Add($"({dto.X},{dto.Y})");

            dto.L = 0;
            while (true)
            {
                switch (lookahead.tag)
                {
                    case Tag.north:
                        Match(Tag.north);
                        if (dto.Walls.Any(wall => wall == (dto.X, dto.Y + 1)) || !CheckBounds(dto.X, dto.Y + 1))
                            Output.Add($"ERROR in instr {cmd}");
                        else
                        {
                            Output.Add($"({dto.X},{++dto.Y})");
                            dto.L++;
                        }
                        break;
                    case Tag.east:
                        Match(Tag.east);
                        if (dto.Walls.Any(wall => wall == (dto.X + 1, dto.Y)) || !CheckBounds(dto.X + 1, dto.Y))
                            Output.Add($"ERROR in instr {cmd}");
                        else
                        {
                            Output.Add($"({++dto.X},{dto.Y})");
                            dto.L++;
                        }
                        break;
                    case Tag.west:
                        Match(Tag.west);
                        if (dto.Walls.Any(wall => wall == (dto.X - 1, dto.Y)) || !CheckBounds(dto.X - 1, dto.Y))
                            Output.Add($"ERROR in instr {cmd}");
                        else
                        {
                            Output.Add($"({--dto.X},{dto.Y})");
                            dto.L++;
                        }
                        break;
                    case Tag.south:
                        Match(Tag.south);
                        if (dto.Walls.Any(wall => wall == (dto.X, dto.Y - 1)) || !CheckBounds(dto.X, dto.Y - 1))
                            Output.Add($"ERROR in instr {cmd}");
                        else
                        {
                            Output.Add($"({dto.X},{--dto.Y})");
                            dto.L++;
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
            Output.Add($"L={dto.L}");
            Output.Add($"D={Math.Sqrt(Math.Pow(dto.Y - dto.Oy, 2) + Math.Pow(dto.X - dto.Ox, 2)):0.0000}");
        }

        void Match(int t)
        {
            if (lookahead.tag == t)
            {
                lookahead = lexer.GetToken();
            }
            else
                throw new Exception($"Syntax Error in line {lexer.line}");
        }

        bool CheckBounds(int x, int y) => x <= dto.Ux && x >= dto.Lx && y <= dto.Uy && y >= dto.Ly;
    }
}
