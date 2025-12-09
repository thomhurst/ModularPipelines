using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "get-asset-property-aggregates")]
public record AwsIotsitewiseGetAssetPropertyAggregatesOptions(
[property: CliOption("--aggregate-types")] string[] AggregateTypes,
[property: CliOption("--resolution")] string Resolution,
[property: CliOption("--start-date")] long StartDate,
[property: CliOption("--end-date")] long EndDate
) : AwsOptions
{
    [CliOption("--asset-id")]
    public string? AssetId { get; set; }

    [CliOption("--property-id")]
    public string? PropertyId { get; set; }

    [CliOption("--property-alias")]
    public string? PropertyAlias { get; set; }

    [CliOption("--qualities")]
    public string[]? Qualities { get; set; }

    [CliOption("--time-ordering")]
    public string? TimeOrdering { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}