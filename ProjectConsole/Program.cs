using ProjectCore;
using System;
using System.Collections.Generic;

namespace ProjectConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ParserDTO dto = GetDTO();
                string input = "";
                do
                {
                    input += Console.ReadLine() + "\n";
                } while (!input.ToLower().Contains("end"));

                Parser parser = new(input, dto);
                parser.S();
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

        static ParserDTO GetDTO()
        {
            var dto = new ParserDTO();
            Console.WriteLine("Welcome to our compiler!");
            Console.Write("Enter Lx value: ");
            dto.SetLx(Console.ReadLine());
            Console.Write("Enter Ux value: ");
            dto.SetUx(Console.ReadLine());
            Console.Write("Enter Ly value: ");
            dto.SetLy(Console.ReadLine());
            Console.Write("Enter Uy value: ");
            dto.SetUy(Console.ReadLine());
            Console.Write("Enter Ox value: ");
            dto.SetOx(Console.ReadLine());
            Console.Write("Enter Oy value: ");
            dto.SetOy(Console.ReadLine());
            Console.Write("Enter number of walls: ");
            dto.SetN(Console.ReadLine());
            for (int i = 1; i <= dto.N; i++)
            {
                int x, y;
                Console.Write($"Enter wall#{i} x value: ");
                x = int.Parse(Console.ReadLine());
                Console.Write($"Enter wall#{i} y value: ");
                y = int.Parse(Console.ReadLine());
                dto.AddWall((x, y));
            }
            Console.WriteLine("Enter your commands: ");
            return dto;
        }
    }
}
