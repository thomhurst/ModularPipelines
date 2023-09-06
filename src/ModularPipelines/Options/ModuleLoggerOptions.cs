using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public record ModuleLoggerOptions
{
    public LogLevel LogLevel { get; set; } = LogLevel.Information;
}
