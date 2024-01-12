using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bms", "networks", "update")]
public record GcloudBmsNetworksUpdateOptions(
[property: PositionalArgument] string Network,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CommandSwitch("--add-ip-range-reservation")]
    public string[]? AddIpRangeReservation { get; set; }

    [BooleanCommandSwitch("start_address")]
    public bool? StartAddress { get; set; }

    [BooleanCommandSwitch("end_address")]
    public bool? EndAddress { get; set; }

    [BooleanCommandSwitch("note")]
    public bool? Note { get; set; }

    [BooleanCommandSwitch("--clear-ip-range-reservations")]
    public bool? ClearIpRangeReservations { get; set; }

    [CommandSwitch("--remove-ip-range-reservation")]
    public string[]? RemoveIpRangeReservation { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }
}