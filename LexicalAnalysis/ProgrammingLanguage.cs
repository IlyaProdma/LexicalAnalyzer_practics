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

    public HashSet<string> KeyWords { get; private set; }
    public HashSet<string> Delimiters { get; private set; }
    public HashSet<string> Operators { get; private set; }

    public virtual bool IsKeyWord(string word) => KeyWords.Contains(word);

    public virtual bool IsDelimiter(string word) => Delimiters.Contains(word);

    public virtual bool IsOperator(string word) => Operators.Contains(word);
}
