using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "list-aggregate-discovered-resources")]
public record AwsConfigserviceListAggregateDiscoveredResourcesOptions(
[property: CliOption("--configuration-aggregator-name")] string ConfigurationAggregatorName,
[property: CliOption("--resource-type")] string ResourceType
) : AwsOptions
{
    [CliOption("--filters")]
    public string? Filters { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}