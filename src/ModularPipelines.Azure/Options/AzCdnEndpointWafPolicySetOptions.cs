using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cdn", "endpoint", "waf", "policy", "set")]
public record AzCdnEndpointWafPolicySetOptions : AzOptions
{
    [CommandSwitch("--endpoint-name")]
    public string? EndpointName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--profile-name")]
    public string? ProfileName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--waf-policy-id")]
    public string? WafPolicyId { get; set; }

    [CommandSwitch("--waf-policy-name")]
    public string? WafPolicyName { get; set; }

    [CommandSwitch("--waf-policy-resource-group-name")]
    public string? WafPolicyResourceGroupName { get; set; }

    [CommandSwitch("--waf-policy-subscription-id")]
    public string? WafPolicySubscriptionId { get; set; }
}