using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "upgrade-review-template-lens-review")]
public record AwsWellarchitectedUpgradeReviewTemplateLensReviewOptions(
[property: CliOption("--template-arn")] string TemplateArn,
[property: CliOption("--lens-alias")] string LensAlias
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}