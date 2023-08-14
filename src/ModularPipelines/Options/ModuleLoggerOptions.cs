using Microsoft.Extensions.Logging;

namespace ModularPipelines.Options;

public record ModuleLoggerOptions
{
    public LogLevel LogLevel { get; set; } = LogLevel.Information;
}
