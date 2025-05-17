using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using Nonlinear_Solvers;
using Numerics.NET;

namespace IntervalsDesktop.ViewModels;

public enum Arithmetic
{
    Standard,
    Interval
}

public partial class MainWindowViewModel : ViewModelBase
{
    public List<string> ArithmeticModes { get; } = new()
    {
        "Standardowa",
        "Przedziałowa"
    };

    private string _selectedMode = "Standardowa";
    
    public string SelectedMode
    {
        get => _selectedMode;
        set
        {
            IsArithmeticModeSelected = value == "Przedziałowa";
            _selectedMode = value;
        }
    }
    
    public List<string> Methods { get; } = new()
    {
        "Metoda połowienia",
        "Regula Falsi",
        "Metoda siecznych"
    };

    [ObservableProperty] private string _selectedMethod = "Metoda połowienia";

    [ObservableProperty] private string _a1 = "";
    [ObservableProperty] private string _a2 = "";
    [ObservableProperty] private string _b1 = "";
    [ObservableProperty] private string _b2 = "";
    
    
    [ObservableProperty] private Result<BigFloat> _resultBF;
    [ObservableProperty] private Result<Interval> _resultINT;
    
    


    [ObservableProperty] private bool _isArithmeticModeSelected;




    
}