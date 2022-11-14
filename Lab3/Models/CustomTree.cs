namespace Lab3.Models;

internal interface ICustomTreeDebug<T>
{
    bool IsEmpty { get; }

    void Add( T key, T value );

    void ForEach( Action<T> func );

    bool Exists( T data );

    List<T> Optimize();

    int GetDepth( T targetValue );
}

public class CustomTree<T> : ICustomTreeDebug<T>
{
    private CustomTreeNode<T>? _head;
    private Dictionary<T, CustomTreeNode<T>> _items;

    private readonly IEqualityComparer<T> _comparer;

    public bool IsEmpty => _items.Count == 0;

    public CustomTree( IEqualityComparer<T> comparer )
    {
        _head = null;
        _items = new Dictionary<T, CustomTreeNode<T>>( comparer );
    }

    public CustomTree( T initialData, IEqualityComparer<T> comparer )
    {
        _head = new CustomTreeNode<T>( initialData );
        _items = new Dictionary<T, CustomTreeNode<T>>( comparer )
        {
            { initialData, _head }
        };
    }

    public CustomTree( List<(T, T)> relatedData, IEqualityComparer<T> comparer )
    {
        _comparer = comparer;
        _items = new Dictionary<T, CustomTreeNode<T>>( comparer );

        if ( relatedData == null || relatedData.Count < 1 )
        {
            return;
        }

        var pair = relatedData.First();
        _head = new CustomTreeNode<T>( pair.Item1 );
        _items.Add( pair.Item1, _head );
        AddWithoutCheckingHead( pair.Item1, pair.Item2 );

        for ( int i = 1; i < relatedData.Count; i++ )
        {
            var item = relatedData[ i ];
            AddWithoutCheckingHead( item.Item1, item.Item2 );
        }
    }

    public void ForEach( Action<T> func )
    {
        if ( IsEmpty )
        {
            return;
        }

        foreach ( var item in _items )
        {
            func( item.Key );
        }
    }

    public void Add( T value1, T value2 )
    {
        if ( IsEmpty )
        {
            _head = new CustomTreeNode<T>( value1 );
            _items.Add( value1, _head );
        }

        CustomTreeNode<T> parent;
        if ( _items.TryGetValue( value1, out parent ) )
        {
            var child = new CustomTreeNode<T>( value2, parent );
            parent.Childs.Add( child );
            _items.Add( value2, child );

            return;
        }

        if ( _items.TryGetValue( value2, out parent ) )
        {
            var child = new CustomTreeNode<T>( value1, parent );
            parent.Childs.Add( child );
            _items.Add( value1, child );

            return;
        }

        throw new IndexOutOfRangeException( $"Don't know where to place pair: {value1}/{value2}" );
    }

    private void AddWithoutCheckingHead( T value1, T value2 )
    {
        CustomTreeNode<T> parent;
        if ( _items.TryGetValue( value1, out parent ) )
        {
            var child = new CustomTreeNode<T>( value2, parent );
            parent.Childs.Add( child );
            _items.Add( value2, child );

            return;
        }

        if ( _items.TryGetValue( value2, out parent ) )
        {
            var child = new CustomTreeNode<T>( value1, parent );
            parent.Childs.Add( child );
            _items.Add( value1, child );

            return;
        }

        throw new IndexOutOfRangeException( $"Don't know where to place pair: {value1}/{value2}" );

    }

    public List<T> Optimize()
    {
        if ( _head == null )
        {
            return new List<T>();
        }

        while ( true )
        {
            (CustomTreeNode<T>? node, int depth) max = (null, int.MinValue);
            (CustomTreeNode<T>? node, int depth) secondMax = (null, int.MinValue);

            foreach ( var child in _head.Childs )
            {
                int childDepth = GetDepth( child );
                if ( childDepth > max.depth )
                {
                    secondMax.node = max.node;
                    secondMax.depth = max.depth;

                    max.node = child;
                    max.depth = childDepth;
                }
                else if ( childDepth > secondMax.depth )
                {
                    secondMax.node = child;
                    secondMax.depth = childDepth;
                }
            }

            int diff = max.depth - secondMax.depth;
            if ( diff == 0 )
            {
                return new List<T>()
                {
                    _head.Data
                };
            }

            if ( diff == 1 )
            {
                return new List<T>()
                {
                    _head.Data,
                    max.node!.Data
                };
            }

            SetHead( max.node! );
        }
    }

    public int GetDepth( T targetValue )
    {
        if ( IsEmpty )
        {
            return 0;
        }

        if ( _items.TryGetValue( targetValue, out var node ) )
        {
            return GetDepth( node );
        }

        return 0;
    }

    private int GetDepth( CustomTreeNode<T> node )
    {
        if ( node.Childs.Count < 1 )
        {
            return 0;
        }

        int depth = 1;
        foreach ( var child in node.Childs )
        {
            int childsDepth = GetDepth( child );
            if ( childsDepth + 1 > depth )
            {
                depth = childsDepth + 1;
            }
        }

        return depth;
    }

    private void SetHead( CustomTreeNode<T> node )
    {
        CustomTreeNode<T>? prev = null;
        CustomTreeNode<T>? curr = node;
        CustomTreeNode<T>? next = node.Parent;

        while ( next != null )
        {
            next.Childs.Remove( curr );
            curr.Childs.Add( next );
            curr.Parent = prev;

            var save = next.Parent;
            next.Parent = curr;

            prev = curr;
            curr = next;
            next = save;
        }

        _head = node;
    }

    public bool Exists( T data )
    {
        if ( IsEmpty )
        {
            return false;
        }

        return _items.ContainsKey( data );
    }
}
