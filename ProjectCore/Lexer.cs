using System;
using System.Collections;
using System.Text;

namespace ProjectCore
{
    public class Lexer
    {
        public int line = 1, C = 0; //C : Pointer for current character to read from input
        private char peek = ' ';
        private readonly Hashtable symbolTable = new();
        private readonly char[] input;

        public Lexer(string input)
        {
            this.input = input.ToCharArray();
            Reserve(new Word(Tag.begin, "begin"));
            Reserve(new Word(Tag.north, "north"));
            Reserve(new Word(Tag.east, "east"));
            Reserve(new Word(Tag.west, "west"));
            Reserve(new Word(Tag.south, "south"));
            Reserve(new Word(Tag.end, "end"));
        }

        void Reserve(Word t) => symbolTable.Add(t.Lexeme, t);

        private char GetNextChar()
        {
            if (input.Length == 0 || C >= input.Length) //if input is empty, set peek to null character, or end of input reached
                return '\0';
            return input[C++];
        }

        public Token GetToken()
        {
            for (; ; peek = GetNextChar())
            {
                if (peek == ' ' || peek == '\t') continue;
                else if (peek == '\n') line++;
                else if (peek == '/')
                {
                    peek = GetNextChar();
                    if (peek == '/')
                    {
                        do
                        {
                            peek = GetNextChar();
                            if (peek == '\0') break; //input is a linear comment only!
                        } while (peek != '\n');
                        if (peek == '\n') line++;
                    }
                    else
                        throw new Exception($"Syntax error in line {line}");
                }
                else if (peek == '{')
                {
                    do
                    {
                        peek = GetNextChar();
                        if (peek == '\0') break; //input is a block comment only!
                        if (peek == '\n') line++;
                    } while (peek != '}');
                }
                else break;
            }

            if (char.IsLetter(peek))
            {
                StringBuilder sb = new();
                do
                {
                    sb.Append(peek);
                    peek = GetNextChar();
                } while (char.IsLetter(peek));
                string s = sb.ToString().ToLower();
                if (symbolTable.Contains(s))
                    return (Word)symbolTable[s];
                return new Token(peek);
            }

            Token t = new(peek);
            peek = ' ';
            return t;
        }
    }
}
