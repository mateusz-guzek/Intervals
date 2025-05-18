using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using Functions;
using Nonlinear_Solvers;
using Numerics.NET;
using IntervalsDesktop.Utility;

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
    
    public ObservableCollection<IFunction> Functions { get; } = new();
    [ObservableProperty] private IFunction _selectedFunction;
    
    
    [ObservableProperty] private Result<Intervals.Interval> _intervalResult;
    [ObservableProperty] private Result<BigFloat> _bigFloatResult;


    public async void onCalculateClicked()
    {
        _intervalResult = new();
        _bigFloatResult = new();
        if (_selectedMode == "Standardowa")
        {
            if (_selectedMethod == "Metoda połowienia")
            {
                Console.WriteLine(A1);
                Console.WriteLine(A1.Length);
                Console.WriteLine(B1);
                Console.WriteLine(B1.Length);
                var a = BigFloat.Parse(A1.Trim());
                var b = BigFloat.Parse(B1.Trim());
                var epsilon = new BigFloat(1e-20);
                _bigFloatResult = Bisection.Eval(_selectedFunction, a, b, 100, epsilon);
                Console.WriteLine(_bigFloatResult.Iterations);
                Console.WriteLine(_bigFloatResult.Status);
                Console.WriteLine(_bigFloatResult.Value);

            }
        }
        Console.WriteLine(_selectedFunction.StringRepresentation);
        
    }





}