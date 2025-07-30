namespace ModularPipelines.Engine;

/// <summary>
/// Collects secondary exceptions that occur during pipeline execution,
/// such as failures in AlwaysRun modules or dependency resolution.
/// The primary/first exception that fails the pipeline is stored in EngineCancellationToken.
/// </summary>
internal interface ISecondaryExceptionContainer
{
    void RegisterException(Exception exception);

    void ThrowExceptions();
}