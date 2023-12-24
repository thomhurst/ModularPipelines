using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("comprehend", "delete-entity-recognizer")]
public record AwsComprehendDeleteEntityRecognizerOptions(
[property: CommandSwitch("--entity-recognizer-arn")] string EntityRecognizerArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}