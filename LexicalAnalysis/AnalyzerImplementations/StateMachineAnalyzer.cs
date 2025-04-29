using System.Text;

namespace LexicalAnalysis.Analyzers;

internal enum AnalysisState {
    DELIMITER,
    END,
    ERROR,
    IDENTIFIER,
    INIT,
    NUMBER,
    STRING
}

public class StateMachineAnalyzer : LexicalAnalyzer
{
    private readonly StringBuilder _buffer; 
    private AnalysisState _currentState;
    private int _currentIndex = 0;
    private string _text = string.Empty;

    public StateMachineAnalyzer(ProgrammingLanguage language) : base(language) {
        _currentState = AnalysisState.INIT;
        _buffer = new();
    }

    public override void Analyze(string text) {
        _text = text;
        while (_currentState != AnalysisState.END) {
            switch(_currentState) {                
                case AnalysisState.INIT:
                    ResolveInitState();
                break;
                case AnalysisState.IDENTIFIER:
                    ResolveIdentificatorState();
                break;
                case AnalysisState.NUMBER:
                    ResolveNumberState();
                break;
                case AnalysisState.DELIMITER:
                    ResolveDelimiterState();
                break;
                case AnalysisState.STRING:
                    ResolveStringLiteralState();
                break;
                case AnalysisState.ERROR:
                    throw new InvalidOperationException(
                        "Text contains invalid constructions or characters: " + _buffer.ToString()
                    );
            }
        }
    }

    private void ResolveInitState() {
        if (_currentIndex == _text.Length) {
            _currentState = AnalysisState.END;
            return;
        }

        if (char.IsWhiteSpace(_text[_currentIndex]))
        {
            _currentIndex++;
        }
        else if (char.IsLetter(_text[_currentIndex]) || _text[_currentIndex] == '_')
        {
            _buffer.Clear();
            _currentState = AnalysisState.IDENTIFIER;
        }
        else if (char.IsDigit(_text[_currentIndex]))
        {
            _buffer.Clear();
            _currentState = AnalysisState.NUMBER;
        }
        else if (_text[_currentIndex] is '"' or '\'')
        {
            _buffer.Clear();
            _buffer.Append(_text[_currentIndex]);
            _currentIndex++;
            _currentState = AnalysisState.STRING;
        }
        else
        {
            _buffer.Clear();
            _currentState = AnalysisState.DELIMITER;
        }
    }

    private void ResolveIdentificatorState()
    {
        if (char.IsLetterOrDigit(_text[_currentIndex]) || _text[_currentIndex] == '_')
        {
            _buffer.Append(_text[_currentIndex]);
            _currentIndex++;
        }
        else
        {
            var value = _buffer.ToString();
            if (_language.IsKeyWord(value))
            {
                _lexems.Add(new Lexem(LexemType.Keyword, value));
            }
            else
            {
                if (!value.Any(char.IsLetterOrDigit))
                {
                    _currentState = AnalysisState.ERROR;
                    return;
                }
                _lexems.Add(new Lexem(LexemType.Identificator, value));
            }
            _buffer.Clear();
            _currentState = AnalysisState.INIT;
        }
    }

    private void ResolveNumberState()
    {
        if (char.IsDigit(_text[_currentIndex]))
        {
            _buffer.Append(_text[_currentIndex]);
            _currentIndex++;
        }
        else
        {
            if (_text[_currentIndex] == '.')
            {
                if (_buffer.ToString().Contains('.'))
                {
                    _currentState = AnalysisState.ERROR;
                }
                else
                {
                    _buffer.Append(_text[_currentIndex]);
                    _currentIndex++;
                }
            }
            else
            {
                _lexems.Add(new Lexem(LexemType.Number, _buffer.ToString()));
                _currentState = AnalysisState.INIT;
                _buffer.Clear();
            }
        }
    }

    private void ResolveDelimiterState()
    {
        if (char.IsPunctuation(_text[_currentIndex]) ||
            char.IsLetter(_text[_currentIndex]) ||
            char.IsSymbol(_text[_currentIndex])
        )
        {
            _buffer.Append(_text[_currentIndex]);
            var value = _buffer.ToString();
            if (_language.IsDelimiter(value))
            {
                _lexems.Add(new Lexem(LexemType.Delimiter, value));
                _currentState = AnalysisState.INIT;
            }
            else if (_language.IsOperator(value))
            {
                _lexems.Add(new Lexem(LexemType.Operator, value));
                _currentState = AnalysisState.INIT;
            }
            _currentIndex++;
        }
        else
        {
            _currentState = AnalysisState.ERROR;
        }
    }

    private void ResolveStringLiteralState()
    {
        _buffer.Append(_text[_currentIndex]);
        if (_text[_currentIndex] == _buffer[0])
        {
            _lexems.Add(new Lexem(LexemType.StringLiteral, _buffer.ToString()));
            _currentState = AnalysisState.INIT;
        }
        _currentIndex++;
    }
}
