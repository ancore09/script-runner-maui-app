using TestMauiApp.Interfaces;
using TestMauiApp.Pages;
using TestMauiApp.Services;
using TestMauiApp.ViewModels;
using CommunityToolkit.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;

namespace TestMauiApp;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        
        builder.Configuration["ApiUrl"] = "https://10.0.2.2:7132";
        
        builder.UseMauiApp<App>().ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        }).UseMauiCommunityToolkit();
        
        builder.Services.AddTransient<MainPage>();
        
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<LoginViewModel>();
        
        builder.Services.AddTransient<ScriptListPage>();
        builder.Services.AddTransient<ScriptsViewModel>();
        
        builder.Services.AddTransient<ScriptRunnerPage>();
        builder.Services.AddTransient<ScriptRunnerViewModel>();
        
        builder.Services.AddSingleton<IScriptRepository, ScriptRepository>();
        builder.Services.AddSingleton<IUserRepository, UserRepository>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        
        return builder.Build();
    }
}