using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptune-graph", "wait", "graph-deleted")]
public record AwsNeptuneGraphWaitGraphDeletedOptions(
[property: CliOption("--graph-identifier")] string GraphIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}