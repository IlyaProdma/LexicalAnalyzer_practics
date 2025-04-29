using LexicalAnalysis;
using LexicalAnalysis.Analyzers;
using LexicalAnalysis.Fortran;
using LexicalAnalysis.Python;

namespace LexicalAnalysisConsoleApp {
    public class Program {

        private const string ASTERISK_LINE = "******************************************************";

        private static string FormatString(string text)
        {
            string output = "** " + text;
            output += new string(' ', ASTERISK_LINE.Length - output.Length - 2) + "**";
            return output;
        }

        private static void StreamOutput(TextWriter stream, string text)
        {
            stream.WriteLine(FormatString(text));
        }

        private static void ConsoleOutput(string text)
        {
            StreamOutput(Console.Out, text);
        }

        private static void PrintMainMenu()
        {
            Console.WriteLine(ASTERISK_LINE);
            ConsoleOutput("Ilya Prodma, MKS244, HW1 \"Lexical Analzer\"");
            Console.WriteLine(ASTERISK_LINE);
            ConsoleOutput("Select language to interpret:");
            ConsoleOutput("1) Fortran (77)");
            ConsoleOutput("2) Python (2.7)");
            ConsoleOutput("0) Exit program");
            Console.WriteLine(ASTERISK_LINE);
        }

        private static ProgrammingLanguage? PrintSelectLanguage()
        {
            int input;
            do {
                ConsoleOutput("Your choice:");
                input = Convert.ToInt32(Console.ReadLine());
                switch (input)
                {
                    case 1:
                        return Fortran77.Instance;
                    case 2:
                        return Python27.Instance;
                    case 0:
                        return null;
                    default:
                        ConsoleOutput("Error! Try again!");
                        input = -1;
                    break;
                }                
            } while (input == -1);

            return null;
        }

        private static string PrintSelectFilename(bool needCreateFile = false)
        {
            string? filename;
            do
            {
                ConsoleOutput("Your input:");
                filename = Console.ReadLine();
                if (filename?.Trim() == "exit()") return string.Empty;

                if (needCreateFile && !string.IsNullOrWhiteSpace(filename))
                {
                    using (File.Create(filename)) {}
                }
                else
                {
                    if (!File.Exists(filename))
                    {
                        filename = string.Empty;
                        ConsoleOutput("Invalid file name! Try again!");
                    }
                }

            } while (string.IsNullOrWhiteSpace(filename));

            return filename;
        }

        private static void PrintAnalysisResultsToStream(TextWriter stream, LexicalAnalyzer analyzer)
        {
            StreamOutput(stream, "Lexical Analysis Result:");
            foreach (var pair in analyzer.Lexems) {
                StreamOutput(stream, pair.ToString());
            }
        }

        public static void Main()
        {
            PrintMainMenu();
            ProgrammingLanguage? language = PrintSelectLanguage();
            if (language is null) return;

            LexicalAnalyzer analyzer = new StateMachineAnalyzer(language);
            ConsoleOutput("Input name of file to analyze or type exit()");
            var filename = PrintSelectFilename();
            if (string.IsNullOrEmpty(filename)) return;

            var text = string.Empty;
            using (var reader = new StreamReader(filename))
            {
                text = reader.ReadToEnd();
            }

            analyzer.Analyze(text);
            ConsoleOutput("Analysis is completed.");
            int input;
            do {
                Console.WriteLine();
                ConsoleOutput("Need to print the results to console or file?");
                ConsoleOutput("1) Console");
                ConsoleOutput("2) File");
                ConsoleOutput("0) Skip");
                do {
                    ConsoleOutput("Your choice:");
                    input = Convert.ToInt32(Console.ReadLine());
                    switch (input)
                    {
                        case 1:
                            PrintAnalysisResultsToStream(Console.Out, analyzer);
                            break;
                        case 2:
                            ConsoleOutput("Input name of file to store analysis results");
                            filename = PrintSelectFilename(true);
                            using (var writer = new StreamWriter(filename))
                            {
                                PrintAnalysisResultsToStream(writer, analyzer);
                            }
                            break;
                        case 0:
                            break;
                        default:
                            ConsoleOutput("Error! Try again!");
                            input = -1;
                        break;
                    }                
                } while (input == -1);
            } while (input != 0);
        }
    }
}
