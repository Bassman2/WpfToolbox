namespace WpfToolbox.Misc;

public static class Tools
{
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
        //process.StartInfo.WorkingDirectory = workingDirectory;
        process.OutputDataReceived += (s, e) => Debug.WriteLine(e.Data);
        process.ErrorDataReceived += (s, e) => error += e.Data;
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
