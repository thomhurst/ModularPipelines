using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "cancel-spot-fleet-requests")]
public record AwsEc2CancelSpotFleetRequestsOptions(
[property: CommandSwitch("--spot-fleet-request-ids")] string[] SpotFleetRequestIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}