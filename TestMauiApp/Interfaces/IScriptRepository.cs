using System.Collections.Generic;
using System.Threading.Tasks;
using TestMauiApp.Models;

namespace TestMauiApp.Interfaces;

public interface IScriptRepository
{
    public Task<List<Script>> LoadScriptsAsync(long userId);
    public Task<string> RunScriptAsync(RunRequest request);
}