using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "describe-db-cluster-snapshots")]
public record AwsRdsDescribeDbClusterSnapshotsOptions : AwsOptions
{
    [CommandSwitch("--db-cluster-identifier")]
    public string? DbClusterIdentifier { get; set; }

    [CommandSwitch("--db-cluster-snapshot-identifier")]
    public string? DbClusterSnapshotIdentifier { get; set; }

    [CommandSwitch("--snapshot-type")]
    public string? SnapshotType { get; set; }

    [CommandSwitch("--filters")]
    public string[]? Filters { get; set; }

    [CommandSwitch("--db-cluster-resource-id")]
    public string? DbClusterResourceId { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}