namespace Zad7.Exceptions;
[Serializable]
public class NoSuchWarehouseException : Exception
{
    public NoSuchWarehouseException ()
    {}

    public NoSuchWarehouseException (string message) 
        : base(message)
    {}

    public NoSuchWarehouseException (string message, Exception innerException)
        : base (message, innerException)
    {}  
}