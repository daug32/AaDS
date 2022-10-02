namespace Lab2.Models;

public class CustomStack<T>
{
    private class CustomStackItem<T>
    {
        public T Value;
        public CustomStackItem<T>? Prev;
    }

    CustomStackItem<T>? Head;

    public CustomStack() 
    {
    }

    public bool IsEmpty => Head == null;

    public void Push( T data )
    {
        Head = new CustomStackItem<T>()
        {
            Value = data,
            Prev = Head
        };
    }

    public T Pop()
    {
        if ( Head == null )
        {
            throw new IndexOutOfRangeException();
        }

        T value = Head.Value;

        Head = Head.Prev;

        return value;
    }
}
