using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "autoscale", "rule", "list")]
public record AzMonitorAutoscaleRuleListOptions(
[property: CliOption("--autoscale-name")] string AutoscaleName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--profile-name")]
    public string? ProfileName { get; set; }
}