using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "disassociate-ipam-resource-discovery")]
public record AwsEc2DisassociateIpamResourceDiscoveryOptions(
[property: CliOption("--ipam-resource-discovery-association-id")] string IpamResourceDiscoveryAssociationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}