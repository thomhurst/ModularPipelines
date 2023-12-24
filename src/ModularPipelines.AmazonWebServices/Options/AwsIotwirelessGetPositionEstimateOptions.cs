using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotwireless", "get-position-estimate")]
public record AwsIotwirelessGetPositionEstimateOptions : AwsOptions
{
    [CommandSwitch("--wi-fi-access-points")]
    public string[]? WiFiAccessPoints { get; set; }

    [CommandSwitch("--cell-towers")]
    public string? CellTowers { get; set; }

    [CommandSwitch("--ip")]
    public string? Ip { get; set; }

    [CommandSwitch("--gnss")]
    public string? Gnss { get; set; }

    [CommandSwitch("--timestamp")]
    public long? Timestamp { get; set; }
}