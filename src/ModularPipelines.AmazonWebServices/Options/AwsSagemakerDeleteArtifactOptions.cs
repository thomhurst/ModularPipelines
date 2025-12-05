using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "delete-artifact")]
public record AwsSagemakerDeleteArtifactOptions : AwsOptions
{
    [CliOption("--artifact-arn")]
    public string? ArtifactArn { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}