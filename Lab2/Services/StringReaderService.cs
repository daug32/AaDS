namespace Lab2.Services;

public class StringReaderService
{
    public readonly string Text;

    public bool IsEndOfLine = false;
    private int _currentPosition = 0;

    public int CurrentLine { get; private set; } = 1;
    public int CurrentSymbolNumberInLine { get; private set; } = 1;

    private bool _endOfString => _currentPosition >= Text.Length;

    private char _symbol
    {
        get
        {
            char symbol = Text[ _currentPosition ];
            if ( symbol == '\n' )
            {
                CurrentLine++;
            }

            return symbol;
        }
    }

    public StringReaderService( string text )
    {
        Text = text;
    }

    public void Reset()
    {
        CurrentLine = 1;
        _currentPosition = 0;
        CurrentSymbolNumberInLine = 1;
    }

    public string NextWord()
    {
        ReadUntilWord();

        if ( _endOfString )
        {
            return "";
        }

        string word = "";

        while ( !Char.IsWhiteSpace( _symbol ) )
        {
            word += _symbol;

            _currentPosition++;
            CurrentSymbolNumberInLine++;
            if ( _endOfString )
            {
                break;
            }
        }

        ReadUntilWordOrEndOfLine();

        return word;
    }

    private void ReadUntilWordOrEndOfLine()
    {
        while ( !_endOfString && Char.IsWhiteSpace( _symbol ) && _symbol != '\r' && _symbol != '\n' )
        {
            _currentPosition++;
            CurrentSymbolNumberInLine++;
        }

        IsEndOfLine = _endOfString || _symbol == '\r' || _symbol == '\n';
        if ( IsEndOfLine )
        {
            CurrentSymbolNumberInLine = 1;
        }
    }

    private void ReadUntilWord()
    {
        while ( !_endOfString && Char.IsWhiteSpace( _symbol ) )
        {
            _currentPosition++;
            CurrentSymbolNumberInLine++;
        }
    }
}
