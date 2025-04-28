using LexicalAnalysis.Analyzers;
using LexicalAnalysis.Fortran;
using LexicalAnalysis.Python;

namespace LexicalAnalysis {
    public class Program {
        public static void Main()
        {
            Console.WriteLine("**************************************************");
            Console.WriteLine("** Ilya Prodma, MKS244, HW1 \"Lexical Analzer\"   **");
            Console.WriteLine("**************************************************");
            Console.WriteLine("** Select language to interpret:                **");
            Console.WriteLine("** 1) Fortran (77)                              **");
            Console.WriteLine("** 2) Python (2.7)                              **");
            Console.WriteLine("** 0) Exit program                              **");
            Console.WriteLine("**************************************************");
            ProgrammingLanguage language = Fortran77.Instance;
            int input;
            do {
                Console.WriteLine("** Your choice:                                 **");
                Console.Write("** ");
                input = Convert.ToInt32(Console.ReadLine());
                switch (input)
                {
                    case 1:
                        language = Fortran77.Instance;
                    break;
                    case 2:
                        language = Python27.Instance;
                    break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("**             Error! Try Again!                **");
                        input = -1;
                    break;
                }                
            } while (input == -1);

            StateMachineAnalyzer analyzer = new(language);
            string? filename;
            do
            {
                Console.WriteLine("** Input name of file to analyze:               **");
                filename = Console.ReadLine();

                if (!File.Exists(filename))
                {
                    filename = string.Empty;
                    Console.WriteLine("**             Error! Try Again!                **");
                }
            } while (string.IsNullOrWhiteSpace(filename));

            var text = string.Empty;
            using (var reader = new StreamReader(filename))
            {
                text = reader.ReadToEnd();
            }
            analyzer.Analyze(text);

            Console.WriteLine("**                    Result:                   **");
            foreach (var pair in analyzer.Lexems) {
                Console.WriteLine(pair.ToString());
            }
        }
    }
}
