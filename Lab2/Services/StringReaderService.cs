namespace Lab2.Services;

public class StringReaderService
{
    public readonly string Text;

    public bool IsEndOfLine = false;

    private int _currentPosition = 0;

    private bool _endOfString => _currentPosition >= Text.Length;

    private char _symbol => Text[ _currentPosition ];

    public StringReaderService( string text )
    {
        Text = text;
    }

    public void Reset()
    {
        _currentPosition = 0;
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
        }

        IsEndOfLine = _endOfString || _symbol == '\r' || _symbol == '\n';
    }

    private void ReadUntilWord()
    {
        while ( !_endOfString && Char.IsWhiteSpace( _symbol ) )
        {
            _currentPosition++;
        }
    }
}
