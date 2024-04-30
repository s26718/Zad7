namespace Zad7.Exceptions;
[Serializable]
public class AmountZeroOrLowerException : Exception
{
    public AmountZeroOrLowerException ()
    {}

    public AmountZeroOrLowerException (string message) 
        : base(message)
    {}

    public AmountZeroOrLowerException (string message, Exception innerException)
        : base (message, innerException)
    {}  
}