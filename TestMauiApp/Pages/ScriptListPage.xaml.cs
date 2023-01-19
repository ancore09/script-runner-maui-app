using Microsoft.Maui.Controls;
using Microsoft.Maui.LifecycleEvents;
using TestMauiApp.Interfaces;
using TestMauiApp.Models;
using TestMauiApp.ViewModels;

namespace TestMauiApp.Pages;

[QueryProperty(nameof(User), "User")]
public partial class ScriptListPage : ContentPage
{
    INavigationService _navigationService;
    
    private ScriptsViewModel _viewModel;
    
    private User _user;
    public User User
    {
        get => _user;

        set
        {
            _user = value;
            _viewModel.User = value;
        }
    }
    
    public ScriptListPage(ScriptsViewModel viewModel, INavigationService navigationService)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
        _navigationService = navigationService;
    }
    
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel != null && _viewModel.Scripts.Count == 0)
        {
            _viewModel.LoadScriptsCommand.Execute(null);
        }
    }

    protected override bool OnBackButtonPressed()
    {
        return true;
    }
}