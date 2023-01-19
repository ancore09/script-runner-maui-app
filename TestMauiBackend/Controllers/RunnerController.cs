using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestMauiBackend.Interfaces;
using TestMauiBackend.Models;

namespace TestMauiBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RunnerController : Controller
{
    private readonly IScriptService _scriptService;
    
    public RunnerController(IScriptService scriptService)
    {
        _scriptService = scriptService;
    }
    
    [Authorize]
    [HttpGet("GetUserScripts")]
    public async Task<IActionResult> GetUserScripts(long id)
    {
        var scripts = await _scriptService.GetUserScripts(id);
        return Ok(scripts);
    }
    
    [Authorize]
    [HttpPost("run")]
    public async Task<IActionResult> Post([FromBody] RunRequest request)
    {
        var result = await _scriptService.RunScript(request.ScriptId, request.ScriptArgs);
        return Ok(result);
    }
}