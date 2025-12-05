using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3outposts", "create-endpoint")]
public record AwsS3outpostsCreateEndpointOptions(
[property: CliOption("--outpost-id")] string OutpostId,
[property: CliOption("--subnet-id")] string SubnetId,
[property: CliOption("--security-group-id")] string SecurityGroupId
) : AwsOptions
{
    [CliOption("--access-type")]
    public string? AccessType { get; set; }

    [CliOption("--customer-owned-ipv4-pool")]
    public string? CustomerOwnedIpv4Pool { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}