using System;
using System.Collections;
using System.Text;

namespace ProjectCore
{
    public class Lexer
    {
        public int line = 1, C = 0;
        private char peek = ' ';
        private bool negativeFlag = false;
        private readonly Hashtable symbolTable = new();
        private readonly char[] input;

        public Lexer(string input)
        {
            this.input = input.ToCharArray();
            Reserve(new Word(Tag.Lx, "lx"));
            Reserve(new Word(Tag.Ux, "ux"));
            Reserve(new Word(Tag.Ly, "ly"));
            Reserve(new Word(Tag.Uy, "uy"));
            Reserve(new Word(Tag.Ox, "ox"));
            Reserve(new Word(Tag.Oy, "oy"));
            Reserve(new Word(Tag.N, "n"));
            Reserve(new Word(Tag.begin, "begin"));
            Reserve(new Word(Tag.north, "north"));
            Reserve(new Word(Tag.east, "east"));
            Reserve(new Word(Tag.west, "west"));
            Reserve(new Word(Tag.south, "south"));
            Reserve(new Word(Tag.end, "end"));
        }

        void Reserve(Word t) => symbolTable.Add(t.lexeme, t);

        public Token GetToken()
        {
            negativeFlag = false;
            for (; ; peek = input[C++])
            {
                if (peek == ' ' || peek == '\t') continue;
                else if (peek == '\n') line++;
                else if (peek == '/')
                {
                    peek = input[C++];
                    if (peek == '/')
                    {
                        do
                        {
                            peek = input[C++];
                        } while (peek != '\n');
                        line++;
                    }
                    else
                        throw new Exception($"Syntax error in line {line}");
                }
                else if (peek == '{')
                {
                    do
                    {
                        peek = input[C++];
                        if (peek == '\n') line++;
                    } while (peek != '}');
                }
                else break;
            }

            if (peek == 45)
            {
                negativeFlag = true;
                peek = input[C++];
            }

            if (char.IsDigit(peek))
            {
                int v = 0;
                do
                {
                    v = v * 10 + (peek - 48);
                    peek = input[C++];
                } while (char.IsDigit(peek));
                return new Num(negativeFlag ? -v : v);
            }

            if (char.IsLetter(peek))
            {
                StringBuilder sb = new();
                do
                {
                    sb.Append(peek);
                    peek = input[C++];
                } while (char.IsLetter(peek));
                string s = sb.ToString().ToLower();
                if (symbolTable.Contains(s))
                    return (Word)symbolTable[s];
                throw new Exception($"Syntax error in line {line}");
            }

            if (peek == ':')
            {
                Token t = new(peek);
                peek = ' ';
                return t;
            }

            throw new Exception($"Syntax error in line {line}");
        }
    }
}
