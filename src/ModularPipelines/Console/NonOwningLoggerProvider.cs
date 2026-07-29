using Microsoft.Extensions.Logging;

namespace ModularPipelines.Console;

internal sealed class NonOwningLoggerProvider(ILoggerProvider inner) : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName) => inner.CreateLogger(categoryName);

    public void Dispose()
    {
    }
}
