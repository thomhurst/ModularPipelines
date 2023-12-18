using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("reservations", "calculate-exchange")]
public record AzReservationsCalculateExchangeOptions : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--ris-to-exchange")]
    public string? RisToExchange { get; set; }

    [CommandSwitch("--ris-to-purchase")]
    public string? RisToPurchase { get; set; }

    [CommandSwitch("--sp-to-purchase")]
    public string? SpToPurchase { get; set; }
}