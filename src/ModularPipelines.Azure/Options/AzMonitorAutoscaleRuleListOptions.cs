using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "autoscale", "rule", "list")]
public record AzMonitorAutoscaleRuleListOptions(
[property: CommandSwitch("--autoscale-name")] string AutoscaleName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--profile-name")]
    public string? ProfileName { get; set; }
}

