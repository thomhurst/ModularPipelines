using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("consumption")]
public class AzConsumptionReservation
{
    public AzConsumptionReservation(
        AzConsumptionReservationDetail detail,
        AzConsumptionReservationSummary summary
    )
    {
        Detail = detail;
        Summary = summary;
    }

    public AzConsumptionReservationDetail Detail { get; }

    public AzConsumptionReservationSummary Summary { get; }
}