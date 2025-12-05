using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "delete-pending-aggregation-request")]
public record AwsConfigserviceDeletePendingAggregationRequestOptions(
[property: CliOption("--requester-account-id")] string RequesterAccountId,
[property: CliOption("--requester-aws-region")] string RequesterAwsRegion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}