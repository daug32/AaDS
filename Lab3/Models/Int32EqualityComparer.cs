using System.Diagnostics.CodeAnalysis;

namespace Lab3.Models;

public class Int32EqualityComparer : IEqualityComparer<Int32>
{
    public bool Equals( int x, int y )
    {
        return x == y;
    }

    public int GetHashCode( [DisallowNull] int obj )
    {
        return obj.GetHashCode();
    }
}
