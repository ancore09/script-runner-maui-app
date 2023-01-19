using Microsoft.Maui.Controls;
using TestMauiApp.Pages;

namespace TestMauiApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        Routing.RegisterRoute($"{nameof(MainPage)}/{nameof(LoginPage)}", typeof(LoginPage));
        Routing.RegisterRoute($"{nameof(MainPage)}/{nameof(ScriptListPage)}", typeof(ScriptListPage));
        Routing.RegisterRoute($"{nameof(MainPage)}/{nameof(ScriptListPage)}/{nameof(ScriptRunnerPage)}", typeof(ScriptRunnerPage));
    }
}