using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resiliencehub", "create-recommendation-template")]
public record AwsResiliencehubCreateRecommendationTemplateOptions(
[property: CommandSwitch("--assessment-arn")] string AssessmentArn,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--bucket-name")]
    public string? BucketName { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [CommandSwitch("--recommendation-ids")]
    public string[]? RecommendationIds { get; set; }

    [CommandSwitch("--recommendation-types")]
    public string[]? RecommendationTypes { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}