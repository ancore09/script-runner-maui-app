using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TestMauiApp.Helpers;
using TestMauiApp.Interfaces;
using TestMauiApp.Models;

namespace TestMauiApp.Services;

public class ScriptRepository: IScriptRepository
{
    
    private string _baseUrl;
    
    public ScriptRepository(IConfiguration configuration)
    {
        _baseUrl = configuration["ApiUrl"];
    }
    
    public async Task<List<Script>> LoadScriptsAsync(long userId)
    {
        DevHttpsConnectionHelper devHttpsConnectionHelper = new DevHttpsConnectionHelper(7132);
        HttpClient client = devHttpsConnectionHelper.HttpClient;
        
        var token = await SecureStorage.GetAsync("access_token");
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        try
        {
            var response = await client.GetAsync($"{_baseUrl}/api/Runner/GetUserScripts?id={userId}");
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    var content = await response.Content.ReadAsStringAsync();
                    var scripts = JsonSerializer.Deserialize<List<Script>>(content);
                    return scripts;
                case HttpStatusCode.Unauthorized:
                    return null;
                default:
                    return null;
            }
        }
        catch (Exception e)
        {
            return null;
        }
    }
    
    public async Task<RunResponse> RunScriptAsync(RunRequest request) {
        DevHttpsConnectionHelper devHttpsConnectionHelper = new DevHttpsConnectionHelper(7132);
        HttpClient client = devHttpsConnectionHelper.HttpClient;
        
        var token = await SecureStorage.GetAsync("access_token");
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        try
        {
            var response = await client.PostAsync($"{_baseUrl}/api/Runner/run", new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    var payload = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<RunResponse>(payload);
                    return result;
                case HttpStatusCode.Unauthorized:
                    return new RunResponse() { StandardOutput = null, StandardError = null, Error = "Unauthorized" };
                default:
                    return null;
            }
        } catch (Exception e) {
            return new RunResponse() { StandardOutput = null, StandardError = null, Error = "Unreachable" };
        }
    }
}