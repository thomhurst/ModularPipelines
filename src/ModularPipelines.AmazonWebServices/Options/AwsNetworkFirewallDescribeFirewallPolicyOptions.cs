using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-firewall", "describe-firewall-policy")]
public record AwsNetworkFirewallDescribeFirewallPolicyOptions : AwsOptions
{
    [CliOption("--firewall-policy-name")]
    public string? FirewallPolicyName { get; set; }

    [CliOption("--firewall-policy-arn")]
    public string? FirewallPolicyArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}