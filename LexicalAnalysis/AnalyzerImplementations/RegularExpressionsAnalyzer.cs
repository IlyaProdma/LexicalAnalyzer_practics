using System.Text;
using System.Text.RegularExpressions;

namespace LexicalAnalysis.Analyzers;

public class RegularExpressionsAnalyzer : LexicalAnalyzer
{
    private readonly Regex _lexemExpression;

    public RegularExpressionsAnalyzer(ProgrammingLanguage language) : base(language) {
        _lexemExpression = new Regex(BuildPattern());
    }

    private string BuildPattern()
    {
        return $"({BuildKeywordPattern()})|({BuildOperatorPattern()})|({BuildDelimiterPattern()})|" +
        $"({STRING_LITERAL_PATTERN})|({IDENTIFICATOR_PATTERN})|({NUMBER_PATTERN})";
    }

    private const string IDENTIFICATOR_PATTERN = @"(_*[A-Za-z][A-Za-z0-9]*_*)";
    private const string NUMBER_PATTERN = @"[0-9]+\.?[0-9]*";
    private const string STRING_LITERAL_PATTERN = @"('[^']*')" + "|(\"[^\"]*\")";
    
    private static string BuildPatternFromHashSet(HashSet<string> set, bool addWordBoundary = false)
    {
        StringBuilder builder = new();
        foreach (var value in set)
        {
            if (addWordBoundary) builder.Append(@"\b");
            builder.Append(
                value
                .Replace("*", @"\*")
                .Replace("!", @"\!")
                .Replace(".", @"\.")
                .Replace("+", @"\+")
                .Replace("/", @"\/")
                .Replace("[", @"\[")
                .Replace("]", @"\]")
                .Replace("(", @"\(")
                .Replace(")", @"\)")
                .Replace("|", @"\|")
                .Replace("^", @"\^")
            );
            if (addWordBoundary) builder.Append(@"\b");
            builder.Append('|');
        }
        // remove last '|'
        builder.Remove(builder.Length - 1, 1);

        return builder.ToString();
    }

    private string BuildKeywordPattern() => BuildPatternFromHashSet(_language.KeyWords, true);

    private string BuildDelimiterPattern() => BuildPatternFromHashSet(_language.Delimiters);

    private string BuildOperatorPattern() => BuildPatternFromHashSet(_language.Operators, false);

    public override void Analyze(string text)
    {
        var matches = _lexemExpression.Matches(text);
        if (matches.Any())
        {
            foreach (Match match in matches)
            {
                var matchStr = match?.ToString() ?? throw new InvalidOperationException("Cannot read match");
                if (_language.IsKeyWord(matchStr))
                {
                    _lexems.Add(new Lexem(LexemType.Keyword, matchStr));
                }
                else if (_language.IsOperator(matchStr))
                {
                    _lexems.Add(new Lexem(LexemType.Operator, matchStr));
                }
                else if (_language.IsDelimiter(matchStr))
                {
                    _lexems.Add(new Lexem(LexemType.Delimiter, matchStr));
                }
                else if (Regex.Match(matchStr, STRING_LITERAL_PATTERN).Success)
                {
                    _lexems.Add(new Lexem(LexemType.StringLiteral, matchStr));
                }
                else if (Regex.Match(matchStr, IDENTIFICATOR_PATTERN).Success)
                {
                    _lexems.Add(new Lexem(LexemType.Identificator, matchStr));
                }
                else if (Regex.Match(matchStr, NUMBER_PATTERN).Success)
                {
                    _lexems.Add(new Lexem(LexemType.Number, matchStr));
                }
                else
                {
                    throw new InvalidOperationException($"Unknown token: {matchStr}");
                }
            }
        }
    }
}