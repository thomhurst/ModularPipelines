using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-artifact")]
public record AwsSagemakerCreateArtifactOptions(
[property: CliOption("--source")] string Source,
[property: CliOption("--artifact-type")] string ArtifactType
) : AwsOptions
{
    [CliOption("--artifact-name")]
    public string? ArtifactName { get; set; }

    [CliOption("--properties")]
    public IEnumerable<KeyValue>? Properties { get; set; }

    [CliOption("--metadata-properties")]
    public string? MetadataProperties { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}