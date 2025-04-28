namespace LexicalAnalysis;

public abstract class LexicalAnalyzer
{
    public LexicalAnalyzer(ProgrammingLanguage language)
    {
        _language = language;
        _lexems = new();
    }

    protected ProgrammingLanguage _language;

    public abstract void Analyze(string text);

    protected readonly List<Lexem> _lexems;

    public List<Lexem> Lexems => _lexems;
}
