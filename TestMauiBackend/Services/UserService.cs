using Dapper;
using TestMauiBackend.Interfaces;
using TestMauiBackend.Models;
using Npgsql;

namespace TestMauiBackend.Services;

public class UserService: IUserService
{
    private readonly string _connectionString;
    
    public UserService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    
    public async Task<User?> Authenticate(string username, string password)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand("SELECT username FROM credentials WHERE username = @username AND password = @password", connection);
        var queryParameters = new
        {
            username = username,
            password = password
        };
        
        var usernameResult = await connection.QueryFirstAsync<string?>(command.CommandText, queryParameters);
        if (usernameResult == null)
        {
            return null;
        }
        
        await using var command2 = new NpgsqlCommand("SELECT id, username FROM users WHERE username = @username", connection);
        var queryParameters2 = new
        {
            username = username
        };
        return await connection.QueryFirstAsync<User>(command2.CommandText, queryParameters2);
    }

    public async Task<User?> GetUserById(long userId)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command2 = new NpgsqlCommand("SELECT id, username FROM users WHERE id = @id", connection);
        var queryParameters2 = new
        {
            id = userId
        };
        return await connection.QueryFirstAsync<User>(command2.CommandText, queryParameters2);
    }
}