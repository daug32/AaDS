namespace Lab3.Services;

public interface ITreeReaderService
{
    int ComputersNumber { get; }

    void Dispose();

    List<(int, int)> ReadFile( bool readAll = false );
}

public class TreeReaderService : IDisposable, ITreeReaderService
{
    public int ComputersNumber { get; init; }

    private StreamReader _reader { get; init; }

    public TreeReaderService( StreamReader reader )
    {
        _reader = reader;

        string? computers = _reader.ReadLine();
        if ( string.IsNullOrEmpty( computers ) || !int.TryParse( computers, out int computersNumber ) )
        {
            throw new ArgumentException( "The target input file is not correct" );
        }

        ComputersNumber = computersNumber;
    }

    public List<(int, int)> ReadFile( bool readAll = false )
    {
        List<(int, int)> values;

        if ( readAll )
        {
            values = ReadAllInMemoryAndParse();
        }
        else
        {
            values = ReadAndParse();
        }

        _reader.Close();

        return values;
    }

    private List<(int, int)> ReadAndParse()
    {
        var result = new List<(int, int)>();

        while ( !_reader.EndOfStream )
        {
            string? line = _reader.ReadLine()!.Trim();
            if ( string.IsNullOrWhiteSpace( line ) )
            {
                continue;
            }

            string[] numbers = line.Split( ' ' );
            if ( numbers.Length != 2 )
            {
                throw new ArgumentException( "The target input file is in incorrect format" );
            }

            int item1 = int.Parse( numbers.First() );
            int item2 = int.Parse( numbers.Last() );

            AddItem( item1, item2, result );
        }

        return result;
    }

    private List<(int, int)> ReadAllInMemoryAndParse()
    {
        var result = new List<(int, int)>();

        string[] lines = _reader.ReadToEnd().Split( '\n' );
        foreach ( string line in lines )
        {
            if ( String.IsNullOrWhiteSpace( line ) )
            {
                continue;
            }

            string[] numbers = line.Trim().Split( ' ' );
            if ( numbers.Length != 2 )
            {
                throw new ArgumentException( $"The target input file is in incorrect format:\n{line}." );
            }

            int item1 = int.Parse( numbers.First() );
            int item2 = int.Parse( numbers.Last() );

            AddItem( item1, item2, result );
        }

        return result;
    }

    private void AddItem( int item1, int item2, List<(int, int)> result )
    {
        result.Add( (item1, item2) );
    }

    public void Dispose()
    {
        _reader.Close();
        _reader.Dispose();
    }
}
