using Lab4.Services;

namespace Lab4.Tests;

internal static class ManufactureReaderTest
{
    public static bool RunAll()
    {
        try
        {
            Reader_GetConnections_SimpleText();
            Reader_GetConnections_TextWithEmptyLines();
            Reader_GetConnections_TextWithLetters();
            Reader_GetConnections_ManyNumbersInTheSameLine();
        }
        catch ( Exception ex )
        {
            Console.WriteLine( $"{nameof( ManufactureReaderTest )}: {ex.Message}" );
            return false;
        }

        Console.WriteLine( $"{nameof( ManufactureReaderTest )} is success" );
        return true;
    }

    private static void Reader_GetConnections_SimpleText()
    {
        var text = @"
            1 2
            1 3 
            1 5
            2 3";

        var result = new List<(int, int)>()
        {
            (1, 2),
            (1, 3),
            (1, 5),
            (2, 3),
        };

        TestGetConnections( text, result );
    }

    private static void Reader_GetConnections_TextWithEmptyLines()
    {
        var text = @"
            1 2

            1 3 

            
            1 5
            2 3

            ";

        var result = new List<(int, int)>()
        {
            (1, 2),
            (1, 3),
            (1, 5),
            (2, 3),
        };

        TestGetConnections( text, result );
    }

    private static void Reader_GetConnections_TextWithLetters()
    {
        var text = @"
            1 2
            1 asd 
            1 3";

        TestGetConnections<ArgumentException>( text );
    }

    private static void Reader_GetConnections_ManyNumbersInTheSameLine()
    {
        var text = @"
            1 2
            1 2 3 
            1 3";

        TestGetConnections<ArgumentException>( text );
    }

    private static void TestGetConnections( string text, List<(int, int)> expected )
    {
        var reader = TextToStreamReader( text );
        var result = ManufactureReader.GetConnections( reader );

        if ( result.Count != expected.Count )
        {
            throw new Exception( "Count of results is not equal to count of expected" );
        }

        result.ForEach( connection =>
        {
            var exists = expected.Any( expectedConnection =>
                expectedConnection.Item1 == connection.Item1 &&
                expectedConnection.Item2 == connection.Item2 );

            if ( !exists )
            {
                throw new Exception( $"Connection ({connection.Item1}, {connection.Item2}) doesn't exists in expected" );
            }
        } );
    }

    private static void TestGetConnections<T>( string text ) where T : Exception
    {
        try
        {
            var reader = TextToStreamReader( text );
            ManufactureReader.GetConnections( reader );
        }
        catch ( T )
        {
            return;
        }

        throw new Exception();
    }

    private static StreamReader TextToStreamReader( string text )
    {
        var ms = new MemoryStream();

        var writer = new StreamWriter( ms );
        writer.Write( text );
        writer.Flush();

        ms.Position = 0;

        return new StreamReader( ms );
    }
}
