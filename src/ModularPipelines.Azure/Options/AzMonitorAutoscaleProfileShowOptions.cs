using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "autoscale", "profile", "show")]
public record AzMonitorAutoscaleProfileShowOptions(
[property: CliOption("--autoscale-name")] string AutoscaleName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;