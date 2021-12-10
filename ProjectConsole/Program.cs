using ProjectCore;
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
                input += Console.ReadLine() + "\n";
            } while (!input.ToLower().Contains("end"));

            try
            {
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
