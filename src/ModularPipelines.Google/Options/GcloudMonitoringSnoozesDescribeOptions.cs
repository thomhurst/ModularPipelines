using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitoring", "snoozes", "describe")]
public record GcloudMonitoringSnoozesDescribeOptions(
[property: CliArgument] string Snooze
) : GcloudOptions;