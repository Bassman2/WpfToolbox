namespace WpfToolbox.Misc;

public sealed class Profiler : IDisposable
{
    private readonly Stopwatch watch;
    private readonly string name;

    public Profiler(string name)
    {
        this.name = name;
        watch = new Stopwatch();
        watch.Start();
    }

    public void Dispose()
    {
        watch.Stop();
        Debug.WriteLine($"Profiler: {name} needs {watch} ms");
    }
}
