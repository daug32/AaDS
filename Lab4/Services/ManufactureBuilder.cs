using System.Data;
using Lab4.Models;

namespace Lab4.Services;

public class ManufactureBuilder
{
    public static Manufacture Build( List<Operation> operations, List<(int id1, int id2)> connections )
    {
        List<Edge<Operation>> edges = new List<Edge<Operation>>( connections.Count );

        foreach ( (int id1, int id2) connection in connections )
        {
            Operation? from = operations.FirstOrDefault( vertex => vertex.Id == connection.id1 );
            if ( from == null )
            {
                string message = $"Operation with given id ({connection.id1}) was not presented in the input file";
                throw new ArgumentOutOfRangeException( message );
            }

            Operation? to = operations.FirstOrDefault( vertex => vertex.Id == connection.id2 );
            if ( to == null )
            {
                string message = $"Operation with given id ({connection.id2}) was not presented in the input file";
                throw new ArgumentOutOfRangeException( message );
            }

            var edge = new Edge<Operation>( from, to, to.Cost );

            edges.Add( edge );
        };

        return new Manufacture( edges, operations );
    }
}
