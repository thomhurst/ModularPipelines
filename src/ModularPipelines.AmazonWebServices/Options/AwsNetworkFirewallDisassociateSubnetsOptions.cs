using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-firewall", "disassociate-subnets")]
public record AwsNetworkFirewallDisassociateSubnetsOptions(
[property: CommandSwitch("--subnet-ids")] string[] SubnetIds
) : AwsOptions
{
    [CommandSwitch("--update-token")]
    public string? UpdateToken { get; set; }

    [CommandSwitch("--firewall-arn")]
    public string? FirewallArn { get; set; }

    [CommandSwitch("--firewall-name")]
    public string? FirewallName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}