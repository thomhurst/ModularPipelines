using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

/// <summary>
/// Attributes module result awaits to the executing module without treating lookups as waits.
/// </summary>
internal sealed class ModuleWaitScope : IDisposable
{
    private static readonly AsyncLocal<ModuleWaitScope?> Current = new();
    private readonly ModuleWaitScope? _previous;
    private readonly IModule _module;
    private readonly IModuleScheduler? _scheduler;
    private int _disposed;

    public ModuleWaitScope(IModule module, IModuleScheduler? scheduler)
    {
        _module = module;
        _scheduler = scheduler;
        _previous = Current.Value;
        Current.Value = this;
    }

    public static void RegisterWait(IModule dependency, Task resultTask)
    {
        if (!resultTask.IsCompleted && Current.Value is { } scope && Volatile.Read(ref scope._disposed) == 0)
        {
            scope._scheduler?.RegisterResultWait(scope._module, dependency, resultTask);
        }
    }

    public void Dispose()
    {
        if (Interlocked.Exchange(ref _disposed, 1) == 0)
        {
            Current.Value = _previous;
        }
    }
}
