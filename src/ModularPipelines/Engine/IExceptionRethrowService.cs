namespace ModularPipelines.Engine;

/// <summary>
/// Service for rethrowing stored exceptions while preserving their original stack traces.
/// Centralizes the exception rethrow pattern used during pipeline execution.
/// </summary>
internal interface IExceptionRethrowService
{
    /// <summary>
    /// Throws the original exception if one is stored, preserving the original stack trace.
    /// Does nothing if no exception has been stored.
    /// </summary>
    void ThrowOriginalExceptionIfPresent();
}
