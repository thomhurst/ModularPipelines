using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-vpc-peering-connection")]
public record AwsEc2CreateVpcPeeringConnectionOptions(
[property: CliOption("--vpc-id")] string VpcId
) : AwsOptions
{
    [CliOption("--peer-owner-id")]
    public string? PeerOwnerId { get; set; }

    [CliOption("--peer-vpc-id")]
    public string? PeerVpcId { get; set; }

    [CliOption("--peer-region")]
    public string? PeerRegion { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}