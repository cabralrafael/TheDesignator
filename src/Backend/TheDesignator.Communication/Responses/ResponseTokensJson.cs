using System;
using System.Collections.Generic;
using System.Text;

namespace TheDesignator.Communication.Responses;

public class ResponseTokensJson
{
    public string AccessToken { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;
}
