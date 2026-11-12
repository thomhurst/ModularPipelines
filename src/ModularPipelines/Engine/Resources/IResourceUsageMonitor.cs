using ModularPipelines.Reporting;

namespace ModularPipelines.Engine.Resources;

/// <summary>
/// Samples machine-wide CPU and memory for the duration of a pipeline run.
/// </summary>
internal interface IResourceUsageMonitor
{
    /// <summary>
    /// Starts sampling. Disposing the returned scope stops it.
    /// </summary>
    IAsyncDisposable Start();

    /// <summary>
    /// Gets the usage sampled so far, or <see langword="null"/> when no sample was taken.
    /// </summary>
    ResourceUsage? GetResult();
}
