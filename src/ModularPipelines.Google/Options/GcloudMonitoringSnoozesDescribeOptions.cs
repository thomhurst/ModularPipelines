using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitoring", "snoozes", "describe")]
public record GcloudMonitoringSnoozesDescribeOptions(
[property: PositionalArgument] string Snooze
) : GcloudOptions;