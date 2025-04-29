namespace LexicalAnalysis.Fortran;

public class Fortran77 : ProgrammingLanguage
{
    private static Fortran77? _instance;

    public static Fortran77 Instance {
        get {
            _instance ??= new Fortran77();

            return _instance;
        }
    }

    private Fortran77() : base(
        new(_keywords.Union(_keywords.Select(k => k.ToUpper()))),
        new(_delimiters),
        new(_operators.Union(_operators.Select(o => o.ToUpper())))
    ) {}

    private static readonly string[] _keywords =
    {
        "assign",
        "backspace",
        "call",
        "close",
        "common",
        "continue",
        "data",
        "dimension",
        "do",
        "else",
        "end",
        "endfile",
        "endif",
        "entry",
        "equivalence",
        "external",
        "format",
        "function",
        "goto",
        "if",
        "implicit",
        "inquire",
        "intrinsic",
        "open",
        "parameter",
        "pause",
        "print",
        "program",
        "read",
        "return",
        "rewind",
        "rewrite",
        "save",
        "stop",
        "subroutine",
        "then",
        "write"
    };

    private static readonly string[] _delimiters =
    {
        "(",
        ")",
        "[",
        "]",
        "{",
        "}",
        "@",
        ",",
        ":",
        "`",
        ";",
    };

    private static readonly string[] _operators =
    {
        ".eq.",
        ".ne.",
        ".gt.",
        ".ge.",
        ".lt.",
        ".le.",
        ".not.",
        ".and.",
        ".or.",
        "+",
        "-",
        "*",
        "/",
        "="
    };
}
