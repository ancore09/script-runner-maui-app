using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TestMauiApp.Interfaces;
using TestMauiApp.Models;

namespace TestMauiApp.ViewModels;

public partial class ScriptRunnerViewModel: ObservableObject
{
    [ObservableProperty] private Script _script;
    [ObservableProperty] private string _scriptOutput;
    
    private readonly IScriptRepository _scriptRepository;
    private readonly IUserRepository _userRepository;

    public ScriptRunnerViewModel(IScriptRepository scriptRepository, IUserRepository userRepository)
    {
        _scriptRepository = scriptRepository;
        _userRepository = userRepository;
    }
    
    [RelayCommand]
    private async Task RunScript()
    {
        var result = await _scriptRepository.RunScriptAsync(new RunRequest() { ScriptId = Script.Id, ScriptArgs = Script.Args });
        switch (result)
        {
            case "Unauthorized":
                var tokens = await _userRepository.RefreshTokenAsync();
                if (tokens.Error == "Unauthorized")
                {
                    await SecureStorage.SetAsync("refresh_token", tokens.RefreshToken);
                    await SecureStorage.SetAsync("access_token", tokens.AccessToken);
                    result = await _scriptRepository.RunScriptAsync(new RunRequest() { ScriptId = Script.Id, ScriptArgs = Script.Args });
                } else
                {
                    await Shell.Current.GoToAsync("//MainPage/LoginPage");
                }
                break;
            default:
                break;
        }
        ScriptOutput = result;
    }
    
    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }
}