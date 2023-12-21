using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzCapacity
{
    public AzCapacity(
        AzCapacityReservation reservation
    )
    {
        Reservation = reservation;
    }

    public AzCapacityReservation Reservation { get; }
}