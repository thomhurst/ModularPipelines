namespace ModularPipelines.Engine;

/// <summary>
/// Service for rethrowing stored exceptions while preserving their original stack traces.
/// Centralizes the exception rethrow pattern used during pipeline execution.
/// </summary>
internal class ExceptionRethrowService : IExceptionRethrowService
{
    private readonly IPrimaryExceptionContainer _primaryExceptionContainer;

    public ExceptionRethrowService(IPrimaryExceptionContainer primaryExceptionContainer)
    {
        _primaryExceptionContainer = primaryExceptionContainer;
    }

    /// <inheritdoc />
    public void ThrowOriginalExceptionIfPresent()
    {
        _primaryExceptionContainer.ThrowIfHasException();
    }
}
