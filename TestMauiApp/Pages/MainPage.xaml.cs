using System;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Controls;
using TestMauiApp.Helpers;
using TestMauiApp.Interfaces;

namespace TestMauiApp.Pages;

public partial class MainPage : ContentPage
{
    private readonly INavigationService _navigationService;
    private readonly IUserRepository _userRepository;

    public MainPage(INavigationService navigationService, IUserRepository userRepository)
    {
        _navigationService = navigationService;
        _userRepository = userRepository;
        InitializeComponent();
        CheckTokens();
    }

    private async void CheckTokens()
    {
        //SecureStorage.RemoveAll();
        var accessToken = await SecureStorage.GetAsync("access_token");
        var refreshToken = await SecureStorage.GetAsync("refresh_token");
        
        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
        {
            await _navigationService.ShellNavigateToAsync($"/{nameof(LoginPage)}", true, new Dictionary<string, object>());
            return;
        }

        var tokens = await _userRepository.Check();
        if (tokens == null)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            string text = "Server Unreachable";
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;
            var toast = Toast.Make(text, duration, fontSize);
            await toast.Show(cancellationTokenSource.Token);
            await _navigationService.ShellNavigateToAsync($"/{nameof(LoginPage)}", true, new Dictionary<string, object>());
            return;
        }
        
        if (tokens.Error == "Unauthorized")
        {
            tokens = await _userRepository.RefreshTokenAsync();
            if (tokens.Error == "Unauthorized")
            {
                await _navigationService.ShellNavigateToAsync($"/{nameof(LoginPage)}", true, new Dictionary<string, object>());
                return;
            }
            await SecureStorage.SetAsync("access_token", tokens.AccessToken);
            await SecureStorage.SetAsync("refresh_token", tokens.RefreshToken);
        } 

        var userId = long.Parse(await SecureStorage.GetAsync("user_id"));
        var user = await _userRepository.GetUserByIdAsync(userId);
        
        await _navigationService.ShellNavigateToAsync($"/{nameof(ScriptListPage)}", true,
            new Dictionary<string, object>()
            {
                {"User", user}
            });
    }
}