using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptune-graph", "update-graph")]
public record AwsNeptuneGraphUpdateGraphOptions(
[property: CliOption("--graph-identifier")] string GraphIdentifier
) : AwsOptions
{
    [CliOption("--provisioned-memory")]
    public int? ProvisionedMemory { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}