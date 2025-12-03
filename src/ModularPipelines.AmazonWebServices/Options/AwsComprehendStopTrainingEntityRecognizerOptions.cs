using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("comprehend", "stop-training-entity-recognizer")]
public record AwsComprehendStopTrainingEntityRecognizerOptions(
[property: CliOption("--entity-recognizer-arn")] string EntityRecognizerArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}