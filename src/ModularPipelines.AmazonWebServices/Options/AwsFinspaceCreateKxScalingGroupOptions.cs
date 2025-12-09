using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("finspace", "create-kx-scaling-group")]
public record AwsFinspaceCreateKxScalingGroupOptions(
[property: CliOption("--environment-id")] string EnvironmentId,
[property: CliOption("--scaling-group-name")] string ScalingGroupName,
[property: CliOption("--host-type")] string HostType,
[property: CliOption("--availability-zone-id")] string AvailabilityZoneId
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}