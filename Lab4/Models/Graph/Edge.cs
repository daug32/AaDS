namespace Lab4.Models;

public class Edge<T> where T : Vertex
{
    public int Weight;
    public T? VertexTo;
    public T? VertexFrom;

    public Edge( T from, T to, int weight )
    {
        VertexFrom = from;
        VertexTo = to;
        Weight = weight;
    }

    public override string ToString()
    {
        var from = VertexFrom?.Id.ToString() ?? "null";
        var to = VertexTo?.Id.ToString() ?? "null";

        return $"Edge from {from} to {to} cost {Weight}";
    }
}
