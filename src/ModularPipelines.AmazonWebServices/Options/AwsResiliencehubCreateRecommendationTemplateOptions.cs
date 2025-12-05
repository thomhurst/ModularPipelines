using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resiliencehub", "create-recommendation-template")]
public record AwsResiliencehubCreateRecommendationTemplateOptions(
[property: CliOption("--assessment-arn")] string AssessmentArn,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--bucket-name")]
    public string? BucketName { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--format")]
    public string? Format { get; set; }

    [CliOption("--recommendation-ids")]
    public string[]? RecommendationIds { get; set; }

    [CliOption("--recommendation-types")]
    public string[]? RecommendationTypes { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}