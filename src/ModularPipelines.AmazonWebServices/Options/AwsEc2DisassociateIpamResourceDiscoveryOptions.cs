using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "disassociate-ipam-resource-discovery")]
public record AwsEc2DisassociateIpamResourceDiscoveryOptions(
[property: CommandSwitch("--ipam-resource-discovery-association-id")] string IpamResourceDiscoveryAssociationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}