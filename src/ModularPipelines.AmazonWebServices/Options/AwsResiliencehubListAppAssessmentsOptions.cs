using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resiliencehub", "list-app-assessments")]
public record AwsResiliencehubListAppAssessmentsOptions : AwsOptions
{
    [CliOption("--app-arn")]
    public string? AppArn { get; set; }

    [CliOption("--assessment-name")]
    public string? AssessmentName { get; set; }

    [CliOption("--assessment-status")]
    public string[]? AssessmentStatus { get; set; }

    [CliOption("--compliance-status")]
    public string? ComplianceStatus { get; set; }

    [CliOption("--invoker")]
    public string? Invoker { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}