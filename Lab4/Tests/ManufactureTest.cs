using Lab4.Models;
using Lab4.Services;

namespace Lab4.Tests;

internal static class ManufactureTest
{
    public static bool RunAll()
    {
        try
        {
            Manufacture_GetBestPath_StartExistsButHasNoChilds_EmptyPath( 10 );

            Manufacture_GetBestPath_StartExists_NextOperation( 1, 2 );
            Manufacture_GetBestPath_StartExists_NextOperation( 2, 6 );
            Manufacture_GetBestPath_StartExists_NextOperation( 6, 8 );
            Manufacture_GetBestPath_StartExists_NextOperation( 8, 10 );

            Manufacture_GetBestPath_StartExists_NextOperation( 3, 6 );

            Manufacture_GetBestPath_StartExists_NextOperation( 8, 10 );

            Manufacture_Execute_OrderedExecution();
        }
        catch ( Exception ex )
        {
            Console.WriteLine( $"{nameof( ManufactureTest )}: {ex.Message}" );
            return false;
        }

        Console.WriteLine( $"{nameof( ManufactureTest )} is success" );
        return true;
    }

    private static void Manufacture_GetBestPath_StartExists_NextOperation( int startId, int expectedEndId )
    {
        // Arrange
        var manufacture = BuildTestManufacture();
        var start = manufacture.Operations.FirstOrDefault( x => x.Id == startId );
        if ( start == null )
        {
            throw new ArgumentOutOfRangeException( $"Given id {startId} doesn't exists" );
        }

        // Act
        var end = manufacture.GetBestPathFrom( start );

        // Assert
        if ( end == null )
        {
            throw new NullReferenceException();
        }

        if ( end.Id != expectedEndId )
        {
            throw new Exception( $"Expected {expectedEndId} but found {end.Id}" );
        }
    }

    private static void Manufacture_GetBestPath_StartExistsButHasNoChilds_EmptyPath( int startId )
    {
        // Arrange
        var manufacture = BuildTestManufacture();
        var start = manufacture.Operations.FirstOrDefault( x => x.Id == startId );
        if ( start == null )
        {
            throw new ArgumentOutOfRangeException( $"Given id {startId} doesn't exists" );
        }

        // Act
        var end = manufacture.GetBestPathFrom( start );

        // Assert
        if ( end != null )
        {
            throw new Exception( $"Expected null, but an object given {end.Id}" );
        }
    }

    private static void Manufacture_Execute_OrderedExecution()
    {
        var manufacture = BuildTestManufacture();
        manufacture.Execute( ( Operation operation, int time ) =>
        {
            OperationTypes type = ( OperationTypes )operation.Type;
            var message = $"Id: {operation.Id}; Type: {type}; [{time - operation.Cost}; {time}]";
            Console.WriteLine( message );
        } );
    }

    private static IManufacture BuildTestManufacture()
    {
        var connections = new List<(int, int)>()
        {
            (1, 2),
            (1, 3),
            (1, 4),
            (1, 5),

            (2, 6),

            (3, 6),
            (3, 7),

            (4, 7),
            (4, 9),

            (6, 8),
            (7, 8),

            (5, 10),
            (8, 10),
            (9, 10),
        };

        var operations = new List<Operation>()
        {
            new Operation(1, (int)OperationTypes.None, 0),

            new Operation(2, (int)OperationTypes.T, 2),
            new Operation(3, (int)OperationTypes.F, 2),
            new Operation(4, (int)OperationTypes.T, 4),
            new Operation(5, (int)OperationTypes.F, 8),

            new Operation(6, (int)OperationTypes.F, 5),
            new Operation(7, (int)OperationTypes.F, 1),
            new Operation(8, (int)OperationTypes.S, 2),
            new Operation(9, (int)OperationTypes.T, 3),

            new Operation(10, (int)OperationTypes.None, 0),
        };

        return ManufactureBuilder.Build( operations, connections );
    }
}
