using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSHC.Avalonia.Services;

/// <summary>
/// Avalonia implementation of dialog services.
/// </summary>
public class AvaloniaDialogService : IDialogService
{
    private static Window? GetMainWindow()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            return desktop.MainWindow;
        }
        return null;
    }

    public async Task ShowMessageAsync(string title, string message)
    {
        var window = GetMainWindow();
        if (window == null) return;

        var dialog = new Window
        {
            Title = title,
            Width = 400,
            Height = 150,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Content = new StackPanel
            {
                Margin = new Thickness(20),
                Children =
                {
                    new TextBlock { Text = message, TextWrapping = TextWrapping.Wrap },
                    new Button { Content = "OK", HorizontalAlignment = HorizontalAlignment.Center, Margin = new Thickness(0, 20, 0, 0) }
                }
            }
        };

        var button = ((StackPanel)dialog.Content).Children.OfType<Button>().First();
        button.Click += (s, e) => dialog.Close();

        await dialog.ShowDialog(window);
    }

    public async Task ShowErrorAsync(string title, string message)
    {
        await ShowMessageAsync(title, message);
    }

    public async Task<bool> ShowConfirmAsync(string title, string message)
    {
        var window = GetMainWindow();
        if (window == null) return false;

        var result = false;

        var dialog = new Window
        {
            Title = title,
            Width = 400,
            Height = 150,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Content = new StackPanel
            {
                Margin = new Thickness(20),
                Children =
                {
                    new TextBlock { Text = message, TextWrapping = TextWrapping.Wrap },
                    new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Margin = new Thickness(0, 20, 0, 0),
                        Spacing = 10,
                        Children =
                        {
                            new Button { Content = "Yes" },
                            new Button { Content = "No" }
                        }
                    }
                }
            }
        };

        var buttons = ((StackPanel)((StackPanel)dialog.Content).Children[1]).Children.OfType<Button>().ToList();
        buttons[0].Click += (s, e) => { result = true; dialog.Close(); };
        buttons[1].Click += (s, e) => { result = false; dialog.Close(); };

        await dialog.ShowDialog(window);
        return result;
    }

    public async Task<string?> ShowSaveFileDialogAsync(string defaultExtension, string defaultFileName, string? filter = null)
    {
        var window = GetMainWindow();
        if (window == null) return null;

        var options = new FilePickerSaveOptions
        {
            SuggestedFileName = defaultFileName,
            DefaultExtension = defaultExtension
        };

        if (!string.IsNullOrEmpty(filter))
        {
            options.FileTypeChoices = ParseFilter(filter);
        }

        var result = await window.StorageProvider.SaveFilePickerAsync(options);
        return result?.Path.LocalPath;
    }

    public async Task<string?> ShowOpenFileDialogAsync(string? filter = null)
    {
        var window = GetMainWindow();
        if (window == null) return null;

        var options = new FilePickerOpenOptions
        {
            AllowMultiple = false
        };

        if (!string.IsNullOrEmpty(filter))
        {
            options.FileTypeFilter = ParseFilter(filter);
        }

        var result = await window.StorageProvider.OpenFilePickerAsync(options);
        return result.FirstOrDefault()?.Path.LocalPath;
    }

    public async Task<string?> ShowFolderPickerAsync()
    {
        var window = GetMainWindow();
        if (window == null) return null;

        var result = await window.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            AllowMultiple = false
        });

        return result.FirstOrDefault()?.Path.LocalPath;
    }

    private static List<FilePickerFileType> ParseFilter(string filter)
    {
        var types = new List<FilePickerFileType>();
        var parts = filter.Split('|');

        for (int i = 0; i < parts.Length - 1; i += 2)
        {
            var name = parts[i];
            var patterns = parts[i + 1].Split(';').ToList();
            types.Add(new FilePickerFileType(name) { Patterns = patterns });
        }

        return types;
    }
}
