using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-firewall", "associate-subnets")]
public record AwsNetworkFirewallAssociateSubnetsOptions(
[property: CommandSwitch("--subnet-mappings")] string[] SubnetMappings
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