using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "delete-route")]
public record AwsEc2DeleteRouteOptions(
[property: CommandSwitch("--route-table-id")] string RouteTableId
) : AwsOptions
{
    [CommandSwitch("--destination-cidr-block")]
    public string? DestinationCidrBlock { get; set; }

    [CommandSwitch("--destination-ipv6-cidr-block")]
    public string? DestinationIpv6CidrBlock { get; set; }

    [CommandSwitch("--destination-prefix-list-id")]
    public string? DestinationPrefixListId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}