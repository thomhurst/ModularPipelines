using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("robomaker", "create-simulation-application-version")]
public record AwsRobomakerCreateSimulationApplicationVersionOptions(
[property: CliOption("--application")] string Application
) : AwsOptions
{
    [CliOption("--current-revision-id")]
    public string? CurrentRevisionId { get; set; }

    [CliOption("--s3-etags")]
    public string[]? S3Etags { get; set; }

    [CliOption("--image-digest")]
    public string? ImageDigest { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}