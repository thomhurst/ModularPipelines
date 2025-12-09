using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "update-artifact")]
public record AwsSagemakerUpdateArtifactOptions(
[property: CliOption("--artifact-arn")] string ArtifactArn
) : AwsOptions
{
    [CliOption("--artifact-name")]
    public string? ArtifactName { get; set; }

    [CliOption("--properties")]
    public IEnumerable<KeyValue>? Properties { get; set; }

    [CliOption("--properties-to-remove")]
    public string[]? PropertiesToRemove { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}