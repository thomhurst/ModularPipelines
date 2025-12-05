using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "put-aggregation-authorization")]
public record AwsConfigservicePutAggregationAuthorizationOptions(
[property: CliOption("--authorized-account-id")] string AuthorizedAccountId,
[property: CliOption("--authorized-aws-region")] string AuthorizedAwsRegion
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}