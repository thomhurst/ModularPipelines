using ModularPipelines.Logging;

namespace ModularPipelines.Context;

/// <summary>
/// Provides access to logging functionality.
/// </summary>
/// <remarks>
/// <para>
/// This interface groups logging-related members for better Interface Segregation.
/// </para>
/// <para><b>Thread Safety:</b></para>
/// <para>
/// The <see cref="Logger"/> property returns a thread-safe logger instance.
/// Multiple threads may concurrently call logging methods without synchronization.
/// Log messages are internally queued and processed in order, ensuring no message loss
/// or corruption during concurrent access.
/// </para>
/// </remarks>
public interface IPipelineLogging
{
    /// <summary>
    /// Gets the logger to be used from the pipeline.
    /// </summary>
    /// <remarks>
    /// This logger is module-scoped and will include the module name in log output.
    /// </remarks>
    IModuleLogger Logger { get; }
}
