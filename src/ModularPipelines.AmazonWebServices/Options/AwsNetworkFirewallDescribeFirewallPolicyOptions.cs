using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-firewall", "describe-firewall-policy")]
public record AwsNetworkFirewallDescribeFirewallPolicyOptions : AwsOptions
{
    [CommandSwitch("--firewall-policy-name")]
    public string? FirewallPolicyName { get; set; }

    [CommandSwitch("--firewall-policy-arn")]
    public string? FirewallPolicyArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}