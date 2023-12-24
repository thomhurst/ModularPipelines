using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("neptune-graph", "restore-graph-from-snapshot")]
public record AwsNeptuneGraphRestoreGraphFromSnapshotOptions(
[property: CommandSwitch("--snapshot-identifier")] string SnapshotIdentifier,
[property: CommandSwitch("--graph-name")] string GraphName
) : AwsOptions
{
    [CommandSwitch("--provisioned-memory")]
    public int? ProvisionedMemory { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--replica-count")]
    public int? ReplicaCount { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}