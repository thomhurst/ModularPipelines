using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logging", "logs", "delete")]
public record GcloudLoggingLogsDeleteOptions(
[property: PositionalArgument] string LogName
) : GcloudOptions;