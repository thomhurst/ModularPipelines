using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "autoscale", "profile", "list")]
public record AzMonitorAutoscaleProfileListOptions(
[property: CliOption("--autoscale-name")] string AutoscaleName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;