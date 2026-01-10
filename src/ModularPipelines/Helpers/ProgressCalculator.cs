namespace ModularPipelines.Helpers;

/// <summary>
/// Provides progress calculation logic for module execution tracking.
/// </summary>
internal class ProgressCalculator : IProgressCalculator
{
    private const double HeadroomMultiplier = 1.1; // Give 10% headroom
    private const double MinEstimatedSeconds = 1.0;
    private const double TotalProgressPercentage = 100.0;

    /// <inheritdoc />
    public ProgressTickInfo CalculateTickInfo(TimeSpan estimatedDuration)
    {
        var estimatedWithHeadroom = estimatedDuration * HeadroomMultiplier;
        var totalEstimatedSeconds = estimatedWithHeadroom.TotalSeconds >= MinEstimatedSeconds
            ? estimatedWithHeadroom.TotalSeconds
            : MinEstimatedSeconds;

        var ticksPerSecond = TotalProgressPercentage / totalEstimatedSeconds;

        return new ProgressTickInfo(ticksPerSecond);
    }

    /// <inheritdoc />
    public double CalculateProgressIncrement(int totalModuleCount)
    {
        if (totalModuleCount <= 0)
        {
            return 0;
        }

        return TotalProgressPercentage / totalModuleCount;
    }
}
