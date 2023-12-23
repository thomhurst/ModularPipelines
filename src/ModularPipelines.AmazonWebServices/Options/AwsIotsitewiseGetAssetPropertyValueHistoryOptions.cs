using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsitewise", "get-asset-property-value-history")]
public record AwsIotsitewiseGetAssetPropertyValueHistoryOptions : AwsOptions
{
    [CommandSwitch("--asset-id")]
    public string? AssetId { get; set; }

    [CommandSwitch("--property-id")]
    public string? PropertyId { get; set; }

    [CommandSwitch("--property-alias")]
    public string? PropertyAlias { get; set; }

    [CommandSwitch("--start-date")]
    public long? StartDate { get; set; }

    [CommandSwitch("--end-date")]
    public long? EndDate { get; set; }

    [CommandSwitch("--qualities")]
    public string[]? Qualities { get; set; }

    [CommandSwitch("--time-ordering")]
    public string? TimeOrdering { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}