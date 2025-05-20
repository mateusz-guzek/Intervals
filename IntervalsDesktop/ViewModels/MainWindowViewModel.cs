using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using Functions;
using Nonlinear_Solvers;
using Numerics.NET;
using IntervalsDesktop.Utility;
using Interval = Intervals.Interval;

namespace IntervalsDesktop.ViewModels;

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

    

    [ObservableProperty] private bool _isArithmeticModeSelected;

    public ObservableCollection<IFunction> Functions { get; } = new();
    [ObservableProperty] private IFunction _selectedFunction;


    [ObservableProperty] private Result<Intervals.Interval> _intervalResult;


    [ObservableProperty] private string _outputField;

    [ObservableProperty] private string _epsilon;
    [ObservableProperty] private int _iterations;


    public async void onCalculateClicked()
    {

        try
        {
            var a = BigFloat.Parse(A1.Trim(), AccuracyGoal.Absolute(20));
            var b = BigFloat.Parse(B1.Trim(), AccuracyGoal.Absolute(20));

            var epsilon = BigFloat.Parse(Epsilon.Trim());

            IntervalResult = new();

            if (SelectedMode == "Standardowa")
            {
                Result<BigFloat> BigFloatResult;
                if (SelectedMethod == "Metoda połowienia")
                {
                    BigFloatResult = Bisection.Eval(SelectedFunction, a, b, 100, epsilon);
                    OutputField = MakeOutputString(BigFloatResult, SelectedFunction, epsilon, SelectedMethod);
                }

                if (SelectedMethod == "Regula Falsi")
                {
                    BigFloatResult = RegulaFalsi.Eval(SelectedFunction, a, b, 100, epsilon);
                    OutputField = MakeOutputString(BigFloatResult, SelectedFunction, epsilon, SelectedMethod);
                }

                if (SelectedMethod == "Metoda siecznych")
                {
                    BigFloatResult = Secant.Eval(SelectedFunction, a, b, 100, epsilon);
                    OutputField = MakeOutputString(BigFloatResult, SelectedFunction, epsilon, SelectedMethod);
                }
            }
            else
            {
                // dodatkowo dla koncow przedzialu
                var a1 = BigFloat.Parse(A2.Trim());
                var b1 = BigFloat.Parse(B2.Trim());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private string MakeOutputString<T>(Result<T> result, IFunction function, BigFloat epsilon,
        string method)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"Wybrana Metoda: {method}");
        sb.AppendLine($"Wybrana Funkcja: {function?.StringRepresentation ?? "(brak)"}");
        sb.AppendLine($"ε: {epsilon}");
        sb.AppendLine($"Liczba iteracji: {result.Iterations}");
        // Status oceny
        sb.Append("Status: ");
        switch (result.Status)
        {
            case EvalStatus.FULL_SUCCESS:
                sb.AppendLine("Sukces – znaleziono rozwiązanie z dokładnością ε.");
                break;
            case EvalStatus.NO_SIGN_CHANGE:
                sb.AppendLine("Błąd – funkcja nie zmienia znaku w podanym przedziale.");
                break;
            case EvalStatus.NO_CONVERGENCE:
                sb.AppendLine("Nie osiągnięto wymaganej dokładności – brak zbieżności.");
                break;
            case EvalStatus.DIVISION_BY_ZERO:
                sb.AppendLine("Błąd – dzielenie przez zero.");
                break;
            case EvalStatus.NOT_ACCURATE:
                sb.AppendLine("Wynik nie spełnia dokładności.");
                break;
            case EvalStatus.ERROR:
            default:
                sb.AppendLine("Wystąpił nieznany błąd.");
                break;
        }

        // Wypisz wynik tylko jeśli istnieje
        if ((result.Status & (EvalStatus.FULL_SUCCESS | EvalStatus.NOT_ACCURATE)) != 0)
        {
            sb.AppendLine($"X\u2080: {result.Value.ToString()}");
        }
        else
        {
            sb.AppendLine("X\u2080: brak wartości.");
        }

        

        return sb.ToString();
    }
}