namespace TheDesignator.Communication.Responses;

public class ResponseErrorJson
{
    public List<string> Errors { get; set; }

    public ResponseErrorJson(List<string> errorsMessages) => Errors = errorsMessages;

    public ResponseErrorJson(string errorMessage) => Errors = [errorMessage];
}
