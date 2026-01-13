using ModularPipelines.Helpers;

namespace ModularPipelines.Exceptions;

/// <summary>
/// Thrown when a module exceeds its configured timeout duration.
/// </summary>
/// <remarks>
/// <para>
/// This exception is thrown when a module's execution time exceeds the configured timeout.
/// The timeout can be configured per-module using the <c>Timeout</c> property or module options.
/// </para>
/// <para><b>When this is thrown:</b></para>
/// <list type="bullet">
/// <item>When a module's <c>ExecuteAsync</c> takes longer than the configured timeout</item>
/// <item>When the module does not respond to cancellation token within the grace period</item>
/// </list>
/// <para><b>Properties available:</b></para>
/// <list type="bullet">
/// <item><see cref="ModuleType"/> - The type of the module that timed out</item>
/// <item><see cref="ConfiguredTimeout"/> - The timeout duration that was configured</item>
/// <item><see cref="ElapsedTime"/> - The actual time elapsed before timeout was enforced</item>
/// <item><see cref="WasCancellationTokenRespected"/> - Whether the module observed the cancellation token</item>
/// </list>
/// <para><b>Handling example:</b></para>
/// <code>
/// try
/// {
///     await pipelineHost.ExecuteAsync();
/// }
/// catch (ModuleTimeoutException ex)
/// {
///     Console.WriteLine($"Module {ex.ModuleType.Name} timed out");
///     Console.WriteLine($"Configured timeout: {ex.ConfiguredTimeout}");
///     Console.WriteLine($"Actual elapsed: {ex.ElapsedTime}");
///
///     if (!ex.WasCancellationTokenRespected)
///     {
///         Console.WriteLine("Warning: Module did not respond to cancellation token");
///     }
/// }
/// </code>
/// <para><b>Resolution:</b></para>
/// <list type="bullet">
/// <item>Increase the module timeout if the operation legitimately takes longer</item>
/// <item>Ensure the module properly observes the <c>CancellationToken</c> parameter</item>
/// <item>Optimize the module's execution to complete within the timeout</item>
/// </list>
/// </remarks>
/// <seealso cref="PipelineException"/>
/// <seealso cref="PipelineCancelledException"/>
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
