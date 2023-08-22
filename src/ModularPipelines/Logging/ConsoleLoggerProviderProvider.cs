using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace ModularPipelines.Logging;

internal class ConsoleLoggerProviderProvider : IConsoleLoggerProviderProvider
{
    private readonly IEnumerable<ILoggerProvider> _loggerProviders;

    public ConsoleLoggerProviderProvider(IEnumerable<ILoggerProvider> loggerProviders)
    {
        _loggerProviders = loggerProviders;
    }
    
    public ConsoleLoggerProvider GetConsoleLoggerProvider()
    {
        return _loggerProviders.OfType<ConsoleLoggerProvider>().Single();
    }
}