using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "delete-queued-reserved-instances")]
public record AwsEc2DeleteQueuedReservedInstancesOptions(
[property: CliOption("--reserved-instances-ids")] string[] ReservedInstancesIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}