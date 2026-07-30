using ModularPipelines.FileSystem;
using ModularPipelines.Models;

namespace ModularPipelines.Testing;

/// <summary>
/// Captures the observable outcome of an isolated module execution.
/// </summary>
public class ModuleTestRun
{
    internal ModuleTestRun(
        IModuleResult result,
        IReadOnlyList<RecordedCommand> commands,
        InMemoryFileSystemProvider fileSystem)
    {
        Result = result;
        Commands = commands;
        FileSystem = fileSystem;
    }

    /// <summary>
    /// Gets the complete module result.
    /// </summary>
    public IModuleResult Result { get; }

    /// <summary>
    /// Gets the module value when execution succeeded.
    /// </summary>
    public object? Value => Result.ValueOrDefault;

    /// <summary>
    /// Gets the failure exception, if any.
    /// </summary>
    public Exception? Exception => Result.ExceptionOrDefault;

    /// <summary>
    /// Gets the skip decision, if any.
    /// </summary>
    public SkipDecision? SkipDecision => Result.SkipDecisionOrDefault;

    /// <summary>
    /// Gets commands intercepted during the run.
    /// </summary>
    public IReadOnlyList<RecordedCommand> Commands { get; }

    /// <summary>
    /// Gets the isolated filesystem used during the run.
    /// </summary>
    public InMemoryFileSystemProvider FileSystem { get; }
}

/// <summary>
/// Captures the strongly typed outcome of an isolated module execution.
/// </summary>
/// <typeparam name="T">The module result value type.</typeparam>
public sealed class ModuleTestRun<T> : ModuleTestRun
{
    internal ModuleTestRun(
        ModuleResult<T> result,
        IReadOnlyList<RecordedCommand> commands,
        InMemoryFileSystemProvider fileSystem)
        : base(result, commands, fileSystem)
    {
        Result = result;
    }

    /// <summary>
    /// Gets the strongly typed module result.
    /// </summary>
    public new ModuleResult<T> Result { get; }

    /// <summary>
    /// Gets the successful value, or the default value otherwise.
    /// </summary>
    public new T? Value => Result.ValueOrDefault;
}
