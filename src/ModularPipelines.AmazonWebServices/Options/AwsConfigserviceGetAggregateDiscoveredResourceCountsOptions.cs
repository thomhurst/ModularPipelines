using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "get-aggregate-discovered-resource-counts")]
public record AwsConfigserviceGetAggregateDiscoveredResourceCountsOptions(
[property: CliOption("--configuration-aggregator-name")] string ConfigurationAggregatorName
) : AwsOptions
{
    [CliOption("--filters")]
    public string? Filters { get; set; }

    [CliOption("--group-by-key")]
    public string? GroupByKey { get; set; }

    [CliOption("--limit")]
    public int? Limit { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}