namespace Lab3.Models;

internal class CustomTreeNode<T>
{
    public T Data;
    public CustomTreeNode<T>? Parent;
    public List<CustomTreeNode<T>> Childs = new List<CustomTreeNode<T>>();

    public CustomTreeNode( T data, CustomTreeNode<T> parent = null )
    {
        Data = data;
        Parent = parent;
    }

    public override string ToString()
    {
        return Data == null ? "Null" : Data.ToString();
    }
}
