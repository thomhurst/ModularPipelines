using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "delete-aggregation-authorization")]
public record AwsConfigserviceDeleteAggregationAuthorizationOptions(
[property: CliOption("--authorized-account-id")] string AuthorizedAccountId,
[property: CliOption("--authorized-aws-region")] string AuthorizedAwsRegion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}