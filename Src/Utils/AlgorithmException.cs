using System;

public class AlgorithmException : SystemException
{

    public AlgorithmException () : base()
    {
    }
    public AlgorithmException (string message) : base(message)
    {
    }
    public AlgorithmException (string message, Exception innerException) : base(message, innerException)
    {
    }
}