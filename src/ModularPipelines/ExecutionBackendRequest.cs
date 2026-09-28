using ModularPipelines.Distributed;
using ModularPipelines.Modules;

namespace ModularPipelines;

/// <summary>
/// The execution plan passed to <see cref="IExecutionBackend.ExecuteAsync"/>.
/// </summary>
public sealed class ExecutionBackendRequest
{
    /// <summary>
    /// Gets the planned modules to execute.
    /// </summary>
    public required IReadOnlyList<IModule> Modules { get; init; }

    /// <summary>
    /// Gets historical duration estimates keyed by <see cref="ModuleId"/>, used to prioritise
    /// scheduling. Modules without history are absent from the dictionary.
    /// </summary>
    public required IReadOnlyDictionary<ModuleId, TimeSpan> EstimatedDurations { get; init; }

    /// <summary>
    /// Gets the operations for executing planned modules locally and applying remotely produced results.
    /// </summary>
    public required IExecutionBackendContext Context { get; init; }

    /// <summary>
    /// Returns the estimated durations keyed by module type for the planned modules.
    /// </summary>
    internal IReadOnlyDictionary<Type, TimeSpan> GetEstimatedDurationsByType()
    {
        var durations = new Dictionary<Type, TimeSpan>();
        foreach (var module in Modules)
        {
            var moduleType = module.GetType();
            if (EstimatedDurations.TryGetValue(ModuleId.FromType(moduleType), out var duration))
            {
                durations[moduleType] = duration;
            }
        }

        return durations;
    }
}
