using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptune-graph", "delete-private-graph-endpoint")]
public record AwsNeptuneGraphDeletePrivateGraphEndpointOptions(
[property: CliOption("--graph-identifier")] string GraphIdentifier,
[property: CliOption("--vpc-id")] string VpcId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}