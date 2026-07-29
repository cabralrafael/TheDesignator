using TheDesignator.Communication.Requests;
using TheDesignator.Communication.Responses;

namespace TheDesignator.Application.UseCases.User.Register;

public interface IRegisterUserAccountUseCase
{
    Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserAccountJson request);
}
