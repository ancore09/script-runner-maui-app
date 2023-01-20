using Dapper;
using Npgsql;
using TestMauiBackend.Helpers;
using TestMauiBackend.Interfaces;
using TestMauiBackend.Models;

namespace TestMauiBackend.Services;

public class ScriptService: IScriptService
{
    private readonly string _connectionString;
    
    public ScriptService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    public async Task<RunResponse> RunScript(long scriptId, string args)
    {
        //await Task.Delay(1000);
        if (scriptId == 0)
        {
            return PowerShellHelper.Shutdown();
        }
        var script = await GetScriptById(scriptId);
        //var output = PowerShellHelper.RunScript(script.Path, args);
        var output = new RunResponse() { StandardOutput = $"{script.Id} | {args} | {script.Path}", StandardError = null };
        return output;
    }
    
    public async Task<List<Script>> GetUserScripts(long userId)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand("select s.id, s.name, s.description, s.args from scripts as s left join public.user_scripts us on s.id = us.script_id left join users u on us.user_id = u.id where u.id = @id;", connection);
        var queryParameters = new
        {
            id = userId
        };
        var result = await connection.QueryAsync(command.CommandText, queryParameters);
        return result.Select(x => new Script
        {
            Id = x.id,
            Name = x.name,
            Description = x.description,
            Args = x.args
        }).ToList();
    }

    public async Task<Script> GetScriptById(long scriptId)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand("select * from scripts where id = @id;", connection);
        var queryParameters = new
        {
            id = scriptId
        };
        var result = await connection.QueryFirstAsync<Script>(command.CommandText, queryParameters);
        return result;
    }
}