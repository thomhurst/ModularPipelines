using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "autoscale", "rule", "delete")]
public record AzMonitorAutoscaleRuleDeleteOptions(
[property: CliOption("--autoscale-name")] string AutoscaleName,
[property: CliOption("--index")] string Index,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--profile-name")]
    public string? ProfileName { get; set; }
}