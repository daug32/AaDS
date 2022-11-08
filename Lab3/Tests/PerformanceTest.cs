using Lab3.Models;
using Lab3.Services;

namespace Lab3.Tests;

internal class PerformanceTest
{
    private static int _nextId = 1;
    private static CustomTree<int> _tree = new CustomTree<int>( new Int32Comparer() );
    private static List<(int, int)> _values = new List<(int, int)>();

    public const int MaxChildsCount = 10;
    public const int RelationsCount = 100000;

    private readonly static Random _random = new Random();

    public static void RunTests()
    {
        Test1();
    }

    private static void Test1()
    {
        GenerateChilds();

        Console.WriteLine( $"Generated! {DateTime.Now.ToString("mm:ss:ff")}" );

        _tree = new CustomTree<int>( _values, new Int32Comparer() );

        Console.WriteLine( $"Tree was generated {DateTime.Now.ToString("mm:ss:ff")}" );

        _tree.Optimize()
            .Select( value =>
            {
                (int, int) pair = (value, _tree.GetDepth( value ));
                return pair;
            } )
            .ToList()
            .ForEach( pair => Console.WriteLine( $"Optimal Id: {pair.Item1}; depth: {pair.Item2}" ) );
    }

    private static void GenerateChilds()
    {
        while ( _nextId < RelationsCount )
        {
            int target = _random.Next( 1, _nextId + 1 );
            int childsCount = _random.Next( 1, MaxChildsCount + 1 );

            for ( int child = 0; child < childsCount; child++ )
            {
                _values.Add( (target, ++_nextId) );
            }
        }
    }
}
