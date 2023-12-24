using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "describe-vpc-peering-connections")]
public record AwsGameliftDescribeVpcPeeringConnectionsOptions : AwsOptions
{
    [CommandSwitch("--fleet-id")]
    public string? FleetId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}