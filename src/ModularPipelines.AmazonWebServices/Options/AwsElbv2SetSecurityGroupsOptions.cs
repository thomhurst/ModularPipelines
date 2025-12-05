using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elbv2", "set-security-groups")]
public record AwsElbv2SetSecurityGroupsOptions(
[property: CliOption("--load-balancer-arn")] string LoadBalancerArn,
[property: CliOption("--security-groups")] string[] SecurityGroups
) : AwsOptions
{
    [CliOption("--enforce-security-group-inbound-rules-on-private-link-traffic")]
    public string? EnforceSecurityGroupInboundRulesOnPrivateLinkTraffic { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}