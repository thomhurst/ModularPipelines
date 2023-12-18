using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "ingress", "access-restriction", "set")]
public record AzContainerappIngressAccessRestrictionSetOptions(
[property: CommandSwitch("--action")] string Action,
[property: CommandSwitch("--ip-address")] string IpAddress,
[property: CommandSwitch("--rule-name")] string RuleName
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}