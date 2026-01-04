using ModularPipelines.Engine;
using ModularPipelines.Exceptions;

namespace ModularPipelines.Models;

/// <summary>
/// Represents the result of a module that timed out during execution.
/// </summary>
/// <typeparam name="T">The type of result the module would have returned.</typeparam>
/// <remarks>
/// This result type provides detailed information about the timeout, including:
/// <list type="bullet">
/// <item>The configured timeout duration</item>
/// <item>How long the module actually ran before timeout</item>
/// <item>Whether the module respected the cancellation token</item>
/// </list>
/// </remarks>
public class TimedOutModuleResult<T> : ModuleResult<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TimedOutModuleResult{T}"/> class.
    /// </summary>
    /// <param name="executionContext">The execution context of the timed out module.</param>
    /// <param name="timeoutException">The timeout exception that occurred.</param>
    internal TimedOutModuleResult(
        ModuleExecutionContext<T> executionContext,
        ModuleTimeoutException timeoutException)
        : base(timeoutException, executionContext)
    {
        ConfiguredTimeout = timeoutException.ConfiguredTimeout;
        ElapsedTime = timeoutException.ElapsedTime;
        WasCancellationTokenRespected = timeoutException.WasCancellationTokenRespected;
    }

    /// <summary>
    /// Gets the timeout duration that was configured for the module.
    /// </summary>
    public TimeSpan ConfiguredTimeout { get; }

    /// <summary>
    /// Gets the actual time elapsed before the timeout was enforced.
    /// </summary>
    public TimeSpan ElapsedTime { get; }

    /// <summary>
    /// Gets a value indicating whether the module properly observed the cancellation token.
    /// </summary>
    /// <remarks>
    /// If <c>false</c>, the module did not respond to the cancellation token and was
    /// forcibly terminated by the timeout enforcement mechanism.
    /// </remarks>
    public bool WasCancellationTokenRespected { get; }
}
