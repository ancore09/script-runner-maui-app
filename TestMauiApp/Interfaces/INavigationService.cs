using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace TestMauiApp.Interfaces;

public interface INavigationService
{
    public Task NavigateToPage<T>() where T : Page;
    public Task ShellNavigateToAsync(string route, bool animated, Dictionary<string, object> parameters);
}