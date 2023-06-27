using Microsoft.Extensions.Logging;

namespace ModularPipelines.Helpers;

internal interface IModuleLoggerProvider
{
    ILogger Logger { get; }
}