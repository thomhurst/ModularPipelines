using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("neptune-graph", "wait", "graph-snapshot-available")]
public record AwsNeptuneGraphWaitGraphSnapshotAvailableOptions(
[property: CommandSwitch("--snapshot-identifier")] string SnapshotIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}