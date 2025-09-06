namespace WpfToolbox.Misc;

/// <summary>
/// A simple profiler utility for measuring the execution time of a code block.
/// Usage: wrap the code to be measured in a <c>using</c> statement with a <see cref="Profiler"/> instance.
/// The elapsed time is written to the debug output when disposed.
/// </summary>
public sealed class Profiler : IDisposable
{
    private readonly Stopwatch watch;
    private readonly string name;

    /// <summary>
    /// Initializes a new instance of the <see cref="Profiler"/> class and starts timing.
    /// </summary>
    /// <param name="name">A name or label for the profiled code block.</param>
    public Profiler(string name)
    {
        this.name = name;
        watch = new Stopwatch();
        watch.Start();
    }

    /// <summary>
    /// Stops timing and writes the elapsed time to the debug output.
    /// </summary>
    public void Dispose()
    {
        watch.Stop();
        Debug.WriteLine($"Profiler: {name} needs {watch} ms");
    }
}
