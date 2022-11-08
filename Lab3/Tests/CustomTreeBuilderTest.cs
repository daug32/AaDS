using Lab3.Models;
using Lab3.Services;

namespace Lab3.Tests;

internal static class CustomTreeBuilderTest
{
    private static readonly CustomTreeBuilder<int> _builder = new CustomTreeBuilder<int>( new Int32Comparer() );

    public static void RunTests()
    {
        Test1();
        Test2();
        Test3();
        Test4();    

        Console.WriteLine( "CustomTreeBuilder test is success" );
    }

    private static void Test1()
    {
        var input = new List<(int, int)>()
        {
            (1, 2),
            (4, 3),
            (2, 3)
        };

        var expected = new CustomTree<int>( new List<(int, int)>()
        {
            (1, 2),
            (2, 3),
            (3, 4)
        }, new Int32Comparer() );

        Test( input, expected );
    }

    private static void Test2()
    {
        var input = new List<(int, int)>()
        {
            (2, 1),
            (3, 1),
            (5, 4),
            (1, 4)
        };

        var expected = new CustomTree<int>( new List<(int, int)>()
        {
            (1, 2),
            (1, 3),
            (1, 4),
            (4, 5)
        }, new Int32Comparer() );

        Test( input, expected );
    }

    private static void Test3()
    {
        var input = new List<(int, int)>()
        {
            (2, 1),
            (3, 1),
            (5, 4)
        };

        try
        {
            Test( input, new CustomTree<int>( new Int32Comparer() ) );
        }
        catch (ArgumentException)
        {
            return;
        }

        throw new Exception();
    }

    private static void Test4()
    {
        var input = new List<(int, int)>()
        {
            (2, 1),
            (3, 1),
            (5, 4),
            (1, 4),
            (1, 4)
        };

        try
        {
            Test( input, new CustomTree<int>( new Int32Comparer() ) );
        }
        catch ( ArgumentException )
        {
            return;
        }

        throw new Exception();
    }

    private static void Test( List<(int, int)> input, CustomTree<int> expected )
    {
        var result = _builder.Build( input );

        expected.ForEach( el =>
        {
            if ( !result.Exists( el ) )
            {
                throw new Exception( $"Item {el} doesn't exists in the built tree" );
            }
        } );
    }
}
