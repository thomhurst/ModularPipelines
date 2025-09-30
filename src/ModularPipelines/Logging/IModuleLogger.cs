using Microsoft.Extensions.Logging;

namespace ModularPipelines.Logging;

/// <summary>
/// Provides logging functionality specifically for modules.
/// Combines standard logging with lifecycle management through disposal.
/// </summary>
/// <remarks>
/// This interface follows the Interface Segregation Principle by focusing only on core logging concerns.
/// For console writing capabilities, see <see cref="IConsoleWriter"/>.
/// Module loggers typically implement both interfaces, but consumers should depend only on what they need.
/// </remarks>
public interface IModuleLogger : ILogger, IDisposable
{
    internal void SetException(Exception exception);
}