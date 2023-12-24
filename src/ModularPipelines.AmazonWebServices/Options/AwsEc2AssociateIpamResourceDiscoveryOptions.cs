using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "associate-ipam-resource-discovery")]
public record AwsEc2AssociateIpamResourceDiscoveryOptions(
[property: CommandSwitch("--ipam-id")] string IpamId,
[property: CommandSwitch("--ipam-resource-discovery-id")] string IpamResourceDiscoveryId
) : AwsOptions
{
    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}