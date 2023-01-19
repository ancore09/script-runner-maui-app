using TestMauiApp.Models;

namespace TestMauiApp.Interfaces;

public interface IUserRepository
{
    public Task<LoginResponse> LoginAsync(string username, string password);
    public Task<TokenResponse> RefreshTokenAsync();
    public Task<TokenResponse> Check();
    public Task<User> GetUserByIdAsync(long userId);
}