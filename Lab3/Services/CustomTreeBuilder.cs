using Lab3.Models;

namespace Lab3.Services;

public interface ICustomTreeBuilder<T>
{
    CustomTree<T> Build( List<(T, T)> values, IEqualityComparer<T> equalityComparer );
}

public class CustomTreeBuilder<T> : ICustomTreeBuilder<T>
{
    private readonly IComparer<T> _comparer;

    public CustomTreeBuilder( IComparer<T> comparer )
    {
        _comparer = comparer;
    }

    public CustomTree<T> Build( List<(T, T)> input, IEqualityComparer<T> equalityComparer )
    {
        if ( input.Count < 1 )
        {
            return new CustomTree<T>( equalityComparer );
        }

        (T, T) pair = input[ 0 ];

        var tree = new CustomTree<T>( pair.Item1, equalityComparer );
        var existingNodes = new List<T>() { pair.Item1 };

        while ( input.Count > 0 )
        {
            bool haveChanges = false;

            for ( int i = 0; i < input.Count; i++ )
            {
                pair = input[ i ];

                bool keyExists = false; 
                bool valueExists = false;
                foreach ( var item in existingNodes )
                {
                    if ( _comparer.Compare( item, pair.Item1 ) == 0 )
                    {
                        keyExists = true;
                        continue;
                    }

                    if ( _comparer.Compare( item, pair.Item2 ) == 0 )
                    {
                        valueExists = true;
                    }
                }

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
