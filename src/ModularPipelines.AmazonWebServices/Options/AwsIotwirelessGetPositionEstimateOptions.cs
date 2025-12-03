using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "get-position-estimate")]
public record AwsIotwirelessGetPositionEstimateOptions : AwsOptions
{
    [CliOption("--wi-fi-access-points")]
    public string[]? WiFiAccessPoints { get; set; }

    [CliOption("--cell-towers")]
    public string? CellTowers { get; set; }

    [CliOption("--ip")]
    public string? Ip { get; set; }

    [CliOption("--gnss")]
    public string? Gnss { get; set; }

    [CliOption("--timestamp")]
    public long? Timestamp { get; set; }
}