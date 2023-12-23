using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("personalize-runtime", "get-personalized-ranking")]
public record AwsPersonalizeRuntimeGetPersonalizedRankingOptions(
[property: CommandSwitch("--campaign-arn")] string CampaignArn,
[property: CommandSwitch("--input-list")] string[] InputList,
[property: CommandSwitch("--user-id")] string UserId
) : AwsOptions
{
    [CommandSwitch("--context")]
    public IEnumerable<KeyValue>? Context { get; set; }

    [CommandSwitch("--filter-arn")]
    public string? FilterArn { get; set; }

    [CommandSwitch("--filter-values")]
    public IEnumerable<KeyValue>? FilterValues { get; set; }

    [CommandSwitch("--metadata-columns")]
    public IEnumerable<KeyValue>? MetadataColumns { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}