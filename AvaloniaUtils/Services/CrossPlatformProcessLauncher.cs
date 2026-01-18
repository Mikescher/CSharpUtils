using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MSHC.Avalonia.Services;

/// <summary>
/// Cross-platform implementation of process launching.
/// </summary>
public class CrossPlatformProcessLauncher : IProcessLauncher
{
    public void OpenFile(string path)
    {
        Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
    }

    public void OpenFolder(string path)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Process.Start("explorer", path);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Process.Start("xdg-open", path);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Process.Start("open", path);
        }
    }

    public void OpenFolderAndSelect(string filePath)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Process.Start("explorer", $"/select,\"{filePath}\"");
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            // Linux doesn't have a standard way to select a file in the file manager
            // Fall back to opening the containing folder
            var folder = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(folder))
            {
                Process.Start("xdg-open", folder);
            }
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Process.Start("open", $"-R \"{filePath}\"");
        }
    }

    public void LaunchProcess(string path, string? arguments = null)
    {
        var psi = new ProcessStartInfo(path)
        {
            UseShellExecute = true
        };

        if (!string.IsNullOrEmpty(arguments))
        {
            psi.Arguments = arguments;
        }

        Process.Start(psi);
    }
}
