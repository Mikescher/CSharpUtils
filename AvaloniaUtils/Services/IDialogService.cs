namespace MSHC.Avalonia.Services;

/// <summary>
/// Abstraction for dialog operations.
/// Replaces MessageBox.Show and file dialogs.
/// </summary>
public interface IDialogService
{
    /// <summary>
    /// Shows an information message dialog.
    /// </summary>
    Task ShowMessageAsync(string title, string message);

    /// <summary>
    /// Shows an error message dialog.
    /// </summary>
    Task ShowErrorAsync(string title, string message);

    /// <summary>
    /// Shows a confirmation dialog.
    /// </summary>
    Task<bool> ShowConfirmAsync(string title, string message);

    /// <summary>
    /// Shows a save file dialog and returns the selected path, or null if cancelled.
    /// </summary>
    Task<string?> ShowSaveFileDialogAsync(string defaultExtension, string defaultFileName, string? filter = null);

    /// <summary>
    /// Shows an open file dialog and returns the selected path, or null if cancelled.
    /// </summary>
    Task<string?> ShowOpenFileDialogAsync(string? filter = null);

    /// <summary>
    /// Shows a folder picker dialog and returns the selected path, or null if cancelled.
    /// </summary>
    Task<string?> ShowFolderPickerAsync();
}
