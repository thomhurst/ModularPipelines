using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("personalize-runtime", "get-action-recommendations")]
public record AwsPersonalizeRuntimeGetActionRecommendationsOptions : AwsOptions
{
    [CommandSwitch("--campaign-arn")]
    public string? CampaignArn { get; set; }

    [CommandSwitch("--user-id")]
    public string? UserId { get; set; }

    [CommandSwitch("--num-results")]
    public int? NumResults { get; set; }

    [CommandSwitch("--filter-arn")]
    public string? FilterArn { get; set; }

    [CommandSwitch("--filter-values")]
    public IEnumerable<KeyValue>? FilterValues { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}