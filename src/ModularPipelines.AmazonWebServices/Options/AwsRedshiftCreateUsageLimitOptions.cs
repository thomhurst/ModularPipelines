using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "create-usage-limit")]
public record AwsRedshiftCreateUsageLimitOptions(
[property: CliOption("--cluster-identifier")] string ClusterIdentifier,
[property: CliOption("--feature-type")] string FeatureType,
[property: CliOption("--limit-type")] string LimitType,
[property: CliOption("--amount")] long Amount
) : AwsOptions
{
    [CliOption("--period")]
    public string? Period { get; set; }

    [CliOption("--breach-action")]
    public string? BreachAction { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}