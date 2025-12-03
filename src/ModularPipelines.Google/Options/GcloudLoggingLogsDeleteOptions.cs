using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logging", "logs", "delete")]
public record GcloudLoggingLogsDeleteOptions(
[property: CliArgument] string LogName
) : GcloudOptions;