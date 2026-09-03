namespace ModularPipelines.Logging;

/// <summary>
/// Identifies the module output destination that flows with an execution context.
/// </summary>
/// <remarks>
/// The shared active state invalidates flowed copies when the owning scope ends, keeping
/// fire-and-forget writes from being appended to a completed module.
/// </remarks>
internal sealed class ModuleOutputContext
{
    private int _isActive = 1;

    public ModuleOutputContext(Type moduleType, IModuleLogger? logger)
    {
        ModuleType = moduleType;
        Logger = logger;
    }

    public Type ModuleType { get; }

    public IModuleLogger? Logger { get; }

    public bool IsActive => Volatile.Read(ref _isActive) == 1;

    public void Deactivate() => Interlocked.Exchange(ref _isActive, 0);
}

/// <summary>
/// Provides internal read access to the current module output destination.
/// </summary>
internal static class AmbientModuleOutputContext
{
    private static readonly AsyncLocal<ModuleOutputContext?> Storage = new();

    public static ModuleOutputContext? Current =>
        Storage.Value is { IsActive: true } context ? context : null;

    internal static ModuleOutputContext? RawValue
    {
        get => Storage.Value;
        set => Storage.Value = value;
    }
}

/// <summary>
/// Establishes one consistent ambient module output destination and restores it on disposal.
/// </summary>
internal sealed class ModuleOutputContextScope : IDisposable
{
    private readonly ModuleOutputContext? _previous;
    private readonly ModuleOutputContext _current;
    private int _disposed;

    public ModuleOutputContextScope(Type moduleType, IModuleLogger? logger = null)
    {
        _previous = AmbientModuleOutputContext.RawValue;
        _current = new ModuleOutputContext(moduleType, logger);
        AmbientModuleOutputContext.RawValue = _current;
    }

    public void Dispose()
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0)
        {
            return;
        }

        _current.Deactivate();
        if (ReferenceEquals(AmbientModuleOutputContext.RawValue, _current))
        {
            AmbientModuleOutputContext.RawValue = _previous;
        }
    }
}
