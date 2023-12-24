using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsitewise", "get-asset-property-aggregates")]
public record AwsIotsitewiseGetAssetPropertyAggregatesOptions(
[property: CommandSwitch("--aggregate-types")] string[] AggregateTypes,
[property: CommandSwitch("--resolution")] string Resolution,
[property: CommandSwitch("--start-date")] long StartDate,
[property: CommandSwitch("--end-date")] long EndDate
) : AwsOptions
{
    [CommandSwitch("--asset-id")]
    public string? AssetId { get; set; }

    [CommandSwitch("--property-id")]
    public string? PropertyId { get; set; }

    [CommandSwitch("--property-alias")]
    public string? PropertyAlias { get; set; }

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