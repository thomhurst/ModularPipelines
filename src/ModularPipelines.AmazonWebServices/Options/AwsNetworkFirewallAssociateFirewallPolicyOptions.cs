using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-firewall", "associate-firewall-policy")]
public record AwsNetworkFirewallAssociateFirewallPolicyOptions(
[property: CliOption("--firewall-policy-arn")] string FirewallPolicyArn
) : AwsOptions
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