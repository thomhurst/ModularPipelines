using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "get-asset-property-value-history")]
public record AwsIotsitewiseGetAssetPropertyValueHistoryOptions : AwsOptions
{
    [CliOption("--asset-id")]
    public string? AssetId { get; set; }

    [CliOption("--property-id")]
    public string? PropertyId { get; set; }

    [CliOption("--property-alias")]
    public string? PropertyAlias { get; set; }

    [CliOption("--start-date")]
    public long? StartDate { get; set; }

    [CliOption("--end-date")]
    public long? EndDate { get; set; }

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