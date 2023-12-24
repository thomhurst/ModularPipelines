using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("neptune-graph", "create-graph-snapshot")]
public record AwsNeptuneGraphCreateGraphSnapshotOptions(
[property: CommandSwitch("--graph-identifier")] string GraphIdentifier,
[property: CommandSwitch("--snapshot-name")] string SnapshotName
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}