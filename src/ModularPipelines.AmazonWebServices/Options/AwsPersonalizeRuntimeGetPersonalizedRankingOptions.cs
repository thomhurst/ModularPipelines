using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("personalize-runtime", "get-personalized-ranking")]
public record AwsPersonalizeRuntimeGetPersonalizedRankingOptions(
[property: CliOption("--campaign-arn")] string CampaignArn,
[property: CliOption("--input-list")] string[] InputList,
[property: CliOption("--user-id")] string UserId
) : AwsOptions
{
    [CliOption("--context")]
    public IEnumerable<KeyValue>? Context { get; set; }

    [CliOption("--filter-arn")]
    public string? FilterArn { get; set; }

    [CliOption("--filter-values")]
    public IEnumerable<KeyValue>? FilterValues { get; set; }

    [CliOption("--metadata-columns")]
    public IEnumerable<KeyValue>? MetadataColumns { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}