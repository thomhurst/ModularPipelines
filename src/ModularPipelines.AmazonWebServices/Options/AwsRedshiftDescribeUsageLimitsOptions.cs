using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "describe-usage-limits")]
public record AwsRedshiftDescribeUsageLimitsOptions : AwsOptions
{
    [CliOption("--usage-limit-id")]
    public string? UsageLimitId { get; set; }

    [CliOption("--cluster-identifier")]
    public string? ClusterIdentifier { get; set; }

    [CliOption("--feature-type")]
    public string? FeatureType { get; set; }

    [CliOption("--tag-keys")]
    public string[]? TagKeys { get; set; }

    [CliOption("--tag-values")]
    public string[]? TagValues { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}