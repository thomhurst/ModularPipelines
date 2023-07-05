using Microsoft.Extensions.Logging;

namespace ModularPipelines.Helpers;

internal interface IModuleLoggerProvider
{
    ILogger GetLogger( Type type );
    ILogger GetLogger();
}
