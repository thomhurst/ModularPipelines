using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dax", "decrease-replication-factor")]
public record AwsDaxDecreaseReplicationFactorOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--new-replication-factor")] int NewReplicationFactor
) : AwsOptions
{
    [CliOption("--availability-zones")]
    public string[]? AvailabilityZones { get; set; }

    [CliOption("--node-ids-to-remove")]
    public string[]? NodeIdsToRemove { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}