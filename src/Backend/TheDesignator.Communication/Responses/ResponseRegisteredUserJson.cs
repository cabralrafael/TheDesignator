using System;
using System.Collections.Generic;
using System.Text;

namespace TheDesignator.Communication.Responses;

public class ResponseRegisteredUserJson
{
    public string Name { get; set; }

    public ResponseTokensJson Tokens { get; set; } = new ResponseTokensJson();
}
