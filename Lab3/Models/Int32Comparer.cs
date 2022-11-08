namespace Lab3.Models;

public class Int32Comparer : IComparer<Int32>
{
    public int Compare( int x, int y )
    {
        if ( x < y ) return -1;
        if ( x > y ) return 1;
        return 0;
    }
}
