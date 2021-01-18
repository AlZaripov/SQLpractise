using System.Data.SqlClient;
using System.Data;
using System;
using System.Configuration;


namespace MSsqlConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var database = new Database();
            Console.WriteLine("Type \"help\" to get inforamation about supported commands");
            while (true)
            {
                Console.Write("> ");
                var instruction = Console.ReadLine();

                switch (instruction.Split(' ')[0].ToLower())
                {
                    case "info":
                        database.GetConnectionInfo(PrintArray);
                        break;
                    case "help":
                        Database.GetHelp(PrintArray);
                        break;
                    case "exit":
                        database.Exit();
                        break;
                    case "select":
                        database.Select(instruction, PrintArray);
                        break;
                    case "insert":
                        database.Insert(instruction, PrintLine);
                        break;
                    case "update":
                        database.Update(instruction, PrintLine);
                        break;
                    case "delete":
                        database.Delete(instruction, PrintLine);
                        break;
                    case "clear":
                        Console.Clear();
                        break;
                    default:
                        PrintLine($"Command \"{instruction}\" is not corect. Type \"help\" to get list of commands");
                        break;
                }
                Console.WriteLine();
            }
        }

        static void PrintArray(string[] array)
        {
            foreach (var item in array)
                Console.WriteLine(item);
        }

        static void PrintLine(string str)
        {
            Console.WriteLine(str);
        }

    }
}
