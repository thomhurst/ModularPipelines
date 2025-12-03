using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bms", "networks", "update")]
public record GcloudBmsNetworksUpdateOptions(
[property: CliArgument] string Network,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliOption("--add-ip-range-reservation")]
    public string[]? AddIpRangeReservation { get; set; }

    [CliFlag("start_address")]
    public bool? StartAddress { get; set; }

    [CliFlag("end_address")]
    public bool? EndAddress { get; set; }

    [CliFlag("note")]
    public bool? Note { get; set; }

    [CliFlag("--clear-ip-range-reservations")]
    public bool? ClearIpRangeReservations { get; set; }

    [CliOption("--remove-ip-range-reservation")]
    public string[]? RemoveIpRangeReservation { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }
}