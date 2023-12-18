using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage-mover")]
public class AzStorageMoverAgent
{
    public AzStorageMoverAgent(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> List(AzStorageMoverAgentListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzStorageMoverAgentShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageMoverAgentShowOptions(), token);
    }

    public async Task<CommandResult> Unregister(AzStorageMoverAgentUnregisterOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageMoverAgentUnregisterOptions(), token);
    }

    public async Task<CommandResult> Update(AzStorageMoverAgentUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageMoverAgentUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzStorageMoverAgentWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageMoverAgentWaitOptions(), token);
    }
}