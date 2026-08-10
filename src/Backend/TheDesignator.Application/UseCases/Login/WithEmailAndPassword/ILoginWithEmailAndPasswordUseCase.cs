using TheDesignator.Communication.Requests;
using TheDesignator.Communication.Responses;

namespace TheDesignator.Application.UseCases.Login.WithEmailAndPassword;

public interface ILoginWithEmailAndPasswordUseCase
{
    Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request);
}
