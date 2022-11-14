using Lab3.Models;

namespace Lab3.Tests;

internal class CustomTreeTest
{
    public static void RunTests()
    {
        Test1();
        Test2();
        Test3();
        Test4();

        Console.WriteLine( "CustomTree test is success" );
    }

    private static void Test1()
    {
        var tree = new CustomTree<int>( new List<(int, int)>()
        {
            (1, 2),
            (1, 5),
            (2, 3),
            (2, 4),
        } );

        Test( tree, 1, 2 );
    }

    private static void Test2()
    {
        var tree = new CustomTree<int>( new List<(int, int)>()
        {
            (1, 2),
            (1, 3),
            (3, 4),
            (4, 5),
        } );

        Test( tree, 3 );
    }

    private static void Test3()
    {
        var tree = new CustomTree<int>( new List<(int, int)>()
        {
            (1, 2),
            (1, 3),
            (3, 4),
            (4, 5),
            (5, 6),
        } );

        Test( tree, 3, 4 );
    }

    private static void Test4()
    {
        var tree = new CustomTree<int>( new List<(int, int)>()
        {
            (1, 2),
            (2, 3),
            (2, 4),
            (4, 8),
            (4, 6),
            (4, 7),
            (8, 9),
            (9, 10),
        } );

        Test( tree, 4, 8 );
    }

    private static void Test( CustomTree<int> tree, params int[] expected )
    {
        List<int> optimalValues = tree.Optimize();
        if ( optimalValues.Count != expected.Length )
        {
            throw new Exception();
        }

        foreach ( var optimalValue in optimalValues )
        {
            bool exists = expected.Any( expectedValue => expectedValue == optimalValue );
            if ( !exists )
            {
                throw new Exception();
            }
        }
    }
}