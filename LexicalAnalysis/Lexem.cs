namespace LexicalAnalysis;

public class Lexem
{
    private readonly LexemType _type;
    private readonly string _value;

    public LexemType Type => _type;
    public string Value => _value;

    public Lexem(LexemType type, string value)
    {
        _type = type;
        _value = value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        var checking = obj as Lexem;

        return _type == checking?._type && _value == checking?._value;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_type, _value);
    }

    public override string ToString()
    {
        return $"{_type}: {_value}";
    }

}
