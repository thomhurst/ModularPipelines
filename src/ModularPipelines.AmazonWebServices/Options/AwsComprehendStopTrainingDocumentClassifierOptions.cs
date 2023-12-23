using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("comprehend", "stop-training-document-classifier")]
public record AwsComprehendStopTrainingDocumentClassifierOptions(
[property: CommandSwitch("--document-classifier-arn")] string DocumentClassifierArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}