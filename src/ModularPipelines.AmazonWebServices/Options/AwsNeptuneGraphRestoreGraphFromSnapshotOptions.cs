using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptune-graph", "restore-graph-from-snapshot")]
public record AwsNeptuneGraphRestoreGraphFromSnapshotOptions(
[property: CliOption("--snapshot-identifier")] string SnapshotIdentifier,
[property: CliOption("--graph-name")] string GraphName
) : AwsOptions
{
    [CliOption("--provisioned-memory")]
    public int? ProvisionedMemory { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--replica-count")]
    public int? ReplicaCount { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}