using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TestMauiApp.Helpers;
using TestMauiApp.Interfaces;
using TestMauiApp.Pages;

namespace TestMauiApp.ViewModels;

public partial class LoginViewModel: ObservableObject
{
    [ObservableProperty] private string _username;
    [ObservableProperty] private string _password;
    
    private readonly IUserRepository _userRepository;
    private readonly INavigationService _navigationService;
    
    public LoginViewModel(IUserRepository userRepository, INavigationService navigationService)
    {
        _userRepository = userRepository;
        _navigationService = navigationService;
    }
    
    [RelayCommand]
    public async void Login()
    {
        var loginResponse = await _userRepository.LoginAsync(Username, Password);
        if (loginResponse != null)
        {
            await SecureStorage.Default.SetAsync("access_token", loginResponse.Tokens.AccessToken);
            await SecureStorage.Default.SetAsync("refresh_token", loginResponse.Tokens.RefreshToken);
            await SecureStorage.Default.SetAsync("user_id", loginResponse.User.Id.ToString());
            await _navigationService.ShellNavigateToAsync($"../{nameof(ScriptListPage)}", true, new Dictionary<string, object>()
            {
                {"User", loginResponse.User}
            });
        }
        else
        {
            ToastHelper.ShowToast("Wrong username or password");
        }
    }
}