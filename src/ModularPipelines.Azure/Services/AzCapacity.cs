using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

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