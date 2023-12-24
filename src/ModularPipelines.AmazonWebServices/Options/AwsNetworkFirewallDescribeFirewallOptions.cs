using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-firewall", "describe-firewall")]
public record AwsNetworkFirewallDescribeFirewallOptions : AwsOptions
{
    [CommandSwitch("--firewall-name")]
    public string? FirewallName { get; set; }

    [CommandSwitch("--firewall-arn")]
    public string? FirewallArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}