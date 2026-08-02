namespace ModularPipelines.Models;

/// <summary>
/// Describes modules whose dependencies allow them to start in the same execution wave.
/// </summary>
public sealed record PipelinePlanWave
{
    /// <summary>
    /// Gets the one-based wave number.
    /// </summary>
    public int Number { get; }

    /// <summary>
    /// Gets the modules in this wave.
    /// </summary>
    public IReadOnlyList<PipelinePlanModule> Modules { get; }

    /// <summary>
    /// Gets the estimated wave duration. Modules in a wave may run concurrently, so this is the longest runnable module estimate.
    /// </summary>
    public TimeSpan EstimatedDuration { get; }

    internal PipelinePlanWave(int number, IReadOnlyList<PipelinePlanModule> modules)
    {
        Number = number;
        Modules = modules;
        EstimatedDuration = modules
            .Where(module => !module.ShouldSkip)
            .Select(module => module.EstimatedDuration)
            .DefaultIfEmpty(TimeSpan.Zero)
            .Max();
    }
}
