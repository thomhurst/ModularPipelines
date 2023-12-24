using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("personalize-runtime", "get-recommendations")]
public record AwsPersonalizeRuntimeGetRecommendationsOptions : AwsOptions
{
    [CommandSwitch("--campaign-arn")]
    public string? CampaignArn { get; set; }

    [CommandSwitch("--item-id")]
    public string? ItemId { get; set; }

    [CommandSwitch("--user-id")]
    public string? UserId { get; set; }

    [CommandSwitch("--num-results")]
    public int? NumResults { get; set; }

    [CommandSwitch("--context")]
    public IEnumerable<KeyValue>? Context { get; set; }

    [CommandSwitch("--filter-arn")]
    public string? FilterArn { get; set; }

    [CommandSwitch("--filter-values")]
    public IEnumerable<KeyValue>? FilterValues { get; set; }

    [CommandSwitch("--recommender-arn")]
    public string? RecommenderArn { get; set; }

    [CommandSwitch("--promotions")]
    public string[]? Promotions { get; set; }

    [CommandSwitch("--metadata-columns")]
    public IEnumerable<KeyValue>? MetadataColumns { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}