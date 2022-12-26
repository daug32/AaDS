namespace Lab4.Models;

public class Operation : Vertex
{
    public int Type;
    public int Cost;

    public Operation() : base( 0 ) { }

    public Operation( int id, int type, int cost ) : base( id )
    {
        Type = type;
        Cost = cost;
    }

    public static Operation EmptyPlaner( int id = 0 )
    {
        return new Operation()
        {
            Id = id,
            Type = 0,
            Cost = 0,
        };
    }

    public override string ToString() => $"Id: {Id}, Type: {Type}, Cost: {Cost}";
}
