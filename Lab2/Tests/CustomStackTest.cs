using Lab2.Models;

namespace Lab2.Tests;

internal class CustomStackTest
{
    private static CustomStack<int> _stack;
    
    public static void RunTests()
    {
        TestEmpty();

        TestPushAndPop();
        TestPushAndPop( 1 );
        TestPushAndPop( 1, 2, 3, 4, 5 );
    }

    private static void TestPushAndPop( params int[] items )
    {
        _stack = new CustomStack<int>();

        foreach ( var item in items )
        {
            _stack.Push( item );
        }

        for ( int i = items.Length - 1; i > -1; i-- )
        {
            var targetValue = items[ i ];
            var actualValue = _stack.Pop();

            if ( targetValue != actualValue )
            {
                throw new Exception();
            }
        }
    }

    private static void TestEmpty()
    {
        _stack = new CustomStack<int>();

        try
        {
            _stack.Pop();
        }
        catch ( IndexOutOfRangeException ex )
        {
            return;
        }

        throw new Exception();
    }
}
