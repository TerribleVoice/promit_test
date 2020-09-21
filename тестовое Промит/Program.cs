using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Design;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace promit_test
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 2)
                RunWithArguments(args);

            if (args.Length == 0)
            {
                using (var db = new GlossaryDbContext())
                {
                    var glossary = db.Words.ToArray();
                    var line = ReadFromConsole();

                    while (!string.IsNullOrEmpty(line))
                    {
                        Console.WriteLine();
                        AutoComplete(line, glossary);
                        line = ReadFromConsole();
                    }
                }
            }
        }

        private static string ReadFromConsole()
        {
            var readKey = new ConsoleKeyInfo();
            var builder = new StringBuilder();

            while (true)
            {
                readKey = Console.ReadKey(true);
                if(readKey.Key == ConsoleKey.Enter)
                    break;
                
                switch (readKey.Key)
                {
                    case ConsoleKey.Escape:
                        return null;

                    case ConsoleKey.Backspace:
                        Console.Write("\b \b");
                        break;

                    default:
                        Console.Write(readKey.KeyChar);
                        builder.Append(readKey.KeyChar);
                        break;
                }
            } 
            var input = builder.ToString();
            return input;
        }
        
        private static void AutoComplete(string line, IEnumerable<Word> glossary)
        {
            var list = glossary.Where(word => word.Value.StartsWith(line))
                .OrderByDescending(word => word.Amount)
                .Take(5);

            foreach (var word in list)
                Console.WriteLine($">{word}");
        }

        private static void RunWithArguments(string[] args)
        {
            var command = $"{args[0]} {args[1]}";
            Console.WriteLine("connecting to Database");
            using (var db = new GlossaryDbContext())
            {                    
                Console.Clear();
                switch (command)
                {
                    case "очистить словарь":
                        db.ClearGlossary();
                        break;
                    
                    case "создание словаря":
                        db.CreateGlossary();
                        Console.WriteLine("Done");
                        break;
                    
                    case "обновление словаря":
                        db.UpdateGlossary();
                        Console.WriteLine("Done");
                        break;
                    
                    default:
                        Console.WriteLine("Unknown command");
                        break;
                }
            }
        }
    }
}
