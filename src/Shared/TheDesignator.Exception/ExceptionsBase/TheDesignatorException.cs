using System.ComponentModel;
using System.Net;

namespace TheDesignator.Exception.ExceptionsBase;

public abstract class TheDesignatorException : System.Exception
{
    public abstract HttpStatusCode GetStatusCode();

    public abstract List<string> GetErrorMessages();

}
