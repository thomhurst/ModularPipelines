namespace ModularPipelines.Engine;

/// <summary>
/// Collects secondary exceptions that occur during pipeline execution,
/// such as failures in AlwaysRun modules or dependency resolution.
/// The primary/first exception that fails the pipeline is stored in <see cref="IPrimaryExceptionContainer"/>.
/// </summary>
/// <remarks>
/// <para>
/// <b>Thread Safety:</b> Implementations of this interface must be thread-safe.
/// <see cref="RegisterException"/> can be called concurrently from multiple threads.
/// </para>
/// </remarks>
internal interface ISecondaryExceptionContainer
{
    /// <summary>
    /// Registers an exception that occurred during pipeline execution.
    /// </summary>
    /// <param name="exception">The exception to register.</param>
    /// <remarks>
    /// This method must be thread-safe and can be called concurrently from multiple modules.
    /// </remarks>
    void RegisterException(Exception exception);

    /// <summary>
    /// Throws all registered exceptions.
    /// </summary>
    /// <remarks>
    /// If exactly one exception was registered, it is rethrown with its original stack trace preserved.
    /// If multiple exceptions were registered, an <see cref="AggregateException"/> is thrown.
    /// </remarks>
    void ThrowExceptions();
}
