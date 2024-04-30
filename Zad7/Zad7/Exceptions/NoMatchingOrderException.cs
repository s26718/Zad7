namespace Zad7.Exceptions;
[Serializable]
public class NoMatchingOrderException : Exception
{
    public NoMatchingOrderException ()
    {}

    public NoMatchingOrderException (string message) 
        : base(message)
    {}

    public NoMatchingOrderException (string message, Exception innerException)
        : base (message, innerException)
    {}  
}