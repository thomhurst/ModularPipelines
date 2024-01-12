using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitoring", "snoozes", "cancel")]
public record GcloudMonitoringSnoozesCancelOptions(
[property: PositionalArgument] string Snooze
) : GcloudOptions;