using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "create-review-template")]
public record AwsWellarchitectedCreateReviewTemplateOptions(
[property: CliOption("--template-name")] string TemplateName,
[property: CliOption("--description")] string Description,
[property: CliOption("--lenses")] string[] Lenses
) : AwsOptions
{
    [CliOption("--notes")]
    public string? Notes { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}