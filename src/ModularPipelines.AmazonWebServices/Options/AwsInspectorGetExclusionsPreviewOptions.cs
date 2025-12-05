using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("inspector", "get-exclusions-preview")]
public record AwsInspectorGetExclusionsPreviewOptions(
[property: CliOption("--assessment-template-arn")] string AssessmentTemplateArn,
[property: CliOption("--preview-token")] string PreviewToken
) : AwsOptions
{
    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--locale")]
    public string? Locale { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}