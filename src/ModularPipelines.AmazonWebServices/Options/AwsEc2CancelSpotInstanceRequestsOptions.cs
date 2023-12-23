using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "cancel-spot-instance-requests")]
public record AwsEc2CancelSpotInstanceRequestsOptions(
[property: CommandSwitch("--spot-instance-request-ids")] string[] SpotInstanceRequestIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}