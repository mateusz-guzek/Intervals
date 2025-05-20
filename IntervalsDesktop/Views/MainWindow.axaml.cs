using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Functions;
using IntervalsDesktop.ViewModels;

namespace IntervalsDesktop.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public async void OnSelectLibraryButtonClicked(object? sender, RoutedEventArgs e)
    {

        try
        {
            // Get top level from the current control. Alternatively, you can use Window reference instead.
            var topLevel = TopLevel.GetTopLevel(this);

            // Start async operation to open the dialog.
            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Otwórz swoją bibliotekę",
                AllowMultiple = false,
                FileTypeFilter = new List<FilePickerFileType>(){new("dll")}
            });

            if (files.Count >= 1)
            {
                string pathToDll = files[0].Path.AbsolutePath;

                Assembly assembly = Assembly.LoadFrom(pathToDll);

                // Znajdź typ implementujący IMyFunctions
                Type? myFunctionsType = assembly
                    .GetTypes()
                    .FirstOrDefault(t => typeof(IMyFunctions).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

                if (myFunctionsType != null)
                {
                    var instance = Activator.CreateInstance(myFunctionsType) as IMyFunctions;

                    if (instance != null)
                    {
                        var functions = instance.Functions();
                        if (DataContext is MainWindowViewModel viewModel)
                        {
                            foreach (var function in functions)
                            {
                                viewModel.Functions.Add(function);
                            }
                            
                        }

                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}