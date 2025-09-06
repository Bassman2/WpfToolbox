namespace WpfToolbox.Misc;

/// <summary>
/// Provides utility methods for launching external processes, opening browsers, file explorers, help files, and running shell commands.
/// </summary>
public static class Tools
{
    /// <summary>
    /// Opens the specified URL in the default web browser.
    /// </summary>
    /// <param name="url">The URL to open.</param>
    public static void OpenBrowser(string url)
    {
        try
        {
            Process myProcess = new();
            myProcess.StartInfo.UseShellExecute = true;
            myProcess.StartInfo.FileName = url;
            myProcess.Start();
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
        }
    }

    /// <summary>
    /// Opens the specified path in the system file explorer.
    /// </summary>
    /// <param name="url">The file or folder path to open.</param>
    public static void OpenExplorer(string url)
    {
        try
        {
            Process myProcess = new();
            myProcess.StartInfo.UseShellExecute = true;
            myProcess.StartInfo.FileName = url;
            myProcess.Start();
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
        }
    }

    /// <summary>
    /// Opens a help file (.chm) if it exists, or shows an error message if not found.
    /// </summary>
    /// <param name="filePath">The path to the help file. If null, uses the entry assembly's .chm file.</param>
    public static void OpenHelp(string? filePath = null)
    {
        filePath ??= System.IO.Path.ChangeExtension(Assembly.GetEntryAssembly()!.Location, ".chm");
        if (System.IO.File.Exists(filePath))
        {
            Process.Start(filePath);
        }
        else
        {
            MessageBox.Show(string.Format("Help file \"{0}\" not found!", filePath), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Runs a shell command using cmd.exe and throws an exception if the command fails.
    /// </summary>
    /// <param name="cmd">The command to execute.</param>
    /// <param name="workingDirectory">The working directory for the command (currently not used).</param>
    public static void RunCommand(string cmd, string? workingDirectory = null)
    {
        string error = string.Empty;

        var process = new Process();
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.FileName = "cmd.exe";
        process.StartInfo.Arguments = $"/C {cmd}";
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.WorkingDirectory = workingDirectory;
        process.OutputDataReceived += (s, e) => Debug.WriteLine(e.Data);
        process.ErrorDataReceived += (s, e) => error += e.Data + Environment.NewLine;
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        process.WaitForExit();
        if (process.ExitCode != 0)
        {
            throw new Exception($"Error {process.ExitCode}: {cmd} {error}");
        }
    }
}
