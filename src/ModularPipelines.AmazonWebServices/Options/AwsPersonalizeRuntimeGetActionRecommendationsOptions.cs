using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("personalize-runtime", "get-action-recommendations")]
public record AwsPersonalizeRuntimeGetActionRecommendationsOptions : AwsOptions
{
    [CliOption("--campaign-arn")]
    public string? CampaignArn { get; set; }

    [CliOption("--user-id")]
    public string? UserId { get; set; }

    [CliOption("--num-results")]
    public int? NumResults { get; set; }

    [CliOption("--filter-arn")]
    public string? FilterArn { get; set; }

    [CliOption("--filter-values")]
    public IEnumerable<KeyValue>? FilterValues { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}