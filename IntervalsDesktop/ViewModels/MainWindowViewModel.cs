using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
        "Automatyczna Przedziałowa",
        "Manualna Przedziałowa"
    };


    private string _selectedMode = "Standardowa";

    public string SelectedMode
    {
        get => _selectedMode;
        set
        {
            IsArithmeticModeSelected = value == "Manualna Przedziałowa";
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
    
    [ObservableProperty] private string _start1 = "";
    [ObservableProperty] private string _end1 = "";


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
            
            BigFloat creationEpsilon = BigFloat.Parse("1e-128");


            var a = BigFloat.Parse(Start.Trim(), AccuracyGoal.Absolute(300));
            var b = BigFloat.Parse(End.Trim(), AccuracyGoal.Absolute(300));

            var epsilon = BigFloat.Parse(Epsilon.Trim());

            IntervalResult = new();

            if (SelectedMode == "Standardowa")
            {
                Console.WriteLine(SelectedFunction.StringRepresentation);
                Result<BigFloat> bigFloatResult;
                if (SelectedMethod == "Metoda połowienia")
                {
                    bigFloatResult = Bisection.EvalR(SelectedFunction, a, b, Iterations, epsilon);
                    OutputField = MakeOutputString(bigFloatResult, SelectedFunction, epsilon, SelectedMethod);
                }

                if (SelectedMethod == "Regula Falsi")
                {
                    bigFloatResult = RegulaFalsi.EvalR(SelectedFunction, a, b, Iterations, epsilon);
                    //Console.WriteLine(bigFloatResult.Value.ToString());
                    OutputField = MakeOutputString(bigFloatResult, SelectedFunction, epsilon, SelectedMethod);
                }

                if (SelectedMethod == "Metoda siecznych")
                {
                    bigFloatResult = Secant.EvalR(SelectedFunction, a, b, Iterations, epsilon);
                    OutputField = MakeOutputString(bigFloatResult, SelectedFunction, epsilon, SelectedMethod);
                }
            }
            else if (SelectedMode == "Automatyczna Przedziałowa")
            {
                Result<Interval> IntervalResult;
                if (SelectedMethod == "Metoda połowienia")
                {
                    IntervalResult = Bisection.EvalI(SelectedFunction, new Interval(a, a+creationEpsilon), new Interval(b - creationEpsilon,b), Iterations, epsilon);
                    OutputField = MakeOutputString(IntervalResult, SelectedFunction, epsilon, SelectedMethod);
                }

                if (SelectedMethod == "Regula Falsi")
                {
                    IntervalResult = RegulaFalsi.EvalI(SelectedFunction, new Interval(a + creationEpsilon), new Interval(b - creationEpsilon, b), Iterations, epsilon);
                    OutputField = MakeOutputString(IntervalResult, SelectedFunction, epsilon, SelectedMethod);
                }

                if (SelectedMethod == "Metoda siecznych")
                {
                    IntervalResult = Secant.EvalI(SelectedFunction, new Interval(a + creationEpsilon), new Interval(b - creationEpsilon,b), Iterations, epsilon);
                    OutputField = MakeOutputString(IntervalResult, SelectedFunction, epsilon, SelectedMethod);
                }
            }
            else if (SelectedMode == "Manualna Przedziałowa")
            {
                var a1 = BigFloat.Parse(Start1.Trim(), AccuracyGoal.Absolute(300));
                var b1 = BigFloat.Parse(End1.Trim(), AccuracyGoal.Absolute(300));
                Result<Interval> IntervalResult;
                if (SelectedMethod == "Metoda połowienia")
                {
                    IntervalResult = Bisection.EvalI(SelectedFunction, new Interval(a, a1), new Interval(b, b1), Iterations, epsilon);
                    OutputField = MakeOutputString(IntervalResult, SelectedFunction, epsilon, SelectedMethod);
                }

                if (SelectedMethod == "Regula Falsi")
                {
                    IntervalResult = RegulaFalsi.EvalI(SelectedFunction, new Interval(a, a1), new Interval(b, b1), Iterations, epsilon);
                    OutputField = MakeOutputString(IntervalResult, SelectedFunction, epsilon, SelectedMethod);
                }

                if (SelectedMethod == "Metoda siecznych")
                {
                    IntervalResult = Secant.EvalI(SelectedFunction, new Interval(a, a1), new Interval(b, b1), Iterations, epsilon);
                    OutputField = MakeOutputString(IntervalResult, SelectedFunction, epsilon, SelectedMethod);
                }
                
            }
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e);
            Console.WriteLine(e.StackTrace);
            OutputField = "Wystąpił niespodziewany błąd.";
        }
    }

    private static string MakeOutputString<T>(Result<T> result, IFunction function, BigFloat epsilon,
        string method)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"Wybrana Metoda: {method}");
        sb.AppendLine($"ε: {epsilon}");
        sb.AppendLine($"Liczba iteracji: {result.Iterations}");
        // Status oceny
        sb.Append("Status: ");
        switch (result.Status)
        {
            case EvalStatus.FULL_SUCCESS:
                sb.AppendLine("Sukces – znaleziono rozwiązanie");
                break;
            case EvalStatus.NO_SIGN_CHANGE:
                sb.AppendLine("Błąd – funkcja nie zmienia znaku w podanym przedziale.");
                break;
            case EvalStatus.NOT_ENOUGH_ITERATIONS:
                sb.AppendLine("Nie wystarczająca ilość iteracji.");
                break;
            case EvalStatus.ERROR:
            default:
                sb.AppendLine("Wystąpił nieznany błąd.");
                break;
        }
        

        // Wypisz wynik tylko jeśli istnieje
        if (result.Value is not null)
        {
            if (result.Value is BigFloat bigFloat)
            {
                sb.AppendLine($"X\u2080: {bigFloat.ToString("e16")}");
            }
            else if (result.Value is Interval interval)
            {
                
                sb.AppendLine($"X\u2080: {interval}");
                sb.AppendLine($"Szerokość przedziału: {interval.Width().ToString("e16")}");
            }
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

        if (IsArithmeticModeSelected)
        {
            try
            {
                BigFloat.Parse(Start1);
            }
            catch (FormatException)
            {
                stringBuilder.AppendLine("- Źle podany początek przedziału!");
                isValid = false;
            }

            try
            {
                BigFloat.Parse(End1);
            }
            catch (FormatException)
            {
                stringBuilder.AppendLine("- Źle podany koniec przedziału!");
                isValid = false;
            }
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