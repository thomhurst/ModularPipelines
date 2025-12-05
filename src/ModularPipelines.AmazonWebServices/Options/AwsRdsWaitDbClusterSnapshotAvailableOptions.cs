using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "wait", "db-cluster-snapshot-available")]
public record AwsRdsWaitDbClusterSnapshotAvailableOptions : AwsOptions
{
    [CliOption("--db-cluster-identifier")]
    public string? DbClusterIdentifier { get; set; }

    [CliOption("--db-cluster-snapshot-identifier")]
    public string? DbClusterSnapshotIdentifier { get; set; }

    [CliOption("--snapshot-type")]
    public string? SnapshotType { get; set; }

    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--db-cluster-resource-id")]
    public string? DbClusterResourceId { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}