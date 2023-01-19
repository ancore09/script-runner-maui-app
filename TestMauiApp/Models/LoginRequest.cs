using System.Text.Json.Serialization;

namespace TestMauiApp.Models;

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}