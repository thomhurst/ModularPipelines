using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dax", "increase-replication-factor")]
public record AwsDaxIncreaseReplicationFactorOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--new-replication-factor")] int NewReplicationFactor
) : AwsOptions
{
    [CliOption("--availability-zones")]
    public string[]? AvailabilityZones { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}