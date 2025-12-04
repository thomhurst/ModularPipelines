using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cdn", "endpoint", "waf", "policy", "set")]
public record AzCdnEndpointWafPolicySetOptions : AzOptions
{
    [CliOption("--endpoint-name")]
    public string? EndpointName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--profile-name")]
    public string? ProfileName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--waf-policy-id")]
    public string? WafPolicyId { get; set; }

    [CliOption("--waf-policy-name")]
    public string? WafPolicyName { get; set; }

    [CliOption("--waf-policy-resource-group-name")]
    public string? WafPolicyResourceGroupName { get; set; }

    [CliOption("--waf-policy-subscription-id")]
    public string? WafPolicySubscriptionId { get; set; }
}