using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "delete-pending-aggregation-request")]
public record AwsConfigserviceDeletePendingAggregationRequestOptions(
[property: CommandSwitch("--requester-account-id")] string RequesterAccountId,
[property: CommandSwitch("--requester-aws-region")] string RequesterAwsRegion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}