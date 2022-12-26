using Lab3.Services;

namespace Lab3.Tests;

internal class TreeReaderServiceTest
{
    public static void RunTests()
    {
        Test1();
        Test2();
        Test3();
        Test4();
        Test5();
        Test6();

        Console.WriteLine( "TreeReaderService test is success" );
    }

    private static void Test1()
    {
        Test( @"4
            2 1
            3 1
            1 4", 3 );
    }

    private static void Test2()
    {
        Test( @"4
            2 1
            3 1
            4 1", 3 );
    }

    private static void Test3()
    {
        Test( @"4
            1 4
            2 4
            4 3", 3 );
    }

    private static void Test4()
    {
        Test( @"12
            6 1
            13 1
            4 1
            5 1
            3 1
            2 3
            7 3
            8 3
            10 8
            9 8
            12 8", 11 );
    }

    private static void Test5()
    {
        Test( @"10
            1 9
            1 7
            1 8
            1 5
            1 10
            5 12
            12 13
            10 6
            10 11
            11 2", 10 );
    }

    private static void Test6()
    {
        Test( @"4
            1 2
            4 3
            2 3", 3 );
    }

    private static void Test( string data, int expectedNodesNumber )
    {
        var ms = new MemoryStream();

        var writer = new StreamWriter( ms );
        writer.Write( data );
        writer.Flush();

        ms.Position = 0;

        var reader = new StreamReader( ms );
        var treeReader = new TreeReaderService( reader );
        var treeNodes = treeReader.ReadFile();

        if ( treeNodes.Count != expectedNodesNumber )
        {
            throw new Exception();
        }
    }
}
