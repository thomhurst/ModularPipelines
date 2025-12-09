using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rekognition", "wait", "project-version-training-completed")]
public record AwsRekognitionWaitProjectVersionTrainingCompletedOptions(
[property: CliOption("--project-arn")] string ProjectArn
) : AwsOptions
{
    [CliOption("--version-names")]
    public string[]? VersionNames { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}