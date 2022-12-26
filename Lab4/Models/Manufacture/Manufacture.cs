using Lab4.Extensions;
using Lab4.Models.Graph;

namespace Lab4.Models;

public interface IManufacture
{
    List<Operation> Operations { get; }
    Operation? GetBestPathFrom( Operation start );
    public void Execute( Action<Operation, int> onOperationExecute );
}

public class Manufacture : Graph<Operation>, IManufacture
{
    public List<Operation> Operations
    {
        get => Vertices;
        protected set => Vertices = value;
    }

    public Operation EndOperation { get; protected set; }
    public Operation StartOperation { get; protected set; }

    public Manufacture(
        IEnumerable<Edge<Operation>> connections,
        IEnumerable<Operation> operations,
        Operation? startOperation = null,
        Operation? endOperation = null ) :
        base( connections, operations )
    {
        EndOperation = endOperation ?? operations.First( x => !GetEdgesFromVertex( x ).Any() );
        StartOperation = startOperation ?? operations.First( x => !GetEdgesToVertex( x ).Any() );
    }

    public Operation? GetBestPathFrom( Operation start )
    {
        if ( start == null )
        {
            throw new NullReferenceException();
        }

        if ( !ContainsVertex( start ) )
        {
            throw new ArgumentOutOfRangeException( $"Operation {start.Id} doesn't exist" );
        }

        var path = this.DijkstraSearch( start.Id, EndOperation.Id, PathSerachingType.LongestWay );
        if ( path.Count == 1 && path.First().VertexFrom!.Id == path.First().VertexTo!.Id )
        {
            return null;
        }

        return path.FirstOrDefault()?.VertexTo;
    }

    public void Execute( Action<Operation, int> onOperationExecute )
    {
        List<Edge<Operation>> orderedOperations = this
            .DijkstraMap( StartOperation.Id, EndOperation.Id, PathSerachingType.LongestWay )
            .Select( pair => pair.Value )
            .Where( edge => edge.VertexTo!.Type != 0 )
            .OrderBy( edge =>
            {
                var end = edge.Weight;
                var start = edge.VertexTo!.Cost;

                return $"{end - start}{start}";
            } )
            .ToList();

        orderedOperations.ForEach( edge =>
        {
            if ( edge.VertexTo.Type != 0 )
            {
                onOperationExecute( edge.VertexTo!, edge.Weight );
            }
        } );
    }
}
