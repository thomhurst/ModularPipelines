using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("comprehend", "describe-document-classifier")]
public record AwsComprehendDescribeDocumentClassifierOptions(
[property: CommandSwitch("--document-classifier-arn")] string DocumentClassifierArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}