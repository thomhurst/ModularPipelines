using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-vpc-peering-connection-options")]
public record AwsEc2ModifyVpcPeeringConnectionOptionsOptions(
[property: CliOption("--vpc-peering-connection-id")] string VpcPeeringConnectionId
) : AwsOptions
{
    [CliOption("--accepter-peering-connection-options")]
    public string? AccepterPeeringConnectionOptions { get; set; }

    [CliOption("--requester-peering-connection-options")]
    public string? RequesterPeeringConnectionOptions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}