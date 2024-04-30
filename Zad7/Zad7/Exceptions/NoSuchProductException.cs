namespace Zad7.Exceptions;
[Serializable]
public class NoSuchProductException : Exception
{
    public NoSuchProductException ()
    {}

    public NoSuchProductException (string message) 
        : base(message)
    {}

    public NoSuchProductException (string message, Exception innerException)
        : base (message, innerException)
    {}  
}