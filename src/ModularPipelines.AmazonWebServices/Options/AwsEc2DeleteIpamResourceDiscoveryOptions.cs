using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "delete-ipam-resource-discovery")]
public record AwsEc2DeleteIpamResourceDiscoveryOptions(
[property: CliOption("--ipam-resource-discovery-id")] string IpamResourceDiscoveryId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}