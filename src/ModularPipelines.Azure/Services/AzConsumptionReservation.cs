using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("consumption")]
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