using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("capacity")]
public class AzCapacityReservation
{
    public AzCapacityReservation(
        AzCapacityReservationGroup group,
        ICommand internalCommand
    )
    {
        Group = group;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCapacityReservationGroup Group { get; }

    public async Task<CommandResult> Create(AzCapacityReservationCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzCapacityReservationDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCapacityReservationDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzCapacityReservationListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzCapacityReservationShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzCapacityReservationUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}