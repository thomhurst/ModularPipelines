using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("greengrassv2", "get-component-version-artifact")]
public record AwsGreengrassv2GetComponentVersionArtifactOptions(
[property: CliOption("--arn")] string Arn,
[property: CliOption("--artifact-name")] string ArtifactName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}