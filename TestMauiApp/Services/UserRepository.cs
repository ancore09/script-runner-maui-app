using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.Extensions.Configuration;
using TestMauiApp.Helpers;
using TestMauiApp.Interfaces;
using TestMauiApp.Models;

namespace TestMauiApp.Services;

public class UserRepository: IUserRepository
{
    private string _baseUrl;
    
    public UserRepository(IConfiguration configuration)
    {
        _baseUrl = configuration["ApiUrl"];
    }
    
    public async Task<LoginResponse> LoginAsync(string username, string password)
    {
        DevHttpsConnectionHelper devHttpsConnectionHelper = new DevHttpsConnectionHelper(7132);
        HttpClient client = devHttpsConnectionHelper.HttpClient;
        
        var request = new LoginRequest()
        {
            Username = username,
            Password = password
        };
        var response = await client.PostAsync($"{_baseUrl}/api/Login/login", new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));
        
        if (response.StatusCode == HttpStatusCode.OK)
        {
            LoginResponse loginResponse = JsonSerializer.Deserialize<LoginResponse>(await response.Content.ReadAsStringAsync());
            return loginResponse;
        }

        return null;
    }   

    public async Task<TokenResponse> RefreshTokenAsync()
    {
        DevHttpsConnectionHelper devHttpsConnectionHelper = new DevHttpsConnectionHelper(7132);
        HttpClient client = devHttpsConnectionHelper.HttpClient;
        
        var token = await SecureStorage.GetAsync("refresh_token");
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        
        var response = await client.PostAsync($"{_baseUrl}/api/Login/refresh", null);
        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                TokenResponse tokenResponse = JsonSerializer.Deserialize<TokenResponse>(await response.Content.ReadAsStringAsync());
                return tokenResponse;
            case HttpStatusCode.Unauthorized:
                return new TokenResponse() {AccessToken = null, RefreshToken = null, Error = "Unauthorized"};
            default:
                return null;
        }
    }

    public async Task<TokenResponse> Check()
    {
        DevHttpsConnectionHelper devHttpsConnectionHelper = new DevHttpsConnectionHelper(7132);
        HttpClient client = devHttpsConnectionHelper.HttpClient;
        
        var token = await SecureStorage.GetAsync("access_token");
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        
        var response = await client.GetAsync($"{_baseUrl}/api/Login/check");
        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                return new TokenResponse();
            case HttpStatusCode.Unauthorized:
                return new TokenResponse() {AccessToken = null, RefreshToken = null, Error = "Unauthorized"};
            default:
                return null;
        }
    }

    public async Task<User> GetUserByIdAsync(long userId)
    {
        DevHttpsConnectionHelper devHttpsConnectionHelper = new DevHttpsConnectionHelper(7132);
        HttpClient client = devHttpsConnectionHelper.HttpClient;

        var token = await SecureStorage.GetAsync("access_token");
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync($"{_baseUrl}/api/Login/getUserById?userId=" + userId);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var payload = await response.Content.ReadAsStringAsync();
            User user = JsonSerializer.Deserialize<User>(payload);
            return user;
        }

        return null;
    }
}