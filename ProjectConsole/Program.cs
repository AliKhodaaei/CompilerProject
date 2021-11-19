﻿using ProjectCore;
using System;

namespace ProjectConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "";
            do
            {
                input += Console.ReadLine() + "\r";
            } while (!input.ToLower().Contains("end"));

            Parser parser = new(input);
            parser.A();
            var output = parser.Output;
            foreach (var line in output)
            {
                Console.ForegroundColor = line.Contains("ERROR") ? ConsoleColor.Red : ConsoleColor.Green;
                Console.WriteLine(line);
                Console.ResetColor();
            }
        }
    }
}
