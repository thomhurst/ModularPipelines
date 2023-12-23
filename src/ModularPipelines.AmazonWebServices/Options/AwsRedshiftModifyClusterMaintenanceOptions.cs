using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "modify-cluster-maintenance")]
public record AwsRedshiftModifyClusterMaintenanceOptions(
[property: CommandSwitch("--cluster-identifier")] string ClusterIdentifier
) : AwsOptions
{
    [CommandSwitch("--defer-maintenance-identifier")]
    public string? DeferMaintenanceIdentifier { get; set; }

    [CommandSwitch("--defer-maintenance-start-time")]
    public long? DeferMaintenanceStartTime { get; set; }

    [CommandSwitch("--defer-maintenance-end-time")]
    public long? DeferMaintenanceEndTime { get; set; }

    [CommandSwitch("--defer-maintenance-duration")]
    public int? DeferMaintenanceDuration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}