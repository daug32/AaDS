using Lab4.Models;

namespace Lab4.Services;

public class ManufactureReader
{
    public static List<(int, int)> GetConnections( string inputFile )
    {
        if ( !File.Exists( inputFile ) )
        {
            throw new ArgumentException( $"File {inputFile} doesn't exist" );
        }

        var reader = new StreamReader( inputFile );

        var result = GetConnections( reader );

        reader.Close();
        reader.Dispose();

        return result;
    }

    public static List<(int, int)> GetConnections( StreamReader reader )
    {
        string recordSeparator = "\n";
        string columnsSeparator = " ";

        var result = new List<(int, int)>();

        foreach ( string record in reader.ReadToEnd().Split( recordSeparator ) )
        {
            if ( String.IsNullOrEmpty( record.Trim() ) )
            {
                continue;
            }

            var keys = record.Trim().Split( columnsSeparator );

            if ( keys.Length != 2 )
            {
                throw new ArgumentException( $"Invalid format. Count of keys found in the line: {keys.Length}" );
            }

            if ( !Int32.TryParse( keys[ 0 ], out int id1 ) )
            {
                throw new ArgumentException( $"Invalid format. {keys[ 0 ]} is not integer" );
            }

            if ( !Int32.TryParse( keys[ 1 ], out int id2 ) )
            {
                throw new ArgumentException( $"Invalid format. {keys[ 1 ]} is not integer" );
            }

            result.Add( (id1, id2) );
        }

        return result;
    }

    public static List<Operation> GetOperations( string inputFile )
    {
        if ( !File.Exists( inputFile ) )
        {
            throw new ArgumentException( $"File {inputFile} doesn't exist" );
        }

        var reader = new StreamReader( inputFile );

        var result = GetOperations( reader );

        reader.Close();
        reader.Dispose();

        return result;
    }

    public static List<Operation> GetOperations( StreamReader reader )
    {
        string recordSeparator = "\n";
        string columnsSeparator = " ";

        var result = new List<Operation>();

        foreach ( string record in reader.ReadToEnd().Split( recordSeparator ) )
        {
            if ( String.IsNullOrEmpty( record.Trim() ) )
            {
                continue;
            }

            var keys = record.Split( columnsSeparator );

            if ( keys.Length != 3 )
            {
                throw new ArgumentException( $"Invalid format. Count of keys found in the line: {keys.Length}" );
            }

            if ( !Int32.TryParse( keys[ 0 ], out int id ) )
            {
                throw new ArgumentException( $"{keys[ 0 ]} is not integer" );
            }

            if ( !Int32.TryParse( keys[ 1 ], out int operationType ) )
            {
                throw new ArgumentException( $"{keys[ 1 ]} is not integer" );
            }

            if ( !Int32.TryParse( keys[ 2 ], out int operationCost ) )
            {
                throw new ArgumentException( $"{keys[ 2 ]} is not integer" );
            }

            var planer = new Operation()
            {
                Id = id,
                Type = operationType,
                Cost = operationCost,
            };

            result.Add( planer );
        }

        return result;
    }
}
