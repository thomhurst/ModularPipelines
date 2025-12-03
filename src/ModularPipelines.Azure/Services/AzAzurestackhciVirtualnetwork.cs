using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("urestackhci")]
public class AzAzurestackhciVirtualnetwork
{
    public AzAzurestackhciVirtualnetwork(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzAzurestackhciVirtualnetworkCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzAzurestackhciVirtualnetworkDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciVirtualnetworkDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzAzurestackhciVirtualnetworkListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciVirtualnetworkListOptions(), token);
    }

    public async Task<CommandResult> Show(AzAzurestackhciVirtualnetworkShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciVirtualnetworkShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzAzurestackhciVirtualnetworkUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciVirtualnetworkUpdateOptions(), token);
    }
}