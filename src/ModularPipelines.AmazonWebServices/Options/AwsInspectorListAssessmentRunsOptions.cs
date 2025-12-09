using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("inspector", "list-assessment-runs")]
public record AwsInspectorListAssessmentRunsOptions : AwsOptions
{
    [CliOption("--assessment-template-arns")]
    public string[]? AssessmentTemplateArns { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}