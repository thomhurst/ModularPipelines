using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "delete-route")]
public record AwsEc2DeleteRouteOptions(
[property: CliOption("--route-table-id")] string RouteTableId
) : AwsOptions
{
    [CliOption("--destination-cidr-block")]
    public string? DestinationCidrBlock { get; set; }

    [CliOption("--destination-ipv6-cidr-block")]
    public string? DestinationIpv6CidrBlock { get; set; }

    [CliOption("--destination-prefix-list-id")]
    public string? DestinationPrefixListId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}