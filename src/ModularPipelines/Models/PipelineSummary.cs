using System.Text.Json.Serialization;
using EnumerableAsyncProcessor.Extensions;
using ModularPipelines.Enums;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;

namespace ModularPipelines.Models;

public record PipelineSummary
{
    /// <summary>
    /// Gets the modules that are part of the pipeline.
    /// </summary>
    [JsonInclude]
    public IReadOnlyList<ModuleBase> Modules { get; private set; }

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
    internal PipelineSummary(IReadOnlyList<ModuleBase> modules,
        TimeSpan totalDuration,
        DateTimeOffset start,
        DateTimeOffset end)
    {
        Modules = modules;
        TotalDuration = totalDuration;
        Start = start;
        End = end;
        
        // If the pipeline is errored, some modules could still be waiting or processing.
        // But we're ending the pipeline so let's signal them to fail.
        foreach (var moduleBase in modules)
        {
            moduleBase.TryCancel();
        }
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
        where T : ModuleBase
        => Modules.GetModule<T>();

    public async Task<IReadOnlyList<IModuleResult>> GetModuleResultsAsync()
    {
        return await Modules.SelectAsync(async x =>
        {
            if (x.Status is Status.Processing or Status.Unknown or Status.NotYetStarted)
            {
                return new ModuleResult(new TaskCanceledException(), x);
            }

            try
            {
                return await x.GetModuleResult();
            }
            catch (Exception e)
            {
                return new ModuleResult(e, x);
            }
        }).ProcessInParallel();
    }

    private Status GetStatus()
    {
        if (Modules.Any(x => x.Status == Status.Failed))
        {
            return Status.Failed;
        }

        if (Modules.Any(x => x.Status == Status.Unknown))
        {
            return Status.Unknown;
        }

        if (Modules.Any(x => x.Status == Status.Processing))
        {
            return Status.Processing;
        }

        if (Modules.Any(x => x.Status == Status.NotYetStarted))
        {
            return Status.Failed;
        }

        if (Modules.Any(x => x.Status == Status.TimedOut))
        {
            return Status.Failed;
        }

        return Status.Successful;
    }
}