using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "application-gateway", "waf-policy", "managed-rule", "rule-set", "remove")]
public record AzNetworkApplicationGatewayWafPolicyManagedRuleRuleSetRemoveOptions(
[property: CliOption("--policy-name")] string PolicyName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--type")] string Type,
[property: CliOption("--version")] string Version
) : AzOptions
{
    [CliOption("--group-name")]
    public string? GroupName { get; set; }
}