using Lab4.Models;
using Lab4.Models.Graph;

namespace Lab4.Extensions;

public static class DijkstraExtension
{
    /// <returns>
    /// Returns path from the start to the end in format (start, end].
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Throws if the start or the end don't exist
    /// </exception>
    public static List<Edge<T>> DijkstraSearch<T>(
        this Graph<T> graph,
        int startId,
        int endId,
        PathSerachingType searchingType = PathSerachingType.ShortestWay )
        where T : Vertex
    {
        var visitedVertices = graph.DijkstraMap( startId, endId, searchingType );

        var start = graph.Vertices.First( vertex => vertex.Id == startId );
        if ( startId == endId )
        {
            return new List<Edge<T>>() { new Edge<T>( start, start, 0 ) };
        }

        var end = graph.Vertices.First( vertex => vertex.Id == endId );

        var path = new List<Edge<T>>();

        var currentVertex = end;
        while ( currentVertex.Id != start.Id )
        {
            var from = visitedVertices[ currentVertex ].VertexFrom!;
            var edge = graph.Edges
                .FirstOrDefault( edge =>
                    edge.VertexFrom?.Id == from?.Id &&
                    edge.VertexTo?.Id == currentVertex?.Id );

            // No way to the target
            if ( edge == null )
            {
                return path;
            }

            path.Add( new Edge<T>( from, currentVertex, edge.Weight ) );

            currentVertex = from;
        }

        path.Reverse();

        return path;
    }

    /// <returns>
    /// Returns path from the start to the end in format (start, end].
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Throws if the start or the end don't exist
    /// </exception>
    public static List<Edge<T>> DijkstraSearch<T>(
        this Graph<T> graph,
        int startId,
        int endId,
        Dictionary<T, Edge<T>> visitedVertices )
        where T : Vertex
    {
        var start = graph.Vertices.FirstOrDefault( x => x.Id == startId );
        if ( start == null )
        {
            throw new ArgumentOutOfRangeException( $"Vertix with given id {startId} doesn't exist" );
        }

        if ( startId == endId )
        {
            return new List<Edge<T>>() { new Edge<T>( start, start, 0 ) };
        }

        var end = graph.Vertices.FirstOrDefault( x => x.Id == endId );
        if ( end == null )
        {
            throw new ArgumentOutOfRangeException( $"Vertix with given id {endId} doesn't exist" );
        }

        var path = new List<Edge<T>>();

        var currentVertex = end;
        while ( currentVertex.Id != start.Id )
        {
            var from = visitedVertices[ currentVertex ].VertexFrom!;
            var edge = graph.Edges
                .FirstOrDefault( edge =>
                    edge.VertexFrom?.Id == from?.Id &&
                    edge.VertexTo?.Id == currentVertex?.Id );

            // No way to the target
            if ( edge == null )
            {
                return path;
            }

            path.Add( new Edge<T>( from, currentVertex, edge.Weight ) );

            currentVertex = from;
        }

        path.Reverse();

        return path;
    }

    public static Dictionary<T, Edge<T>> DijkstraMap<T>(
        this Graph<T> graph,
        int startId,
        int endId,
        PathSerachingType searchingType = PathSerachingType.ShortestWay )
        where T : Vertex
    {
        // Validation 

        var start = graph.Vertices.FirstOrDefault( x => x.Id == startId );
        if ( start == null )
        {
            throw new ArgumentOutOfRangeException( $"Vertix with given id {startId} doesn't exist" );
        }

        if ( startId == endId )
        {
            return new Dictionary<T, Edge<T>>()
            {
                { start, new Edge<T>( start, start, 0 ) }
            };
        }

        var end = graph.Vertices.FirstOrDefault( x => x.Id == endId );
        if ( end == null )
        {
            throw new ArgumentOutOfRangeException( $"Vertix with given id {endId} doesn't exist" );
        }

        // Initialization

        List<T> unvisitedVertices = graph.GetVerticesOrderedByAppearence( start );

        Dictionary<T, Edge<T>> visitedVertices = new Dictionary<T, Edge<T>>( graph.Vertices.Count );
        int initialValue = GetInitialValueForDijkstraSearching( searchingType );
        foreach ( var vertex in graph.Vertices )
        {
            var edge = new Edge<T>( null, vertex, initialValue );
            visitedVertices.Add( vertex, edge );
        }
        visitedVertices[ start ].Weight = 0;

        // Searching

        while ( unvisitedVertices.Any() )
        {
            var currentVertex = GetNextVertexToProcess( searchingType, unvisitedVertices, visitedVertices );

            unvisitedVertices.Remove( currentVertex );

            var edges = graph.GetEdgesFromVertex( currentVertex );
            foreach ( var edge in edges )
            {
                var connectedVertex = edge.VertexTo;
                if ( connectedVertex == null )
                {
                    continue;
                }

                var cost = visitedVertices[ currentVertex ].Weight + edge.Weight;
                if ( NeedToUpdate( searchingType, cost, visitedVertices[ connectedVertex ].Weight ) )
                {
                    visitedVertices[ connectedVertex ].Weight = cost;
                    visitedVertices[ connectedVertex ].VertexFrom = currentVertex;
                }
            }
        }

        return visitedVertices;
    }

    private static T GetNextVertexToProcess<T>( PathSerachingType searchingType, IEnumerable<T> unvisited, Dictionary<T, Edge<T>> dist ) where T : Vertex
    {
        return unvisited.First();

        if ( searchingType == PathSerachingType.LongestWay )
        {
            return unvisited.MaxBy( x => dist[ x ].Weight )!;
        }

        return unvisited.MinBy( x => dist[ x ].Weight )!;
    }

    private static bool NeedToUpdate( PathSerachingType searchingType, int cost, int dist )
    {
        if ( searchingType == PathSerachingType.LongestWay )
        {
            return cost > dist;
        }

        return cost < dist;
    }

    private static int GetInitialValueForDijkstraSearching( PathSerachingType searchingType )
    {
        if ( searchingType == PathSerachingType.LongestWay )
        {
            return Int32.MinValue;
        }

        return Int32.MaxValue;
    }
}

