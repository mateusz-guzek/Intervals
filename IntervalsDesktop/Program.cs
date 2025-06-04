using Avalonia;
using System;
using Classic.CommonControls;
using Numerics.NET;

namespace IntervalsDesktop;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.

    [STAThread]
    public static void Main(string[] args)
    {
        BigFloat.DefaultAccuracyGoal = AccuracyGoal.Absolute(300);
        BigFloat.InitialAccuracyGoal = AccuracyGoal.Absolute(300);
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseMessageBoxSounds();
}