using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzReservations
{
    public AzReservations(
        AzReservationsCatalog catalog,
        AzReservationsReservation reservation,
        AzReservationsReservationOrder reservationOrder,
        AzReservationsReservationOrderId reservationOrderId,
        ICommand internalCommand
    )
    {
        Catalog = catalog;
        Reservation = reservation;
        ReservationOrder = reservationOrder;
        ReservationOrderId = reservationOrderId;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzReservationsCatalog Catalog { get; }

    public AzReservationsReservation Reservation { get; }

    public AzReservationsReservationOrder ReservationOrder { get; }

    public AzReservationsReservationOrderId ReservationOrderId { get; }

    public async Task<CommandResult> CalculateExchange(AzReservationsCalculateExchangeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzReservationsCalculateExchangeOptions(), token);
    }

    public async Task<CommandResult> Exchange(AzReservationsExchangeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzReservationsExchangeOptions(), token);
    }

    public async Task<CommandResult> List(AzReservationsListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzReservationsListOptions(), token);
    }
}