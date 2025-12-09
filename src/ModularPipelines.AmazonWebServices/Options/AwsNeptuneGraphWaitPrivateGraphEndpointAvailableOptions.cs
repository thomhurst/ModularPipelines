using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptune-graph", "wait", "private-graph-endpoint-available")]
public record AwsNeptuneGraphWaitPrivateGraphEndpointAvailableOptions(
[property: CliOption("--graph-identifier")] string GraphIdentifier,
[property: CliOption("--vpc-id")] string VpcId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}