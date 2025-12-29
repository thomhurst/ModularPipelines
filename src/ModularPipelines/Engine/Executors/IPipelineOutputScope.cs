namespace ModularPipelines.Engine.Executors;

/// <summary>
/// Represents the lifecycle scope for pipeline output operations.
/// Disposes of output resources when the pipeline execution completes.
/// </summary>
internal interface IPipelineOutputScope : IAsyncDisposable;
