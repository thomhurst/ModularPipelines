using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("inspector", "get-telemetry-metadata")]
public record AwsInspectorGetTelemetryMetadataOptions(
[property: CliOption("--assessment-run-arn")] string AssessmentRunArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}