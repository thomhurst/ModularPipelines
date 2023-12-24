using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elbv2", "set-security-groups")]
public record AwsElbv2SetSecurityGroupsOptions(
[property: CommandSwitch("--load-balancer-arn")] string LoadBalancerArn,
[property: CommandSwitch("--security-groups")] string[] SecurityGroups
) : AwsOptions
{
    [CommandSwitch("--enforce-security-group-inbound-rules-on-private-link-traffic")]
    public string? EnforceSecurityGroupInboundRulesOnPrivateLinkTraffic { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}