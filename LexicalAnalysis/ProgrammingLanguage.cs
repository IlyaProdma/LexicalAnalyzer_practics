namespace LexicalAnalysis;

public abstract class ProgrammingLanguage
{
    public ProgrammingLanguage(
        HashSet<string> keyWords,
        HashSet<string> delimiters,
        HashSet<string> operators)
    {
        KeyWords = keyWords;
        Delimiters = delimiters;
        Operators = operators;
    }

    protected HashSet<string> KeyWords { get; private set; }
    protected HashSet<string> Delimiters { get; private set; }
    protected HashSet<string> Operators { get; private set; }

    public virtual bool IsKeyWord(string word) => KeyWords.Contains(word);

    public virtual bool IsDelimiter(string word) => Delimiters.Contains(word);

    public virtual bool IsOperator(string word) => Operators.Contains(word);
}
