using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsitewise", "list-time-series")]
public record AwsIotsitewiseListTimeSeriesOptions : AwsOptions
{
    [CommandSwitch("--asset-id")]
    public string? AssetId { get; set; }

    [CommandSwitch("--alias-prefix")]
    public string? AliasPrefix { get; set; }

    [CommandSwitch("--time-series-type")]
    public string? TimeSeriesType { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}