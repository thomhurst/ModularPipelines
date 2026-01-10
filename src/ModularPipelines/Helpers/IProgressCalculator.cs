namespace ModularPipelines.Helpers;

/// <summary>
/// Provides progress calculation logic for module execution tracking.
/// </summary>
internal interface IProgressCalculator
{
    /// <summary>
    /// Calculates the tick information for progress updates based on estimated duration.
    /// </summary>
    /// <param name="estimatedDuration">The estimated duration for the module.</param>
    /// <returns>Information about how to tick progress.</returns>
    ProgressTickInfo CalculateTickInfo(TimeSpan estimatedDuration);

    /// <summary>
    /// Calculates the progress increment for total progress when a module completes.
    /// </summary>
    /// <param name="totalModuleCount">Total number of modules in the pipeline.</param>
    /// <returns>The progress increment percentage.</returns>
    double CalculateProgressIncrement(int totalModuleCount);
}

/// <summary>
/// Contains calculated tick information for progress updates.
/// </summary>
/// <param name="TicksPerSecond">Number of progress ticks to add per second.</param>
internal readonly record struct ProgressTickInfo(double TicksPerSecond);
