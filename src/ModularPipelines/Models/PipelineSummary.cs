using System.Text.Json.Serialization;
using EnumerableAsyncProcessor.Extensions;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;

namespace ModularPipelines.Models;

public record PipelineSummary
{
    private readonly IModuleResultRegistry? _resultRegistry;

    /// <summary>
    /// Gets the modules that are part of the pipeline.
    /// </summary>
    /// <remarks>
    /// This property is excluded from JSON serialization as interface types cannot be deserialized.
    /// </remarks>
    [JsonIgnore]
    public IReadOnlyList<IModule> Modules { get; private set; }

    /// <summary>
    /// Gets how long the pipeline took to run.
    /// </summary>
    [JsonInclude]
    public TimeSpan TotalDuration { get; private set; }

    /// <summary>
    /// Gets when the pipeline started.
    /// </summary>
    [JsonInclude]
    public DateTimeOffset Start { get; private set; }

    /// <summary>
    /// Gets when the pipeline finished.
    /// </summary>
    [JsonInclude]
    public DateTimeOffset End { get; private set; }

    [JsonConstructor]
    internal PipelineSummary(
        TimeSpan totalDuration,
        DateTimeOffset start,
        DateTimeOffset end)
    {
        Modules = Array.Empty<IModule>();
        TotalDuration = totalDuration;
        Start = start;
        End = end;
    }

    internal PipelineSummary(IReadOnlyList<IModule> modules,
        TimeSpan totalDuration,
        DateTimeOffset start,
        DateTimeOffset end)
    {
        Modules = modules;
        TotalDuration = totalDuration;
        Start = start;
        End = end;
    }

    internal PipelineSummary(IReadOnlyList<IModule> modules,
        TimeSpan totalDuration,
        DateTimeOffset start,
        DateTimeOffset end,
        IModuleResultRegistry resultRegistry)
        : this(modules, totalDuration, start, end)
    {
        _resultRegistry = resultRegistry;
    }

    /// <summary>
    /// Gets the status of the pipeline.
    /// </summary>
    public Status Status => GetStatus();

    /// <summary>
    /// Get the Module of type {T}.
    /// </summary>
    /// <typeparam name="T">The module type to get.</typeparam>
    /// <returns>{T}.</returns>
    public T GetModule<T>()
        where T : IModule
        => Modules.GetModule<T>();

    public async Task<IReadOnlyList<IModuleResult>> GetModuleResultsAsync()
    {
        return await Modules.SelectAsync(async x =>
        {
            // Get the result from the registry if available
            if (_resultRegistry != null)
            {
                var result = _resultRegistry.GetResult(x.GetType());
                if (result != null)
                {
                    return result;
                }
            }

            // Fallback: create a cancellation result for modules that haven't completed
            return (IModuleResult) new ModuleResult(new TaskCanceledException(), new ModuleExecutionContext(x, x.GetType()));
        }).ProcessInParallel();
    }

    private Status GetStatus()
    {
        // Check if we have a result registry to get module statuses
        if (_resultRegistry != null)
        {
            foreach (var module in Modules)
            {
                var result = _resultRegistry.GetResult(module.GetType());
                if (result != null)
                {
                    if (result.ModuleResultType == ModuleResultType.Failure)
                    {
                        return Status.Failed;
                    }
                }
            }

            return Status.Successful;
        }

        // Fallback to checking modules directly
        return Status.Unknown;
    }
}