using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("comprehend", "stop-training-entity-recognizer")]
public record AwsComprehendStopTrainingEntityRecognizerOptions(
[property: CommandSwitch("--entity-recognizer-arn")] string EntityRecognizerArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}