using TestMauiBackend.Models;

namespace TestMauiBackend.Interfaces;

public interface IScriptService
{
    public Task<string> RunScript(long scriptId, string args);
    public Task<List<Script>> GetUserScripts(long userId);
}