using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("reservations", "calculate-exchange")]
public record AzReservationsCalculateExchangeOptions : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--ris-to-exchange")]
    public string? RisToExchange { get; set; }

    [CliOption("--ris-to-purchase")]
    public string? RisToPurchase { get; set; }

    [CliOption("--sp-to-purchase")]
    public string? SpToPurchase { get; set; }
}