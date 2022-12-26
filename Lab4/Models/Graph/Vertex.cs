namespace Lab4.Models;

public class Vertex
{
    public int Id { get; set; }

    public Vertex( int id ) => Id = id;

    public override string ToString() => $"Vertex {Id}";
}