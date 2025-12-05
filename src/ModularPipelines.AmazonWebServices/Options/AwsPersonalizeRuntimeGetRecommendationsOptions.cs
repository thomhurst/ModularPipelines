using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("personalize-runtime", "get-recommendations")]
public record AwsPersonalizeRuntimeGetRecommendationsOptions : AwsOptions
{
    [CliOption("--campaign-arn")]
    public string? CampaignArn { get; set; }

    [CliOption("--item-id")]
    public string? ItemId { get; set; }

    [CliOption("--user-id")]
    public string? UserId { get; set; }

    [CliOption("--num-results")]
    public int? NumResults { get; set; }

    [CliOption("--context")]
    public IEnumerable<KeyValue>? Context { get; set; }

    [CliOption("--filter-arn")]
    public string? FilterArn { get; set; }

    [CliOption("--filter-values")]
    public IEnumerable<KeyValue>? FilterValues { get; set; }

    [CliOption("--recommender-arn")]
    public string? RecommenderArn { get; set; }

    [CliOption("--promotions")]
    public string[]? Promotions { get; set; }

    [CliOption("--metadata-columns")]
    public IEnumerable<KeyValue>? MetadataColumns { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}