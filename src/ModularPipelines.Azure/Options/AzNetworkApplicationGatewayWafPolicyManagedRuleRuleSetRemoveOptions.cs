using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "waf-policy", "managed-rule", "rule-set", "remove")]
public record AzNetworkApplicationGatewayWafPolicyManagedRuleRuleSetRemoveOptions(
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--type")] string Type,
[property: CommandSwitch("--version")] string Version
) : AzOptions
{
    [CommandSwitch("--group-name")]
    public string? GroupName { get; set; }
}

