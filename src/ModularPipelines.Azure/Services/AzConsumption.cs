using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzConsumption
{
    public AzConsumption(
        AzConsumptionBudget budget,
        AzConsumptionMarketplace marketplace,
        AzConsumptionPricesheet pricesheet,
        AzConsumptionReservation reservation,
        AzConsumptionUsage usage
    )
    {
        Budget = budget;
        Marketplace = marketplace;
        Pricesheet = pricesheet;
        Reservation = reservation;
        Usage = usage;
    }

    public AzConsumptionBudget Budget { get; }

    public AzConsumptionMarketplace Marketplace { get; }

    public AzConsumptionPricesheet Pricesheet { get; }

    public AzConsumptionReservation Reservation { get; }

    public AzConsumptionUsage Usage { get; }
}