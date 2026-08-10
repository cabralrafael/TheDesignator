using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheDesignator.Application.UseCases.Login.WithEmailAndPassword;
using TheDesignator.Communication.Requests;
using TheDesignator.Communication.Responses;

namespace TheDesignator.API.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromServices] ILoginWithEmailAndPasswordUseCase useCase,
                                           [FromBody] RequestLoginJson request)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }
}
