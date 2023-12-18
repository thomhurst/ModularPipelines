using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("reservations", "catalog", "show")]
public record AzReservationsCatalogShowOptions(
[property: CommandSwitch("--subscription-id")] string SubscriptionId
) : AzOptions
{
    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--offer-id")]
    public string? OfferId { get; set; }

    [CommandSwitch("--plan-id")]
    public string? PlanId { get; set; }

    [CommandSwitch("--publisher-id")]
    public string? PublisherId { get; set; }

    [CommandSwitch("--reserved-resource-type")]
    public string? ReservedResourceType { get; set; }
}

