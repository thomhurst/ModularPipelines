using Microsoft.Extensions.Logging;

namespace ModularPipelines.Logging;

public interface IModuleLogger : ILogger, IDisposable, IConsoleWriter;