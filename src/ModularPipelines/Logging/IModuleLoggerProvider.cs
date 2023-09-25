using Microsoft.Extensions.Logging;

namespace ModularPipelines.Logging;

public interface IModuleLoggerProvider
{
    internal ILogger GetLogger(Type type);

    ILogger GetLogger();
}
