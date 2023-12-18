using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("communication")]
public class AzCommunicationRooms
{
    public AzCommunicationRooms(
        AzCommunicationRoomsParticipant participant,
        ICommand internalCommand
    )
    {
        Participant = participant;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCommunicationRoomsParticipant Participant { get; }

    public async Task<CommandResult> Create(AzCommunicationRoomsCreateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCommunicationRoomsCreateOptions(), token);
    }

    public async Task<CommandResult> Delete(AzCommunicationRoomsDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Get(AzCommunicationRoomsGetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzCommunicationRoomsListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCommunicationRoomsListOptions(), token);
    }

    public async Task<CommandResult> Update(AzCommunicationRoomsUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}