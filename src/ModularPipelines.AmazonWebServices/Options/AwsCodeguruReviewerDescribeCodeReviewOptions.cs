using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codeguru-reviewer", "describe-code-review")]
public record AwsCodeguruReviewerDescribeCodeReviewOptions(
[property: CliOption("--code-review-arn")] string CodeReviewArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}