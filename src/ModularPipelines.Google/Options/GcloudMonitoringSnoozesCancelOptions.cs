using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitoring", "snoozes", "cancel")]
public record GcloudMonitoringSnoozesCancelOptions(
[property: CliArgument] string Snooze
) : GcloudOptions;