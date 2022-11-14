using Lab3.Models;

namespace Lab3.Services;

public interface ICustomTreeBuilder<T>
{
    CustomTree<T> Build( List<(T, T)> values );
}

public class CustomTreeBuilder<T> : ICustomTreeBuilder<T>
{
    public CustomTree<T> Build( List<(T, T)> input )
    {
        if ( input.Count < 1 )
        {
            return new CustomTree<T>(  );
        }

        (T, T) pair = input[ 0 ];

        var tree = new CustomTree<T>( pair.Item1 );
        var existingNodes = new SortedSet<T>() { pair.Item1 };

        if ( input.Count == 1 )
        {
            tree.Add( pair.Item1, pair.Item2 );
            return tree;
        }

        while ( input.Count > 0 )
        {
            bool haveChanges = false;

            for ( int i = 0; i < input.Count; i++ )
            {
                pair = input[ i ];

                bool keyExists = existingNodes.Contains( pair.Item1 );
                bool valueExists = existingNodes.Contains( pair.Item2 );

                if ( keyExists && valueExists )
                {
                    var message = $"Invalid list of nodes. Duplicates were found: {pair.Item1}/{pair.Item2}";
                    throw new ArgumentException( message );
                }

                if ( keyExists )
                {
                    existingNodes.Add( pair.Item2 );
                    tree.Add( pair.Item1, pair.Item2 );
                    input.RemoveAt( i-- );
                    haveChanges = true;
                    continue;
                }

                if ( valueExists )
                {
                    existingNodes.Add( pair.Item1 );
                    tree.Add( pair.Item2, pair.Item1 );
                    input.RemoveAt( i-- );
                    haveChanges = true;
                    continue;
                }
            }

            if ( !haveChanges )
            {
                var message = $"Invalid list of nodes. An unlinked bundle of values were found: {pair.Item1}/{pair.Item2}";
                throw new ArgumentException( message );
            }
        }

        return tree;
    }
}
