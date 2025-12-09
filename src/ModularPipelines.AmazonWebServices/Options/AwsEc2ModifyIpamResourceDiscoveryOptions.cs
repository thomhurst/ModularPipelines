using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-ipam-resource-discovery")]
public record AwsEc2ModifyIpamResourceDiscoveryOptions(
[property: CliOption("--ipam-resource-discovery-id")] string IpamResourceDiscoveryId
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--add-operating-regions")]
    public string[]? AddOperatingRegions { get; set; }

    [CliOption("--remove-operating-regions")]
    public string[]? RemoveOperatingRegions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}