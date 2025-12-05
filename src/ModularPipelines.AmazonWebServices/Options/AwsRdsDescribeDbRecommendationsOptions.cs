using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "describe-db-recommendations")]
public record AwsRdsDescribeDbRecommendationsOptions : AwsOptions
{
    [CliOption("--last-updated-after")]
    public long? LastUpdatedAfter { get; set; }

    [CliOption("--last-updated-before")]
    public long? LastUpdatedBefore { get; set; }

    [CliOption("--locale")]
    public string? Locale { get; set; }

    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}