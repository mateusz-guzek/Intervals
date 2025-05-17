using System.Collections.Generic;

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

    private string _selectedMode;

    public string SelectedMode
    {
        get => _selectedMode;
        set {
        _selectedMode = value;
    }
}

}