using System.Net;

namespace TheDesignator.Exception.ExceptionsBase;

public class InvalidLoginException : TheDesignatorException
{
    public override List<string> GetErrorMessages() => [ResourceMessagesException.VALIDATION_LOGIN_INVALID];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
