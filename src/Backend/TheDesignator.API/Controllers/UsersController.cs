using Microsoft.AspNetCore.Mvc;
using TheDesignator.Application.UseCases.User.Register;
using TheDesignator.Communication.Requests;
using TheDesignator.Communication.Responses;

namespace TheDesignator.API.Controllers;

[Route("[controller]")]
[ApiController]
[ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
public class UsersController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Register([FromBody] RequestRegisterUserAccountJson request, 
                                             [FromServices] IRegisterUserAccountUseCase userCase)
    {
        var result = await userCase.Execute(request);

        return Created(string.Empty, result);
    }
}
