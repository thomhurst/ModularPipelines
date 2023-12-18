using System.Diagnostics.CodeAnalysis;

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