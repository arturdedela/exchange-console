using System;
using System.Diagnostics;

namespace ExchangeConsole
{
    internal class Program
    {
        private static string DataBaseName = "exchange";

        public static void Main(string[] args)
        {
            var command = "";

            while (command != "exit")
            {
                Console.Write("-> ");
                command = Console.ReadLine();
                HandleCommand(command);
            }

            Console.WriteLine("Bye");
            Console.ReadLine();
        }

        public static void HandleCommand(string command)
        {
            if (command == "help")
            {
                HelpCommandHandler();
            }
            else if (command == "exit" || command == "")
            {
                return;
            }
            else if (command == "dump")
            {
                DumpCommandHandler();
            }
            else if (command == "restore")
            {
                RestoreCommandHandler();
            }
            else
            {
                Console.WriteLine("Incorrect command! Try write help");
            }
        }

        public static void HelpCommandHandler()
        {
            Console.WriteLine("help, exit, dump, restore");
        }

        public static void DumpCommandHandler()
        {
            Console.Write("Enter output filename (with extension .dump): ");
            var outFileName = Console.ReadLine();

            Console.Write("Enter DataBase username: ");
            var userName = Console.ReadLine();

            Console.Write("Enter DataBase password: ");
            var password = Console.ReadLine();

            var arguments = $"-Fc -U {userName} -d {DataBaseName} -f \"{outFileName}\"";

            ExecuteCommand("pg_dump", arguments, password);

            Console.WriteLine("Dump created");
        }

        public static void RestoreCommandHandler()
        {
            Console.Write("NOTE: close all connections to database before continue! Ready? (y/n): ");
            if (Console.ReadLine() != "y")
            {
                Console.WriteLine("Not restored");
                return;
            }

            Console.Write("Enter input filename (with extension .dump): ");
            var inFileName = Console.ReadLine();

            Console.Write("Enter DataBase username: ");
            var userName = Console.ReadLine();

            Console.Write("Enter DataBase password: ");
            var password = Console.ReadLine();

            ExecuteCommand("dropdb", $"-U {userName} {DataBaseName}", password);
            ExecuteCommand("createdb", $"-U {userName} {DataBaseName}", password);
            ExecuteCommand("pg_restore", $"-U {userName} -d {DataBaseName} {inFileName}", password);

            Console.WriteLine("Restored");
        }

        public static void ExecuteCommand(string command, string arguments, string password)
        {
            var dmCommandsPath = "C:/Program Files/PostgreSQL/11/bin/";
            var startInfo = new ProcessStartInfo(dmCommandsPath + command, arguments);
            startInfo.EnvironmentVariables.Add("PGPASSWORD", password);
            startInfo.UseShellExecute = false;

            var process = new Process { StartInfo = startInfo };
            process.Start();
            process.WaitForExit();
        }
    }
}