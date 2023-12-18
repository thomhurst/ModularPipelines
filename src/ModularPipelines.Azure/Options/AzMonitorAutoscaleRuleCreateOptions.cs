using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "autoscale", "rule", "create")]
public record AzMonitorAutoscaleRuleCreateOptions(
[property: CommandSwitch("--autoscale-name")] string AutoscaleName,
[property: CommandSwitch("--condition")] string Condition,
[property: CommandSwitch("--scale")] string Scale
) : AzOptions
{
    [CommandSwitch("--cooldown")]
    public string? Cooldown { get; set; }

    [CommandSwitch("--profile-name")]
    public string? ProfileName { get; set; }

    [CommandSwitch("--resource")]
    public string? Resource { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--resource-namespace")]
    public string? ResourceNamespace { get; set; }

    [CommandSwitch("--resource-parent")]
    public string? ResourceParent { get; set; }

    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }

    [CommandSwitch("--timegrain")]
    public string? Timegrain { get; set; }
}

