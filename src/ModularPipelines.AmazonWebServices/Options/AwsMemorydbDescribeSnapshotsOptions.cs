using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("memorydb", "describe-snapshots")]
public record AwsMemorydbDescribeSnapshotsOptions : AwsOptions
{
    [CliOption("--cluster-name")]
    public string? ClusterName { get; set; }

    [CliOption("--snapshot-name")]
    public string? SnapshotName { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}