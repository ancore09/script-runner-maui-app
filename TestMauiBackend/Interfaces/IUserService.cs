using TestMauiBackend.Models;

namespace TestMauiBackend.Interfaces;

public interface IUserService
{
    public Task<User?> Authenticate(string username, string password);
    public Task<User?> GetUserById(long userId);
}