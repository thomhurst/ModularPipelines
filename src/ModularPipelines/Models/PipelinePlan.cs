using System.Text.Json.Serialization;
using ModularPipelines.Modules;

namespace ModularPipelines.Models;

/// <summary>
/// Describes the work a pipeline would perform without executing any modules.
/// </summary>
public sealed record PipelinePlan
{
    /// <summary>
    /// Gets the modules in the pipeline.
    /// </summary>
    [JsonIgnore]
    public IReadOnlyList<IModule> Modules { get; }

    /// <summary>
    /// Gets the dependency-ordered execution waves.
    /// </summary>
    public IReadOnlyList<PipelinePlanWave> Waves { get; }

    /// <summary>
    /// Gets the estimated pipeline duration, calculated as the sum of wave estimates.
    /// </summary>
    public TimeSpan EstimatedDuration { get; }

    internal PipelinePlan(IReadOnlyList<IModule> modules, IReadOnlyList<PipelinePlanWave> waves)
    {
        Modules = modules;
        Waves = waves;
        EstimatedDuration = waves.Aggregate(TimeSpan.Zero, static (total, wave) => total + wave.EstimatedDuration);
    }
}
