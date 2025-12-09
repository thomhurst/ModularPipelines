using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "associate-ipam-resource-discovery")]
public record AwsEc2AssociateIpamResourceDiscoveryOptions(
[property: CliOption("--ipam-id")] string IpamId,
[property: CliOption("--ipam-resource-discovery-id")] string IpamResourceDiscoveryId
) : AwsOptions
{
    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}