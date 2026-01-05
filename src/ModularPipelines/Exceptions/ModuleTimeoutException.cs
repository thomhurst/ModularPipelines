using ModularPipelines.Helpers;

namespace ModularPipelines.Exceptions;

/// <summary>
/// Exception thrown when a module exceeds its configured timeout duration.
/// </summary>
/// <remarks>
/// This exception contains diagnostic information about the timeout, including:
/// <list type="bullet">
/// <item>The configured timeout duration</item>
/// <item>The actual elapsed time before timeout was enforced</item>
/// <item>Whether the module properly respected the cancellation token</item>
/// </list>
/// </remarks>
public class ModuleTimeoutException : PipelineException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ModuleTimeoutException"/> class.
    /// </summary>
    /// <param name="moduleType">The type of the module that timed out.</param>
    /// <param name="configuredTimeout">The timeout duration that was configured.</param>
    internal ModuleTimeoutException(Type moduleType, TimeSpan configuredTimeout)
        : this(moduleType, configuredTimeout, configuredTimeout, wasCancellationTokenRespected: true)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ModuleTimeoutException"/> class
    /// with detailed timeout information.
    /// </summary>
    /// <param name="moduleType">The type of the module that timed out.</param>
    /// <param name="configuredTimeout">The timeout duration that was configured.</param>
    /// <param name="elapsedTime">The actual time elapsed before timeout was enforced.</param>
    /// <param name="wasCancellationTokenRespected">Whether the module observed the cancellation token.</param>
    internal ModuleTimeoutException(
        Type moduleType,
        TimeSpan configuredTimeout,
        TimeSpan elapsedTime,
        bool wasCancellationTokenRespected)
        : base(FormatMessage(moduleType, configuredTimeout, elapsedTime, wasCancellationTokenRespected))
    {
        ModuleType = moduleType;
        ConfiguredTimeout = configuredTimeout;
        ElapsedTime = elapsedTime;
        WasCancellationTokenRespected = wasCancellationTokenRespected;
    }

    /// <summary>
    /// Gets the type of the module that timed out.
    /// </summary>
    public Type ModuleType { get; }

    /// <summary>
    /// Gets the timeout duration that was configured for the module.
    /// </summary>
    public TimeSpan ConfiguredTimeout { get; }

    /// <summary>
    /// Gets the actual time elapsed before the timeout was enforced.
    /// </summary>
    /// <remarks>
    /// This may differ from <see cref="ConfiguredTimeout"/> due to grace periods
    /// or timing imprecision.
    /// </remarks>
    public TimeSpan ElapsedTime { get; }

    /// <summary>
    /// Gets a value indicating whether the module properly observed the cancellation token.
    /// </summary>
    /// <remarks>
    /// When <c>true</c>, the module responded to the cancellation token and exited gracefully.
    /// When <c>false</c>, the module ignored the cancellation token and was forcibly terminated
    /// by the timeout enforcement mechanism (Task.WhenAny pattern).
    /// </remarks>
    public bool WasCancellationTokenRespected { get; }

    private static string FormatMessage(
        Type moduleType,
        TimeSpan configuredTimeout,
        TimeSpan elapsedTime,
        bool wasCancellationTokenRespected)
    {
        var baseMessage = $"{moduleType.Name} has timed out after {configuredTimeout.ToDisplayString()}";

        if (!wasCancellationTokenRespected)
        {
            return $"{baseMessage}. The module did not respond to the cancellation token and was forcibly terminated after {elapsedTime.ToDisplayString()}.";
        }

        return baseMessage;
    }
}
