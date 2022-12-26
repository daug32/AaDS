using Lab4.Extensions;
using Lab4.Models;

namespace Lab4.Tests;

internal static class PathFinderTest
{
    public static bool RunAll()
    {
        try
        {
            DijkstraSearch_NoPathBetweenStartAndEnd_EmptyPath( BuildTestGraph1(), 2, 1 );
            DijkstraSearch_StartIsEnd_PathWithOneEdge( BuildTestGraph1(), 1 );
            DijkstraSearch_StartIsEnd_PathWithOneEdge( BuildTestGraph1(), 6 );

            DijkstraSearch_StartAndEndExist_ShortestWay( BuildTestGraph1(), 1, 2, 3, new int[] { 2 } );
            DijkstraSearch_StartAndEndExist_ShortestWay( BuildTestGraph1(), 1, 5, 3, new int[] { 4, 5 } );
            DijkstraSearch_StartAndEndExist_ShortestWay( BuildTestGraph1(), 1, 6, 4, new int[] { 4, 5, 6 } );
            DijkstraSearch_StartAndEndExist_ShortestWay( BuildTestGraph1(), 4, 6, 3, new int[] { 5, 6 } );

            DijkstraSearch_StartAndEndExist_LongestWay( BuildTestGraph1(), 1, 6, 5 );
            DijkstraSearch_StartAndEndExist_LongestWay( BuildTestGraph1(), 4, 6, 4 );

            DijkstraSearch_StartAndEndExist_LongestWay( BuildTestGraph2(), 1, 10, 9, new int[] { 2, 6, 8, 10 } );
        }
        catch ( Exception ex )
        {
            Console.WriteLine( $"{nameof( PathFinderTest )}: {ex.Message}" );
            return false;
        }

        Console.WriteLine( $"{nameof( PathFinderTest )} is success" );
        return true;
    }

    private static void DijkstraSearch_NoPathBetweenStartAndEnd_EmptyPath( Graph<Vertex> graph, int startId, int endId )
    {
        // Act 
        var path = graph.DijkstraSearch( startId, endId );

        // Assert
        if ( path.Count > 0 )
        {
            throw new Exception( "Expected empty path" );
        }
    }

    private static void DijkstraSearch_StartIsEnd_PathWithOneEdge( Graph<Vertex> graph, int start )
    {
        // Act 
        var path = graph.DijkstraSearch( start, start );

        // Assert
        if ( path.Count != 1 )
        {
            throw new Exception( "Expected path with one edge" );
        }

        var edge = path.First();
        if ( edge.VertexFrom.Id != edge.VertexTo.Id )
        {
            throw new Exception( "Expected path that starts from the same vertex as the end" );
        }
    }

    private static void DijkstraSearch_StartAndEndExist_ShortestWay(
        Graph<Vertex> graph,
        int startId,
        int endId,
        int expectedCost,
        int[]? expectedPath = null )
    {
        // Act 
        var path = graph.DijkstraSearch( startId, endId );

        // Assert
        var resultCost = path.Sum( edge => edge.Weight );
        if ( resultCost != expectedCost )
        {
            throw new Exception( $"Expected cost {expectedCost}, but got {resultCost}" );
        }

        if ( path.Count != expectedPath.Length )
        {
            throw new Exception();
        }

        if ( expectedPath != null )
        {
            for ( int i = 0; i < expectedPath.Length; i++ )
            {
                var resultVertex = path[ i ].VertexTo!.Id;
                var expectedVertex = expectedPath[ i ];

                if ( resultVertex != expectedVertex )
                {
                    throw new Exception( $"At the step {i} expected {expectedVertex}, but got {resultVertex}" );
                }
            }
        }
    }

    private static void DijkstraSearch_StartAndEndExist_LongestWay(
        Graph<Vertex> graph,
        int startId,
        int endId,
        int expectedCost, 
        int[]? expectedPath = null )
    {
        // Act 
        var path = graph.DijkstraSearch( startId, endId, Models.Graph.PathSerachingType.LongestWay );

        // Assert
        var resultCost = path.Sum( edge => edge.Weight );
        if ( resultCost != expectedCost )
        {
            throw new Exception( $"Expected cost {expectedCost}, but got {resultCost}" );
        }

        if ( expectedPath != null )
        {
            for ( int i = 0; i < expectedPath.Length; i++ )
            {
                var expected = expectedPath[ i ];
                var result = path[ i ];

                if ( expected != result.VertexTo!.Id )
                {
                    throw new Exception( $"Expected Operation {expected} at the step {i}, but {result.VertexTo.Id}" );
                }
            }
        }
    }

    private static Graph<Vertex> BuildTestGraph1()
    {
        var vertices = new List<Vertex>()
        {
            new Vertex(1),
            new Vertex(2),
            new Vertex(3),
            new Vertex(4),

            new Vertex(5),
            new Vertex(6),
            new Vertex(7)
        };

        var edges = new List<Edge<Vertex>>()
        {
            new Edge<Vertex>( vertices[1 - 1], vertices[2 - 1], 3),
            new Edge<Vertex>( vertices[1 - 1], vertices[4 - 1], 1),
            new Edge<Vertex>( vertices[2 - 1], vertices[3 - 1], 1),
            new Edge<Vertex>( vertices[3 - 1], vertices[6 - 1], 1),

            new Edge<Vertex>( vertices[3 - 1], vertices[7 - 1], 2),
            new Edge<Vertex>( vertices[4 - 1], vertices[3 - 1], 3),
            new Edge<Vertex>( vertices[4 - 1], vertices[5 - 1], 2),
            new Edge<Vertex>( vertices[5 - 1], vertices[6 - 1], 1),
        };

        return new Graph<Vertex>( edges, vertices );
    }

    private static Graph<Vertex> BuildTestGraph2()
    {
        var vertices = new List<Vertex>()
        {
            new Vertex( 1 ),
            new Vertex( 2 ),
            new Vertex( 3 ),
            new Vertex( 4 ),

            new Vertex( 5 ),
            new Vertex( 6 ),
            new Vertex( 7 ),
            new Vertex( 8 ),

            new Vertex( 9 ),
            new Vertex( 10 )
        };

        var edges = new List<Edge<Vertex>>()
        {
            new Edge<Vertex>(vertices[1 - 1], vertices[2 - 1], 2),
            new Edge<Vertex>(vertices[1 - 1], vertices[3 - 1], 2),
            new Edge<Vertex>(vertices[1 - 1], vertices[4 - 1], 4),
            new Edge<Vertex>(vertices[1 - 1], vertices[5 - 1], 8),

            new Edge<Vertex>(vertices[2 - 1], vertices[6 - 1], 5),
            new Edge<Vertex>(vertices[3 - 1], vertices[6 - 1], 5),

            new Edge<Vertex>(vertices[3 - 1], vertices[7 - 1], 1),
            new Edge<Vertex>(vertices[4 - 1], vertices[7 - 1], 1),

            new Edge<Vertex>(vertices[6 - 1], vertices[8 - 1], 2),
            new Edge<Vertex>(vertices[7 - 1], vertices[8 - 1], 2),

            new Edge<Vertex>(vertices[4 - 1], vertices[9 - 1], 3),

            new Edge<Vertex>(vertices[8 - 1], vertices[10 - 1], 0),
            new Edge<Vertex>(vertices[9 - 1], vertices[10 - 1], 0),
            new Edge<Vertex>(vertices[5 - 1], vertices[10 - 1], 0)
        };

        return new Graph<Vertex>( edges, vertices );
    }
}
