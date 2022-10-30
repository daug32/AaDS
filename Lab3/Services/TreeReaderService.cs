using System.Linq;
using System.Xml.Linq;
using Lab3.Models;

namespace Lab3.Services;

public interface ITreeReaderService
{
    int ComputersNumber { get; }

    void Dispose();

    CustomTree<int> ReadFile( bool readAll = false );
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

    public CustomTree<int> ReadFile( bool readAll = false )
    {
        Dictionary<int, int> values;

        if ( readAll )
        {
            values = ReadAllInMemoryAndParse();
        }
        else
        {
            values = ReadAndParse();
        }

        _reader.Close();

        var tree = new CustomTree<int>();
        foreach ( var pair in values )
        {
            tree.Add( pair.Key, pair.Value );
        }

        return tree;
    }

    private Dictionary<int, int> ReadAndParse()
    {
        var result = new Dictionary<int, int>();

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

    private Dictionary<int, int> ReadAllInMemoryAndParse()
    {
        var result = new Dictionary<int, int>();

        string[] lines = _reader.ReadToEnd().Split( '\n' );
        foreach ( string line in lines )
        {
            string[] numbers = line.Trim().Split( ' ' );
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

    private void AddItem( int item1, int item2, Dictionary<int, int> result )
    {
        if ( !result.ContainsKey( item1 ) )
        {
            result.Add( item1, item2 );
            return;
        }

        if ( !result.ContainsKey( item2 ) )
        {
            result.Add( item2, item1 );
            return;
        }

        var canSwitch = result.FirstOrDefault(
            pair => pair.Key == item1 && !result.ContainsKey( pair.Value ),
            new KeyValuePair<int, int>( -1, -1 ) );
        if ( canSwitch.Key != -1 )
        {
            result.Remove( canSwitch.Key );
            result.Add( canSwitch.Value, canSwitch.Key );
            result.Add( item1, item2 );
            return;
        }

        canSwitch = result.FirstOrDefault(
            pair => pair.Key == item2 && !result.ContainsKey( pair.Value ),
            new KeyValuePair<int, int>( -1, -1 ) );
        if ( canSwitch.Key != -1 )
        {
            result.Remove( canSwitch.Key );
            result.Add( canSwitch.Value, canSwitch.Key );
            result.Add( item2, item1 );
            return;
        }
    }

    public void Dispose()
    {
        _reader.Close();
        _reader.Dispose();
    }
}
