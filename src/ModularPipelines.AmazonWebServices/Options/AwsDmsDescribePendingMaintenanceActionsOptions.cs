using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "describe-pending-maintenance-actions")]
public record AwsDmsDescribePendingMaintenanceActionsOptions : AwsOptions
{
    [CliOption("--replication-instance-arn")]
    public string? ReplicationInstanceArn { get; set; }

    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--marker")]
    public string? Marker { get; set; }

    [CliOption("--max-records")]
    public int? MaxRecords { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}