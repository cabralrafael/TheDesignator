using System.Net;

namespace TheDesignator.Exception.ExceptionsBase;

public class ErrorOnValidationException : TheDesignatorException
{
    private readonly List<string> _errors;

    public ErrorOnValidationException(List<string> errorsMessages)
    {
        _errors = errorsMessages;
    }

    public override List<string> GetErrorMessages() => _errors;

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
}
