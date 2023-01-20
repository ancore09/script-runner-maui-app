using TestMauiBackend.Models;

namespace TestMauiBackend.Interfaces;

public interface IScriptService
{
    public Task<RunResponse> RunScript(long scriptId, string args);
    public Task<List<Script>> GetUserScripts(long userId);
    public Task<Script> GetScriptById(long scriptId);
}