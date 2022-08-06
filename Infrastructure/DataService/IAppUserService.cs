using Entities.Enums;

namespace Infrastructure.DataService;

public interface IAppUserService
{
    IEnumerable<Role> GetRolesForUser(string username);
    bool TryLogin(string username, string password);
}