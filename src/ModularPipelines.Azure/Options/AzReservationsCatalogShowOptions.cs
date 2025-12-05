using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("reservations", "catalog", "show")]
public record AzReservationsCatalogShowOptions(
[property: CliOption("--subscription-id")] string SubscriptionId
) : AzOptions
{
    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--offer-id")]
    public string? OfferId { get; set; }

    [CliOption("--plan-id")]
    public string? PlanId { get; set; }

    [CliOption("--publisher-id")]
    public string? PublisherId { get; set; }

    [CliOption("--reserved-resource-type")]
    public string? ReservedResourceType { get; set; }
}