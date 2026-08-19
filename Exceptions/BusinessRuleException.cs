namespace SPMS_PROJECT.Exceptions;

public class BusinessRuleException : Exception
{
    public int StatusCode { get; }

    public BusinessRuleException(string message, int statusCode)
        : base(message)
    {
        StatusCode = statusCode;
    }
}