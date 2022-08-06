using Entities.Enums;
using Infrastructure.DataAccess;

namespace Infrastructure.DataService;

public class AppUserService : IAppUserService
{
    private readonly IAppUserRepository _userRepository;

    public AppUserService(IAppUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public IEnumerable<Role> GetRolesForUser(string username)
    {
        return _userRepository.GetRolesForUser(username);
    }

    public bool TryLogin(string username, string password)
    {
        return _userRepository.TryLogin(username, password);
    }
}