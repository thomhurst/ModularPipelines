using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "autoscale", "rule", "create")]
public record AzMonitorAutoscaleRuleCreateOptions(
[property: CliOption("--autoscale-name")] string AutoscaleName,
[property: CliOption("--condition")] string Condition,
[property: CliOption("--scale")] string Scale
) : AzOptions
{
    [CliOption("--cooldown")]
    public string? Cooldown { get; set; }

    [CliOption("--profile-name")]
    public string? ProfileName { get; set; }

    [CliOption("--resource")]
    public string? Resource { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-namespace")]
    public string? ResourceNamespace { get; set; }

    [CliOption("--resource-parent")]
    public string? ResourceParent { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }

    [CliOption("--timegrain")]
    public string? Timegrain { get; set; }
}