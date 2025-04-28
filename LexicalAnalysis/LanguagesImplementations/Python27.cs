namespace LexicalAnalysis.Python;

public class Python27 : ProgrammingLanguage
{
    private static Python27? _instance;

    public static Python27 Instance {
        get {
            _instance ??= new Python27();

            return _instance;
        }
    }

    private Python27() : base(new(_keywords), new(_delimiters), new(_operators)) {}

    private static readonly string[] _keywords =
    {
        "and",
        "as",
        "assert",
        "break",
        "class",
        "continue",
        "def",
        "del",
        "elif",
        "else",
        "except",
        "exec",
        "finally",
        "for",
        "from",
        "global",
        "if",
        "import",
        "in",
        "is",
        "lambda",
        "not",
        "or",
        "pass",
        "print",
        "raise",        
        "return",
        "try",
        "while",
        "with",
        "yield",
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
        ".",
        "`",
        ";",
    };

    private static readonly string[] _operators =
    {
        "=",
        "+",
        "-",
        "*",
        "**",
        "/",
        "//",
        "%",
        "<<",
        ">>",
        "&",
        "|",
        "^",
        "~",
        "<",
        ">",
        "<=",
        ">=",
        "==",
        "!=",
        "<>",
        "+=",
        "-=",
        "*=",
        "/=",
        "//=",
        "%=",
        "&=",
        "|=",
        "^=",
        ">>=",
        "<<=",
        "**=",
    };
}
