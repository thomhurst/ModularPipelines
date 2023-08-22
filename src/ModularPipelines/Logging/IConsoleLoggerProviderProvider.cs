using Microsoft.Extensions.Logging.Console;

namespace ModularPipelines.Logging;

internal interface IConsoleLoggerProviderProvider
{
    ConsoleLoggerProvider GetConsoleLoggerProvider();
}