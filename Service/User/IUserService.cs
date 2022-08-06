using Entities.DataContract;

namespace Service;

public interface IUserService
{
    //Task<IEnumerable<User>> GetUsers();
    //Task<User> GetUserById(int id);
    Task<string> GetUsers();
    Task<string> GetUserById(int id);
}