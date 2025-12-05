using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift-serverless", "get-usage-limit")]
public record AwsRedshiftServerlessGetUsageLimitOptions(
[property: CliOption("--usage-limit-id")] string UsageLimitId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}