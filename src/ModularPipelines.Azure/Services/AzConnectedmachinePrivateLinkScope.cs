using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("connectedmachine")]
public class AzConnectedmachinePrivateLinkScope
{
    public AzConnectedmachinePrivateLinkScope(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzConnectedmachinePrivateLinkScopeCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzConnectedmachinePrivateLinkScopeDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedmachinePrivateLinkScopeDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzConnectedmachinePrivateLinkScopeListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedmachinePrivateLinkScopeListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzConnectedmachinePrivateLinkScopeShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedmachinePrivateLinkScopeShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzConnectedmachinePrivateLinkScopeUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedmachinePrivateLinkScopeUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> UpdateTag(AzConnectedmachinePrivateLinkScopeUpdateTagOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedmachinePrivateLinkScopeUpdateTagOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzConnectedmachinePrivateLinkScopeWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedmachinePrivateLinkScopeWaitOptions(), cancellationToken: token);
    }
}