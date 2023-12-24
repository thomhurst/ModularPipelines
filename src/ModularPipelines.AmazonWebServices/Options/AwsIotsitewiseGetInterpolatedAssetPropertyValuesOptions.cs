using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsitewise", "get-interpolated-asset-property-values")]
public record AwsIotsitewiseGetInterpolatedAssetPropertyValuesOptions(
[property: CommandSwitch("--start-time-in-seconds")] long StartTimeInSeconds,
[property: CommandSwitch("--end-time-in-seconds")] long EndTimeInSeconds,
[property: CommandSwitch("--quality")] string Quality,
[property: CommandSwitch("--interval-in-seconds")] long IntervalInSeconds,
[property: CommandSwitch("--type")] string Type
) : AwsOptions
{
    [CommandSwitch("--asset-id")]
    public string? AssetId { get; set; }

    [CommandSwitch("--property-id")]
    public string? PropertyId { get; set; }

    [CommandSwitch("--property-alias")]
    public string? PropertyAlias { get; set; }

    [CommandSwitch("--start-time-offset-in-nanos")]
    public int? StartTimeOffsetInNanos { get; set; }

    [CommandSwitch("--end-time-offset-in-nanos")]
    public int? EndTimeOffsetInNanos { get; set; }

    [CommandSwitch("--interval-window-in-seconds")]
    public long? IntervalWindowInSeconds { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}