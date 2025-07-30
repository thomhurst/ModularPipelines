using Microsoft.Extensions.Logging;

namespace ModularPipelines.Logging;

/// <summary>
/// Provides logging functionality specifically for modules, combining standard logging with console output capabilities.
/// </summary>
public interface IModuleLogger : ILogger, IDisposable, IConsoleWriter
{
    internal void SetException(Exception exception);
}