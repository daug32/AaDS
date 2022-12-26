namespace Lab4.Models;

public interface IGraph<T> where T : Vertex
{
    List<T> Vertices { get; }
    List<Edge<T>> Edges { get; }

    void AddEdge( Edge<T> edge );
    void AddEdges( IEnumerable<Edge<T>> edges );

    void AddVertex( T vertex );
    void AddVertices( IEnumerable<T> vertices );

    bool ContainsVertex( Vertex vertex );
    List<T> GetVerticesOrderedByAppearence( T start );
    IEnumerable<Edge<T>> GetEdgesToVertex( Vertex vertex );
    IEnumerable<Edge<T>> GetEdgesFromVertex( Vertex vertex );
}

public class Graph<T> : IGraph<T> where T : Vertex
{
    public List<T> Vertices { get; protected set; } = new List<T>();

    public List<Edge<T>> Edges { get; protected set; } = new List<Edge<T>>();

    public Graph( IEnumerable<Edge<T>>? edges = null, IEnumerable<T>? vertices = null )
    {
        if ( vertices != null )
        {
            AddVertices( vertices );
        }

        if ( edges != null )
        {
            AddEdges( edges );
        }
    }

    public void AddVertices( IEnumerable<T> vertices )
    {
        foreach ( var vertex in vertices )
        {
            AddVertex( vertex );
        }
    }

    public void AddVertex( T vertex )
    {
        if ( ContainsVertex( vertex ) )
        {
            throw new IndexOutOfRangeException( "Vertex already exists in the graph." );
        }

        Vertices.Add( vertex );
    }

    public void AddEdges( IEnumerable<Edge<T>> edges )
    {
        foreach ( var edge in edges )
        {
            AddEdge( edge );
        }
    }

    public void AddEdge( Edge<T> edge )
    {
        if ( edge.VertexFrom != null && !ContainsVertex( edge.VertexFrom ) ||
             edge.VertexTo != null && !ContainsVertex( edge.VertexTo ) )
        {
            throw new IndexOutOfRangeException( "Edge reffer to a vertex that doesn't exist." );
        }

        Edges.Add( edge );
    }

    public bool ContainsVertex( Vertex vertex )
    {
        return Vertices.Any( x => x.Id == vertex.Id );
    }

    public IEnumerable<Edge<T>> GetEdgesFromVertex( Vertex vertex )
    {
        return Edges
            .Where( edge =>
                edge.VertexFrom != null &&
                edge.VertexFrom.Id == vertex.Id );
    }

    public IEnumerable<Edge<T>> GetEdgesToVertex( Vertex vertex )
    {
        return Edges
            .Where( edge =>
                edge.VertexTo != null &&
                edge.VertexTo.Id == vertex.Id );
    }

    public List<T> GetVerticesOrderedByAppearence( T start )
    {
        var result = new List<T>( Vertices.Count ) { start };

        for ( int i = 0; i < result.Count; i++ )
        {
            T currentVertex = result[ i ];

            foreach ( Edge<T> edge in GetEdgesFromVertex( currentVertex ) )
            {
                T? relatedVertex = edge.VertexTo;
                if ( relatedVertex == null )
                {
                    continue;
                }

                if ( result.Any( vertex => vertex.Id == relatedVertex.Id ) )
                {
                    continue;
                }

                result.Add( relatedVertex );
            }
        }

        return result;
    }
}