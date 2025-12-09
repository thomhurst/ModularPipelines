using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("comprehend", "delete-entity-recognizer")]
public record AwsComprehendDeleteEntityRecognizerOptions(
[property: CliOption("--entity-recognizer-arn")] string EntityRecognizerArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}