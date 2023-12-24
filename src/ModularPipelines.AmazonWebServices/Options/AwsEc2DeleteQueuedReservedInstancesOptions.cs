using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "delete-queued-reserved-instances")]
public record AwsEc2DeleteQueuedReservedInstancesOptions(
[property: CommandSwitch("--reserved-instances-ids")] string[] ReservedInstancesIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}