using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-firewall", "create-firewall")]
public record AwsNetworkFirewallCreateFirewallOptions(
[property: CommandSwitch("--firewall-name")] string FirewallName,
[property: CommandSwitch("--firewall-policy-arn")] string FirewallPolicyArn,
[property: CommandSwitch("--vpc-id")] string VpcId,
[property: CommandSwitch("--subnet-mappings")] string[] SubnetMappings
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--encryption-configuration")]
    public string? EncryptionConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}