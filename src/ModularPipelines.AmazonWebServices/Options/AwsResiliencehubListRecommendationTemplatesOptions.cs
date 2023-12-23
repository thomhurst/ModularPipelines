using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resiliencehub", "list-recommendation-templates")]
public record AwsResiliencehubListRecommendationTemplatesOptions(
[property: CommandSwitch("--assessment-arn")] string AssessmentArn
) : AwsOptions
{
    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--recommendation-template-arn")]
    public string? RecommendationTemplateArn { get; set; }

    [CommandSwitch("--status")]
    public string[]? Status { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}