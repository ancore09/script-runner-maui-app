using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TestMauiApp.Interfaces;
using TestMauiApp.Models;
using TestMauiApp.Pages;
using Toast = CommunityToolkit.Maui.Alerts.Toast;

namespace TestMauiApp.ViewModels;

public partial class ScriptsViewModel: ObservableObject, IScriptsViewModel
{
    [ObservableProperty]
    private List<Script> _scripts;

    [ObservableProperty] private bool _isRefreshing;
    
    [ObservableProperty]
    private User _user;

    private readonly IScriptRepository _scriptRepository;
    private readonly INavigationService _navigationService;
    private readonly IUserRepository _userRepository;
    
    public ScriptsViewModel(IScriptRepository scriptRepository, INavigationService navigationService, IUserRepository userRepository)
    {
        _scripts = new List<Script>();
        _scriptRepository = scriptRepository;
        _navigationService = navigationService;
        _userRepository = userRepository;
    }
    
    [RelayCommand]
    public async Task LoadScripts()
    {
        var scripts = await _scriptRepository.LoadScriptsAsync(User.Id);
        if (scripts == null)
        {
            var tokens = await _userRepository.RefreshTokenAsync();
            if (tokens != null)
            {
                await SecureStorage.SetAsync("refresh_token", tokens.RefreshToken);
                await SecureStorage.SetAsync("access_token", tokens.AccessToken);
                scripts = await _scriptRepository.LoadScriptsAsync(User.Id);
            } else
            {
                await Shell.Current.GoToAsync("//MainPage/LoginPage");
                return;
            }
        }
        Scripts = scripts;
        IsRefreshing = false;
    }
    
    [RelayCommand]
    async Task RunScript(Script script)
    {
        await _navigationService.ShellNavigateToAsync($"/{nameof(ScriptRunnerPage)}", true, new Dictionary<string, object>() {{"Script", script}});
    }

    [RelayCommand]
    async Task ShutdownServer()
    {
        var result = await _scriptRepository.RunScriptAsync(new RunRequest() {ScriptId = 0, ScriptArgs = ""});
        
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        ToastDuration duration = ToastDuration.Short;
        double fontSize = 14;
        var toast = Toast.Make(result, duration, fontSize);
        await toast.Show(cancellationTokenSource.Token);
    }
}