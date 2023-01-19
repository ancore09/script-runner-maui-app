using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMauiApp.Models;
using TestMauiApp.ViewModels;

namespace TestMauiApp.Pages;

[QueryProperty(nameof(Script), "Script")]
public partial class ScriptRunnerPage : ContentPage
{
    readonly ScriptRunnerViewModel _viewModel;

    private Script _script;
    public Script Script
    {
        get => _script;

        set
        {
            _script = value;
            _viewModel.Script = value;
        }
    }

    public ScriptRunnerPage(ScriptRunnerViewModel viewModel)
    {
        _viewModel = viewModel;
        BindingContext = viewModel;
        InitializeComponent();
    }
}