namespace WebApi.Tests.Resources;

public class UserIdentityManager
{
    private readonly TheDesignator.Domain.Entities.User _user;
    private readonly string _password;

    public UserIdentityManager(TheDesignator.Domain.Entities.User user, string password)
    {
        _user = user;
        _password = password;
    }

    public Guid GetId() => _user.Id;
    public string GetName() => _user.Name;
    public string GetEmail() => _user.Email;
    public string GetPassword() => _password;
}
