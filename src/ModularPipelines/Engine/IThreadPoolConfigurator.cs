namespace ModularPipelines.Engine;

/// <summary>
/// Provides thread pool configuration for optimal pipeline execution.
/// </summary>
internal interface IThreadPoolConfigurator
{
    /// <summary>
    /// Configures the thread pool to ensure adequate parallelism for module execution.
    /// </summary>
    void Configure();
}
