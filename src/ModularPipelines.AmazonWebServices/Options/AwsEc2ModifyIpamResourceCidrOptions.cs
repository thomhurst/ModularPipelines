using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-ipam-resource-cidr")]
public record AwsEc2ModifyIpamResourceCidrOptions(
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--resource-cidr")] string ResourceCidr,
[property: CliOption("--resource-region")] string ResourceRegion,
[property: CliOption("--current-ipam-scope-id")] string CurrentIpamScopeId
) : AwsOptions
{
    [CliOption("--destination-ipam-scope-id")]
    public string? DestinationIpamScopeId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}