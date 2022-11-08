﻿namespace Lab3.Models;

internal interface ICustomTreeDebug<T>
{
    T Head { get; set; }

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
    private readonly IComparer<T> _comparer;

    public T Head
    {
        get
        {
            if ( IsEmpty )
            {
                throw new ArgumentOutOfRangeException();
            }

            return _head!.Data;
        }

        set
        {
            if ( IsEmpty )
            {
                _head = new CustomTreeNode<T>( value );
                return;
            }

            _head!.Data = value;
        }
    }

    public bool IsEmpty => _head == null;

    public CustomTree( IComparer<T> comparer )
    {
        _head = null;
        _comparer = comparer;
    }

    public CustomTree( T initialData, IComparer<T> comparer )
    {
        _head = new CustomTreeNode<T>( initialData );
        _comparer = comparer;
    }

    public CustomTree( List<(T, T)> relatedData, IComparer<T> comparer )
    {
        _comparer = comparer;

        if ( relatedData == null || relatedData.Count < 1 )
        {
            return;
        }

        for ( int i = 0; i < relatedData.Count; i++ )
        {
            var item = relatedData[ i ];
            Add( item.Item1, item.Item2 );
        }
    }

    public void ForEach( Action<T> func )
    {
        if ( IsEmpty )
        {
            return;
        }

        func( _head!.Data );
        Iterate( ( node ) => func( node.Data ), _head.Childs );
    }

    public void Add( T value1, T value2 )
    {
        if ( IsEmpty )
        {
            _head = new CustomTreeNode<T>( value1 );
            _head.Childs.Add( new CustomTreeNode<T>( value2, _head ) );
            return;
        }

        if ( _comparer.Compare( value1, _head!.Data ) == 0 )
        {
            _head.Childs.Add( new CustomTreeNode<T>( value2, _head ) );
            return;
        }

        if ( _comparer.Compare( value2, _head!.Data ) == 0 )
        {
            _head.Childs.Add( new CustomTreeNode<T>( value1, _head ) );
            return;
        }

        var parent = Find( value1, _head!.Childs );
        if ( parent != null )
        {
            parent.Childs.Add( new CustomTreeNode<T>( value2, parent ) );
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

        var node = Find( targetValue, new List<CustomTreeNode<T>> { _head } );

        return node == null ? 0 : GetDepth( node );
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

    private CustomTreeNode<T>? Find( T key, List<CustomTreeNode<T>> nodes )
    {
        foreach ( var node in nodes )
        {
            if ( _comparer.Compare( node!.Data, key ) == 0 )
            {
                return node;
            }
        }

        foreach ( var node in nodes )
        {
            if ( node.Childs.Count < 1 )
            {
                continue;
            }

            var result = Find( key, node.Childs );

            if ( result != null )
            {
                return result;
            }
        }

        return null;
    }

    private void Iterate( Action<CustomTreeNode<T>> func, List<CustomTreeNode<T>> nodes )
    {
        foreach ( var node in nodes )
        {
            func( node );
        }

        foreach ( var node in nodes )
        {
            Iterate( func, node.Childs );
        }
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

        return Find( data, new List<CustomTreeNode<T>>() { _head! } ) != null;
    }
}
