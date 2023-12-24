using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("inspector", "get-telemetry-metadata")]
public record AwsInspectorGetTelemetryMetadataOptions(
[property: CommandSwitch("--assessment-run-arn")] string AssessmentRunArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}