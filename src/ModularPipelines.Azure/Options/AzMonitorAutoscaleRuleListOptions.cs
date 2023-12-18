using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

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