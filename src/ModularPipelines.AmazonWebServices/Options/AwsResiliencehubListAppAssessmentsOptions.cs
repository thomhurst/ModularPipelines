using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resiliencehub", "list-app-assessments")]
public record AwsResiliencehubListAppAssessmentsOptions : AwsOptions
{
    [CommandSwitch("--app-arn")]
    public string? AppArn { get; set; }

    [CommandSwitch("--assessment-name")]
    public string? AssessmentName { get; set; }

    [CommandSwitch("--assessment-status")]
    public string[]? AssessmentStatus { get; set; }

    [CommandSwitch("--compliance-status")]
    public string? ComplianceStatus { get; set; }

    [CommandSwitch("--invoker")]
    public string? Invoker { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}