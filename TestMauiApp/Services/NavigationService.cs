using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using TestMauiApp.Interfaces;

namespace TestMauiApp.Services;

public class NavigationService: INavigationService
{
    private readonly IServiceProvider _serviceProvider;
    
    protected INavigation Navigation
    {
        get
        {
            INavigation? navigation = Application.Current?.MainPage?.Navigation;
            if (navigation is not null)
                return navigation;
            else
            {
                //This is not good!
                if (Debugger.IsAttached)
                    Debugger.Break();
                throw new Exception();
            }
        }
    }
    
    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task ShellNavigateToAsync(string route, bool animated, Dictionary<string, object> parameters)
    {
        return Shell.Current.GoToAsync(route, animated, parameters);
    }

    public Task NavigateToPage<T>() where T : Page
    {
        var page = ResolvePage<T>();
        if(page is not null)
            return Navigation.PushAsync(page, true);
        throw new InvalidOperationException($"Unable to resolve type {typeof(T).FullName}");
    }
    private T? ResolvePage<T>() where T : Page
        => _serviceProvider.GetService<T>();
}