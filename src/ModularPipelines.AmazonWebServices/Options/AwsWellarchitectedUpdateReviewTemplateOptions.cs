using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "update-review-template")]
public record AwsWellarchitectedUpdateReviewTemplateOptions(
[property: CliOption("--template-arn")] string TemplateArn
) : AwsOptions
{
    [CliOption("--template-name")]
    public string? TemplateName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--notes")]
    public string? Notes { get; set; }

    [CliOption("--lenses-to-associate")]
    public string[]? LensesToAssociate { get; set; }

    [CliOption("--lenses-to-disassociate")]
    public string[]? LensesToDisassociate { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}