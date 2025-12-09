using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-firewall", "update-firewall-policy-change-protection")]
public record AwsNetworkFirewallUpdateFirewallPolicyChangeProtectionOptions : AwsOptions
{
    [CliOption("--update-token")]
    public string? UpdateToken { get; set; }

    [CliOption("--firewall-arn")]
    public string? FirewallArn { get; set; }

    [CliOption("--firewall-name")]
    public string? FirewallName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}