using Microsoft.Extensions.Logging;
using ModularPipelines.Logging;

namespace ModularPipelines.Logging;

/// <summary>
/// Internal module logger contract that combines MEL logging with framework-owned lifecycle.
/// </summary>
internal interface IModuleLogger : ILogger, IDisposable
{
}
