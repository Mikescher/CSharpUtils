namespace MSHC.Avalonia.Services;

/// <summary>
/// Abstraction for launching external processes.
/// Provides cross-platform file/folder opening.
/// </summary>
public interface IProcessLauncher
{
    /// <summary>
    /// Opens a file with the system's default application.
    /// </summary>
    void OpenFile(string path);

    /// <summary>
    /// Opens a folder in the system's file explorer.
    /// </summary>
    void OpenFolder(string path);

    /// <summary>
    /// Opens the containing folder and selects the specified file.
    /// </summary>
    void OpenFolderAndSelect(string filePath);

    /// <summary>
    /// Launches a process with the specified path.
    /// </summary>
    void LaunchProcess(string path, string? arguments = null);
}
