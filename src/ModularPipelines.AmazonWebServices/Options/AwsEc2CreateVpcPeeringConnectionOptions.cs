using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-vpc-peering-connection")]
public record AwsEc2CreateVpcPeeringConnectionOptions(
[property: CommandSwitch("--vpc-id")] string VpcId
) : AwsOptions
{
    [CommandSwitch("--peer-owner-id")]
    public string? PeerOwnerId { get; set; }

    [CommandSwitch("--peer-vpc-id")]
    public string? PeerVpcId { get; set; }

    [CommandSwitch("--peer-region")]
    public string? PeerRegion { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}