namespace Service;

public interface IPostService
{
    Task<string> GetPostsByUserId(int userId);
}