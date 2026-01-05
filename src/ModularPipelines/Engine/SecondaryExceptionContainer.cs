using System.Collections.Concurrent;
using System.Runtime.ExceptionServices;

namespace ModularPipelines.Engine;

/// <summary>
/// Collects secondary exceptions that occur during pipeline execution,
/// such as failures in AlwaysRun modules or dependency resolution.
/// The primary/first exception that fails the pipeline is stored in <see cref="IPrimaryExceptionContainer"/>.
/// </summary>
/// <remarks>
/// <para>
/// <b>Thread Safety:</b> This class is thread-safe. All public methods can be called
/// concurrently from multiple threads without external synchronization.
/// </para>
/// <para>
/// Exceptions are stored in a <see cref="ConcurrentBag{T}"/> which provides
/// lock-free thread-safe operations for concurrent module execution scenarios.
/// </para>
/// </remarks>
/// <threadsafety static="true" instance="true"/>
internal class SecondaryExceptionContainer : ISecondaryExceptionContainer
{
    private readonly ConcurrentBag<ExceptionDispatchInfo> _exceptions = [];

    /// <summary>
    /// Registers an exception that occurred during pipeline execution.
    /// </summary>
    /// <param name="exception">The exception to register.</param>
    /// <remarks>
    /// This method is thread-safe and can be called concurrently from multiple modules.
    /// </remarks>
    public void RegisterException(Exception exception)
    {
        _exceptions.Add(ExceptionDispatchInfo.Capture(exception));
    }

    /// <summary>
    /// Throws all registered exceptions.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If exactly one exception was registered, it is rethrown with its original stack trace preserved.
    /// If multiple exceptions were registered, an <see cref="AggregateException"/> is thrown containing all of them.
    /// </para>
    /// <para>
    /// This method should typically be called after all module execution has completed,
    /// when no more concurrent registrations are expected.
    /// </para>
    /// </remarks>
    public void ThrowExceptions()
    {
        // Take a snapshot for consistent behavior
        var exceptionsList = _exceptions.ToArray();

        if (exceptionsList.Length == 1)
        {
            exceptionsList[0].Throw();
        }

        if (exceptionsList.Length > 0)
        {
            throw new AggregateException(exceptionsList.Select(e => e.SourceException));
        }
    }
}
