using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Classic.CommonControls.Dialogs;
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

    [ObservableProperty] private string _start = "";
    [ObservableProperty] private string _end = "";


    [ObservableProperty] private bool _isArithmeticModeSelected;

    public ObservableCollection<IFunction> Functions { get; } = new();
    [ObservableProperty] private IFunction? _selectedFunction;


    [ObservableProperty] private Result<Intervals.Interval> _intervalResult;


    [ObservableProperty] private string _outputField;

    [ObservableProperty] private string _epsilon;
    [ObservableProperty] private int _iterations;


    public void OnCalculateClicked()
    {
        try
        {
            var valid = AreInputsValid();
            if (!valid) return;


            var a = BigFloat.Parse(Start.Trim(), AccuracyGoal.Absolute(20));
            var b = BigFloat.Parse(End.Trim(), AccuracyGoal.Absolute(20));

            var epsilon = BigFloat.Parse(Epsilon.Trim());

            IntervalResult = new();

            if (SelectedMode == "Standardowa")
            {
                Console.WriteLine(SelectedFunction.StringRepresentation);
                Result<BigFloat> BigFloatResult;
                if (SelectedMethod == "Metoda połowienia")
                {
                    BigFloatResult = Bisection.EvalR(SelectedFunction, a, b, 100, epsilon);
                    OutputField = MakeOutputString(BigFloatResult, SelectedFunction, epsilon, SelectedMethod);
                }

                if (SelectedMethod == "Regula Falsi")
                {
                    BigFloatResult = RegulaFalsi.EvalR(SelectedFunction, a, b, 100, epsilon);
                    OutputField = MakeOutputString(BigFloatResult, SelectedFunction, epsilon, SelectedMethod);
                }

                if (SelectedMethod == "Metoda siecznych")
                {
                    BigFloatResult = Secant.EvalR(SelectedFunction, a, b, 100, epsilon);
                    OutputField = MakeOutputString(BigFloatResult, SelectedFunction, epsilon, SelectedMethod);
                }
            }
            else if (SelectedMode == "Przedziałowa")
            {
                Result<Interval> IntervalResult;
                if (SelectedMethod == "Metoda połowienia")
                {
                    IntervalResult = Bisection.EvalI(SelectedFunction, a, b, 100, epsilon);
                    OutputField = MakeOutputString(IntervalResult, SelectedFunction, epsilon, SelectedMethod);
                }

                if (SelectedMethod == "Regula Falsi")
                {
                    IntervalResult = RegulaFalsi.EvalI(SelectedFunction, a, b, 100, epsilon);
                    OutputField = MakeOutputString(IntervalResult, SelectedFunction, epsilon, SelectedMethod);
                }

                if (SelectedMethod == "Metoda siecznych")
                {
                    IntervalResult = Secant.EvalI(SelectedFunction, a, b, 100, epsilon);
                    OutputField = MakeOutputString(IntervalResult, SelectedFunction, epsilon, SelectedMethod);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            OutputField = "Wystąpił niespodziewany błąd.";
        }
    }

    private static string MakeOutputString<T>(Result<T> result, IFunction function, BigFloat epsilon,
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
        if (result.Value is not null)
        {
            sb.AppendLine($"X\u2080: {result.Value.ToString()}");
        }
        else
        {
            sb.AppendLine("X\u2080: brak wartości.");
        }


        return sb.ToString();
    }

    private bool AreInputsValid()
    {
        StringBuilder stringBuilder = new StringBuilder();
        bool isValid = true;

        if (SelectedFunction == null)
        {
            stringBuilder.AppendLine("- Nie wybrałeś badanej funkcji!");
            isValid = false;
        }

        try
        {
            BigFloat.Parse(Start);
        }
        catch (FormatException)
        {
            stringBuilder.AppendLine("- Źle podany początek przedziału!");
            isValid = false;
        }

        try
        {
            BigFloat.Parse(End);
        }
        catch (FormatException)
        {
            stringBuilder.AppendLine("- Źle podany koniec przedziału!");
            isValid = false;
        }

        try
        {
            BigFloat.Parse(Epsilon);
        }
        catch (FormatException)
        {
            stringBuilder.AppendLine("- Źle podany Epsilon!");
            isValid = false;
        }
        
        OutputField = stringBuilder.ToString();

        return isValid;
    }
}