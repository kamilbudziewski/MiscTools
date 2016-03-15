using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLineParser.Arguments;
using CommandLineParser.Exceptions;

namespace LargeDirectoryCleaner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var parser = new CommandLineParser.CommandLineParser();
            var version = new ValueArgument<string>('d', "directory", "Directory to clean");
            version.ValueOptional = false;
            parser.Arguments.Add(version);

            try
            {
                parser.ParseCommandLine(args);

                CleanDirectory(version.Value);
            }
            catch (CommandLineException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void CleanDirectory(string directory)
        {
            try
            {
                Console.WriteLine($"{DateTime.UtcNow.ToString("s")} In dir: {directory}");
                foreach (string f in Directory.EnumerateFiles(directory))
                {
                    File.Delete(f);
                    Console.WriteLine($"{DateTime.UtcNow.ToString("s")} Deleted file: {f}");
                }

                if (!Directory.EnumerateDirectories(directory).Any())
                {
                    Directory.Delete(directory);
                    Console.WriteLine($"{DateTime.UtcNow.ToString("s")} Deleted dir: {directory}");
                }

                foreach (string d in Directory.EnumerateDirectories(directory))
                {
                    CleanDirectory(d);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{DateTime.UtcNow.ToString("s")} Exc: {e}");
            }
        }
    }
}
