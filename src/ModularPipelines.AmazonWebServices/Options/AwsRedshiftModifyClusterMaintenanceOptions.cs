using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "modify-cluster-maintenance")]
public record AwsRedshiftModifyClusterMaintenanceOptions(
[property: CliOption("--cluster-identifier")] string ClusterIdentifier
) : AwsOptions
{
    [CliOption("--defer-maintenance-identifier")]
    public string? DeferMaintenanceIdentifier { get; set; }

    [CliOption("--defer-maintenance-start-time")]
    public long? DeferMaintenanceStartTime { get; set; }

    [CliOption("--defer-maintenance-end-time")]
    public long? DeferMaintenanceEndTime { get; set; }

    [CliOption("--defer-maintenance-duration")]
    public int? DeferMaintenanceDuration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}