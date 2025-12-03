using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitoring", "snoozes", "list")]
public record GcloudMonitoringSnoozesListOptions : GcloudOptions;